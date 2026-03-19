using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	public float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	private Camera3D Camera;
	private AnimationPlayer AnimPlayer;
	private AnimationTree AnimTree;
	private float _leanLerp;

	public override void _Ready()
	{
		Camera = GetNode<Camera3D>("SpringArm3D/Camera3D");
		AnimPlayer = GetNode<AnimationPlayer>("Mesh/AnimationPlayer");
		AnimTree = GetNode<AnimationTree>("AnimationTree");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed(Inputs.MOVE_JUMP) && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		Vector2 inputDir = Input.GetVector(Inputs.MOVE_LEFT, Inputs.MOVE_RIGHT, Inputs.MOVE_FORWARD, Inputs.MOVE_BACK);
		Vector3 direction = Camera.GlobalBasis * new Vector3(inputDir.X, 0, inputDir.Y);
		// Remove the Y axis taken from the camera, otherwize Lunk slows down when looking downwards
		// then multiply by how far the analog stick is pushed
		direction = new Vector3(direction.X, 0, direction.Z).Normalized() * inputDir.Length();

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
		TurnTo(direction);

		/* Basic animimations using the Animation player */
		/*
		// Set animation based on velocity
		var currentSpeed = Velocity.Length(); // Max = custom Speed property of this class
		const float runSpeedTreshold = 4.5f;
		const float blendSpeed = 0.2f;
		if (currentSpeed > runSpeedTreshold)
		{
			AnimPlayer.Play(
				name: Animations.Freehand_Run, 
				customBlend: blendSpeed);
		}
		else if (currentSpeed > 0)
		{
			AnimPlayer.Play(
				name: Animations.Freehand_Walk, 
				customBlend: blendSpeed,
				customSpeed: Mathf.Lerp(0.5f, 1.75f, currentSpeed/runSpeedTreshold)); // set walk speed animation between 50% and 125%, weighted by how close you are to full running speed
		}
		else
		{
			AnimPlayer.Play(
				name: Animations.Freehand_Idle,
				customBlend: blendSpeed);
		}
		*/

		/* Complex animations using the AnimationTree */
		// Set animation based on velocity
		var currentSpeed = Velocity.Length(); // Max = custom Speed property of this class
		const float runSpeedTreshold = 4.5f;
		// const float blendSpeed = 0.2f;

		const string movementTransitionRequest = "parameters/movement/transition_request"; //  "movement" is taken from the Transition node in the AnimationTree graph (case-sensitive!)

		// animation names ("run", "walk", "idle") are taken from the AnimationTree graph (case-sensitive!)
		if (!IsOnFloor())
		{
			AnimTree.Set(movementTransitionRequest, "fall");
		}
		else if (currentSpeed > runSpeedTreshold)
		{
			var lean = direction.Dot(GlobalBasis.X);
			_leanLerp = Mathf.Lerp(_leanLerp, lean, 0.3f);
			AnimTree.Set("parameters/run_lean/add_amount", _leanLerp);
			AnimTree.Set(movementTransitionRequest, "run");
		}
		else if (currentSpeed > 0)
		{
			var walkSpeed = Mathf.Lerp(0.5, 1.75, currentSpeed / Speed);
			AnimTree.Set("parameters/walk_timescale/scale", walkSpeed);
			AnimTree.Set(movementTransitionRequest, "walk");
		}
		else
		{
			AnimTree.Set(movementTransitionRequest, "idle");
		}
	}

	public void TurnTo(Vector3 direction)
	{
		// If there is no directional input, do nothing, otherwise Lunk's orientation will reset.
		if (direction == Vector3.Zero)
			return;

		var yaw = (float)Math.Atan2(-direction.X, -direction.Z); // convert direction into rotation
		yaw = (float)Mathf.LerpAngle(Rotation.Y, yaw, 0.25); // interpolate, so Lunk doesn't snap between angles when using WASD
		Rotation = new Vector3(Rotation.X, yaw, Rotation.Z);
	}
}
