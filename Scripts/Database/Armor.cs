using System.Collections.Generic;
using LiteDB;

public class Armor
{
    public ObjectId _id { get; set; }

    public string Name { get; set; }

    public int Defense { get; set; }
    public int MagicDefense { get; set; }
    public int Evade { get; set; }
    public int MagicBlock { get; set; }
    public int Stamina { get; set; }
    public int Speed { get; set; }
    public int Strength { get; set; }
    public int MagicPower { get; set; }

    public List<Enums.Status> StatusesInduced { get; set; }
    public List<Enums.Status> StatusesBlocked { get; set; }

    // String will be the element, the int will be the percentage damage reduced
    public IDictionary<Enums.Elemental, int> ElementalDamageReduction { get; set; }
    // String will be the element, the int will be the percentage damage absorbed
    public IDictionary<Enums.Elemental, int> ElementalDamageAbsorbtion { get; set; }

    public float MPMultiplier { get; set; }

    public int Cost { get; set; }

    public List<string> CharactersCanEquip;
}