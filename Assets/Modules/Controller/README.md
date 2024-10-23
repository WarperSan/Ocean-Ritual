# Controller Module
This module covers the controller system, how to use it, how to create new controllers for specific movements and what each component does.

## Inputs
This is where we will talk about the keybinds and how to add an input.

### Change keybinds
To change the actual keybind, you just need to edit [this file](./Input%20Assets/PlayerInput.inputactions).

### Add an input
To add an input to the game, you will need to do multiple things. 

1. Create an action inside [this file](./Input%20Assets/PlayerInput.inputactions).

2. Go inside [InputMaster](./Scripts/InputMaster.cs).

3. Add a delegate for your input. This is where you will add the needed parameters.

4. Add an event that uses the delegate.

5. Add a method with the parameter `InputAction.CallbackContext`. This is where you will need to call the delegate.

6. Create a new interface for your input inside [this folder](./Scripts/Interfaces).

7. Inside the interface, add a method for your input. This has the same signature as your delegate.

8. Go to either [IPlayerActionable](./Scripts/Interfaces/Player/IPlayerActionable.cs) or [IUIActionable](./Scripts/Interfaces/UI/IUIActionable.cs), depending if your input is a UI or a player input.

9. Subscribe your method inside `Operations.+`.

10. Unsubscribe your method inside `Operations.-`.

11. In Unity, add your method from step 5 to the proper callback inside `Player Input`.

This is a long process, but it allows to centralize the inputs between controllers. **If, at any step, you are confused, you can look at the other methods or ask the authors**.

Of course, depending on the desired result, the process can change.

## Action Interfaces
In order to make an object react to player inputs, you need to implement the appropriate interface. For example, if you want to get notified when the player fires, you should implement [IFirable](./Scripts/Interfaces/Player/IFirable.cs).

*Note:*
- Only controllers are able to receive inputs from [theses interfaces](./Scripts/Interfaces/Player)
- Only UI components are able to receive inputs from [theses interfaces](./Scripts/Interfaces/UI)

## Controllers
The magic is managed by `InputMaster` and `ControllerManager`. They both ease the addition of new controllers.

### How to add a new controller
To add a new controller, you need to inherit `Controller`. You can then override the desired functions to execute what you need.

Sometimes, the controller could actually hide the base function without any consequences. However, it is hard to predict what will be the needs for the future (*maybe we will need to execute stuff in the start method for every controller*).

### Controller Stack
`ControllerManager` works by using a stack to recover previous used controllers. For example, you can switch to a controller and easily exit to the previous controller, no matter the controller used.

By default, the controller on top of the stack is `PlayerController`. The system won't let you exit a controller if no other controller are present in the stack.

### Authors:
- WarperSan