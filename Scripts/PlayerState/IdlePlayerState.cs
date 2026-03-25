using Godot;
using System;

public partial class IdlePlayerState : PlayerStateBase
{
	public override void Enter(Player player)
	{
		player.SetAnimation(PlayerAnimations.idle);
	}

	public override void DetermineNextState(Player player)
	{
		if (!player.IsOnFloor())
			player.ChangeStateTo(PlayerStates.Fall);
				
		else if (player.MoveInput.Length() > 0)
			player.ChangeStateTo(PlayerStates.Walk);

		else if (Input.IsActionJustPressed(Inputs.MOVE_JUMP))
			player.ChangeStateTo(PlayerStates.Jump);
	}
}
