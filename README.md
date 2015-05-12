<img src="https://raw.githubusercontent.com/gmich/Gem/master/Gem.Engine/Content/gem.png" width="52" height="50" alt="Gem logo" title="Gem" hspace="10" align="left">    Gem  
===================================

|       Linux             |      Windows           | 
|-------------------------|------------------------|
|[![Linux Build Status](https://travis-ci.org/gmich/Gem.svg)](https://travis-ci.org/gmich/Gem) | [![Windows Build status](https://ci.appveyor.com/api/projects/status/2kb9f1h05hksb3ry?svg=true)](https://ci.appveyor.com/project/gmich/gem) | 



 
Gem is a cross-platform game framework with GUI and multiplayer support using Monogame, Farseer and Lidgren

##Developement Progress

* Gem Network is not fully tested and not ready to use. 
* Gem Gui API is likely to change.
* Gem Engine is still in early development.

##Description

###Gem Network
A library for client server applications using network sockets. Gem Network extends Lidgren Network, offering an easy to use API for creating network events and a configurable and expendable server console for executing remote commands

Documentation for Gem Network can be found [here](https://github.com/gmich/Gem/wiki/Gem.Network)

###Gem GUI
A library to render [GUI controls](https://github.com/gmich/Gem/wiki/GemGui-Controls) in Monogame applications. 
Gem Gui supports the basic control events for mouse / keyboard / controller / touch. 
The control's style is easily customisable. 

Controls can be grouped into [GuiHosts](https://github.com/gmich/Gem/wiki/GemGui-Host) and show/hide them using the GemGui's screen management.

You can also assing controls into layout groups for automatic alignment that respond to window and resolution changes.

[A Gist with an example](https://gist.github.com/gmich/aee2e5cd3e7866df1446#file-gistfile1-cs)


Documentation and how to get started can be found [here](https://github.com/gmich/Gem/wiki)
