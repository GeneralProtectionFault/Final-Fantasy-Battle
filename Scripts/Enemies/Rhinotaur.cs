using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class Rhinotaur : Node2D, IEnemyAction
{
	// Interface properties
	public int TurnsExecuted { get; set; } = 0;
	public Enemy EnemyStats { get; set; }

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
		
		NormalAttack = new Ability() {
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


	public async void ExecuteTurn(int EnemyIndex)
	{
		TurnsExecuted += 1;

		using (SceneTreeTimer Delay = GetTree().CreateTimer(2.0f))
			await ToSignal(Delay, SceneTreeTimer.SignalName.Timeout);

		// Use this to get the stats
		var ThisEnemy = BattleController.Enemies[EnemyIndex];

		// Picks a random target among the party
		var Target = BattleAlgorithms.EnemyPickCharacterTarget();


		// BattleAlgorithms.EnemySetFightVariables(ThisEnemy, this,
		// 				Characters[HandCursor.GetCurrentCursorIndex()], Target);




		Debug.WriteLine($"Rhinotaur attacking: {Target.Name}");
		
		Random R = new Random();
		// By default, between 0 and 1 - use to act based on % chance
		var Num = R.NextDouble();

		if (TurnsExecuted == 1)
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
				var Dmg = BattleAlgorithms.GetEnemyPhysicalAttackDamage(ThisEnemy, Vigor);
				BattleAlgorithms.SetDamage(Dmg);
				BattleAlgorithms.PopulateDamageText(Enums.BattleTurn.Party);
			}
		}
		// Debug.WriteLine($"Setting to previous state: {Globals.PreviousGameState}");
		Globals.Battle_UpdateGameState(Globals.PreviousGameState);
		Debug.WriteLine($"State at start of attack: {Globals.GameState}");
	}


	public void Attacked(Ability AttackingAbility, int EnemyIndex)
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
