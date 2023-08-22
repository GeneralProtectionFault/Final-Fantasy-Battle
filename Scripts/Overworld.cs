using Godot;
using System;

public partial class Overworld : Node
{
	private static bool FirstLoad = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Initialize();
		FirstLoad = false;
	}


	public void ReInitialize()
	{
		if (!FirstLoad)
			Initialize();
	} 

	public void Initialize()
	{
		GD.Print("Overworld method running...");

		// Get the location where the character will spawn
		var SpawnObject = GetNode<Node2D>($"%{Globals.OverworldSpawnNode}");

		var Leader = DatabaseHandler.GetPartyLeader();
		GD.Print($"Party Leader: {Leader.Name}");
		var LeadCharacter = GD.Load<PackedScene>($"res://Scenes/Characters/{Leader.Name}.tscn").Instantiate();
		
		// This will have been disabled going into a battle
		LeadCharacter.GetNode<AnimationTree>("AnimationTree").Active = true;


		// Spawn the character according to the previous game state **************
		// If we were just in battle, spawn at the stored location
		if (Globals.BattleStates.Contains(Globals.GameState))
			(LeadCharacter as Node2D).GlobalPosition = Globals.EnteredBattlePosition;
		else
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

	
}
