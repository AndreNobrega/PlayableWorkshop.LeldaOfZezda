using Godot;
using System;

public partial class RunPlayerState : PlayerStateBase
{
	public override void Enter(Player player)
	{
		var lean = player.Direction.Dot(player.GlobalBasis.X);
		player.LeanLerp = Mathf.Lerp(player.LeanLerp, lean, 0.3f);
		player.AnimTree.Set("parameters/run_lean/add_amount", player.LeanLerp);
		player.AnimTree.Set(Player.MOVEMENT_TRANSITION_REQUEST, "run");
	}
}
