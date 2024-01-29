using Godot;
using System;
using System.Diagnostics;

public partial class OverworldTown : Node
{
    [Export]
    public NodePath TownCollider;

    public void EnterTown(Node2D EnteringBody)
    {
        Debug.WriteLine("Town entered!!");
        Debug.WriteLine($"And that town is: {this.Name}");

        Globals.GameState = Enums.GameState.Town;
        var TownSceneFile = $"res://Scenes/Towns/{this.Name}.tscn";
        Node TownScene = ResourceLoader.Load<PackedScene>(TownSceneFile).Instantiate();

        // GetTree().Root.AddChild(TownScene);
        GameRoot.Instance.RemoveOverworldScene();
        
        // Load the town
        GetTree().Root.CallDeferred("add_child", TownScene);
    }

}
