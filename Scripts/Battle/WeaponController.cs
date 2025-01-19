using Godot;
using System;

public partial class WeaponController : Node
{
	public void TriggerDamageText()
	{
		BattleAlgorithms.PopulateDamageText(Enums.BattleTurn.Party);
	}
}
