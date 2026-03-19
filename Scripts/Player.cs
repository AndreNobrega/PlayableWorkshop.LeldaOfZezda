using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	public float RunSpeed = 5.0f;
	[Export]
	public float RunSpeedTreshold = 4.5f;
	public const float JUMP_VELOCITY = 4.5f;
	public const string MOVEMENT_TRANSITION_REQUEST = "parameters/movement/transition_request"; //  "movement" is taken from the Transition node in the AnimationTree graph (case-sensitive!)
	public Camera3D Camera;
	private AnimationPlayer _animPlayer;
	public AnimationTree AnimTree;
	public Vector3 Direction;
	public float LeanLerp;

	// The current state the player is in.
	PlayerStateBase _state;

	public override void _Ready()
	{
		// Get nodes
		Camera = GetNode<Camera3D>("SpringArm3D/Camera3D");
		_animPlayer = GetNode<AnimationPlayer>("Mesh/AnimationPlayer");
		AnimTree = GetNode<AnimationTree>("AnimationTree");
		
		// Instantiate player state
		_state = PlayerStates.Idle;
		_state.Enter(this);
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
			velocity.Y = Player.JUMP_VELOCITY;
		}

		// Get the input direction and handle the movement/deceleration.
		Vector2 inputDir = Input.GetVector(Inputs.MOVE_LEFT, Inputs.MOVE_RIGHT, Inputs.MOVE_FORWARD, Inputs.MOVE_BACK);
		Vector3 direction = Camera.GlobalBasis * new Vector3(inputDir.X, 0, inputDir.Y);
		// Remove the Y axis taken from the camera, otherwize Lunk slows down when looking downwards
		// then multiply by how far the analog stick is pushed
		direction = new Vector3(direction.X, 0, direction.Z).Normalized() * inputDir.Length();

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * RunSpeed;
			velocity.Z = direction.Z * RunSpeed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, RunSpeed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, RunSpeed);
		}

		Velocity = velocity;
		MoveAndSlide();
		TurnTo(direction);

		_state.DetermineNextState(this);
		_state.Update(this, delta);
	}

	/// <summary>
	/// Turn the player character to face the input direction.
	/// </summary>
	/// <param name="direction">The direction the character should be facing.</param>
	public void TurnTo(Vector3 direction)
	{
		// If there is no directional input, do nothing, otherwise Lunk's orientation will reset.
		if (direction == Vector3.Zero)
			return;

		var yaw = (float)Math.Atan2(-direction.X, -direction.Z); // convert direction into rotation
		yaw = (float)Mathf.LerpAngle(Rotation.Y, yaw, 0.25); // interpolate, so Lunk doesn't snap between angles when using WASD
		Rotation = new Vector3(Rotation.X, yaw, Rotation.Z);
	}

	/// <summary>
	/// Ends the current state, changes it to the next and starts it.
	/// </summary>
	/// <param name="nextState">The state to change to.</param>
	public void ChangeStateTo(PlayerStateBase nextState)
	{
		_state.Exit(this);
		_state = nextState;
		_state.Enter(this);
	}
}
