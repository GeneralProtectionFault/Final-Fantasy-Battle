using Godot;
using System;

public partial class Overworld : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get the location where the character will spawn
		var SpawnObject = GetNode<Node2D>($"%{Globals.OverworldSpawnNode}");

		var Leader = DatabaseHandler.GetPartyLeader();
		GD.Print($"Party Leader: {Leader.Name}");
		var LeadCharacter = GD.Load<PackedScene>($"res://Scenes/Characters/{Leader.Name}.tscn").Instantiate();
		
		(LeadCharacter as Node2D).GlobalPosition = SpawnObject.GlobalPosition;

		var Camera = new Camera2D();
		Camera.Name = "Camera2D";
		LeadCharacter.AddChild(Camera);


		// Add the black image as a child of the character.
		// It will be used to darken/fade the screen upon entering a battle.
		var FadeoutSprite = new Sprite2D();
		var FadeoutImage = ResourceLoader.Load("res://Graphics/Black.png");
		FadeoutSprite.Texture = FadeoutImage as Texture2D;
		// Name important - called from character node when doing fade-out entering battle
		FadeoutSprite.Name = "Black";
		// Start transparent (changes on the fade)
		FadeoutSprite.Set("modulate", new Color(1f,1f,1f,0f));

		LeadCharacter.AddChild(FadeoutSprite);


		AddChild(LeadCharacter);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
