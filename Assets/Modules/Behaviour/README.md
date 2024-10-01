# Behaviour Module
This module covers the AI system, how to use it, how to create custom nodes and how to use the `Tree Visualizer`.

## Nodes
This is where we will talk about the nodes, their use in the trees and how to create custom ones.

### Terminology
Just to not be confused, here are the terms used and what they are:
- Leaf: Node that does an actual
- Control: Node that only decides which node to call

### Usage
An AI tree is made of nodes that controls how the AI behaves. Each node has 4 states:

| State                                     | Signification                                                          |
|-------------------------------------------|------------------------------------------------------------------------|
| <span style="color:green">SUCCESS</span>  | A node's task is completed and has succeeded                           |
| <span style="color:red">FAILURE</span>    | A node's task has failed and did not complete its task                 |
| <span style="color:yellow">RUNNING</span> | A node's task is currently running and the result is yet to be defined |
| <span style="color:gray">NONE</span>      | A node's task was not executed since the last reset                    |

Usually, you will never encounter the state `NONE`. This is purely for the visualizer only.

### Controls available
As stated earlier, you need some logic in order to build a behaviour tree. This document will provide a quick summary of each *important* control node and their definition:

| Name     | Definition                                       |
|----------|--------------------------------------------------|
| Sequence | Evaluates every child until one does not succeed |
| Selector | Evaluates every child until one does not failed  |
| Parallel | Evaluates every child until one fails            |
| Inverter | Inverts the result of its child                  |


### Create a custom node
As you use this module, you might realize that you are missing a control or a leaf to do what you are looking for. Here is how you can create such node yourself:
1. Create a class in the proper folder that inherits `Node` (or one of its implementation)
2. Create the proper constructor. *Don't forget to call the parent's constructor*
3. Implement the method `Node.OnEvaluate` to put your own logic
4. (*Optional*) Override the method `Node.GetText` to customize the display in the visualizer

### Create a module node
You might want to create a prefab of nodes that can be used in different trees. In order to do that, you can simply create a custom node that inherits one of the controls. You can also override the method `IsAutomaticallyHidden` and returns `true` to make the module look like it was a single node.

*Note that the user will still be able to unfold the module.*

### Transferring data through the tree
One of the problem with the node is that it is hard to share data between nodes. Unless you create a separate manager for it, it would be almost impossible. However, the tree already has a manager for this.

In any node, you can set data to a given key by calling `Node.SetData`. This will give access to this information to every child of the node. If you want to pass data upwards (to a node that is not your child), you need to set the data in a shared parent (like the root).

To collect the data, you need to call `Node.GetData` and pass the key for the data. This will look into every parent to see if any has data associated with the key.

### Attach a node to another node
In order to build a tree, you need to attach nodes together to form the actual tree. In order to do that, there are 3 ways to do it:
1. Passing the children in the constructor
2. Calling `Node.Attach` while passing the children
3. Adding the parent with the child by doing `root += child`

*Note that the methods can vary depending on the parent's type. All methods are valid for most operator nodes*

## Trees
Now that we covered how to create custom nodes, we need to create the actual tree with which the AI will work.

### How to create an automatic tree
If you just want to use a tree without wanting to control when the tree is evaluated, you can create a tree like this:
1. Create a class that inherits `Tree`
2. Implement the method `Tree.SetUpTree`

In this method, the tree will be evaluated each frame. Once you have this set up, you need to create your tree inside `SetUpTree` and returns the root of the tree.

### How to create a manual tree
If you want to have control on when the tree is evaluated and/or need the result of the tree, you can create a tree like this:
1. Create a field of type `Node`
2. Add the interface `IVisualizable` to your class
3. Evaluate your tree whenever you need to

In this method, you will have full responsibility on creating and updating the tree. 

## Tree Visualizer
The visualizer is a useful tool that allows you to see your tree in action. 

To open it, you need to go to `Window -> Tree Visualizer`, This will open a window that will show you the current tree inspected. You will see the structure of the tree (which node is connected to which node) and their different state (during the current frame).

### Custom Type Name
To improve the readability of the visualizer, you can override the method `Node.GetText` inside your custom Node to show a unique text. This will replace the default text shown by a node. *Note that this will change the text for every node of this type*

### Alias
In the same category, if you want to give a particular name to a certain node without changing `Node.GetText`, you can assign a value by calling `Node.Alias` of the node in question. If the alias is set, it will override the text given by the node. This can be useful when you want to name a certain branch of the tree.

### Minimize the tree
As tree gets bigger, the visualizer becomes harder to read. By pressing on a node, you can hide all of its children. You can see that a node is minimized by looking at the line connecting to its parent.