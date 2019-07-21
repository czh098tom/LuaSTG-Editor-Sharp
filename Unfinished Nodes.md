# Unfinished Nodes（Comapring with EX+）

Nodes with (?) token may not be implemented.

Nodes with (*) token may be changed in this version of editor.

Nodes with (!) token may be added in this version of editor.


Nodes with * token may cost more time to finish.

Nodes with & token may need extra time to create GUI (hence finishing time is unknown)


## General
	CodeBlock(?)

## Stage
	StageGroupInfo
	StageGoTo
	StageFinish
	StageGroupFinish
	--
	SetWorlds
	AddWorld

## Task
	FinishTask
	ClearTasks
	DefineTask *
	AttachTask *
	--
	SetSignal
	WaitForSignal
	WaitTo(*)
	--
	MoveToBezier &
	MoveByBezier &
	MoveToCR &
	MoveByCR &
	MoveToBasis2 &
	MoveByBasis2 &
	--
	SmoothSetValue

## Enemy
	DefineEnemy *
	CreateEnemy *
	CreateSimpleEnemy(*)

## Boss
	BossMoveTo(?)
	Dialog
	Sentence
	--
	BossWander
	BossCast
	BossCharge
	BossExplode
	--
	BossAura
	BossUI
	--
	BossEX(?)

## Bullet
	BulletStyle
	BulletClear
	BulletClearRange

## BulletEX is killed

## Laser
	Finished.

## Object (merged old Unit)
	ObjectChangeImage
	Properties
	--
	ObjectList
	ObjectListAdd
	ObjectListRemove
	ObjectListForEach
	--
	SetGravity
	SetVelocityLimit
	PreserveUnit
	GroupForEach
	CreateSmear (From old Effect)
	--
	SetParent(*)
	SetRelativePositiion(*)
	--
	DropItem (From old Effect)

## Graphics
	LoadAnimation
	LoadParticle
	LoadTexture
	LoadFX
	--
	ShakeScreen (From old Effect)

## Audio
	PauseBGM
	ResumeBGM
	StopBGM
	SetPace(?)
	--
	PlaySound &
	LoadSound
