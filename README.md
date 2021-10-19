# Chaaming
DDW GameJam SL

# Info
Add your scene to the buildIndex (build options, add open scene) Make this always be placed BELOW/AFTER the main scene.

`Start game(float Time, bool StartAsWin (win condition), string gameplayText)`

**Time** = give a value ranging from 4-7. Will decide how long the game lasts.

**StartAsWin** = the win condition, if true you lose when you set the hasWon to false. If StartAsWin is false you win on setting hasWon to true.

**Gameplaytext** = the text you will see on the UI before the game starts. If you have a balancing game it should say "Balance it!" or something similar. Call in the start function from your script (once!) (edited)

`SetWon(bool won)`

**Won** = set the bool hasWon to true or false. Keep your win condition in mind!

<-This you likely won't need but just in case of needing it.->

`GetTime`

Returns the time(float) in case you want to make events time based on the used timer.

`GetProgressValue`

Returns the progressValue(float) in case you want to see how far you are along the current timing. Returns a value ranging from 0.0f-1.0f.

Public value `gamesCount` on the gamemanager (UI in Hierarchy). Set this to the amount of games, It will use this value to randomly select the needed index.
