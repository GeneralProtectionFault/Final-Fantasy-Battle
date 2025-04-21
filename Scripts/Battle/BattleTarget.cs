using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;


[Description("Class to store all the effects a battle action has on a target")]
public class BattleTarget
{
	public BattleGameObject TargetEntity { get; set; }

    public int DamageHP { get; set; } = 0;
	public int DamageMP { get; set; } = 0;

    public int ReplenishHP { get; set; } = 0;
    public int ReplenishMP { get; set; } = 0;

	public List<Enums.Status> StatusesAdded { get; set; } = new();
	public List<Enums.Status> StatusesRemoved { get; set; } = new();



	public Boolean ChangesRow { get; set;} = false;		// For the few moves that mess with the party's rows
}