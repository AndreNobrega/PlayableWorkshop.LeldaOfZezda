using Godot;
using System;

public partial class FallPlayerState : PlayerStateBase
{
	public override void Enter(Player player)
	{
		player.ChangeStateTo(PlayerStates.Fall);
	}

	public override void DetermineNextState(Player player)
	{
		if (player.IsOnFloor())
		{
			player.ChangeStateTo(PlayerStates.Idle);
		}
	}
}
