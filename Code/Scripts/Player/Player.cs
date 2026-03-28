using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public RayCast3D _interactRay;

	public const string MOVEMENT_TRANSITION_REQUEST = "parameters/movement/transition_request"; //  "movement" is taken from the Transition node in the AnimationTree graph (case-sensitive!)
	public Camera3D Camera;
	public AnimationTree AnimTree;

	public float LeanLerp;

	// The current state the player is in.
	PlayerStateBase _state = PlayerStates.Idle;
	private InteractWithItem _lastInteractedItem;
	private InteractWithItem _lastItem; // Our "memory" variable


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
