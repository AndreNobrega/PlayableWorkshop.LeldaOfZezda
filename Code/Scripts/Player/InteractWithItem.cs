using Godot;
using System;

public partial class InteractWithItem : Node
{
	public Color Modulate { get; private set; }
	private MeshInstance3D _myMesh;
	private Label3D _myLabel;
	private Color _originalColor;
	AnimationPlayer _animPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitMesh();
		InitLabel();
		InitAnimationPlayer();
	}

    private void InitMesh()
    {
        		// 1. Automatically find the MeshInstance3D child (no dragging needed)
		// This assumes your Mesh is a direct child of this node.
		_myMesh = GetNode<MeshInstance3D>("MeshInstance3D");
		// 2. Store the original color so we can go back to it later
		if (_myMesh.GetActiveMaterial(0) is StandardMaterial3D mat)
		{
			_originalColor = mat.AlbedoColor;
		}

    }

    private void InitLabel()
    {
        _myLabel = GetNode<Label3D>("Label3D");
		_myLabel.Visible = false; // Hide the label by default
		_myLabel.Text = "Press E to interact"; // Set the label text
    }

    private void InitAnimationPlayer()
    {
		_animPlayer = GetNode<AnimationPlayer>("../AnimationPlayer");
       if (_animPlayer != null && _animPlayer.HasAnimation("consume"))
		{
		_animPlayer.AnimationFinished += (StringName animName) =>
			{
				if (animName == "consume")
				{
					GetParent().QueueFree(); // Remove the item from the scene after the animation finishes
				}
			};
		}
    }

    public override void _Process(double delta)
	{
		var camera = GetViewport().GetCamera3D();
		if (camera != null)
		{
			// Point the back of the node at the camera
			_myLabel.LookAt(camera.GlobalTransform.Origin);
			
			// Flip it 180 degrees so the text faces the camera correctly
			_myLabel.RotateObjectLocal(Vector3.Up, Mathf.Pi); 
		}

		//handle interaction input

		if (Input.IsActionJustPressed(Inputs.INTERACT) && _myLabel.Visible)
		{
			OnInteracted();
		}

	}

	public void Highlight()
	{

	   SetColor(new Color(1, 0, 0));
	   ShowLabel();
	}

	private void ShowLabel()
	{
		_myLabel.Visible = true;
	}

		private void HideLabel()
	{
		_myLabel.Visible = false;
	}

	private bool SetColor(Color color)
	{
		if (_myMesh == null)
		{
			return false;
		}

		// Create a unique material for THIS instance only
		StandardMaterial3D highlightMat = new()
		{
			AlbedoColor = color // Red
		};

		// This override ONLY affects this specific cube in the scene
		_myMesh.SetSurfaceOverrideMaterial(0, highlightMat);
		return true;
	}

	public void Dehighlight()
	{
		if (_myMesh == null) return;

		// Removing the override automatically snaps it back to its original color
		_myMesh.SetSurfaceOverrideMaterial(0, null);

		HideLabel();
	}

	public void OnInteracted()
	{
		GD.Print("Item interacted with!");
		// You can add custom interaction logic here, like opening a door, picking up an item, etc.
		
		if (_animPlayer != null && _animPlayer.HasAnimation("consume") && !_animPlayer.IsPlaying())
		{
			_animPlayer.Play("consume");
		}
	}
}
