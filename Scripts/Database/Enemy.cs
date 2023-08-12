using System.Collections.Generic;
using LiteDB;

public class Enemy
{
	public ObjectId _id { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }

    public int Strength { get; set; }
    public int Agility { get; set; }
    public int Stamina { get; set; }
    public int Magic { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Evasion { get; set; }
    public int MagicDefense { get; set; }
    public int MagicEvasion { get; set; }
    public int HitRate { get; set; }
    public int EscapeSuccess { get; set; }


    public int Hp { get; set; }
    public int HpMax { get; set; }
    
    public int Mp { get; set; }
    public int MpMax { get; set; }


    
    public List<Ability> MagicList { get; set; }
    
    public List<Ability> UniqueAbilityList { get; set; }

    public List<Enums.Status> Statuses { get; set; }

    public List<Enums.Status> StatusImmunities { get; set; }

    // Some enemies 
    public List<Enums.Status> AutomaticStatus { get; set; }


	// List is for multiple elements
    // Dictionary is to store the element and the % effect if strong against that element,
    // or the % to deduct from defense if weak against that element.

    // Example (Strong):
    // Values 100 - 0 (decreasing):  Damage done by acosting ability (spell) will approach 0 as value approaches 100
    // Values 0 - (minus) -100:  Damage done will be ABSORBED (converted to healing) as value approaches - 100 
    //     i.e....  Multiply by the negative, get a negative.  If result is negative, make healing, etc...
    public IDictionary<Enums.Elemental, float> ElementsStrong { get; set; }
	public IDictionary<Enums.Elemental, float> ElementsWeak { get; set; }
    

    // Abilities when using Sketch and their probabilities of use
    public IDictionary<string, float> SketchAbilities { get; set; }
    // Abilities available when using Control or enemy is confused.  If confused, dictionary value is the probability of use.
    public IDictionary<string,float> ControlConfuseAbilities { get; set; }

	// List of abilities that will be called for this enemy in Rage
    // number is percentage likelihood of executing when Rage is used.
	public List<Ability> RageAbilities { get; set; }

    // List of Lore abilities that can be learned
    // Similarly, lookup value is % chance of executing when Lore is used.
    public IDictionary<string, float> LoreAbilities { get; set; }
    

    // Abilities actually to be used in a battle
    //public IDictionary<string, float> BattleAbilities { get; set; }
    [BsonRef("abilities")]
    public List<Ability> BattleAbilities { get; set; }


    // Items derived from using Ragnarok on the enemy & chance of obtaining
    public IDictionary<string, float> RagnarokItems { get; set; }


    // Experience & Gil/GP gained when defeated in battle
    public int Experience { get; set; }
    public int Gil { get; set; }

    // Dropped items and their probabilities of being dropped
    public IDictionary<string, float> DroppedItems { get; set; }
    // Items that can be stolen, and their probability of being stolen
    public IDictionary<string, float> StolenItems { get; set; }

}
