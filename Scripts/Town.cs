using Godot;
using System;
using System.Diagnostics;

public partial class Town : Node
{
    public override void _Ready()
    {
        var Leader = DatabaseHandler.GetPartyLeader();
		var LeadCharacter = GD.Load<PackedScene>($"res://Scenes/Characters/{Leader.Name}.tscn").Instantiate();
        CallDeferred("add_child", LeadCharacter);
        (LeadCharacter as Node2D).GlobalPosition = GetNode<Node2D>("TownSpawnNode2D").GlobalPosition;

        // Get background sprite coordinates
        var TownBorder = GetNode<Node2D>("Area2D_Border/CollisionShape2D");

        GameRoot.Instance.AddPhantomCamera(this, LeadCharacter, TownBorder);
    }
}
