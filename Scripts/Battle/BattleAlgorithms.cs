using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;



public static class BattleAlgorithms
{
    
    private static Node2D ObjectToDamage;
    private static int DamageAmount;
    // private static PackedScene DamageHealText = GD.Load<PackedScene>("res://Scenes/Battle/DamageHealText.tscn");


    // Store for state updates
    private static Character TargetCharacter;
    private static Enemy TargetEnemy;



    public static event EventHandler<Character> DamagingCharacter;
    public static event EventHandler<Enemy> DamagingEnemy;


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
            if (TargetCharacter != null)
                UpdateCharacterDamage();
            else if (TargetEnemy != null)
                DamageEnemy();
        }
        else // This doesn't fire off the event back to the BattleController, which would re-enable the fight menu, etc...
        {
            GD.Print("Chipping the HP off the character");
            var CurrentHP = DatabaseHandler.GetCharacterStatAsString(TargetCharacter.Name, "Hp");
            var HP = CurrentHP.ToInt() - DamageAmount;
            DamageAmount = 0;

            TargetCharacter.Hp = HP;
            DatabaseHandler.UpdateCharacter(TargetCharacter);
        }
    }



    /// <summary>
    /// Set the character attacking both class object & Node2D
    /// Set the target, both class object & Node2D
    /// </summary>
    /// <param name="Fighter">The database/class object that's going to attack</param>
    /// <param name="FighterObject">The Node2D of the attacker</param>
    /// <param name="Target">The database/class object of the target</param>
    /// <param name="FighterObject">The Node2D of the target</param>

    public static void SetFightVariables(Character Fighter, Node2D FighterObject, Character Target, Node2D TargetObject)
    {
        // GD.Print("FIGHTING CHARACTER!");
        ObjectToDamage = TargetObject;
        TargetEnemy = null;
        TargetCharacter = Target;
        DamageAmount = 19;
    }

    public static void SetFightVariables(Character Fighter, Node2D FighterObject, Enemy Target, Node2D TargetObject)
    {
        // GD.Print("FIGHTING ENEMY!");
        ObjectToDamage = TargetObject;
        TargetCharacter = null;
        TargetEnemy = Target;
        DamageAmount = 31;
    }

    /// <summary>
    /// Use this when the enemy is attacking - no character
    /// </summary>
    // public static void EnemySetFightVariables(Enemy Attacker, Node2D AttackerObject, Character Target, Node2D TargetObject, int Damage)
    // {
    //     ObjectToDamage = TargetObject;
    //     TargetCharacter = Target;
    //     TargetEnemy = null;
    //     DamageAmount = Damage;
    // }




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
        if (TargetCharacter != null)
        {
            var CurrentHP = DatabaseHandler.GetCharacterStatAsString(TargetCharacter.Name, "Hp");
            var HP = CurrentHP.ToInt() - DamageAmount;
            DamageAmount = 0;

            TargetCharacter.Hp = HP;
            DatabaseHandler.UpdateCharacter(TargetCharacter);

            // Picked up in BattleController.cs
            DamagingCharacter?.Invoke(null, TargetCharacter);
        }
    }


    private static void DamageEnemy()
    {
        if (TargetEnemy != null)
        {
            // Damage the enemy object, etc...
            TargetEnemy.Hp -= DamageAmount;
            DamagingEnemy?.Invoke(null, TargetEnemy);
        }
    }


    /// <summary>
    /// In battle, chooses a member of the party with HP greater than 0
    /// Returns character as godot object (Node2D)
    /// </summary>
    public static Node2D EnemyPickCharacterTarget()
    {
        List<Node2D> ValidCharacterTargets = new List<Node2D>();
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
        TargetCharacter = BattleController.Characters[SharedIndex];

        ObjectToDamage = PickedCharacter;
        TargetEnemy = null;
        return PickedCharacter;
    }


    
    public static int GetRandomEnemyVigor()
    {
        Random r = new Random();
        return r.Next(53, 63);
    }



    public static int GetEnemyPhysicalAttackDamage(Enemy TheEnemy, int Vigor)
    {
        var Damage = TheEnemy.Level * TheEnemy.Level * (TheEnemy.Attack * 4 + Vigor) / 256;
        return Damage;
    }
    


 
}
