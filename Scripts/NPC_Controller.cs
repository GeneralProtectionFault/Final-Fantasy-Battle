using System;
using System.Diagnostics;
using Godot;

public partial class NPC_Controller : CharacterBody2D
{
    [Export]
    public int CharacterID;

    private NavigationAgent2D _navigationAgent;

    private float _movementSpeed = 100.0f;
    private Vector2 _movementTargetPosition = new Vector2(0f, 1f);
    private float PathLength = 100.0f;
    private AnimationPlayer AnimationPlayer;
    private bool IsPaused = false;
    private float PauseTimeLimit = 1.5f;
    private double PausedTime = 0f;
    private float PauseLikelihood = .2f;


    public Vector2 MovementTarget
    {
        get { return _navigationAgent.TargetPosition; }
        set { _navigationAgent.TargetPosition = value; }
    }

    public override void _Ready()
    {
        base._Ready();

        _navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");

        // These values need to be adjusted for the actor's speed
        // and the navigation layout.
        _navigationAgent.PathDesiredDistance = 4.0f;
        _navigationAgent.TargetDesiredDistance = 4.0f;

        // Make sure to not await during _Ready.
        Callable.From(ActorSetup).CallDeferred();

        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (_navigationAgent.IsNavigationFinished())
        {
            HandlePause();

            // Set new target for NPC to walk towards
            // The vector will be of magnitude 1 in whatever direction - multiply by desired amount to get how far each random walk goes
            // This could be randomized within a range too :)
            if (!IsPaused)
            {
			    MovementTarget = GlobalPosition + (GetRandomDirection() * PathLength);
            }
            return;
        }

        Vector2 currentAgentPosition = GlobalTransform.Origin;
        Vector2 nextPathPosition = _navigationAgent.GetNextPathPosition();

        Vector2 newVelocity = (nextPathPosition - currentAgentPosition).Normalized();
        newVelocity *= _movementSpeed;

        Velocity = newVelocity;

        MoveAndSlide();
    }


    public override void _Process(double delta)
    {
        base._Process(delta);
        if (IsPaused)
        {
            PausedTime += delta;
            if (PausedTime >= PauseTimeLimit)
            {
                IsPaused = false;
                PausedTime = 0f;
            }
        }
    }




    private async void ActorSetup()
    {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

        // Now that the navigation map is no longer empty, set the movement target.
        MovementTarget = _movementTargetPosition;
    }

	private Vector2 GetRandomDirection()
	{
		Random R = new Random();
		var RValue = R.NextDouble();

		// Derive random direction based on random number between 0 and 1
		// left, right, up, down
		Vector2 Direction = RValue switch
		{
			>= 0f and <= .25f => new Vector2(-1,0),
			> .25f and <= .5f => new Vector2(1,0),
			> .5f and <= .75f => new Vector2(0,-1),
			_ => new Vector2(0,1)
		};

        AnimateInDirection(Direction);

		return Direction;
	}

    private void AnimateInDirection(Vector2 Direction)
    {
        string AnimationName = Direction switch
        {
            { X: -1 } and { Y: 0 } => "WalkLeft",
            { X: 1 } and { Y: 0 }  => "WalkRight",
            { X: 0 } and { Y: 1 } => "WalkDown",
            { X: 0 } and { Y: -1} => "WalkUp",
            _ => "None"
        };

        Debug.Assert(AnimationName != "None");

        AnimationPlayer.Play(AnimationName);
    }

    private void HandlePause()
    {
        if (!IsPaused)
        {
            Random R = new Random();
            var Num = R.NextDouble();

            if (Num < PauseLikelihood)
            {
                IsPaused = true;
                AnimationPlayer.Stop();
            }
        }
    }
}