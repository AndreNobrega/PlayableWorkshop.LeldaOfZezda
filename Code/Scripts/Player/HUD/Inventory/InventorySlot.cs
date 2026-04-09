using Godot;
using System;

public partial class InventorySlot : Button
{
	public InventoryItem Item { get; set; }
	public int Quantity { get; set; }
	public TextureRect ItemIcon { get; set; }
	public Label QuantityText { get; set; }
	public Inventory Inventory { get; set; }
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ItemIcon = GetNode<TextureRect>("ItemIcon");
		QuantityText = GetNode<Label>("QuantityText");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetItem(InventoryItem item)
	{
		throw new NotImplementedException();
	}

	public void AddItem(InventoryItem item)
	{
		var slot = GetItemSlot(item);

		if (slot == null)
			return;

		
	}

	public void RemoveItem()
	{
		throw new NotImplementedException();
	}

	public void UpdateQuantityText()
	{
		throw new NotImplementedException();
	}

	private InventorySlot GetItemSlot(InventoryItem item)
	{
		throw new NotImplementedException();
	}
}
