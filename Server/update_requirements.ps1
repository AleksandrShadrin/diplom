Remove-Item ./src/requirements.txt
./venv/Scripts/activate
pip freeze > ./src/requirements.txt
deactivate