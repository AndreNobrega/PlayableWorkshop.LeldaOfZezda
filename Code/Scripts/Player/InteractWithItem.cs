using Godot;
using System;

public partial class InteractWithItem : Node
{
	public Color Modulate { get; private set; }

	private MeshInstance3D _myMesh;

	private Label3D _myLabel;
	private Color _originalColor;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		// 1. Automatically find the MeshInstance3D child (no dragging needed)
		// This assumes your Mesh is a direct child of this node.
		_myMesh = GetNode<MeshInstance3D>("MeshInstance3D");
		_myLabel = GetNode<Label3D>("Label3D");
		_myLabel.Visible = false; // Hide the label by default
		_myLabel.Text = "Press E to interact"; // Set the label text

		// 2. Store the original color so we can go back to it later
		var mat = _myMesh.GetActiveMaterial(0) as StandardMaterial3D;
		if (mat != null)
		{
			_originalColor = mat.AlbedoColor;
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
	}
}
