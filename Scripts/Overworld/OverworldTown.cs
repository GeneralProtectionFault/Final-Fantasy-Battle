using Godot;
using System;
using System.Diagnostics;

public partial class OverworldTown : Node
{
    [Export]
    public NodePath TownCollider;

    public void EnterTown(Node2D EnteringBody)
    {
        Globals.GameState = Enums.GameState.Town;
        var TownSceneFile = $"res://Scenes/Towns/{this.Name}.tscn";
        Node TownScene = ResourceLoader.Load<PackedScene>(TownSceneFile).Instantiate();

        // Set the position to the town so this is where the character reappears upon exit
        Globals.OverworldPosition = EnteringBody.GlobalPosition + new Vector2(0,-35); // Offset to make sure we don't immediately re-enter the town when exiting

        // GetTree().Root.AddChild(TownScene);
        GameRoot.Instance.RemoveOverworldScene();
        
        // Load the town
        GetTree().Root.CallDeferred("add_child", TownScene);
    }

}
