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
		{
			player.SetAnimation(PlayerAnimations.landing_soft);
			
			if (player.MoveInput != Vector2.Zero)
				player.ChangeStateTo(PlayerStates.Walk);
			else
				player.ChangeStateTo(PlayerStates.Idle);
		}
	}

	public override void Update(Player player, double delta)
	{
		player.UpdateVerticalTrajectory(delta);
	}
}
