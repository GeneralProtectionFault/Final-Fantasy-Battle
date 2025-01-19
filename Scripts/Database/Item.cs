using System.Collections.Generic;
using LiteDB;


public class Item
{
    public ObjectId _id { get; set; }

    public List<Enums.Status> StatusesInduced { get; set; }
    public List<Enums.Status> StatusesRemoved { get; set; }

    public string Name { get; set; }
    public int HpIncrease { get; set; }
    public int MpIncrease { get; set; }

    public bool WarpsFromBattle { get; set; }

    public bool TargetsMultiple { get; set; }
    public bool CanBeThrown { get; set; }
    public int PhysicalDamage { get; set; }
    public int MagicDamage { get; set; }

    public int Cost { get; set; }

    // For items like Shadow's "Edges"
    public Enums.Elemental Element { get; set; }

    // Fish, etc... :)
    // These items will not be selectable/useable at the player's command and will appear in the "RARE" list
    public bool IsRareItem { get; set; }

    public int InventoryCount { get; set; }
}