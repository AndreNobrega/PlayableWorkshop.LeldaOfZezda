using Godot;
using System;

public partial class FallPlayerState : PlayerStateBase
{
	public override void Enter(Player player)
	{
		player.SetAnimation(PlayerAnimations.fall);
	}

	public override void DetermineNextState(Player player)
	{
		if (player.IsOnFloor())
			player.ChangeStateTo(PlayerStates.Idle);
	}

	public override void Update(Player player, double delta)
	{
		var velocity = player.Velocity;
		
		// Add fall speed to the player's velocity...
		velocity += player.GetGravity() * player.FallGravity * (float)delta;
		// ... and cap their fall speed, without influencing horizontal movement.
		velocity.Y = Math.Max(velocity.Y, player.MaxFallSpeed);
		
		player.Velocity = velocity;

		var move = player.MoveDirection * player.MoveInput.Length();
		//player.UpdateVelocity(move, player.BaseSpeed * 0.25f); // Give the character 25% movement control in the air
		//player.UpdateVelocity(move, player.BaseSpeed); 
		player.MoveAndSlide();
		player.TurnTo(move);
	}
}
