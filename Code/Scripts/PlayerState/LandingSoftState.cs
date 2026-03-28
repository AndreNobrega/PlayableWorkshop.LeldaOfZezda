using Godot;
using System;

public partial class LandingSoftState : PlayerStateBase
{
    public override void Enter(Player player)
    {
        player.SetAnimation(PlayerAnimations.landing_soft);
    }

    public override void DetermineNextState(Player player)
    {

        if (!player.IsOnFloor())
			player.ChangeStateTo(PlayerStates.Fall);
		else if (player.MoveInput != Vector2.Zero)
			player.ChangeStateTo(PlayerStates.Walk);
		else
			player.ChangeStateTo(PlayerStates.Idle);
    }
}
