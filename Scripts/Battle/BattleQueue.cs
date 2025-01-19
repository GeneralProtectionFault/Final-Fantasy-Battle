using Godot;
using System;

public class BattleQueue
{
	public Enums.BattleTurn TurnType { get; }
	
	// Use the character or enemy performing the action to act as an index
	public GodotObject Initiator { get; }
}
