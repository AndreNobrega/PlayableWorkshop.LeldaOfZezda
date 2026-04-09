using Godot;
using System;

public partial class InventoryWindow : Panel
{
	public InventorySlot[] InventorySlots;
	private bool IsWindowOpen = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed(Inputs.INVENTORY))
            ToggleWindow();
	}

	public void ToggleWindow()
    {
        if (IsWindowOpen)
            this.Hide();
        else
            this.Show();

        IsWindowOpen = !IsWindowOpen;
    }

    public void GiveItem(InventoryItem item, int quantity)
    {
        throw new NotImplementedException();
    }
}
