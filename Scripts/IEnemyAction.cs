using Godot;
using System;

interface IEnemyAction
{   
    public int TurnsExecuted { get; set; }
    public Enemy EnemyStats { get; set; }


    public void ExecuteTurn(int EnemyIndex);
    public void Attacked(Ability AttackingAbility, int EnemyIndex);
}
