using Godot;
using System;

public partial class CharacterController : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = 0;// ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	



	public override void _PhysicsProcess(double delta)
	{
		if (Globals.OverworldInputEnabled)
		{
			Vector2 velocity = Velocity;

			// Add the gravity.
			// if (!IsOnFloor())
			// 	velocity.y += gravity * (float)delta;

			// Handle Jump.
			// if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			// 	velocity.y = JumpVelocity;

			// Get the input direction and handle the movement/deceleration.
			// As good practice, you should replace UI actions with custom gameplay actions.
			Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
			Vector2 NormalizedDirection = direction.Normalized();

			if (direction != Vector2.Zero)
			{
				velocity = NormalizedDirection * Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
				velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
			}
			
			
			// Animation
			var AnimTree = GetNode<AnimationTree>("./AnimationTree");
			var Playback = (AnimationNodeStateMachinePlayback)AnimTree.Get("parameters/playback");

			// GD.Print($"Idle Blend Position: {AnimTree.Get("parameters/Idle_BlendSpace2D/blend_position")}");
			// GD.Print($"Walk Blend Position: {AnimTree.Get("parameters/Walk_BlendSpace2D/blend_position")}");

			// If not moving
			if (NormalizedDirection == Vector2.Zero)
			{
				// If the character is NOT walking, set the variable so the random encounter algorithm can be told to shut it.
				EncounterController.IsWalking = false;

				Playback.Travel("Idle_BlendSpace2D");
			}
			else
			{
				// If the character is walking, set the variable so the random encounter algorithm can do its stuff
				EncounterController.IsWalking = true;

				AnimTree.Set("parameters/Idle_BlendSpace2D/blend_position", NormalizedDirection);
				AnimTree.Set("parameters/Walk_BlendSpace2D/blend_position", NormalizedDirection);
				Playback.Travel("Walk_BlendSpace2D");
			}

			Velocity = velocity;
			MoveAndSlide();

		}
	}



	public void FightAnimation()
	{
		BattleAnimations.FightAnimation();
	}


	
}
