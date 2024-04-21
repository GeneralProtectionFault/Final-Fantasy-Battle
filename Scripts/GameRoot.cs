using Godot;
using System;
using System.Diagnostics;

public partial class GameRoot : Node
{
	public static GameRoot Instance;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Singleton 
		if (Instance is null)
			Instance = this;
		else
			QueueFree();



		Globals.GameState = Enums.GameState.Overworld;

		// Set overworld start position
		// Near Jidoor
		// Globals.OverworldPosition = new Vector2(2300,12400);
		// Near Narshe
		Globals.OverworldPosition = new Vector2(8000,3500);
		GetTree().Root.CallDeferred("add_child", Globals.Overworld);
	}




	public void AddOverworldScene()
	{
		GetTree().Root.CallDeferred("add_child", Globals.Overworld);
		Globals.GameState = Enums.GameState.Overworld;
	}


	public void RemoveOverworldScene()
    {
        // var OverworldScene = GetTree().Root.GetNode("Overworld");
		var Characters = GetTree().GetNodesInGroup("Characters");
        
        // Remove added objects
        // Since the scene will remain in memory, the existing code to add the lead character to the scene
        // would create duplicates
        // foreach loop is to maintain flexibility as before, in case there was desire to have multiple characters on screen
        foreach(var Character in Characters)
		{
			Globals.Overworld.CallDeferred("remove_child", Character);
		}

        // GetTree().Root.RemoveChild(OverworldScene);
		// The PhantomCamera is stored in this static variable so it can be removed here.
		// It is not a child of the lead character, so it will not get removed otherwise (w/ removal of characters just done above in foreach loop).
		// This is necessary since the Overworld scene is being stored in memory, and the camera is added along w/ the character(s),
		// so we'd end up with multiple if not removing it when *leaving* the overworld
		Globals.Overworld.CallDeferred("remove_child", Overworld.OverworldPhantomCamera);

		// Get the Overworld camera so we can remove the host
		var Camera = Globals.Overworld.GetNode<Camera2D>("Camera2D");
		//Camera.CallDeferred("remove_child", Overworld.OverworldPhantomCameraHost);
		Overworld.OverworldPhantomCameraHost.CallDeferred("free");

		Overworld.OverworldPhantomCamera.CallDeferred("free");
		GetTree().Root.CallDeferred("remove_child", Globals.Overworld);
    }


	public GodotObject AddPhantomCameraHost(Camera2D Camera)
	{
		// Add the host to the camera
		var PhantomHostScript = GD.Load<GDScript>("res://addons/phantom_camera/scripts/phantom_camera_host/phantom_camera_host.gd");
		var PhantomCameraHost = (GodotObject)PhantomHostScript.New() as Node;
		Camera.CallDeferred("add_child", PhantomCameraHost);

		return PhantomCameraHost;
	}


	public GodotObject AddPhantomCamera(Node Parent, Node Target, Node2D LimitTarget)
	{
		// Set the camera zoom to default, because it will remain zoomed in if after a battle
		var Camera = GetViewport().GetCamera2D();
		Debug.WriteLine($"Camera: {Camera}");
		Camera.Zoom = new Vector2(1f,1f);
		Camera.GlobalPosition = (Target as Node2D).GlobalPosition;	

		var PhantomScript = GD.Load<GDScript>("res://addons/phantom_camera/scripts/phantom_camera/phantom_camera_2D.gd");
		var PhantomCamera = (GodotObject)PhantomScript.New() as Node;

		var AddPhantomCamera = Callable.From(() => {
			
			PhantomCamera.Set("follow_mode", 1); // 1 - GLUED, 2 - SIMPLE
			PhantomCamera.Set("pixel_perfect", true);
			PhantomCamera.Set("tween_on_load", false);
			
			// For setting boundaries to keep the camera from showing anything outside the background, for example.
			if (LimitTarget is not null)
			{
				// Debug.WriteLine($"Setting limit target: {LimitTarget}");
				PhantomCamera.Call("set_limit_node", LimitTarget);
			}

			// Set the target - MUST be after adding the camera to the tree, or GetPathTo will not work 
			// because Godot checks that both nodes have a common parent.
			Parent.AddChild(PhantomCamera);
			var RelativePath = PhantomCamera.GetPathTo(Target);
			PhantomCamera.Set("follow_target", RelativePath);	
		});

		AddPhantomCamera.CallDeferred();
		return PhantomCamera;
	}


}
