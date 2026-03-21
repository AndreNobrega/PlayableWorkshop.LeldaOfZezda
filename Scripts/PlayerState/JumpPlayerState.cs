using Godot;
using System;

public partial class JumpPlayerState : PlayerStateBase
{
	public override void Enter(Player player)
	{
		player.SetAnimation(PlayerAnimations.jump_start);
		player.Velocity = new Vector3(player.Velocity.X, player.Velocity.Y + player.JumpImpulse, player.Velocity.Z);
	}

	public override void DetermineNextState(Player player)
	{
		player.ChangeStateTo(PlayerStates.Fall);
	}
}
