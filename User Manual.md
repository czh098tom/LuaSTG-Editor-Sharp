# LuaSTG Editor Sharp User Manual
## 1. Installation
### 1.1 Prerequisite
Platform required: Windows system (Windows 10 is recommend)

Runtime library: .Net Framework 4.6.1.

Storage space: At least 32MB is recommended

### 1.2 Steps
To install LuaSTG Editor Sharp, you need to extract all files in the zip archive.  Run the executable in extracted, and click next. The LuaSTG Editor Sharp can be installed anywhere unless UAC banned you from installing.

## 2. Tips
### 2.1 For Newcomers
This document talk about editor itself, NOT how to create danmaku. To learn how to make danmaku, ER tutorial is highly recommended to help you learn about basics, such as Repeat, Task, Define and Create nodes. The basics are same. Advanced topics are mostly covered in this document, so you may skip them once I mentioned MAY or ADVANCED words. Demo from ER tutorial may not be able to run on this editor, so you may need old version (wxlua) editor or wait for porting.

### 2.2 For Old Editor Users
This editor covers most of the wxlua editor can do. However, you need to focus more on differences (usually better than old ones), which is main topic covered in this document. Old theories can mostly fits in the editor.

## 3. File
### 3.1 Basics
#### 3.1.1 Creating Files
To create files, click button on toolbar or menu. For hotkey it is ctrl+N. Then the New File window is shown. You can select any type of template given. For newcomers, SpellCard is highly recommended (but SpellCard Template cannot be used if not in default plugin, see 7). Project may be selected if you need to create a file depending other files (For further information, see 8). Addition settings can be configured, but can also changed later in node.

#### 3.1.2 Saving
Files can be saved by clicking button on toolbar or menu, or hotkey ctrl+S. Save As function is also supported.

#### 3.1.3 Multiple File management
The editor cannot be runned multiple times at the same time, but you can open many files at a single editor. Files can be closed if unwanted, and once closing the editor, you will be asked to save all unsaved files.

### 3.2 Templates
You may need a template if you rapidly reuse some nodes or structure in your work. Hence a template can be created. The templates can be found at "(installation directory)/Templates". If you need a template to be created, simply copy it here. An optional description file can be created if you need, using the name same with template file and extension .txt.

## 4. Editing
### 4.1 Inserting Nodes
LuaSTG Editor nodes are organized in tree hierarchy. Before inserting each node, you need to ensure your inserting state is in the state you need. If you need to change, you can click on the arrow buttons, or hotkey Alt+arrow button.
There are four insert state in this editor.

#### 4.1.1 Insert Before (Up arrow)
Insert before state indicates the new node to be inserted before the current selected node.

For example:

```
Node A
	<Node B>
		Node C
	Node D
```

In the tree structured above, node B is selected, the node to insert is:

```
Node X
	Node Y
```

You choose insert before, result will be:

```
Node A
	Node X
		Node Y
	Node B
		Node C
	Node D
```

#### 4.1.2 Insert After (Down arrow)
Insert after state indicates the new node to be inserted after the current selected node.

For the example above, keep the origin tree, selected node and node to insert, the result will be:

```
Node A
	Node B
		Node C
	Node X
		Node Y
	Node D
```

#### 4.1.3 Insert As Child (Right arrow)
Insert as child state indicates the new node to be inserted as the child of the current selected node. It will become last node if the selected node have had a child already. It is usually used to create first child.

For the example above, keep the origin tree, selected node and node to insert, the result will be:

```
Node A
	Node B
		Node C
		Node X
			Node Y
	Node D
```

#### 4.1.4 Insert As Parent (Left arrow, new function)
Insert as parent state indicates the new node to be inserted as the parent of the current selected node. The selected node will become last node if the node to insert have had a child already. It is usually used to encapsule a node by a new one.

For the example above, keep the origin tree, selected node and node to insert, the result will be:

```
Node A
	Node X
		Node Y
		Node B
			Node C
	Node D
```

### 4.2 Modifying Nodes
### 4.3 Moving Nodes
#### 4.3.1 Cut, Copy and Paste
#### 4.3.2 Region
### 4.4 Reverting
### 4.5 Messages
### 4.6 Generalization Nodes
### 4.7 Temporary Discarding Nodes
## 5. Running
### 5.1 Run
### 5.2 SCDebugger and StageDebugger
### 5.3 Debug Output
### 5.4 Publishing
## 6. Settings
### 6.1 General Settings
### 6.2 Compiler Settings
### 6.3 Debug Settings
### 6.4 Editor Settings
## 7. Plugin

## 8. Project Management
