pyinstaller -F --add-data "config.json;." --hidden-import "dependency_injector.errors" --hidden-import "babel.dates" --hidden-import "babel.numbers" .\main.py