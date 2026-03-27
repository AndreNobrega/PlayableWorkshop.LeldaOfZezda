using Godot;
using System;

public static class PlayerStates
{
	public static IdlePlayerState Idle {get;} = new();
	public static WalkPlayerState Walk {get;} = new();
	public static RunPlayerState Run {get;} = new();
	public static FallPlayerState Fall {get;} = new();
	public static JumpPlayerState Jump {get;} = new();
}
