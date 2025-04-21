using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;




[Description("General object for both characters & enemies in battle.  Stores database stats Node, etc...")]
public class BattleGameObject
{
    public int Index { get; set; }                          // Use this to indicate the "order," which should correspond to the battle canvas objects w/ 1-4 naming, etc...
    public bool IsValidTarget { get; set; }

    public Node2D EntityNode { get; set; }                  
    public IBattleEntity EntityData { get; set; }           // From Database
    public Enums.BattleObjectType EntityType { get; set; }  // Character or Enemy


    
    public Boolean FullTimerBar { get; set; }
    public Boolean IsActiveCharacter { get; set; }          // The character (w/ full timer bar) being controlled/selected
    public TextureProgressBar ProgressBar { get; set; }


    /// If true, battle action, etc... already in queue--don't add another
    public bool IsQueued { get; set; }
}