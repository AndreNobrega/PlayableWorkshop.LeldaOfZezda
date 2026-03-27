using Godot;
using System;

public partial class JumpPlayerState : PlayerStateBase
{
	public override void Enter(Player player)
	{
		player.SetAnimation(PlayerAnimations.jump_start);
				
		var velocity = player.Velocity;

		velocity.Y += player.JumpImpulse;
		
		player.Velocity = velocity;
	}

	public override void DetermineNextState(Player player)
	{
		if (Input.IsActionJustReleased(Inputs.MOVE_JUMP) || player.Velocity.Y <= 0)
			player.ChangeStateTo(PlayerStates.Fall);
	}

	public override void Update(Player player, double delta)
	{
		// TODO ANDRE: still floaty, need to boost upwards acceleration
		var move = player.MoveDirection * player.MoveInput.Length();
		
		player.Velocity += player.GetGravity() * player.JumpGravity * (float)delta;

		player.UpdateVelocity(move, player.BaseSpeed);

		player.MoveAndSlide();
		player.TurnTo(move);		
	}
}
