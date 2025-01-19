using System.Collections.Generic;
using Godot;


public static class DatabaseDefaults_Armor
{
    private static void AddArmor(Armor Armor)
    {
        var ArmorResult = DatabaseHandler.ArmorCollection.FindOne(x => x.Name == Armor.Name);

		if( !(ArmorResult is null) ) {
			GD.Print($"{Armor.Name} is already in database, aborting add armor.");
			return;
		}

		DatabaseHandler.ArmorCollection.Insert(Armor);
		DatabaseHandler.ArmorCollection.EnsureIndex("Name", true);

        DatabaseHandler.GameDatabase.Commit();

		GD.Print($"{Armor.Name} added to database.");
    }


    public static void AddAllArmor()
    {
        var BehemothSuit = new Armor { Name = "Behemoth Suit", Defense = 94, MagicDefense = 73, Evade = 0, MagicBlock = 0, Stamina = 6, Speed = 6, Strength = 6, MagicPower = 6,
        CharactersCanEquip = new List<string>() {"Relm","Strago"}};
        AddArmor(BehemothSuit);
        
        var BlackGarb = new Armor { Name = "Black Garb", Defense = 68, MagicDefense = 46, Evade = 0, MagicBlock = 0, Stamina = 0, Speed = 6, Strength = 0, MagicPower = 0,
        CharactersCanEquip = new List<string>() {"Locke","Shadow","Setzer","Sabin","Gau","Gogo"}};
        AddArmor(BlackGarb);
        
        var ChocoboSuit = new Armor { Name = "Chocobo Suit", Defense = 56, MagicDefense = 38, Evade = 0, MagicBlock = 0, Stamina = 2, Speed = 6, Strength = 3, MagicPower = 0,
        ElementalDamageAbsorbtion = new Dictionary<Enums.Elemental, int>() {{Enums.Elemental.Poison, 100}},
        CharactersCanEquip = new List<string>() {"Relm","Strago"}};
        AddArmor(ChocoboSuit);
        
        var CottonRobe = new Armor { Name = "Cotton Robe", Defense = 32, MagicDefense = 21, Evade = 0, MagicBlock = 0, Stamina = 0, Speed = 6, Strength = 3, MagicPower = 0,
        CharactersCanEquip = new List<string>() {"Relm","Strago","Terra","Gogo"}};
        AddArmor(CottonRobe);
        
        var CrystalMail = new Armor { Name = "Crystal Mail", Defense = 72, MagicDefense = 49, Evade = 0, MagicBlock = 0, Stamina = 0, Speed = 0, Strength = 0, MagicPower = 0,
        CharactersCanEquip = new List<string>() {"Terra","Locke","Celes","Edgar","Cyan","Setzer"}};
        AddArmor(CrystalMail);
        
        var DiamondArmor = new Armor { Name = "Diamond Armor", Defense = 70, MagicDefense = 47, Evade = 0, MagicBlock = 0, Stamina = 0, Speed = 0, Strength = 0, MagicPower = 0,
        CharactersCanEquip = new List<string>() {"Terra","Cyan","Setzer","Edgar","Celes"}};
        AddArmor(DiamondArmor);
        
        var DiamondVest = new Armor { Name = "Diamond Vest", Defense = 65, MagicDefense = 44, Evade = 0, MagicBlock = 0, Stamina = 0, Speed = 0, Strength = 0, MagicPower = 0,
        CharactersCanEquip = new List<string>() {"Locke","Terra","Cyan","Celes","Edgar","Sabin","Gogo","Setzer","Shadow","Mog","Gau"}};
        AddArmor(DiamondVest);
        
        var ForceArmor = new Armor { Name = "Force Armor", Defense = 69, MagicDefense = 68, Evade = 0, MagicBlock = 30, Stamina = 0, Speed = 0, MagicPower = 0,
        ElementalDamageReduction = new Dictionary<Enums.Elemental, int>() {
            {Enums.Elemental.Earth, 50},
            {Enums.Elemental.Wind, 50},
            {Enums.Elemental.Fire, 50},
            {Enums.Elemental.Thunder, 50},
            {Enums.Elemental.Ice, 50}
            },
        CharactersCanEquip = new List<string>() {"Locke","Terra","Setzer","Cyan","Edgar","Celes"}
        };
        AddArmor(ForceArmor);
        
        var GaiaGear = new Armor { Name = "Gaia Gear", Defense = 53, MagicDefense = 43, Evade = 0, MagicBlock = 0, Stamina = 0, Speed = 0, Strength = 0, MagicPower = 0,
        ElementalDamageAbsorbtion = new Dictionary<Enums.Elemental, int>() {{Enums.Elemental.Earth, 100}},
        CharactersCanEquip = new List<string>() {"Terra","Locke","Celes","Relm","Strago","Sabin","Gogo","Setzer","Shadow","Mog","Gau"}};
        AddArmor(GaiaGear);
        
        var GenjiArmor = new Armor { Name = "Genji Armor", Defense = 90, MagicDefense = 80, Evade = 0, MagicBlock = 0, Stamina = 2, Speed = 3, Strength = 5, MagicPower = 3,
        CharactersCanEquip = new List<string>() {"Terra","Locke","Shadow","Setzer","Edgar","Cyan","Celes"}};
        AddArmor(GenjiArmor);
        
        var GoldenArmor = new Armor { Name = "Golden Armor", Defense = 55, MagicDefense = 37, Evade = 0, MagicBlock = 0, Stamina = 0, Speed = 0, Strength = 0, MagicPower = 0,
        CharactersCanEquip = new List<string>() {"Terra","Shadow","Setzer","Edgar","Cyan","Celes"}};
        AddArmor(GoldenArmor);
       
        var IronArmor = new Armor { Name = "Iron Armor", Defense = 40, MagicDefense = 27, Evade = 0, MagicBlock = 0, Stamina = 0, Speed = -2, Strength = 0, MagicPower = 0,
        CharactersCanEquip = new List<string>() {"Locke","Cyan","Edgar","Setzer"}};
        AddArmor(IronArmor);
        
        var KenpoGi = new Armor { Name = "Kenpo Gi", Defense = 34, MagicDefense = 23, Evade = 0, MagicBlock = 0, Stamina = 0, Speed = 0, Strength = 0, MagicPower = 0,
        CharactersCanEquip = new List<string>() {"Locke","Sabin","Shadow","Gau"}};
        AddArmor(KenpoGi);
        
        var LeatherArmor = new Armor { Name = "Leather Armor", Defense = 28, MagicDefense = 19, Evade = 0, MagicBlock = 0, Stamina = 0, Speed = 0, Strength = 0, MagicPower = 0,
        CharactersCanEquip = new List<string>() {"Locke","Terra","Cyan","Celes","Relm","Strago","Edgar","Gogo","Setzer","Shadow","Mog","Gau"}};
        AddArmor(LeatherArmor);
        
        var LuminousRobe = new Armor { Name = "Luminous Robe", Defense = 60, MagicDefense = 48, Evade = 0, MagicBlock = 0, Stamina = 0, Speed = 0, Strength = 0, MagicPower = 2,
        CharactersCanEquip = new List<string>() {"Strago","Relm","Gogo"}};
        AddArmor(LuminousRobe);
        
        var MagusRobe = new Armor { Name = "Magus Robe", Defense = 68, MagicDefense = 50, Evade = 0, MagicBlock = 10, Stamina = 0, Speed = 0, Strength = 0, MagicPower = 5,
        CharactersCanEquip = new List<string>() {"Strago","Relm","Gogo"}};
        AddArmor(MagusRobe);
        
        // MP boost does not stack w/ other MP-raising equipment
        var Minerva = new Armor { Name = "Minerva", Defense = 88, MagicDefense = 70, Evade = 0, MagicBlock = 10, Stamina = 1, Speed = 2, Strength = 1, MagicPower = 4,
        MPMultiplier = 1.25f, 
        ElementalDamageReduction = new Dictionary<Enums.Elemental, int> () {
            {Enums.Elemental.Water, 50},
            {Enums.Elemental.Earth, 50},
            {Enums.Elemental.Holy, 50},
            {Enums.Elemental.Poison, 50}
        },
        ElementalDamageAbsorbtion = new Dictionary<Enums.Elemental, int> () {
            {Enums.Elemental.Wind, 100},
            {Enums.Elemental.Thunder, 100},
            {Enums.Elemental.Ice, 100},
            {Enums.Elemental.Fire, 100}
        },
        CharactersCanEquip = new List<string>() {"Terra","Celes"}};
        AddArmor(Minerva);
        
        // Creates Image status on wearer
        var MirageVest = new Armor { Name = "Mirage Vest", Defense = 46, MagicDefense = 36, Evade = 0, MagicBlock = 10, Stamina = 0, Speed = 6, Strength = 0, MagicPower = 0,
        StatusesInduced = new List<Enums.Status> () {Enums.Status.Image},
        CharactersCanEquip = new List<string>() {"Locke","Terra","Cyan","Celes","Strago","Relm","Edgar","Sabin","Gogo","Setzer","Shadow","Mog","Gau"}};
        AddArmor(MirageVest);

        // Gives moogle sprite in battle
        var MoogleSuit = new Armor { Name = "Moogle Suit", Defense = 58, MagicDefense = 52, Evade = 0, MagicBlock = 0, Stamina = 0, Speed = 0, Strength = 0, MagicPower = 5,
        ElementalDamageReduction = new Dictionary<Enums.Elemental, int> () {{Enums.Elemental.Poison, 100}},
        StatusesBlocked = new List<Enums.Status> () {Enums.Status.Poisoned},
        CharactersCanEquip = new List<string>() {"Stragus","Relm"}
        };
        AddArmor(MoogleSuit);

        var MythrilMail = new Armor { Name = "Mythril Mail", Defense = 51, MagicDefense = 34, Evade = 0, MagicBlock = 0, Stamina = 0, Speed = 0, Strength = 0, MagicPower = 0,
        CharactersCanEquip = new List<string>() {"Terra","Locke","Cyan","Edgar","Celes","Setzer"}};
        AddArmor(MythrilMail);

        var MythrilVest = new Armor { Name = "Mythril Vest", Defense = 45, MagicDefense = 30, Evade = 0, MagicBlock = 0, Stamina = 0, Speed = 0, Strength = 0, MagicPower = 0,
        CharactersCanEquip = new List<string>() {"Locke","Terra","Cyan","Celes","Strago","Relm","Edgar","Sabin","Gogo","Setzer","Shadow","Mog","Gau"}};
        AddArmor(MythrilVest);

        var NinjaGear = new Armor { Name = "Ninja Gear", Defense = 47, MagicDefense = 32, Evade = 0, MagicBlock = 0, Stamina = 0, Speed = 2, Strength = 0, MagicPower = 0,
        CharactersCanEquip = new List<string>() {"Locke","Shadow","Setzer","Sabin","Gau","Gogo"}};
        AddArmor(NinjaGear);

        var NutkinSuit = new Armor { Name = "Nutkin Suit", Defense = 86, MagicDefense = 67, Evade = 0, MagicBlock = 0, Stamina = 0, Speed = 7, Strength = 0, MagicPower = 3,
        CharactersCanEquip = new List<string>() {"Strago","Relm"}};
        AddArmor(NutkinSuit);

    }

}