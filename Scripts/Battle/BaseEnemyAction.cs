using System.Diagnostics;
using Godot;


public abstract partial class BaseEnemyAction : Node2D
{
    public int TurnCounter { get; set; }
    public Enemy EnemyStats { get; set; }

    private PackedScene AttackEffect;


    public async virtual void QueueTurn(BattleGameObject Enemy)
    {
        TurnCounter += 1;
    }


    public async virtual void ExecuteTurn(BattleGameObject Enemy)
    {
        // Flash effect!
        var FlashUniformMaterial = Enemy.EntityNode.GetNode<Sprite2D>("Sprite2D").Material as ShaderMaterial;
        FlashUniformMaterial.SetShaderParameter("attack_flash", true);
        Tween FlashTween = GetTree().CreateTween();
        // Activates the flash for .2 seconds (waits that long before shutting the shader off again)
        FlashTween.TweenCallback(Callable.From(() =>
        {
            FlashUniformMaterial.SetShaderParameter("attack_flash", false);
        }
        )).SetDelay(0.2f);

        // Simple delay so it's not absurdly instantaneous
        // using (SceneTreeTimer Delay = GetTree().CreateTimer(1.0f))
        //     await ToSignal(Delay, SceneTreeTimer.SignalName.Timeout);

        BattleTurn.Instance.DamageTargets(1); // This will include popping the active battle turn off the queue, etc...

        Enemy.IsQueued = false;
        Enemy.ProgressBar.Value = 0;
        Enemy.FullTimerBar = false;
    }


    /// Process what happenes to enemy when they are attacked
    public async virtual void Attacked(Ability AttackingAbility)
    {

    }
}
