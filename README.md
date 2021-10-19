# Chaaming
DDW GameJam SL

# Info
Place the GameManager.cs on an empty object on your scene

`Start game(float Time, bool StartAsWin (win condition), string gameplayText)`

**Time** = give a value ranging from 4-7. Will decide how long the game lasts.

**StartAsWin** = the win condition, if true you lose when you set the hasWon to false. If StartAsWin is false you win on setting hasWon to true.

**Gameplaytext** = the text you will see on the UI before the game starts. If you have a balancing game it should say "Balance it!" or something similar. Call in the start function from your script (once!) (edited)

`SetWon(bool won)`

**Won** = set the bool hasWon to true or false. Keep your win condition in mind!