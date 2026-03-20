using Godot;
using System;

public partial class JumpPlayerState : PlayerStateBase
{
	public override void Enter(Player player)
	{
		player.Velocity = new Vector3(player.Velocity.X, Player.JUMP_VELOCITY, player.Velocity.Z);
	}

	public override void DetermineNextState(Player player)
	{
		player.ChangeStateTo(PlayerStates.Fall);
	}
}
