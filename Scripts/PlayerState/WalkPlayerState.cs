using Godot;
using System;

public partial class WalkPlayerState : PlayerStateBase
{
	public override void Enter(Player player)
	{
		var currentSpeed = player.Velocity.Length();
		var walkSpeed = Mathf.Lerp(0.5, 1.75, currentSpeed / player.RunSpeed);
		player.AnimTree.Set("parameters/walk_timescale/scale", walkSpeed);
		player.AnimTree.Set(Player.MOVEMENT_TRANSITION_REQUEST, "walk");
	}
}
