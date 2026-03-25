using Godot;
using System;

public partial class JumpPlayerState : PlayerStateBase
{
	public override void Enter(Player player)
	{
		player.SetAnimation(PlayerAnimations.jump_start);
		player.Velocity = new Vector3(
			player.Velocity.X, 
			//(player.Velocity.Y + player.JumpHeight) * player.JumpImpulseMult, 
			player.JumpHeight * player.JumpImpulseMult,
			player.Velocity.Z);
	}

	public override void DetermineNextState(Player player)
	{
		if (player.Velocity.Y == 0)
			player.ChangeStateTo(PlayerStates.Fall);
	}

	public override void Update(Player player, double delta)
	{
		var move = player.MoveDirection * player.MoveInput.Length();

		player.Velocity += player.GetGravity() * (float)delta;
		//player.UpdateVelocity(move, player.BaseSpeed * 0.25f); // Give the character 25% movement control in the air
		player.UpdateVelocity(move, player.BaseSpeed * player.AirControlDegree); // Give the character 25% movement control in the air
		player.MoveAndSlide();
		player.TurnTo(move);
		
	}
}
