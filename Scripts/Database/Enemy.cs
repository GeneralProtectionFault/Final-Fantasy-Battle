using System.Collections.Generic;
using LiteDB;

public class Enemy : IBattleEntity
{
	



    // Some enemies 
    public List<Enums.Status> AutomaticStatus { get; set; }


    

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
    public int ExperienceGiven { get; set; }
    public int Gil { get; set; }

    // Dropped items and their probabilities of being dropped
    public IDictionary<string, float> DroppedItems { get; set; }
    // Items that can be stolen, and their probability of being stolen
    public IDictionary<string, float> StolenItems { get; set; }


    // Use this to track the position in the list of enemies during battle,
    // since we only want to affect the in-battle object, not the database
    public int BattleListIndex { get; set; }
}
