using Godot;
using System;
using System.Diagnostics;


public partial class Town : Node
{
    [Export]
    public NodePath Boundary;

    public Node TownPhantomCamera;
    private Node LeadCharacter;

    public override void _Ready()
    {
        var Leader = DatabaseHandler.GetPartyLeader();
		LeadCharacter = GD.Load<PackedScene>($"res://Scenes/Characters/{Leader.Name}.tscn").Instantiate();
        CallDeferred("add_child", LeadCharacter);
        (LeadCharacter as Node2D).GlobalPosition = GetNode<Node2D>("TownSpawnNode2D").GlobalPosition;

        // Get background sprite coordinates
        var TownBorder = GetNode<Node2D>("Area2D_Border/CollisionShape2D");
        var TownExitBorder = GetNode<Node2D>("Area2D_ExitBorder/CollisionShape2D");

        GameRoot.Instance.AddPhantomCameraHost(GetViewport().GetCamera2D());
        TownPhantomCamera = GameRoot.Instance.AddPhantomCamera(this, LeadCharacter, TownBorder) as Node;
    }



    public void LeaveTown(Node2D EnteringBody)
    {
        // TODO:  Segment collider unreliable, replaced w/ rectangle.
        // Create a town dictionary that holds a list of exit directions which are acceptable,
        // so each town will detect the exit in only the acceptable direction.
        // This currently only treats a south exit as valid.
        if ((EnteringBody.Owner as CharacterBody2D).Velocity.Y > 0)
        {
            GameRoot.Instance.AddOverworldScene();
            CallDeferred("free");
        }
    }



    public void ChangeCameraArea(Node2D EnteringBody)
    {
        // Debug.WriteLine("Changing Camera Focus!");

        // Get the current area2d
        var AreaNodes = GetTree().GetNodesInGroup("TownAreas");
        Node2D CurrentArea = new Node2D();

        foreach (var Area in AreaNodes)
        {
            // Exclude the ExitBorder, which we're only using for exiting
            if (Area.Name == "Area2D_ExitBorder")
                continue;


            var OverlappingBodies = (Area as Area2D).GetOverlappingBodies();
            if (OverlappingBodies.Count > 0)
            {
                CurrentArea = Area as Node2D;
                Debug.WriteLine($"Current Area: {CurrentArea}");
            }
        }

        TownPhantomCamera.Call("set_limit_target", CurrentArea.GetNode<CollisionShape2D>("CollisionShape2D").GetPath());
    }
}
