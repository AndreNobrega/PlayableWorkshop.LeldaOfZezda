using Godot;
using System;

public partial class InventoryItem : RefCounted
{
    public string Name { get; set; }
    public bool IsStackable { get; set; }
}