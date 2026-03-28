using System;
using Godot;

public partial class Player : CharacterBody3D
{
	public Vector2 MoveInput = Vector2.Zero;
	public Vector3 MoveDirection = Vector3.Zero;

    [ExportGroup("Movement")]
	[Export]
	public float BaseSpeed = 6.5f;
	[Export]
	public float RunAnimationTriggerSpeed = 4.5f;

	[ExportGroup("Jumping")]
	[Export]
	public float JumpHeight = 5f;
	[Export]
	public float TimeToPeak = 0.4f;
	public float JumpSpeed { get; private set; }
	public float JumpGravity { get; private set; }
	[Export]
	public float TimeToFall = 0.3f;
	public float FallGravity { get; private set; }
	[Export]
	public float JumpDistance = 5.0f;
	public float HorizontalJumpSpeed { get; private set; }
	[Export]
	public float MaxFallSpeed = 25.0f;
	[Export]
	public float ShortHopGravityMultiplier = 1.5f;
	[Export]
	public float ShortHopVerticalSpeedCap = 2.5f;
    [Export]
    public float ApexBoostThreshold = 1.25f;
    [Export]
    public float ApexGravityMultiplier = 1.5f;

    private void SetMoveDirection()
	{
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
	public void UpdateHorizontalVelocity(Vector3 direction, float? speed = null)
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

    public void CalculateJumpSpeed()
	{
		JumpSpeed = (float)((2.0 * JumpHeight) / TimeToPeak);
	}

	public void CalculateJumpGravity()
	{
		JumpGravity = (float)((-2.0 * JumpHeight) / Math.Pow(TimeToPeak, 2.0));
	}

	public void CalculateFallGravity()
	{
		FallGravity = (float) ((-2.0 * JumpHeight) / Math.Pow(TimeToFall, 2.0));
	}

	public void CalculateHorizontalJumpSpeed()
	{
		HorizontalJumpSpeed = JumpDistance / (TimeToPeak + TimeToFall);
	}

	/// <summary>
	/// Update the player's vertical trajectory when jumping or falling.
	/// Different gravity models are applied to rising after a jump and falling.
	/// If the jump button is released, rising gravity is cut off.
	/// </summary>
	/// <param name="delta"></param>
	public void UpdateVerticalTrajectory(double delta)
	{
		var velocity = Velocity;

		var move = MoveDirection * MoveInput.Length();
		velocity.X = move.X * HorizontalJumpSpeed;
		velocity.Z = move.Z * HorizontalJumpSpeed;
		
		var falling = velocity.Y <= 0.0f;
		var gravity = falling ? FallGravity : JumpGravity;	
		
        var nearApex = !falling && velocity.Y <= ApexBoostThreshold;
        if (nearApex)
            gravity *= ApexGravityMultiplier;
        
		// Short hop
		if (!falling && !Input.IsActionPressed(Inputs.MOVE_JUMP))
		{
			gravity *= ShortHopGravityMultiplier;
			velocity.Y = Math.Min(velocity.Y, ShortHopVerticalSpeedCap);
		}

		// Add gravity
		velocity.Y += gravity * (float) delta;

		// Clamp fall speed for terminal velocity
		velocity.Y = Math.Max(velocity.Y, - Math.Abs(MaxFallSpeed));

		Velocity = velocity;
		MoveAndSlide();
		TurnTo(MoveDirection);
	}
}