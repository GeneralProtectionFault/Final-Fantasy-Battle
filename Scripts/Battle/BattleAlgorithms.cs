using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
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



    /// <summary>
    /// Public method to trigger the damage text to populate
    /// </summary>
    public static void PopulateDamageText()
    {
        DamageText(ObjectToDamage, DamageAmount);

        if (TargetCharacter != null)
            UpdateCharacterDamage();
        else if (TargetEnemy != null)
            DamageEnemy();
    }



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



    


 
}
