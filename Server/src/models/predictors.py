from datetime import datetime
import os
from typing import Any, Dict
from models.TimeSeries import TimeSeries
from models.Dataset import Dataset
from sklearn.preprocessing import LabelEncoder
import pickle
import gzip
from models.Result import Result
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score, balanced_accuracy_score, f1_score
from tensorflow import keras
from keras.models import Model
import joblib
import orjson
import numpy as np


class BasePredictor:
    """Base predictor for TimeSeries"""
    id: str
    path: str
    _label_encoder: LabelEncoder
    _labels_count: int
    _model_stats: Dict[str, float]
    """
    Parameters: 
    id: str - id of dataset
    path: str - path to application models storage
    _model_stats: Dict[str, float] - stats of trained model
    """

    def __init__(self, id: str, path: str) -> None:
        self._label_encoder = LabelEncoder()
        self.path = path
        self.id = id

    def train(self, dataset: Dataset) -> Result:
        """Train predictor using dataset"""
        res = self.fit_label_encoder(dataset)

        if res == Result.ERROR:
            return res

        time_series_with_encoded_labels = [
            TimeSeries(values=ts.values,
                       id=ts.id,
                       timeseries_class=self._label_encoder.transform(
                           [ts.timeseries_class]))
            for ts in dataset.time_series
        ]

        dataset_with_encoded_labels = Dataset(
            time_series=time_series_with_encoded_labels, name=dataset.name)

        res = self._train_model(dataset_with_encoded_labels)

        return Result.OK

    def predict(self, time_series: TimeSeries) -> str:
        """Make prediction"""

        prediction = self._make_model_prediction(time_series)

        return self._label_encoder.inverse_transform([prediction])[0]

    def load(self, **kwargs) -> Result:
        """Load predictor parameters"""
        try:
            with gzip.open(os.path.join(self.path, self.id, 'encoder.data'),
                           'rb') as f:
                self._label_encoder = pickle.load(f)
        except Exception:
            return Result.ERROR

        return self._load_model()

    def save(self, **kwargs) -> Result:
        """Save predictor parameters"""
        path = os.path.join(self.path, self.id)

        if os.path.exists(path) is False:
            os.makedirs(path)

        try:
            with gzip.open(os.path.join(path, 'encoder.data'), 'wb') as f:
                pickle.dump(self._label_encoder, f)
        except OSError:
            return Result.ERROR

        if self._model_stats is not None:
            self._model_stats['модель обучена'] = datetime.now()
            with open(os.path.join(self.path, self.id, 'stats.json'),
                      'wb') as fw:
                fw.write(
                    orjson.dumps(self._model_stats,
                                 option=orjson.OPT_SERIALIZE_NUMPY))

        return self._save_model()

    def get_labels_count(self) -> int:
        if self._label_encoder.classes_ is None:
            return 0
        else:
            return self._label_encoder.classes_.shape[0]

    def _make_model_prediction(self, time_series: TimeSeries) -> int:
        """model prediction"""
        raise NotImplementedError("BasePredictor don't imlement this method")

    def _train_model(self, dataset: Dataset) -> Result:
        """Train model.
        Should be implemented"""
        raise NotImplementedError("BasePredictor don't imlement this method")

    def _load_model(self) -> Result:
        """Load model parameters.
        Should be implemented"""
        raise NotImplementedError("BasePredictor don't imlement this method")

    def _save_model(self) -> Result:
        """Save model parameters.
        Should be implemented"""
        raise NotImplementedError("BasePredictor don't imlement this method")

    def fit_label_encoder(self, dataset: Dataset) -> Result:
        labels = [ts.timeseries_class for ts in dataset.time_series]
        try:
            self._label_encoder.fit(labels)
        except Exception:
            return Result.ERROR

        return Result.OK

    def get_stats(self) -> Dict[str, float]:
        """return dict stats of trained model"""
        raise NotImplementedError("BasePredictor don't imlement this method")


