using System.Collections.Generic;
using LiteDB;

public class Weapon
{
    public ObjectId _id { get; set; }

    public string Name { get; set; }

    public List<Enums.Status> StatusesInduced { get; set; }
    public List<Enums.Status> StatusesBlocked { get; set; }

    public Ability RandomAbilityCast { get; set; }

    public bool TwoHandCapable { get; set; }
    public bool RunicCapable { get; set; }
    public bool BushidoCapable { get; set; }
    
    
    public int Attack { get; set; }
    public int Strength { get; set; }
    public int Stamina { get; set; }
    public int Magic { get; set; }
    public int Speed { get; set; }
    public int Evade { get; set; }
    public int MagicEvade { get; set; }

    public bool TargetsMultiple { get; set; }

    public Enums.Elemental Element { get; set; }

    public bool SameDamageBackRow { get; set; }

    public Enums.WeaponType WeaponType { get; set; }

    // For weapons that consume MP to do extra damage
    public int[] MpCost { get; set; }

    public int Cost { get; set; }
}