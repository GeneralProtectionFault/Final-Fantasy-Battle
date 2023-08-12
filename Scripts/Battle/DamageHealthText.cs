using Godot;
using System;

public partial class DamageHealthText : Node
{
	
	public int Amount { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		var DamageObject = GetNode<Control>("Control");
		var DamageText = DamageObject.GetNode<Label>("Label");
		DamageText.Text = Amount.ToString();


		GD.Print($"Dmg Obj: {DamageObject}");
		GD.Print($"Dmg Obj Position: {DamageObject.Position}");

		Vector2 OriginalPosition = DamageObject.Position;
        Vector2 HighApex = DamageObject.Position - new Vector2(0, 85);
		Vector2 LowApex = DamageObject.Position - new Vector2(0,25);
		
		Tween BounceTween = GetTree().CreateTween();//.SetParallel(true);
        BounceTween.TweenProperty(DamageObject, "position", HighApex, .2f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);
		BounceTween.Chain().TweenProperty(DamageObject, "position", OriginalPosition, .23f);
		BounceTween.Chain().TweenProperty(DamageObject, "position", LowApex, .12f).SetEase(Tween.EaseType.Out);
		BounceTween.Chain().TweenProperty(DamageObject, "position", OriginalPosition, .12f).SetTrans(Tween.TransitionType.Bounce).SetEase(Tween.EaseType.In);

		BounceTween.TweenCallback(Callable.From(() => this.QueueFree())).SetDelay(1f);
	}


}
