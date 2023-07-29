# YOTE
![image](https://github.com/Huytimeclock/YOTE/assets/75327686/f5f45f70-21e5-435e-8099-109cdb2925bd)


# Setup

Download latest version on release.

When you download, the game already playable, but you should take notice on 2 things.
- In Game_data folder, make sure to have setting.txt file, and have 3 parts like this. 
![image](https://github.com/Huytimeclock/YOTE/assets/75327686/d73250eb-7deb-4583-8341-8c045176854d)
![image](https://github.com/Huytimeclock/YOTE/assets/75327686/3b21b2d9-df6a-4c7b-a7be-ec8331f5a768)

The AR represent time when a note complete enlarge ( upcoming note ) ( exp : 0.5 mean the note will spawn and end at 0.5, the longer, the more hard to read, the shorter, the more quick it is )



# How to play

The game use the whole keyboard to play, it present each of block on screen 
There is two type of note. ( Ground and Air )

The ground represent in red color, exp in this case, you will click R
![image](https://github.com/Huytimeclock/YOTE/assets/75327686/b5149d7a-019f-49c3-a5ff-6b1ecca5d122)

The air represent in green color, exp in this case, you will click Shift + G
![image](https://github.com/Huytimeclock/YOTE/assets/75327686/731902f0-6056-483a-9ddb-378e5236816f)

The game also represent many note at the same time, so be free to explore and mapping as you want.



# Map structure

Placing: in Game_data/Beatmaps

Name rule: should be [id] - [name]
![image](https://github.com/Huytimeclock/YOTE/assets/75327686/d8f6e1f5-6b7d-4e37-a606-d3fad01d72f9)

Folder beatmap structure:

- Audio: mp3 file ( naming audio.mp3 )
- Img: jpg file ( naming bg.jpg )
- Map file: txt file ( naming map.txt )
- Score file: txt file ( naming store.txt )
  * for now, game dont support create file score automatically, so make sure to create it.

![image](https://github.com/Huytimeclock/YOTE/assets/75327686/b737fd45-9dd2-440b-9f17-c646edfd4d58)


# Creating map

The beat map has 3 parts, all of them should be place in same txt file
- [MAP INFO]
 ![image](https://github.com/Huytimeclock/YOTE/assets/75327686/43358327-11bb-4c61-b21c-a81c02a1ff97)

- [MAP VISUAL] ( optional )
  This part is just you messing around with color of background to make effect, the number represent r,g,b,o.
![image](https://github.com/Huytimeclock/YOTE/assets/75327686/20f71cce-9838-4406-ae5f-091386ce6ced)

- [MAP DATA]
  * Structure: [time][key to press]
- There is 3 notice in creating map
  * If you want to have ground note, just put normal letter ( exp : a,b,c,... )
  * If you want to have air note, upcase letter ( exp : A,B,C,... )
  * If you want to have many note in the same time, just have "," as example show below

  ![image](https://github.com/Huytimeclock/YOTE/assets/75327686/f16aadb5-16fa-4f0d-b73d-1f552614a6c6)

If the game failed to run the map, make sure you check the blank space like this

![image](https://github.com/Huytimeclock/YOTE/assets/75327686/348498a1-57b0-44cf-9d50-427ef6630b94)

# Multiplayer

For now, the game support multiplayer, but you need account to do, and I cant public account since I used free photon which might limit the bandwith, so if you really want to play with your friend, please contact me for account 
- Discord: Huytimeclock

Also here is some screenshot about multiplayera
- The game now dont support map checking so make sure you guy have the same file ( id and content )

Creating/join room
![image](https://github.com/Huytimeclock/YOTE/assets/75327686/a60fc3e7-dfe3-4b25-be2a-ceda2212c442)

Pick Beatmap ( only the host )
![image](https://github.com/Huytimeclock/YOTE/assets/75327686/0d163baf-6d8a-463a-a674-9cc6c2f032c5)

After pressing play, all of the player will navigate to play.

# Have fun

This project made for Project1 subject in my school, for now, Im learning in creating web, draw, and japanese so might not be able to update for a long time, but this thing atleast you can play around and mess which, any feedback pls contact me through discord: Huytimeclock

Thanks again for playing my game <3
