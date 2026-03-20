using Godot;
using System;

public partial class FallPlayerState : PlayerStateBase
{
	public override void Enter(Player player)
	{
		player.AnimTree.Set(Player.MOVEMENT_TRANSITION_REQUEST, "fall");
	}

	public override void DetermineNextState(Player player)
	{
		if (player.IsOnFloor())
			player.ChangeStateTo(PlayerStates.Idle);
	}

	public override void Update(Player player, double delta)
	{
		var move = player.MoveDirection * player.MoveInput.Length();

		player.Velocity += player.GetGravity() * (float)delta;
		player.UpdateVelocity(move, player.BaseSpeed * 0.25f); // Give the character 25% movement control in the air
		player.MoveAndSlide();
		player.TurnTo(move);
	}
}
