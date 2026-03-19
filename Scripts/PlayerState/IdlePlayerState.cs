using Godot;
using System;

public partial class IdlePlayerState : PlayerStateBase
{
	public override void Enter(Player player)
	{
		player.ChangeStateTo(PlayerStates.Idle);
	}

	public override void DetermineNextState(Player player)
	{
		if (!player.IsOnFloor())
			player.ChangeStateTo(PlayerStates.Fall);
		
		var currentSpeed = player.Velocity.Length();
		
		if (currentSpeed > player.RunSpeedTreshold)
		{
			player.ChangeStateTo(PlayerStates.Run);
		}
		else if (currentSpeed > 0)
		{
			player.ChangeStateTo(PlayerStates.Walk);
		}
	}

	public override void Update(Player player, double delta)
	{
		
	}
}
