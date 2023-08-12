using System;
using Godot;

[GlobalClass]
public partial class AudioStreamPlayerIntro : AudioStreamPlayer
{
	[Export]
	public float IntroSeconds { get; set; }
	[Export]
	public float TrimEndSeconds { get; set; }
	[Export]
	public bool HasIntro { get; set; }


	private bool FirstPlay = true; // Use this to flag if we've looped yet
	
	private float Length;
	private float End;
	private float PlaybackPosition;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		
		// Playback = this.GetStreamPlayback();
		Length = (float)this.Stream.GetLength();
		End = Length - TrimEndSeconds;

		GD.Print($"Intro Seconds: {IntroSeconds}");
		GD.Print($"Length: {Length}");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);

		if (HasIntro)
		{
			if (this.Playing)
			{
				PlaybackPosition = this.GetPlaybackPosition();

				// GD.Print($"Playback position: {this.GetPlaybackPosition()}");

				if (FirstPlay && PlaybackPosition >= IntroSeconds)
					FirstPlay = false;

				// Start from desired position
				if (!FirstPlay && 
				(PlaybackPosition < IntroSeconds || Math.Abs(PlaybackPosition - End)  < .05))
				{
					GD.Print($"Looping at {GetPlaybackPosition()} seconds.");
					this.Seek(IntroSeconds);
				}

			}
		}
	}

	
}
