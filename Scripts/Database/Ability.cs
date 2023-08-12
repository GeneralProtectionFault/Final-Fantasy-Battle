using LiteDB;
using System.Collections.Generic;


public class Ability 
{
    public ObjectId _id { get; set; }

    
    public string Name { get; set; }
    public int Power { get; set; }
    public bool Physical { get; set; }
    public bool IgnoreDefense { get; set; }
    // (Ignores MBlock)
    public bool Unblockable { get; set; }
    public int HitRate { get; set; }

    public int MPCost { get; set; }

    public bool Reflectable { get; set; }

    public float DamageMultiplier { get; set; }
    // If true, damage will be reduced according to multiple targets
    public bool SplitDamage { get; set; }

    public List<Enums.Elemental> ElementalEffects { get; set; }
    public List<Enums.Status> StatusEffects { get; set; }


    public Enums.AbilityType Type { get; set; }
    public string MagicClass { get; set; }

    // If the ability happens randomly--percent chance of activation, etc... 
    public float RandomPercentage { get; set; }

    public bool IsActive { get; set; }

}
