using Godot;
using System;

public partial class Footstepper : AudioStreamPlayer3D
{
	private CharacterBody3D Character;

	public override void _Ready()
	{
		base._Ready();
		Character = GetParent<CharacterBody3D>();
	}

	public void PlayFootstep()
	{
		if (!Character.IsOnFloor())
			return;

		// run a check for all collisions
		for (int i = 0; i < Character.GetSlideCollisionCount(); i++)
		{
			var collision = Character.GetSlideCollision(i);

			// check if the collision was with the floor
			if (collision.GetNormal() == Character.GetFloorNormal())
			{
				var collider = collision.GetCollider();

				if (collider is WorldSound)
				{
					Stream = ((WorldSound)collider).FootstepSounds;
					break;
				}
			}
		}

		Play();
	}
}
