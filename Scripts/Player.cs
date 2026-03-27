using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[ExportGroup("Movement")]
	[Export]
	public float BaseSpeed = 5.0f;
	[Export]
	public float RunSpeed = 4.5f;
	
	[ExportGroup("Jumping")]
	[Export]
	public float JumpImpulse = 4.5f;
	[Export]
	public float JumpGravity = 0.5f;
	[Export]
	public float FallGravity = 1.6f;
	[Export]
	public float MaxFallSpeed = -45f;

	public RayCast3D _interactRay;

	public const string MOVEMENT_TRANSITION_REQUEST = "parameters/movement/transition_request"; //  "movement" is taken from the Transition node in the AnimationTree graph (case-sensitive!)
	public Camera3D Camera;
	public AnimationTree AnimTree;
	public Vector2 MoveInput = Vector2.Zero;
	public Vector3 MoveDirection = Vector3.Zero;
	public float LeanLerp;

	// The current state the player is in.
	PlayerStateBase _state = PlayerStates.Idle;
	private InteractWithItem _lastInteractedItem;

	public override void _Ready()
	{
		// Get nodes
		Camera = GetNode<Camera3D>("SpringArm3D/Camera3D");
		AnimTree = GetNode<AnimationTree>("AnimationTree");
		_interactRay = GetNode<RayCast3D>("SpringArm3D/Camera3D/RayCast3D");
		
		_state.Enter(this);
	}
	
	// Called every physics frame. Delegates update logic to the current state.
	public override void _PhysicsProcess(double delta)
	{
		// Read movement input and store it on the player.
		MoveInput = Input.GetVector(Inputs.MOVE_LEFT, Inputs.MOVE_RIGHT, Inputs.MOVE_FORWARD, Inputs.MOVE_BACK);

		if (MoveInput != Vector2.Zero)
		// Calculate adjusted movement direction based on camera.
			SetMoveDirection();

		// Delegate to the current state.
		_state.DetermineNextState(this);
		_state.Update(this, delta);
	}

private InteractWithItem _lastItem; // Our "memory" variable

	public override void _Process(double delta)
	{
		// 1. Check what the ray is hitting
		if (_interactRay.IsColliding() && _interactRay.GetCollider() is InteractWithItem currentItem)
		{
			// If we are looking at a NEW item, reset the OLD one first
			if (_lastItem != null && _lastItem != currentItem)
			{
				_lastItem.Dehighlight();
			}

			// Highlight the new item and save it to memory
			currentItem.Highlight();
			_lastItem = currentItem;
		}
		else
		{
			// 2. If the ray hits NOTHING (or a wall), reset the last item and clear memory
			if (_lastItem != null)
			{
				_lastItem.Dehighlight();
				_lastItem = null;
			}
		}
	}
	
	private void SetMoveDirection()
	{
		GD.Print("Move input: " + MoveInput);
		// Get the input direction and handle the movement/deceleration.
		Vector3 direction = Camera.GlobalBasis * new Vector3(MoveInput.X, 0, MoveInput.Y);
		// Remove the Y axis taken from the camera, otherwize Lunk slows down when looking downwards
		// then multiply by how far the analog stick is pushed
		direction = new Vector3(direction.X, 0, direction.Z).Normalized() * MoveInput.Length();

		MoveDirection = direction;
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
	/// Update the player's velocity.
	/// </summary>
	/// <param name="speed">(Optional) The player's speed. If null, defaults to the character's base speed.</param>
	public void UpdateVelocity(Vector3 direction, float? speed = null)
	{
		Vector3 velocity = Velocity;

		if (direction != Vector3.Zero)
		{
			velocity.X = MoveDirection.X * (speed ?? BaseSpeed);
			velocity.Z = MoveDirection.Z * (speed ?? BaseSpeed);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed ?? BaseSpeed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed ?? BaseSpeed);
		}

		Velocity = velocity;
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

	/// <summary>
	/// Transition to a different character animation.
	/// Only works with animations defined in the Transition node of the character's animation tree.
	/// </summary>
	/// <param name="animation"></param>
	public void SetAnimation(PlayerAnimations animation)
	{
		AnimTree.Set(MOVEMENT_TRANSITION_REQUEST, animation.ToString());
	}
}
