using Godot;
using System;

public partial class IdlePlayerState : PlayerStateBase
{
	public override void Enter(Player player)
	{
		player.AnimTree.Set(Player.MOVEMENT_TRANSITION_REQUEST, "idle");
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

	public override void Update(Player player, double delta)
	{
		player.Velocity = player.Velocity.MoveToward(Vector3.Zero, player.BaseSpeed);
		player.MoveAndSlide();
	}
}
