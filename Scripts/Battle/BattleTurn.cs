using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class BattleTurn : Node
{
    public static event EventHandler<BattleGameObject> DamagingCharacter;
    public static event EventHandler<BattleGameObject> DamagingEnemy;


    public static List<BattleTurn> BattleQueue = new();

    // This will store whichever turn in the BattleQueue is active
    public static BattleTurn ActiveBattleTurn;



    public Enums.BattleTurn TurnType { get; set; }
    public Enums.TargetMode TargetMode { get; set; }


    // Use the character or enemy performing the action to act as an index
    public BattleGameObject Initiator { get; set; }
    public IBattleEntity InitiatorStats { get; set; }
    public PackedScene AbilityEffect { get; set; }



    public List<BattleTarget> Targets { get; set; } = new();



    public static BattleTurn Instance { get; private set; }

    public override void _Ready()
    {
        // (Singleton)
        if (Instance == null)
            Instance = this;
    }




    public override void _Process(double delta)
    {
        if (Globals.BattleWaitStates.Contains(Globals.SelectionState) ||
        Globals.BattleActionStates.Contains(Globals.GameState) ||
        Globals.GameState == Enums.GameState.Battle_Won ||
        Globals.GameState == Enums.GameState.Battle_End ||
        Globals.GameState == Enums.GameState.Battle_Lost)
        {
            return;
        }
        else if (BattleQueue.Count > 0)
        {
            ActiveBattleTurn = BattleQueue.FirstOrDefault();
            if (ActiveBattleTurn.TurnType == Enums.BattleTurn.Party)
            {
                Globals.Battle_UpdateGameState(this, Enums.GameState.Battle_Party_Action);
                CommencePartyTurn();
            }
            else
            {
                Globals.Battle_UpdateGameState(this, Enums.GameState.Battle_Enemy_Action);
                // Get the enemy's script to perform whatever diverse stuff they do...
                (ActiveBattleTurn.Initiator.EntityNode as BaseEnemyAction).ExecuteTurn(ActiveBattleTurn.Initiator);
            }
        }
    }




    public static void CommencePartyTurn()
    {
        // Initiator animation
        var AttackerAnimationPlayer = ActiveBattleTurn.Initiator.EntityNode.GetNode<AnimationPlayer>("Battle_AnimationPlayer");

        // This animation will trigger the Fight (Weapon) Animation via the method in CharacterController.cs (attached to the character)
        // This will, in turn, trigger the target animation (slash, pow, thwack, whatever...) on all of the targets
        // That target animation will call the attached DamageTarget() method in this script. from WeaponController.cs
        AttackerAnimationPlayer.Play("Attack");
    }




    /// <summary>
    /// Method that actually makes the damage text appear
    /// </summary>
    /// <param name="ParentObject"></param>
    /// <param name="Damage"></param>
    private static void DamageText(BattleTarget target)
    {
        var DamageText = GD.Load<PackedScene>("res://Scenes/Battle/DamageHealText.tscn").Instantiate();
        (DamageText as DamageHealthText).Amount = target.DamageHP;
        target.TargetEntity.EntityNode.AddChild(DamageText);
    }


    /// This should loop through all the targets in cases of simultaneous effects on multiple targets
    /// However, multiple/separated hits on a single target should be handled
    public static void DamageTargets()
    {
        // TODO: This would be for a simultaneous effect, Offering attach should be handled differently
        foreach (var target in ActiveBattleTurn.Targets)
        {
            // Right now, the character attack is handled elsewhere, so it will not have the "slash" or whatever stored here
            // This is currently only for enemy attack
            if (ActiveBattleTurn.AbilityEffect is not null)
                target.TargetEntity.EntityNode.AddChild(ActiveBattleTurn.AbilityEffect.Instantiate());

            DamageText(target);

            if (target.TargetEntity.EntityData is Enemy)
            {
                DamageEnemy(target.TargetEntity.EntityData, target.DamageHP);   // Actually updates the damage
                DamagingEnemy?.Invoke(null, target.TargetEntity);
            }
            else if (target.TargetEntity.EntityData is Character)
            {
                // TODO:  Add whatever algorithm/enemy script implementation to get the damage done to the character(s)
                UpdateCharacterDamage(target.TargetEntity.EntityData, target.DamageHP);
                DamagingCharacter?.Invoke(null, target.TargetEntity);
            }
        }

        ActiveBattleTurn.Initiator.IsQueued = false;
        ActiveBattleTurn.Initiator.ProgressBar.Value = 0;

        BattleQueue.Remove(ActiveBattleTurn);
        if (BattleQueue.Count() > 0)
            ActiveBattleTurn = BattleQueue.First();
        else
            ActiveBattleTurn = null;

        Globals.Battle_UpdateGameState(null, Enums.GameState.Battle);
    }



    private static void UpdateCharacterDamage(IBattleEntity Target, int DamageAmount)
    {
        var CurrentHP = DatabaseHandler.GetCharacterStatAsString(Target.Name, "Hp");
        var HP = Mathf.Max(CurrentHP.ToInt() - DamageAmount, 0);

        Target.Hp = HP;
        if (HP == 0)
            Target.Statuses.Add(Enums.Status.Wounded);

        DatabaseHandler.UpdateCharacter(Target as Character);
        // TODO:  Need to update the BattleGameObject??
    }


    private static void DamageEnemy(IBattleEntity Target, int DamageAmount)
    {
        Target.Hp -= DamageAmount;
    }


}