class SkLearnPredictor(BasePredictor):
    """Predictor that based on random forest classifier"""
    classifier: Any
    _train_stats: Dict[str, float]

    def __init__(self, id: str, path: str, classifier: Any) -> None:
        """Initialize of SkLearnPredictor
        params: 
        id: str - id of predictor
        path: str - path to folder of application
        classifier: Any - classifier from sklearn
        """
        self.classifier = classifier
        super().__init__(id, path)

    def _make_model_prediction(self, time_series: TimeSeries) -> int:

        return self.classifier.predict([time_series.values])[0]

    def _save_model(self) -> Result:
        """Save RandomForestClassifier"""
        filename = os.path.join(self.path, self.id, 'classifier.gz')

        try:
            joblib.dump(self.classifier, filename)
        except Exception:
            return Result.ERROR

        return Result.OK

    def _load_model(self) -> Result:
        """Load RandomForestClassifier"""
        filename = os.path.join(self.path, self.id, 'classifier.gz')

        try:
            self.classifier = joblib.load(filename)
        except Exception:
            return Result.ERROR

        with open(os.path.join(self.path, self.id, 'stats.json'), 'rb') as fr:
            self._model_stats = orjson.loads(fr.read())

        return Result.OK

    def _train_model(self, dataset: Dataset) -> Result:
        """Train RandomForestClassifier"""

        ts_values = [ts.values for ts in dataset.time_series]
        ts_labels = [ts.timeseries_class for ts in dataset.time_series]

        X_train, X_test, y_train, y_test = train_test_split(ts_values,
                                                            ts_labels,
                                                            test_size=0.15)

        self.classifier.fit(X_train, np.ravel(y_train))

        self._model_stats = {
            'точность':
            accuracy_score(np.ravel(y_test), self.classifier.predict(X_test)),
            'сбалансированная точность':
            balanced_accuracy_score(np.ravel(y_test),
                                    self.classifier.predict(X_test)),
            'F-score(macro)':
            f1_score(np.ravel(y_test),
                     self.classifier.predict(X_test),
                     average='macro'),
            'F-score(micro)':
            f1_score(np.ravel(y_test),
                     self.classifier.predict(X_test),
                     average='micro')
        }

        return Result.OK

    def get_stats(self) -> Dict[str, float]:
        return self._model_stats


class KerasModel(BasePredictor):
    """Base keras Predictor"""
    _train_stats: Dict[str, float]
    classifier: Model
    epochs: int

    def __init__(self, id: str, path: str, epochs: int = 200) -> None:
        """Initialize of CNNModelM1Predictor
        params: 
        id: str - id of predictor
        path: str - path to folder of application
        """
        super().__init__(id, path)
        self.epochs = epochs

    def _make_model_prediction(self, time_series: TimeSeries) -> int:

        values = self._reshape_timeseries(time_series).reshape(1, -1, 1)
        return np.argmax(self.classifier.predict(values), axis=1)[0]

    def _save_model(self) -> Result:
        """Save RandomForestClassifier"""
        filename = os.path.join(self.path, self.id, 'classifier.h5')

        try:
            self.classifier.save(filename)
        except Exception:
            return Result.ERROR

        return Result.OK

    def _load_model(self) -> Result:
        """Load RandomForestClassifier"""
        filename = os.path.join(self.path, self.id, 'classifier.h5')

        try:
            self.classifier = keras.models.load_model(filename)
        except Exception:
            return Result.ERROR

        with open(os.path.join(self.path, self.id, 'stats.json'), 'rb') as fr:
            self._model_stats = orjson.loads(fr.read())

        return Result.OK

    def _train_model(self, dataset: Dataset) -> Result:
        """Train RandomForestClassifier"""
        values = [self._reshape_timeseries(ts) for ts in dataset.time_series]

        input_shape = np.shape(values[0])
        labels_count = self.get_labels_count()

        print(input_shape)
        print(labels_count)

        self.classifier = self._compile_model(input_shape, labels_count)

        callbacks = [
            keras.callbacks.ReduceLROnPlateau(monitor="val_loss",
                                              factor=0.5,
                                              patience=20,
                                              min_lr=0.0001),
            keras.callbacks.EarlyStopping(monitor="val_loss",
                                          patience=50,
                                          verbose=1),
        ]

        ts_labels = [ts.timeseries_class for ts in dataset.time_series]

        X_train, X_test, y_train, y_test = train_test_split(values,
                                                            ts_labels,
                                                            test_size=0.15)

        y_train = np.ravel(y_train)
        y_test = np.ravel(y_test)
        X_train = np.array(X_train)
        X_test = np.array(X_test)

        self.classifier.fit(
            X_train,
            y_train,
            batch_size=32,
            epochs=self.epochs,
            callbacks=callbacks,
            validation_split=0.1,
            verbose=0,
        )

        classified_values = np.argmax(self.classifier.predict(X_test), axis=1)

        self._model_stats = {
            'точность':
            accuracy_score(np.ravel(y_test), classified_values),
            'сбалансированная точность':
            balanced_accuracy_score(np.ravel(y_test), classified_values),
            'F-score(macro)':
            f1_score(np.ravel(y_test), classified_values, average='macro'),
            'F-score(micro)':
            f1_score(np.ravel(y_test), classified_values, average='micro')
        }

        return Result.OK

    def get_stats(self) -> Dict[str, float]:
        return self._model_stats

    def _reshape_timeseries(self, timeseries: TimeSeries) -> np.ndarray:

        values = np.array(timeseries.values)
        values = values.reshape((values.shape[0], 1))

        return values

    def _compile_model(self, input_shape, labels_len: int) -> Model:
        raise NotImplementedError("should be implemented")


