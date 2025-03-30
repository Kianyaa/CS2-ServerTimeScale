# CS2-ServerTimeScale

**CS2 admin plugin command provides feature to control time scale for the server can multiply between 0-10**

`!timescale 2` : Increase server time scale multiply by 2 <br><br>
![สกรีนช็อต 2025-03-30 132753](https://github.com/user-attachments/assets/45973b1c-7ebe-4948-bdb6-377b8796f052)

`!timescale 2.85` : support float digit, Increase server time scale multiply by 2.85 <br><br>
![สกรีนช็อต 2025-03-30 132832](https://github.com/user-attachments/assets/94316a59-3ad1-49ec-a5ba-c60c803f4114)

<br>

## Features

- automatically reset the time scale when a new round starts or end of the round
- support float digit and automatically round the float to 2 digits.
- print to chat all for notice the player server time scale is changed

> [!WARNING]  
> If you timescale too high (such as !timescale 10) or too low (such as !timescale 0.1)
>
> Can cause server crash, Please be careful when using it

<br>

> [!CAUTION]  
> DO NOT use `!timescale` while the map gonna changing or force change the map
>
> Can cause client to slow tick rate (need player re-join the server to fix)
  

## Requirements
- [MetaMod](https://cs2.poggu.me/metamod/installation)
- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp) (Build version 1.0.305)

## Installation

download latest relase version from [Releases Latest](https://github.com/Kianyaa/CS2-ServerTimeScale/releases/tag/Latest)
and extract zip file and paste `ServerTimeScale.dll` on `addons\counterstrikesharp\plugins\ServerTimeScale` folder 
restart server or change the map for apply the plugin on load




    
