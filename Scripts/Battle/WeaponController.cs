using Godot;
using System;

public partial class WeaponController : Node
{
	public static void TriggerDamage()
	{
		// BattleAlgorithms.PopulateDamageText(Enums.BattleTurn.Party);
		BattleTurn.DamageTargets();
	}
}