class CNNModelM1Predictor(KerasModel):
    """CNN Predictor Model1"""

    def __init__(self, id: str, path: str) -> None:
        """Initialize of CNNModelM1Predictor
        params: 
        id: str - id of predictor
        path: str - path to folder of application
        """
        super().__init__(id, path)

    def _compile_model(self, input_shape, labels_len: int) -> Model:

        input_layer = keras.layers.Input(input_shape)

        conv1 = keras.layers.Conv1D(filters=64, kernel_size=3,
                                    padding="same")(input_layer)
        conv1 = keras.layers.BatchNormalization()(conv1)
        conv1 = keras.layers.ReLU()(conv1)

        conv1 = keras.layers.Conv1D(filters=64, kernel_size=3,
                                    padding="same")(input_layer)
        conv1 = keras.layers.BatchNormalization()(conv1)
        conv1 = keras.layers.ReLU()(conv1)

        conv2 = keras.layers.Conv1D(filters=64, kernel_size=3,
                                    padding="same")(conv1)
        conv2 = keras.layers.BatchNormalization()(conv2)
        conv2 = keras.layers.ReLU()(conv2)

        conv3 = keras.layers.Conv1D(filters=64, kernel_size=3,
                                    padding="same")(conv2)
        conv3 = keras.layers.BatchNormalization()(conv3)
        conv3 = keras.layers.ReLU()(conv3)

        gap = keras.layers.GlobalAveragePooling1D()(conv3)

        output_layer = keras.layers.Dense(labels_len,
                                          activation="softmax")(gap)

        model = keras.models.Model(inputs=input_layer, outputs=output_layer)

        model.compile(
            optimizer="adam",
            loss="sparse_categorical_crossentropy",
            metrics=["sparse_categorical_accuracy"],
        )

        return model


class CNNModelM2Predictor(KerasModel):
    """CNN Predictor Model2"""

    def __init__(self, id: str, path: str) -> None:
        """Initialize of CNNModelM1Predictor
        params: 
        id: str - id of predictor
        path: str - path to folder of application
        """
        super().__init__(id, path)

    def _compile_model(self, input_shape, labels_len: int) -> Model:

        input_layer = keras.layers.Input(input_shape)

        conv1 = keras.layers.Conv1D(filters=64, kernel_size=3,
                                    padding="same")(input_layer)
        conv1 = keras.layers.BatchNormalization()(conv1)
        conv1 = keras.layers.ReLU()(conv1)

        conv1 = keras.layers.Conv1D(filters=64, kernel_size=3,
                                    padding="same")(input_layer)
        conv1 = keras.layers.BatchNormalization()(conv1)
        conv1 = keras.layers.ReLU()(conv1)

        conv2 = keras.layers.Conv1D(filters=64, kernel_size=3,
                                    padding="same")(conv1)
        conv2 = keras.layers.BatchNormalization()(conv2)
        conv2 = keras.layers.ReLU()(conv2)

        conv3 = keras.layers.Conv1D(filters=64, kernel_size=3,
                                    padding="same")(conv2)
        conv3 = keras.layers.BatchNormalization()(conv3)
        conv3 = keras.layers.ReLU()(conv3)

        gap = keras.layers.GlobalAveragePooling1D()(conv3)

        flat = keras.layers.Flatten()(gap)

        dense1 = keras.layers.Dense(256)(flat)

        dense2 = keras.layers.Dense(128)(dense1)

        output_layer = keras.layers.Dense(labels_len,
                                          activation="softmax")(dense2)

        model = keras.models.Model(inputs=input_layer, outputs=output_layer)

        model.compile(
            optimizer="adam",
            loss="sparse_categorical_crossentropy",
            metrics=["sparse_categorical_accuracy"],
        )

        return model
