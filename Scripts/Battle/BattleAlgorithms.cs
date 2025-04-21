using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;



public static class BattleAlgorithms
{
    

   
    

    /// <summary>
    /// In battle, chooses a member of the party with HP greater than 0
    /// Returns character as godot object (Node2D)
    /// </summary>
    public static List<BattleGameObject> EnemyPickCharacterTarget()
    {
        // Get all the characters that are valid targets (not wounded, petrified, etc...)
        List<BattleGameObject> ValidCharacterTargets = BattleController.Characters.Where(x => x.IsValidTarget == true).ToList();
        List<BattleGameObject> PickedCharacterTargets = new();

        // Pick one of the list
        var RCharacter = new Random();
        var PickedIndex = RCharacter.Next(ValidCharacterTargets.Count);
        var PickedCharacters = ValidCharacterTargets[PickedIndex];
        
        // TODO: Handle multiple targets

        return new List<BattleGameObject> { PickedCharacters };
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
    



    public static BattleTarget CharacterAttack(BattleGameObject Initiator, BattleGameObject Target)
    {
        var Vigor2 = Mathf.Min(Initiator.EntityData.Strength * 2, 255);
        var Attack = Vigor2 + Initiator.EntityData.Attack;
        // TODO: Update RelicsEquipped (and other properties) to not be null and cause an exception
        // if (Initiator.EntityData.RelicsEquipped.Contains("Gauntlet"))
        //     Attack += (int)(Initiator.EntityData.Attack * .75);
        
        var Damage = Initiator.EntityData.Attack + 
        Math.Pow(Initiator.EntityData.Level, 2) * Initiator.EntityData.Attack / 256 * 3 / 2;

        // TODO: Same as above...
        // if (Initiator.EntityData.RelicsEquipped.Contains("Offering"))
        //     Damage /= 2;

        // TODO:
        // If the attack is a standard fight attack and the character is
        // equipped with a Genji Glove, but only one or zero weapons:
        // Damage = Mathf.Ceil(Damage * 3 / 4);

        if (Initiator.EntityData.RowPosition == Enums.BattleRowPositions.BackRow)
            Damage /= 2;

        var DamageMultiplier = 0;
        if (Initiator.EntityData.Statuses.Contains(Enums.Status.Morph))
            DamageMultiplier += 2;
        if (Initiator.EntityData.Statuses.Contains(Enums.Status.Berserk))
            DamageMultiplier += 1;

        // Critical chance (1/32)
        Random r = new Random();

        var IsCriticalHit = r.Next(32) == 0;
        if (IsCriticalHit)
            DamageMultiplier += 2;

        Damage += Damage / 2 * DamageMultiplier;
        

        // Random variance
        Damage = (Damage * r.Next(224,255) / 256) + 1;

        // Target's defense ---------------------------------------------------------------------
        Damage = (Damage * (255 - Target.EntityData.Defense) / 256) + 1;
        // Safe damage reduction
        if (Target.EntityData.Statuses.Contains(Enums.Status.Safe))
            Damage = (Damage * 170 / 256) + 1;

        // Defend
        if (Target.EntityData.Statuses.Contains(Enums.Status.Defending))
            Damage /= 2;

        // Row
        if (Target.EntityData.RowPosition == Enums.BattleRowPositions.BackRow)
            Damage /= 2;

        // Halve damage if attacking self/party
        if (Target.EntityType == Enums.BattleObjectType.Character)
            Damage /= 2;

        // Petrify (nullifies damage)
        if (Target.EntityData.Statuses.Contains(Enums.Status.Petrified))
            Damage = 0;

        // Elemental...
        // TODO: Element nullified by force fieldd => Damage = 0
        // TODO: Handle elemental damage, maybe extend functionality (IBattleEntity.cs) beyond strong/weak

        // Hit Determination ---------------------------------------------------------------------
        bool HitSuccess;
        // Vanish
        if (Target.EntityData.Statuses.Contains(Enums.Status.Clear))
            HitSuccess = false;
        // TODO: Death Protection? (Maybe weapon w/ random dispatch ability?)

        
        if (Globals.AlwaysHitStatuses.Intersect(Target.EntityData.Statuses).Any())
            HitSuccess = true;
        
        // TODO: Back attack (always hits)
        // If max hitrate
        if (Initiator.EntityData.HitRate == 255)
            HitSuccess = true;

        // Image
        if (Target.EntityData.Statuses.Contains(Enums.Status.Image))
        {
            HitSuccess = false;
            // 1/4 chance of removing the status
            var RemoveImageStatus = r.Next(4) == 0;
            if (RemoveImageStatus)
                Target.EntityData.Statuses.Remove(Enums.Status.Image);
        }

        // TODO:  Block/MBlock
        
        // TODO: Hit blocked by stamina:  Only Break, Doom, Demi,
        // Quartr, X-Zone, W Wind, Shoat, Odin, Raiden, Antlion, Snare, X-Fer, and Grav Bomb

        // TODO:  Deperation Attacks

        return new BattleTarget() { TargetEntity = Target, DamageHP = (int)Damage };

    }


 
}
