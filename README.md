# Final-Fantasy-Battle

This repository is to publicize the code in this tutorial series for replicating much of the Final Fantasy 6 battle system in Godot:
https://www.youtube.com/playlist?list=PLqe-MDIGWTboCnmZmepgm5dnd1j636DqG

Please note that the audio & graphic assets are not included since it technically treads on proprietary grounds, so the project will not work without replacing them.
The MissingFiles.txt file is a list of these files that are excluded (from the "Audio" & "Graphics" folders).  The Scripts\GitIgnoreList.cs file will repopulate this text file every time the game is run.
These are the files that can be replaced with whatever is desired.  Bear in mind resolutions will matter and sprite sheets will go by order of the images within the image file.
The videos will show what was used originally.

A general rule:  I have taken the original sprites and made everything 4x their original size, typically a quick & dirty AI upscale.
However, this strategy is implemented so that one could use either original-style low-res/pixel graphics (by simply upscaling using "nearest" so that it is as much as possible, a copy, except w/ more resolution),
or, replace the images with either 2x or 4x images.
