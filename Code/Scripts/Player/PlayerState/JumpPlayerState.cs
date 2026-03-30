using Godot;
using System;

public partial class JumpPlayerState : PlayerStateBase
{
	public override void Enter(Player player)
    {
        player.SetAnimation(PlayerAnimations.jump_start);

		player.StartJumpHeight = player.GlobalPosition.Y;
		player.CurrentJumpHeight = 0f;

		// Putting thess calculations here instead of the Player _Ready() method,
        // so we can tweak the numbers whilst live testing, instead of it being set on startup.
        player.CalculateJumpSpeed();
		player.CalculateJumpGravity();
		player.CalculateHorizontalJumpSpeed();

        var velocity = player.Velocity;
        velocity.Y = player.JumpSpeed;
        player.Velocity = velocity;
    }
    
    public override void DetermineNextState(Player player)
	{
		if (player.Velocity.Y <= 0)
			player.ChangeStateTo(PlayerStates.Fall);
	}

	public override void Update(Player player, double delta)
	{
		player.UpdateVerticalTrajectory(delta);
	}
}
