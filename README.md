# QUIZLET POL-ENG
Welcome to my new Polish - English Quiz app. 
## Introduction
This desktop application was written using WPF freamework. It can help us learn new english words, translate polish words into English and vice versa, while checking for any mistakes we may have made. The app is managed by JSON files and to read data from JSON files we use JSON serialization and deserialization, so we can transform files with JSON extension on dictionaries. Every save and load operation is asynchronous, so we can complete our tasks more efficiently and without stopping the main thread. The app contains a bank of words, which the users can expand themselves. The LINQ mechanism prevents doubling of the words. When a certain word already exists in the dictionary, another meaning will be added. Furthermore, we can display a complete list of all available words. When we close the app, all of the learning progress will be saved automatically.  
I'm interested in expanding this quiz by adding more languages in the nearest future.

## App functions
* **Change mode** - change mode beewtwen POL-ENG and ENG-POL
* **Rand word** - shows a random word from the dictionary
* **Check** - checks the translation
* **Delete word** - deletes learned word from the bank
* **Add word** - opens a new window for adding a new word
* **Show answer** - shows a translation of the word
* **Reset button** - resets progress bar and bank of words
* **Show word list** - shows a list of all available words
* **Reload app** - Reloads the app. Use this function after adding new words


---
![image](https://user-images.githubusercontent.com/126328327/233987595-bdef8699-86fe-46bc-b1fc-7f16db85df8f.png)
![image](https://user-images.githubusercontent.com/126328327/233987682-59bee3be-e2aa-4c3b-841e-502c04592f51.png)
![image](https://user-images.githubusercontent.com/126328327/233987799-ac856d5f-c237-4754-a82f-562dc079c5c3.png)


## Running the application

Clone repository

    git clone https://github.com/Martinpl99/Quizlet-POL-ENG

1st option:

    Run application using IDE (Microsoft Visual Studio)

2nd option:

    Run application using .exe file from directory ../Dictionary-POL-ENG/bin/Debug/net6.0-windows/Dictionary-POL-ENG.exe
