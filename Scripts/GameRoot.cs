using Godot;
using System;

public partial class GameRoot : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetTree().Root.CallDeferred("add_child", Globals.Overworld);
		// GetTree().Root.AddChild(Globals.Overworld);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
