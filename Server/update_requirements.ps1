Remove-Item requirements.txt
./venv/Scripts/activate
pip freeze > requirements.txt
deactivate