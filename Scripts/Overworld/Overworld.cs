using Godot;
using System;
using System.Diagnostics;

public partial class Overworld : Node
{
	private bool ReadyCompleted = false;
	public static Node OverworldPhantomCamera;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var PhantomScript = GD.Load<GDScript>("res://addons/phantom_camera/scripts/phantom_camera/phantom_camera_2D.gd");
		OverworldPhantomCamera = (GodotObject)PhantomScript.New() as Node;

		ReadyCompleted = true;
		Initialize();
	}


	public void ReInitialize()
	{		
		if (ReadyCompleted)
			Initialize();	
	}  

	public async void Initialize()
	{
		// Get the location where the character will spawn
		// var SpawnObject = GetNode<Node2D>($"%{Globals.OverworldSpawnNode}");

		var Leader = DatabaseHandler.GetPartyLeader();
		GD.Print($"Party Leader: {Leader.Name}");
		var LeadCharacter = GD.Load<PackedScene>($"res://Scenes/Characters/{Leader.Name}.tscn").Instantiate();
		
		// This will have been disabled going into a battle
		LeadCharacter.GetNode<AnimationTree>("AnimationTree").Active = true;


		// Spawn the character according to the previous game state **************
		// If we were just in battle, spawn at the stored location
		if (Globals.ReturningFromBattle)
		{
			(LeadCharacter as Node2D).GlobalPosition = Globals.OverworldPosition;
			Globals.ReturningFromBattle = false;
		}
		else
		{
			// (LeadCharacter as Node2D).GlobalPosition = SpawnObject.GlobalPosition;
			(LeadCharacter as Node2D).GlobalPosition = Globals.OverworldPosition;
		}


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
		
		CallDeferred("add_child", LeadCharacter);
		await ToSignal(LeadCharacter, "tree_entered");

		GameRoot.Instance.AddPhantomCamera(this, LeadCharacter, null);
	}


	
}
