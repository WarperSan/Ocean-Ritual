# Interact Module
This module covers the interaction system, how to use it, how to create interaction scripts for specific behaviours and what each component does.

## Interact system
The controllers can also interact with the world. To make a controller able to interact, you will need to call the appropriate functions from `IInteractable`.

### Add an interactable object
To make an object interactable, you need to give it a scriptthat implements the interface `IInteractable`. This can be a premade script or a new script that implements the interface.

Once you have your script, you simply needs to add it to the object that will be interactable. Note that it is **important** that the GameObject has a collider and the GameObject is on the `Interactable` layer. The first is forced when using premade scripts and the second is managed by an editor that will notify you if there is an error.  

### Add an interaction blocker
There is no built-in way to add a blocker. You can make a blocker by making an interactable object while omitting to add the script. This will prevent the system to look beyond this collider.

### Interaction Assets
Interaction assets are what determines the icon to show. If the interactable does not specify an asset, the system will use the default one. If you want your interaction to have a custom icon, you can simply create a new asset and put the desired icon.

### Authors:
- WarperSan