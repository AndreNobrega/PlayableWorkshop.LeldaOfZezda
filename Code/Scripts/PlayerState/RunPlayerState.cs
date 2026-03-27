using Godot;
using System;

public partial class RunPlayerState : PlayerStateBase
{
	private static string RUN_LEAN_AMOUNT = "parameters/run_lean/add_amount";

	public override void Enter(Player player)
	{
		player.SetAnimation(PlayerAnimations.run);
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
		
		else if (currentSpeed < player.RunAnimationTriggerSpeed)
			player.ChangeStateTo(PlayerStates.Walk);
	}

	public override void Update(Player player, double delta)
	{
		var move = player.MoveDirection * player.MoveInput.Length();
		player.UpdateHorizontalVelocity(move);
		player.MoveAndSlide();
		player.TurnTo(move);

		var lean = player.MoveDirection.Dot(player.GlobalBasis.X);
		player.LeanLerp = Mathf.Lerp(player.LeanLerp, lean, 0.3f);
		player.AnimTree.Set(RUN_LEAN_AMOUNT, player.LeanLerp);
	}
}
