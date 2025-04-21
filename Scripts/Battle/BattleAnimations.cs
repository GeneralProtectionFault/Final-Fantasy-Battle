using Godot;
using System;
using System.IO;

public partial class BattleAnimations : Node
{
	// private static PackedScene WeaponScene;
	private static Character ActiveCharacter;
	
	// From database - update stats!
	private static Character CharacterTarget;
	private static Enemy EnemyTarget;

	private static Node2D CharacterObj;
	private static Node2D TargetObj;




	#region CharacterBattleAnimation

	// public static void FightAnimation()
	// {
	// 	string Weapon = ActiveCharacter.RightHandEquipped;
	// 	var WeaponSpawnPoint = CharacterObj.GetNode<Node2D>("Sprite2D/WeaponOrigin");

	// 	// Need to have action for bare hands if nothing equipped and all that jazz
	// 	if (Weapon != null)
	// 	{
	// 		var WeaponScene = GD.Load<PackedScene>($"res://Scenes/Weapons/{Weapon}.tscn");
	// 		WeaponSpawnPoint.AddChild(WeaponScene.Instantiate());

	// 		// Add slash through target
	// 		var WeaponAttackScene = GD.Load<PackedScene>($"res://Scenes/Weapons/{Weapon}_Attack.tscn").Instantiate();
	// 		TargetObj.AddChild(WeaponAttackScene);
	// 	}

	// 	// handle bare-handed attack, et...

	// }


	// /// <summary>
    // /// Public method to trigger the damage text to populate
    // /// </summary>
    // public static void PopulateDamageText(Node2D Target, int DamageAmount)
    // {
    //     DamageText(Target, DamageAmount);

	// 	if (Target is Character)
	// 		UpdateCharacterDamage();
	// 	else if (Target is Enemy)
	// 		DamageEnemy();

    //     // Enemy is attacking character
    //     else // This doesn't fire off the event back to the BattleController, which would re-enable the fight menu, etc...
    //     {
    //         GD.Print("Chipping the HP off the character");
    //         var CurrentHP = DatabaseHandler.GetCharacterStatAsString(Target.Name, "Hp");
    //         var HP = CurrentHP.ToInt() - DamageAmount;

    //         // Reset variable
    //         DamageAmount = 0;

    //         Target.Hp = HP;
    //         DatabaseHandler.UpdateCharacter(Target as Character);
    //     }
    // }


	
	// public static void TriggerFightAnimation(Character Character, Node2D CharacterObject, Enemy Target, Node2D TargetObject)
	// {
	// 	// TODO::Need check for Genji Glove, Offering, Gauntlet...
	// 	ActiveCharacter = Character;
	// 	CharacterObj = CharacterObject;
	// 	CharacterTarget = null;
	// 	EnemyTarget = Target;
	// 	TargetObj = TargetObject;

	// 	// Play character's animation, which is the little hop forward.
	// 	// The weapon will instantiate on the positional node on the player and play its own animation
	// 	var Player = CharacterObj.GetNode<AnimationPlayer>("Battle_AnimationPlayer");
	// 	Player.Play("Attack");
	// }


	// public static void TriggerFightAnimation(Character Character, Node2D CharacterObject, Character Target, Node2D TargetObject)
	// {
	// 	// TODO::Need check for Genji Glove, Offering, Gauntlet...

	// 	ActiveCharacter = Character;
	// 	CharacterObj = CharacterObject;
	// 	EnemyTarget = null;
	// 	CharacterTarget = Target;
	// 	TargetObj = TargetObject;

	// 	// Play character's animation, which is the little hop forward.
	// 	// The weapon will instantiate on the positional node on the player and play its own animation
	// 	var Player = CharacterObj.GetNode<AnimationPlayer>("Battle_AnimationPlayer");
	// 	Player.Play("Attack");
	// }

	#endregion


	#region EnemyBattleAnimation

	public static void SpawnEnemyAttackEffect(Node2D Target, Ability Attack)
	{
		var ScenePath = "res://Scenes/AttackEffects";
		var AttackEffectScene = GD.Load<PackedScene>(Path.Combine(ScenePath, $"{Attack.Name}.tscn")).Instantiate();
		Target.AddChild(AttackEffectScene);
	}

	#endregion




}
