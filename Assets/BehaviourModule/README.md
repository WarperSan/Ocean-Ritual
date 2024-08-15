# Behaviour Module
This module covers the AI system, how to use it, how to create custom nodes and how to use the `Tree Visualizer`.

## Nodes
This is where we will talk about the nodes, their use in the trees and how to create custom ones.

### Usage
An AI tree is made of nodes that controls how the AI behaves. Each node has 4 states:

| State   | Signification                                                          |
|---------|------------------------------------------------------------------------|
| SUCCESS | A node's task is completed and has succeeded                           |
| FAILURE | A node's task has failed and did not complete its task                 |
| RUNNING | A node's task is currently running and the result is yet to be defined |
| NONE    | A node's task was not executed since the last reset                    |

### Nodes available
As stated earlier, you need some logic in order to build a behaviour tree. This document will provide a quick summary of each *important* node and their definition:

| Name     | Definition                                       |
|----------|--------------------------------------------------|
| Sequence | Evaluates every child until one does not succeed |
| Selector | Evaluates every child until one does not failed  |
| Parallel | Evaluates every child until one fails            |
| Inverter | Inverts the result of its child                  |

### Create a custom node
To create a custom node, you need to:
1. Create a class that inherits `Node`
2. Create a constructor that uses the constructor of the parent
3. Override the method `Node.OnEvaluate` to put your own logic
4. (*Optional*) Override the method `Node.GetText` to customize the display in `Tree Visualizer`

### Transferring data through the tree
One of the problem with the node is that it is hard to share data between nodes. Unless you create a separate manager for it, it would be almost impossible. However, the tree already has a manager for this.

In any node, you can set data to a given key by calling `Node.SetData`. This will give access to this information to every child of the node. If you want to pass data upwards (to a node that is not your child), you need to set the data in a shared parent (like the root).

To collect the data, you need to call `Node.GetData` and pass the key for the data. This will look into every parent to see if any has data associated with the key,

## Trees
Now that we covered how to create custom nodes, we need to create the actual tree with which the AI will work.

### How to create a tree
To create a tree, you need to:
1. Create a class that inherits `Tree`
2. Implement the method `Tree.SetUpTree`

Once you have this set up, you need to create your tree inside `SetUpTree` and returns the root of the tree.

## Tree Visualizer
The visualizer is a neat tool that allows you to see your tree in action. 

To open it, you need to go to `Window -> Tree Visualizer`, This will open a window that will show you the current tree inspected. This tool shows you the structure of the tree and the different state of each node.