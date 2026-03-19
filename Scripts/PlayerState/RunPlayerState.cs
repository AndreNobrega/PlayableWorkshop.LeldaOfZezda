using Godot;
using System;

public partial class RunPlayerState : PlayerStateBase
{
	public override void Enter(Player player)
	{
		player.AnimTree.Set(Player.MOVEMENT_TRANSITION_REQUEST, "run");
	}

	public override void DetermineNextState(Player player)
	{

		if (player.Velocity.Length() > player.RunSpeedTreshold)
			return;

		else if (player.Velocity.Length() > 0)
			player.ChangeStateTo(PlayerStates.Walk);
		
		else if (player.Velocity == Vector3.Zero)
			player.ChangeStateTo(PlayerStates.Idle);
	}

	public override void Update(Player player, double delta)
	{
		var lean = player.Direction.Dot(player.GlobalBasis.X);
		player.LeanLerp = Mathf.Lerp(player.LeanLerp, lean, 0.3f);
		player.AnimTree.Set("parameters/run_lean/add_amount", player.LeanLerp);
	}
}
