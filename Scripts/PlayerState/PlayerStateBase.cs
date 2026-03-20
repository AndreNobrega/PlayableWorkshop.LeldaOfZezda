using Godot;
using System;

// Inherit from RefCounted, which the Node class inherits from
public abstract partial class PlayerStateBase : RefCounted
{
	// Called when we first enter this state.
	public virtual void Enter(Player player){}

	// Called when we exit this state.
	public virtual void Exit(Player player){}

	// Called before Update(), allows for state changes.
	public virtual void DetermineNextState(Player player){}

	// Called for every physics frame that we're in this state.
	public virtual void Update(Player player, double delta){}
}
