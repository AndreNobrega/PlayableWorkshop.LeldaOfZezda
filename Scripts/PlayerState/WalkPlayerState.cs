using Godot;
using System;

public partial class WalkPlayerState : PlayerStateBase
{
	private readonly string WALK_TIMESCALE = "parameters/walk_timescale/scale";

	public override void Enter(Player player)
	{
		player.SetAnimation(PlayerAnimations.walk);
	}

	public override void DetermineNextState(Player player)
	{
		var currentSpeed = player.Velocity.Length();

		if (Input.IsActionJustPressed(Inputs.MOVE_JUMP))
			player.ChangeStateTo(PlayerStates.Jump);

		else if (!player.IsOnFloor())
			player.ChangeStateTo(PlayerStates.Fall);

		else if (player.MoveInput.Length() == 0)
			player.ChangeStateTo(PlayerStates.Idle);

		else if (currentSpeed > player.RunSpeed)
			player.ChangeStateTo(PlayerStates.Run);
	}

	public override void Update(Player player, double delta)
	{
		var move = player.MoveDirection * player.MoveInput.Length();
		player.UpdateVelocity(move);
		player.MoveAndSlide();
		player.TurnTo(move);

		var currentSpeed = player.Velocity.Length();
		var walkSpeed = Mathf.Lerp(0.5, 1.75, currentSpeed / player.BaseSpeed);
		player.AnimTree.Set(WALK_TIMESCALE, walkSpeed);
	}
}
