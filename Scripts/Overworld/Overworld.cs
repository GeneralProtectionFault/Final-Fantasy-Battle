using Godot;
using System;
using System.Diagnostics;

public partial class Overworld : Node2D
{
	private bool ReadyCompleted = false;

	// These camera objects will be stored here so they can be removed using this variable during transitioning to a different scene.
	public static Node OverworldPhantomCamera;
	public static Node OverworldPhantomCameraHost;



	// Get sprite into variable in order to draw at the edges/simulate for wrapping
	private static Sprite2D OverworldSprite;

	private static float UpperLeftX;
	private static float UpperLeftY;
	private static float LowerRightX;
	private static float LowerRightY;
	private static float SpriteWidth;
	private static float SpriteHeight;

	// When warping to the other side of the map, we have to account for the character's movement,
	// which will be the velocity divided by this number (frames per second)
	// (float)Engine.GetFramesPerSecond()
	// ...can be used, but this didn't seem to yield any improvement
	private static float WarpMovementOffset = 60.0f;
	
	// Variables will get used alot...defined here to avoid creating them each time
	private static float NewXPosition = 0f;
	private static float NewYPosition = 0f;
	private static Node2D Character;
	private static Vector2 CharacterVelocity = new Vector2();
	private static Vector2 CharacterPosition = new Vector2();

	[Export] public bool WarpWaitForPhysics = true;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		OverworldSprite = GetNode<Sprite2D>("OverworldSprite");
		Debug.WriteLine($"Sprite position: {OverworldSprite.GlobalPosition}");
		Debug.WriteLine($"Sprite X size:{OverworldSprite.Texture.GetWidth()}");
		Debug.WriteLine($"Sprite Y size:{OverworldSprite.Texture.GetHeight()}");

