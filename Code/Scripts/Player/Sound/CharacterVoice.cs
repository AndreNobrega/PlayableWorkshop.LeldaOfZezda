using Godot;
using System;

public partial class CharacterVoice : AudioStreamPlayer3D
{
	Player Player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		Player = GetParent<Player>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed(Inputs.FUCK))
		{
			Play();
		}
	}
}
