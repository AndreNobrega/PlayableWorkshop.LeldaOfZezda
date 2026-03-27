using Godot;
using System;

public partial class FallPlayerState : PlayerStateBase
{
	public override void Enter(Player player)
	{
		player.SetAnimation(PlayerAnimations.fall);

		player.CalculateFallGravity();
		player.CalculateHorizontalJumpSpeed();
	}

	public override void DetermineNextState(Player player)
	{
		if (player.IsOnFloor())
			player.ChangeStateTo(PlayerStates.Idle);
	}

	public override void Update(Player player, double delta)
	{
		player.UpdateJumpTrajectory(delta);
	}
}
