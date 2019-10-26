# Unfinished Nodes（Comapring with EX+）

Nodes with (?) token may not be implemented.

Nodes with (*) token may be changed in this version of editor.

Nodes with (!) token may be added in this version of editor.


Nodes with * token may cost more time to finish.

Nodes with & token may need extra time to create GUI (hence finishing time is unknown)


Names below are desired class names for the unfinished nodes.
"--" means a group separation.

## General
	CodeBlock(?)

## Data
    InverseVar(!)(Inverse a given variable)

## Stage
	StageGroupInfo
	StageGoTo
	StageFinish
	StageGroupFinish
	--
	[x]SetWorlds
	[x]AddWorld

## Task
	FinishTask
	ClearTasks
	--
	[x]SetSignal
	[x]WaitForSignal
	[x]WaitTo(*)
	--
	MoveToBezier &
	MoveByBezier &
	MoveToCR &
	MoveByCR &
	MoveToBasis2 &
	MoveByBasis2 &

## Enemy
	CreateSimpleEnemy(*)

## Boss
	BossMoveTo(?)
	--
	BossAura
	BossUI
	--
	[x]BossEX(?)

## Bullet
	Finished.

## BulletEX Is Killed

## Laser
	Finished.

## Object (Merged Old "Unit")
	ObjectChangeImage
	[x]Properties
	--
	[x]ObjectList
	[x]ObjectListAdd
	[x]ObjectListRemove
	[x]ObjectListForEach
	--
	PreserveUnit
	CreateSmear (From old "Effect")
	--
	DropItem (From old "Effect")

## Graphics
	LoadAnimation *
	LoadParticle *
	LoadTexture *
	LoadFX *
	--
	ShakeScreen (From old "Effect")

## Audio
	PauseBGM
	ResumeBGM
	StopBGM
	[x]SetPace(?)
