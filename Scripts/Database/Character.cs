using System.Collections.Generic;
using LiteDB;

public class Character
{
    public ObjectId _id { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }

    // (A.K.A. "Vigor")
    public int Strength { get; set; }
    public int Agility { get; set; }
    public int Stamina { get; set; }
    public int Magic { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Evasion { get; set; }
    public int MagicDefense { get; set; }
    public int MagicEvasion { get; set; }
    // public int HitRate { get; set; }
    public int EscapeSuccess { get; set; }


    public int ExtraHP { get; set; }
    public int ExtraMP { get; set; }

    public int Hp { get; set; }
    public int HpMax { get; set; }
    
    public int Mp { get; set; }
    public int MpMax { get; set; }

    public int InitialLevelOffset { get; set; }

    public bool InParty { get; set; }
    public bool IsWounded { get; set; }

    
    // Spells that the character currently has access to
    public List<Ability> MagicList { get; set; }
    // List of spell names and the level at which they are activated
    public IDictionary<string, int> NativeMagicActivationList { get; set; }
    
    
    public List<Ability> UniqueAbilityList { get; set; }
    // Levels at which characters activate their special abilities (Blitz, SwdTech, etc...)
    public IDictionary<string, int> UniqueAbilityActivationList { get; set; }


    public List<Enums.Status> Statuses { get; set; }

    // Set these per the battle circumstance, just use as a check when a status is trying to set from an attack
    public List<Enums.Status> StatusImmunities { get; set; }

    public Enums.BattleRowPositions RowPosition { get; set; }

    public bool IsPartyLead { get; set; }

    // List is for multiple elements
    // Dictionary is to store the element and the % effect if strong against that element,
    // or the % to deduct from defense if weak against that element.

    // Example (Strong):
    // Values 100 - 0 (decreasing):  Damage done by acosting ability (spell) will approach 0 as value approaches 100
    // Values 0 - (minus) -100:  Damage done will be ABSORBED (converted to healing) as value approaches -100 
    //     i.e....  Multiply by the negative, get a negative.  If result is negative, make healing, etc...
    public IDictionary<Enums.Elemental, float> ElementsStrong { get; set; }
	public IDictionary<Enums.Elemental, float> ElementsWeak { get; set; }


    public string RightHandEquipped { get; set; }
    public string LeftHandEquipped { get; set; }    
    public string BodyEquipped { get; set; }
    public string RelicEquipped { get; set; }

    public int Experience { get; set; }



}
