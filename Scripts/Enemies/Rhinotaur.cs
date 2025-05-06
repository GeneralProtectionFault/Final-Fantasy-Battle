using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class Rhinotaur : BaseEnemyAction
{
    // Specific properties
    public bool WillRespondWithMegaVolt { get; set; } = false; // Counters with MegaVolt if so


    // Abilities
    public Ability NormalAttack;
    public Ability Rush;

    // This is randomly calculated within a range @ battle start
    public int Vigor;


    // Constructor
    Rhinotaur()
    {
        EnemyStats = (Enemy)DatabaseHandler.EnemyCollection.Find(x => x.Name == "Rhinotaur").FirstOrDefault();
        Debug.Assert(EnemyStats is not null);

        NormalAttack = new Ability()
        {
            Power = 25,
            Physical = true,
            IgnoreDefense = false,
            Unblockable = false,
            HitRate = EnemyStats.HitRate,
            MPCost = 0,
            Reflectable = false,
            DamageMultiplier = 1,
            SplitDamage = false,
            Type = Enums.AbilityType.EnemyNormal,
            RandomPercentage = 0,
            IsActive = true
        };

        Rush = NormalAttack;
        Rush.Power = NormalAttack.Power * 2;

        Vigor = BattleAlgorithms.GetRandomEnemyVigor();
    }


    public async override void QueueTurn(BattleGameObject Enemy)
    {
        Random R = new Random();
        // By default, between 0 and 1 - use to act based on % chance
        var Num = R.NextDouble();

        if (TurnCounter == 1)
        {
            // 33% chance of doing nothing on only the first action
            if (Num < .33)
            {
                Debug.WriteLine("1st turn--Rhinotaur skipping attack");
            }
        }
        else
        {
            if (Num < .33)
            {
                // Rush
                Debug.WriteLine("Rhinotaur attacking with Rush");
            }
            else
            {
                // Normal attack
                Debug.WriteLine("Rhinotaur attacking with normal attack");
                var Dmg = BattleAlgorithms.GetEnemyPhysicalAttackDamage((Enemy)Enemy.EntityData, Vigor);

            }
        }


        // ************** Add the turn to the queue! *******************
        var SelectedTargets = BattleAlgorithms.EnemyPickCharacterTarget();
        var BattleTargetObjects = new List<BattleTarget>();

        // TODO:  Handle multiple targets
        // TODO:  Store the type of action (attack, spell, etc...)
        // Right now, SelectedTargets will only have 1 object
        foreach (var Target in SelectedTargets)
        {
            // CharacterAttack() method returns a BattleTarget object with all the damage, etc... calculated
            // TODO:  MAKE THIS ENEMY ATTACK - check gamefaqs for the enemy algorithm!
            BattleTarget TargetObject = new BattleTarget()
            {
                TargetEntity = Target,
                DamageHP = 10       // Etc, etc...
            };

            BattleTargetObjects.Add(TargetObject);
            Debug.WriteLine($"Rhinotaur attacking: {Target.EntityData.Name}");
        }

        // Finishing the above code will give us battle target objects w/ damage calculations, etc... to add to the BattleTurn

        var Turn = new BattleTurn()
        {
            TurnType = Enums.BattleTurn.Enemy,
            TargetMode = Enums.TargetMode.Adversaries,
            Initiator = Enemy,
            InitiatorStats = EnemyStats,
            AbilityEffect = GD.Load<PackedScene>($"res://Scenes/AbilityEffects/TripleSlash_Red.tscn"),
            Targets = BattleTargetObjects
        };

        BattleTurn.BattleQueue.Add(Turn);
    }


    public async override void ExecuteTurn(BattleGameObject Enemy)
    {
        // var AttackEffect = GD.Load<PackedScene>("res://Scenes/AttackEffects/TripleSlash_Red.tscn");
        // (DamageText as DamageHealthText).Amount = target.DamageHP;
        // target.TargetEntity.EntityNode.AddChild(DamageText);
        base.ExecuteTurn(Enemy);
    }


    public async override void Attacked(Ability AttackingAbility, int EnemyIndex)
    {
        // Log anything relevant--in this case, if attacked by magic
        if (AttackingAbility.Type == Enums.AbilityType.Spell)
        {
            // 33 % chance
            Random R = new Random();
            var Num = R.NextDouble();

            if (Num < .33)
                WillRespondWithMegaVolt = true;
        }

    }


}
