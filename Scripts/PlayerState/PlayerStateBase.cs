using Godot;
using System;

// Inherit from RefCounted, which the Node class inherits from
public partial class PlayerStateBase : RefCounted
{
	// Called when we first enter this state.
	public void Enter(){}

	// Called when we exit this state.
	public void Exit(){}

	// Called for every physics frame that we're in this state.
	public void Update(float delta)
	{
		return;
	}
}