		ReadyCompleted = true;
		Initialize();
	}

    public override void _Process(double delta)
    {
        WarpMovementOffset = (float)Engine.GetFramesPerSecond();
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
		Globals.GameState = Enums.GameState.Overworld;

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

		OverworldPhantomCameraHost = GameRoot.Instance.AddPhantomCameraHost(GetViewport().GetCamera2D()) as Node;
		OverworldPhantomCamera = GameRoot.Instance.AddPhantomCamera(this, LeadCharacter, null) as Node;
	}


	
	public override void _Draw()
	{	
		SpriteWidth = OverworldSprite.Texture.GetWidth();
		SpriteHeight = OverworldSprite.Texture.GetHeight();

		// Top-left corner of image
		UpperLeftX = OverworldSprite.GlobalPosition.X;
		UpperLeftY = OverworldSprite.GlobalPosition.Y;
		LowerRightX = UpperLeftX + SpriteWidth;
		LowerRightY = UpperLeftY + SpriteHeight;

		// Debug.WriteLine($"Sprite bottom: {LowerRightY}");

		// Top-left corner NW duplicate
		DrawTexture(OverworldSprite.Texture, new Vector2(UpperLeftX - SpriteWidth,  UpperLeftY - SpriteHeight));
		// Top-left corner N duplicate
		DrawTexture(OverworldSprite.Texture, new Vector2(UpperLeftX, UpperLeftY - SpriteHeight));
		// Top-left corner NE duplicate
		DrawTexture(OverworldSprite.Texture, new Vector2(LowerRightX, UpperLeftY - SpriteHeight));
		// Top-left corner W duplicate
		DrawTexture(OverworldSprite.Texture, new Vector2(UpperLeftX - SpriteWidth, UpperLeftY));
		// Top-left corner E duplicate
		DrawTexture(OverworldSprite.Texture, new Vector2(LowerRightX, UpperLeftY));
		// Top-left corner SW duplicate
		DrawTexture(OverworldSprite.Texture, new Vector2(UpperLeftX - SpriteWidth, LowerRightY));
		// Top-left corner S duplicate
		DrawTexture(OverworldSprite.Texture, new Vector2(UpperLeftX, LowerRightY));
		// Top-left corner SE duplicate
		DrawTexture(OverworldSprite.Texture, new Vector2(LowerRightX, LowerRightY));

	}


	public async void WarpCharacterTop(Node2D CharacterNode)
	{
		if (WarpWaitForPhysics)
			await ToSignal(GetTree(), "physics_frame");

		Character = CharacterNode.Owner as Node2D;
		CharacterVelocity = (Character as CharacterBody2D).Velocity;
		CharacterPosition = Character.GlobalPosition;
		NewXPosition = CharacterPosition.X; //+ (CharacterVelocity.X / WarpMovementOffset);
		NewYPosition = LowerRightY + (CharacterVelocity.Y / WarpMovementOffset);

		if (CharacterVelocity.Y < 0)
		{
			Debug.WriteLine($"Character Velocity @ crossing: {CharacterVelocity}");
			Debug.WriteLine($"Character Position: {CharacterNode.GlobalPosition}");
			Debug.WriteLine("Crossed Top Border");
			Character.GlobalPosition = new Vector2(NewXPosition, NewYPosition);
		}
	}

	public async void WarpCharacterBottom(Node2D CharacterNode)
	{	
		if (WarpWaitForPhysics)
			await ToSignal(GetTree(), "physics_frame");

		Character = CharacterNode.Owner as Node2D;
		CharacterVelocity = (Character as CharacterBody2D).Velocity;
		CharacterPosition = Character.GlobalPosition;
		NewXPosition = CharacterPosition.X; // + (CharacterVelocity.X / WarpMovementOffset);
		NewYPosition = UpperLeftY + (CharacterVelocity.Y / WarpMovementOffset);

		if (CharacterVelocity.Y > 0)
		{
			Debug.WriteLine($"Character Velocity @ crossing: {CharacterVelocity}");
			Debug.WriteLine($"Character Position: {CharacterNode.GlobalPosition}");
			Debug.WriteLine("Crossed Bottom Border");
			Character.GlobalPosition = new Vector2(NewXPosition, NewYPosition);
		}
	}

	public async void WarpCharacterLeft(Node2D CharacterNode)
	{	
		if (WarpWaitForPhysics)
			await ToSignal(GetTree(), "physics_frame");

		Character = CharacterNode.Owner as Node2D;
		CharacterVelocity = (Character as CharacterBody2D).Velocity;
		CharacterPosition = Character.GlobalPosition;
		NewXPosition = UpperLeftX + (CharacterVelocity.X / WarpMovementOffset) + SpriteWidth;
		NewYPosition = CharacterPosition.Y; // + (CharacterVelocity.Y / WarpMovementOffset);


		if (CharacterVelocity.X < 0)
		{
			Debug.WriteLine($"Character Velocity @ crossing: {CharacterVelocity}");
			Debug.WriteLine($"Character Position: {CharacterNode.GlobalPosition}");
			Debug.WriteLine("Crossed Left Border");
			Character.GlobalPosition = new Vector2(NewXPosition, NewYPosition);
		}
	}

	public async void WarpCharacterRight(Node2D CharacterNode)
	{
		if (WarpWaitForPhysics)
			await ToSignal(GetTree(), "physics_frame");
		
		Character = CharacterNode.Owner as Node2D;
		CharacterVelocity = (Character as CharacterBody2D).Velocity;
		CharacterPosition = Character.GlobalPosition;
		NewXPosition = UpperLeftX + (CharacterVelocity.X / WarpMovementOffset);
		NewYPosition = CharacterPosition.Y; // + (CharacterVelocity.Y / WarpMovementOffset);

		if (CharacterVelocity.X > 0)
		{
			Debug.WriteLine($"Character Velocity @ crossing: {CharacterVelocity}");
			Debug.WriteLine($"Character Position: {CharacterNode.GlobalPosition}");
			Debug.WriteLine("Crossed Right Border");
			Character.GlobalPosition = new Vector2(NewXPosition, NewYPosition);
		}
	}
}
