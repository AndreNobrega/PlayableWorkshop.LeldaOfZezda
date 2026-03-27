using Godot;
using System;

public partial class JumpPlayerState : PlayerStateBase
{
	public override void Enter(Player player)
    {
        player.SetAnimation(PlayerAnimations.jump_start);

		// Putting thess calculations here instead of the Player _Ready() method,
        // so we can tweak the numbers whilst live testing, instead of it being set on startup.
        player.CalculateJumpSpeed();
		player.CalculateJumpGravity();

        var velocity = player.Velocity;
        velocity.Y = player.JumpSpeed;
        player.Velocity = velocity;
    }
    
    public override void DetermineNextState(Player player)
	{
		if (Input.IsActionJustReleased(Inputs.MOVE_JUMP) || player.Velocity.Y <= 0)
			player.ChangeStateTo(PlayerStates.Fall);
	}

	public override void Update(Player player, double delta)
	{
		/*
		// TODO ANDRE: still floaty, need to boost upwards acceleration
		var move = player.MoveDirection * player.MoveInput.Length();
		
		player.Velocity += player.GetGravity() * player.JumpGravity * (float)delta;

		player.UpdateVelocity(move, player.BaseSpeed);

		player.MoveAndSlide();
		player.TurnTo(move);		
		*/

		var velocity = player.Velocity;

		velocity.Y += (float)(player.JumpGravity * delta);
		player.Velocity = velocity;
	}
}
