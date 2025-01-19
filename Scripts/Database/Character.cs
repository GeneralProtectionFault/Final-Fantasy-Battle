using System.Collections.Generic;
using LiteDB;

public class Character : IBattleEntity
{
    
    public int ExtraHP { get; set; }
    public int ExtraMP { get; set; }


    public int InitialLevelOffset { get; set; }

    public bool InParty { get; set; }
    

    
    // List of spell names and the level at which they are activated
    public IDictionary<string, int> NativeMagicActivationList { get; set; }
    
    
    
    // Levels at which characters activate their special abilities (Blitz, SwdTech, etc...)
    public IDictionary<string, int> UniqueAbilityActivationList { get; set; }


    



    public Enums.BattleRowPositions RowPosition { get; set; }

    public bool IsPartyLead { get; set; }




    public string RightHandEquipped { get; set; }
    public string LeftHandEquipped { get; set; }    
    public string BodyEquipped { get; set; }
    public string RelicEquipped { get; set; }

    public int Experience { get; set; }



}
