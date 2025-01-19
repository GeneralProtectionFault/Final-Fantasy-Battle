using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;



public static class BattleAlgorithms
{
    
    public static event EventHandler<Character> DamagingCharacter;
    public static event EventHandler<Enemy> DamagingEnemy;


    // // Store for state updates
    // private static Character TargetCharacter;
    // private static Enemy TargetEnemy;

    // Store the (databse) object to be damaged - will be either a character or an enemy
    private static IBattleEntity Target;
    private static Node2D ObjectToDamage;
    private static int DamageAmount;




    public static void SetDamage(int Value)
    {
        DamageAmount = Value;
    }


    /// <summary>
    /// Public method to trigger the damage text to populate
    /// </summary>
    public static void PopulateDamageText(Enums.BattleTurn Attacker)
    {
        DamageText(ObjectToDamage, DamageAmount);

        if (Attacker == Enums.BattleTurn.Party)
        {
            if (Target is Character)
                UpdateCharacterDamage();
            else if (Target is Enemy)
                DamageEnemy();
        }
        // Enemy is attacking character
        else // This doesn't fire off the event back to the BattleController, which would re-enable the fight menu, etc...
        {
            GD.Print("Chipping the HP off the character");
            var CurrentHP = DatabaseHandler.GetCharacterStatAsString(Target.Name, "Hp");
            var HP = CurrentHP.ToInt() - DamageAmount;

            // Reset variable
            DamageAmount = 0;

            Target.Hp = HP;
            DatabaseHandler.UpdateCharacter(Target as Character);
        }
    }



    public static void SetFightVariables(IBattleEntity TargetDBObject, Node2D TargetObject)
    {
        ObjectToDamage = TargetObject;
        Target = TargetDBObject;
        DamageAmount = 19;
    }



    /// <summary>
    /// Method that actually makes the damage text appear
    /// </summary>
    /// <param name="ParentObject"></param>
    /// <param name="Damage"></param>
    private static void DamageText(Node2D ParentObject, int Damage)
    {
        var DamageText = GD.Load<PackedScene>("res://Scenes/Battle/DamageHealText.tscn").Instantiate();
        (DamageText as DamageHealthText).Amount = Damage;
        ParentObject.AddChild(DamageText);
    }
    


    private static void UpdateCharacterDamage()
    {
        if (Target != null)
        {
            var CurrentHP = DatabaseHandler.GetCharacterStatAsString(Target.Name, "Hp");
            var HP = CurrentHP.ToInt() - DamageAmount;
            DamageAmount = 0;

            Target.Hp = HP;
            DatabaseHandler.UpdateCharacter(Target as Character);

            // Picked up in BattleController.cs
            DamagingCharacter?.Invoke(null, Target as Character);
        }
    }


    private static void DamageEnemy()
    {
        if (Target != null)
        {
            // Damage the enemy object, etc...
            Target.Hp -= DamageAmount;
            DamagingEnemy?.Invoke(null, Target as Enemy);
        }
    }


    /// <summary>
    /// In battle, chooses a member of the party with HP greater than 0
    /// Returns character as godot object (Node2D)
    /// </summary>
    public static Node2D EnemyPickCharacterTarget()
    {
        List<Node2D> ValidCharacterTargets = new();
        foreach (Character Character in BattleController.Characters.Where(x => x.Hp > 0))
        {
            // Get the list index so we can get the Node2D from that list (indices will be the same)
            var Index = BattleController.Characters.FindIndex(x => x == Character);
            ValidCharacterTargets.Add(BattleController.CharacterObjects[Index]);
        }

        // Pick one of the list
        var RCharacter = new Random();
        var PickedIndex = RCharacter.Next(ValidCharacterTargets.Count);
        var PickedCharacter = ValidCharacterTargets[PickedIndex];

        // Get the index from the original Character & CharacterObject lists.
        // The valid targets, excluding wounded characters might have different indices, so this will "translate"
        // back to the original so we can get both the Character (database) object and the Node2D
        var SharedIndex = BattleController.CharacterObjects.FindIndex(x => x == PickedCharacter);
        Target = BattleController.Characters[SharedIndex];
        ObjectToDamage = PickedCharacter;
        
        return PickedCharacter;
    }


    
    public static int GetRandomEnemyVigor()
    {
        Random r = new();
        return r.Next(53, 63);
    }



    public static int GetEnemyPhysicalAttackDamage(Enemy TheEnemy, int Vigor)
    {
        var Damage = TheEnemy.Level * TheEnemy.Level * (TheEnemy.Attack * 4 + Vigor) / 256;
        return Damage;
    }
    


 
}
