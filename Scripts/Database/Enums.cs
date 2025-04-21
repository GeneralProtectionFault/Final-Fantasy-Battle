using Godot;
using System;

public static class Enums
{
	public enum BattleTurn {Party, Enemy}
	public enum TargetMode {Allies, Adversaries, All}
	public enum BattleObjectType {Character, Enemy}


    public enum AbilityType {Spell, Unique, SwordTech, Blitz, Dance, Rage, Lore, Tool, Magitek, Desperation, EnemyNormal, EnemySpecial}
	
	// Magitek "status" will change the available battle options (fire beam, etc...)
	public enum Status {Berserk, Float, Poisoned, Regen, Confused, Condemned, Undead, Imp, Clear, Image, Mute, Seizure, Sleep, Slow, Haste, Dark, Zombie, Petrified, Stop, Frozen,
	Morph,
	Shell, Safe, Reflect, NearFatal,
	Wounded, InvulnerableToDeath, InstantDeath, FractionalDamage, Magitek, Defending}

	public enum Elemental {None, Fire, Water, Ice, Thunder, Poison, Earth, Wind, Holy}

	public enum FishQuality {JustAFish, Fish, RottenFish, YummyFish}

	public enum WeaponType {Dirk, Knife, Sword, Lance, Special, Gambler, Brush, Claw, Rod, Shuriken, Skean, Tool}

	public enum BattleRowPositions {FrontRow, BackRow}

	public enum BattleMode {Active, Wait}

	public enum HandCursorMode {Menu, Object}

	public enum GameState {
		
		Battle,
		Battle_Menu_Normal,
		Battle_Menu_Magic,
		Battle_Menu_SwordTech,
		Battle_Menu_Blitz,
		Battle_Menu_Dance,
		Battle_Menu_Rage,
		Battle_Menu_Lore,
		Battle_Menu_Tool,
		Battle_Menu_Item,
		
		Battle_Magitek,
		Battle_Menu_Magitek,

		Battle_Fight_Selecting_Target_Characters,
		Battle_Fight_Selecting_Target_Enemies,
		Battle_Magic_Selecting_Target_Characters,
		Battle_Magic_Selecting_Target_Enemies,
		Battle_Jump_Selecting_Target,
		Battle_Item_Selecting_Target_Characters,
		Battle_Item_Selecting_Target_Enemies,
		Battle_Tool_Selecting_Target,

		Battle_Fight_Selecting_Target_Multiple_Characters,
		Battle_Fight_Selecting_Target_Multiple_Enemies,
		Battle_Magic_Selecting_Target_Multiple_Characters,
		Battle_Magic_Selecting_Target_Multiple_Enemies,
		Battle_Item_Selecting_Target_Multiple_Characters,
		Battle_Item_Selecting_Target_Multiple_Enemies,
		Battle_Tool_Selecting_Target_Multiple,

		Battle_Party_Action,
		Battle_Enemy_Action,

		Battle_Won,
		Battle_Lost,
		Battle_End // Use this to distinguish between won/lost.  We will set to this in Process, but not run Process once @ Battle_End
		
		// NON-BATTLE STATES
		,Overworld,
		Town,
		Party_Menu
	}
}

