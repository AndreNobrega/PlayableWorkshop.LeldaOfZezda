using Godot;
using System;

public partial class SpringArm3d : SpringArm3D
{
	Camera3D Camera => (Camera3D)GetNode("Camera3D");

	[Export]
	float TurnRate = 200;
	[Export]
	float MouseSensitivity = 0.1f;
	private CharacterBody3D Player; 

	private Vector2 _mouseInput;

	private Vector3 _heightCorrection;
	private float _cameraRigHeight;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		Player = (CharacterBody3D)GetParent();
		_mouseInput = new Vector2();
		SpringLength = Camera.Position.Z;
		_cameraRigHeight = Position.Y;
		_heightCorrection = new Vector3(0, Position.Y, 0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var lookInput = Input.GetVector(Inputs.VIEW_RIGHT, Inputs.VIEW_LEFT, Inputs.VIEW_DOWN, Inputs.VIEW_UP);
		
		float yInput = lookInput.Y * TurnRate * (float)delta;
		yInput += _mouseInput.Y;
		
		float xInput = lookInput.X * TurnRate * (float)delta;
		xInput += _mouseInput.X;

		_mouseInput = new Vector2(); // mouse movement stopping does not call _Input, that's why the camera feels fucky and we need to reset _mouseInput

		var rotation = RotationDegrees;
		rotation.X = Mathf.Clamp(rotation.X + yInput, -70f, 50f);
		rotation.Y += xInput;
		RotationDegrees = rotation;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion)
			_mouseInput = ((InputEventMouseMotion)@event).Relative * MouseSensitivity * - 1;
		
		else if (@event is InputEventKey && ((InputEventKey)@event).Keycode == Key.Escape && ((InputEventKey)@event).Pressed)
			Input.MouseMode = Input.MouseModeEnum.Visible;

		else if (@event is InputEventMouseButton && ((InputEventMouseButton)@event).ButtonIndex == MouseButton.Left)
			Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		// Make the camera follow the character, and add _heightCorrection
		// so the camera isn't stuck to their feet (origin point)
		Position = Player.Position + _heightCorrection;
	}
}
