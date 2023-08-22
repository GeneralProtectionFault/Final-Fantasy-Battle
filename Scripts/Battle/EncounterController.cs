using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class EncounterController : Node
{
    public static bool IsWalking = false;
    public static int CounterIncrement = 192;
    private static int Counter = 0;


    private static Random RandomGenerator = new Random();
    private static int RandomNumber = 0;

    private static double SecondsCounter = 0;




    private static AudioStreamPlayer RandomEncounterSound;


    public override void _Ready()
    {
        RandomEncounterSound = new AudioStreamPlayer();
        AddChild(RandomEncounterSound);
        RandomEncounterSound.Stream = ResourceLoader.Load("res://Audio/random_encounter.mp3") as AudioStream;

    }


    public override void _Process(double delta)
    {
        // base._Process(delta);
        
        SecondsCounter += delta;

        // 0.2 seconds is akin to a frame of the walk animations created
        // Use 0.1 for Sprint Shoes :)
        if (IsWalking && SecondsCounter >= .2)
        {
            SecondsCounter = 0;
            Counter += CounterIncrement;
            
            RandomNumber = RandomGenerator.Next(0,256);
            // GD.Print($"Random Number: {RandomNumber}");

            // *** FIGHT! *** 
            if (RandomNumber < (Counter / 256) && !Globals.InBattle)
            {
                GD.Print("FIGHT!");
                Globals.InBattle = true;
                Counter = 0;

                // Get all of the battle area nodes (polygons) 
                var AreaNodes = GetTree().GetNodesInGroup("EncounterAreas");
                string BattleArea = "";

                foreach(var Area in AreaNodes)
                {
                    // Check if the Area is overlapping any CHARACTER bodies
                    // GetType returns "CharacterController" here, which is the script name...meh.
                    var OverlappingBodies = (Area as Area2D).GetOverlappingBodies();
                    
                    // Loop in case the character is touching 2 areas...
                    // foreach(var OverlappingBody in OverlappingBodies)
                    // {
                    //     GD.Print($"{Area.Name} is overlapping: {OverlappingBody.Name}");
                    //     GD.Print($"Type: {OverlappingBody.GetType()}");
                    // }

                    // This will be null except for the area that has overlaps :)
                    if (OverlappingBodies.Count > 0)
                        BattleArea = Area.Name;
                }
                

                BattleStart(BattleArea);
                
                // Take the 1st node and pass that to a method to switch to the respective battle scene
                

            }

        }
    }
    


    private void BattleStart(string BattleArea)
    {
        GD.Print($"Battle Area: {BattleArea}");

        // Play sound effect
        RandomEncounterSound.Play();

        // Disable character movement
        Globals.OverworldInputEnabled = false;

        Sprite2D BlackImage = new Sprite2D();
        Sprite2D Sprite = new Sprite2D();

        // Make character(s) invisible
        var Characters = GetTree().GetNodesInGroup("Characters");
        foreach (var Character in Characters)
        {
            // Left as deafult name
            Sprite = Character.GetNode<Sprite2D>("Sprite2D");
            BlackImage = Character.GetNode<Sprite2D>("Black");

            // Make character invisible
            Sprite.Visible = false;
        }

        var Camera = GetViewport().GetCamera2D();
        
        // Store the location battle was entered at for re-entry
        Globals.EnteredBattlePosition = Sprite.GlobalPosition;

        // Camera zoom/shake
        // Putting this in the camera tween causes it to happen instantaneously, epic fail
        // Fade/Dim screen
        Tween FadeTween = GetTree().CreateTween().SetParallel(true);
        FadeTween.TweenProperty(BlackImage, "modulate:a", .7f, .5f).SetDelay(.1f);
        
        Tween CameraTween = GetTree().CreateTween().SetParallel(true);
        CameraTween.TweenProperty(Camera, "zoom", new Vector2(4,4), .2f);
        CameraTween.Chain().TweenProperty(Camera, "zoom", new Vector2(1,1), .2f);
        CameraTween.Chain().TweenProperty(Camera, "zoom", new Vector2(4,4), .2f);

        // Call the scene change after the tween finishes
        CameraTween.TweenCallback(Callable.From(() => 
            LoadBattleScene($"res://Scenes/Battle/{BattleArea}.tscn", Characters)
        )).SetDelay(.15f);
    }



    private void LoadBattleScene(string SceneFile, Godot.Collections.Array<Node> Characters)
    {
        Globals.GameState = Enums.GameState.Battle;
        // GetTree().ChangeSceneToFile(SceneFile);

        Node BattleScene = ResourceLoader.Load<PackedScene>(SceneFile).Instantiate();
        GetTree().Root.AddChild(BattleScene);

        var OverworldScene = GetTree().Root.GetNode("Overworld");
        
        // Remove added objects
        // Since the scene will remain in memory, the existing code to add the lead character to the scene
        // would create duplicates
        // foreach loop is to maintain flexibility as above, in case there was desire to have multiple characters on screen
        foreach(var Character in Characters)
            OverworldScene.RemoveChild(Character);
        
        GetTree().Root.RemoveChild(OverworldScene);
    }
 

    public static void ResetEncounter()
    {
        Counter = 0;
        RandomNumber = 0;
        SecondsCounter = 0;
    }

}
