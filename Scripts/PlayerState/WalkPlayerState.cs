using Godot;
using System;

public partial class WalkPlayerState : PlayerStateBase
{
	private readonly string WALK_TIMESCALE = "parameters/walk_timescale/scale";

	public override void Enter(Player player)
	{
		player.AnimTree.Set(Player.MOVEMENT_TRANSITION_REQUEST, "walk");
	}

	public override void DetermineNextState(Player player)
	{
		if (player.Velocity.Length() > player.RunSpeedTreshold)
			player.ChangeStateTo(PlayerStates.Run);

		else if (player.Velocity.Length() > 0)
			return;
		
		else if (player.Velocity == Vector3.Zero)
			player.ChangeStateTo(PlayerStates.Idle);
	}

	public override void Update(Player player, double delta)
	{
		var currentSpeed = player.Velocity.Length();

		var walkSpeed = Mathf.Lerp(0.5, 1.75, currentSpeed / player.RunSpeed);
		player.AnimTree.Set(WALK_TIMESCALE, walkSpeed);
	}
}
