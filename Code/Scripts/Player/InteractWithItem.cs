using Godot;
using System;

public partial class InteractWithItem : Node
{
    public Color Modulate { get; private set; }

    private MeshInstance3D _myMesh;
    private Color _originalColor;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Connect("mouse_entered", new Callable(this, "OnMouseEntered"));
        this.Connect("mouse_exited", new Callable(this, "OnMouseExited"));

        // 1. Automatically find the MeshInstance3D child (no dragging needed)
        // This assumes your Mesh is a direct child of this node.
        _myMesh = GetNode<MeshInstance3D>("MeshInstance3D");

        // 2. Store the original color so we can go back to it later
        var mat = _myMesh.GetActiveMaterial(0) as StandardMaterial3D;
        if (mat != null)
        {
            _originalColor = mat.AlbedoColor;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

    }


    private void OnMouseEntered()
    {
        GD.Print("Mouse entered");
    }

    private void OnMouseExited()
    {
        GD.Print("Mouse exited");
    }

    // Using an Export is safer than GetChild(1)
    [Export] private MeshInstance3D _mesh;
    public void Highlight()
    {
        if (_myMesh == null) return;

        // Create a unique material for THIS instance only
        StandardMaterial3D highlightMat = new StandardMaterial3D();
        highlightMat.AlbedoColor = new Color(1, 0, 0); // Red

        // This override ONLY affects this specific cube in the scene
        _myMesh.SetSurfaceOverrideMaterial(0, highlightMat);
    }

    public void Dehighlight()
    {
        if (_myMesh == null) return;

        // Removing the override automatically snaps it back to its original color
        _myMesh.SetSurfaceOverrideMaterial(0, null);
    }

    public void OnInteracted()
    {
        GD.Print("Item interacted with!");
        // You can add custom interaction logic here, like opening a door, picking up an item, etc.
    }
}
