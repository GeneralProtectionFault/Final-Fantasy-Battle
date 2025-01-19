using System.Collections.Generic;
using Godot;
using System.Linq;


public static class DatabaseDefaults_Characters
{
    private static void AddCharacter(Character Character)
	{
		var CharacterResult = DatabaseHandler.CharacterCollection.FindOne(x => x.Name == Character.Name);

		if( !(CharacterResult is null) ) {
			GD.Print($"{Character.Name} is already in database, aborting add character.");
			return;
		}
        
		DatabaseHandler.CharacterCollection.Insert(Character);
		DatabaseHandler.CharacterCollection.EnsureIndex("Name", true);

        DatabaseHandler.GameDatabase.Commit();

		GD.Print($"{Character.Name} added to database.");
	}



    public static void AddCharacters()
    {

        #region Tina

        var Tina = new Character
		{
			Name = "Tina",
            Level = 3,

            Strength = 31,
            Agility = 33,
            Stamina = 28,
            Magic = 39,
            Attack = 22,
            Defense = 42,
            Evasion = 5,
            MagicDefense = 33,
            MagicEvasion = 7,
            EscapeSuccess = 4,

            ExtraHP = 40,
            ExtraMP = 16,

            Hp = 40,
            HpMax = 40,
            Mp = 45,
            MpMax = 45,

            InitialLevelOffset = 0,

            InParty = true,

            Statuses = new List<Enums.Status>(),

            // MagicList = new List<Ability>() {
            //     DatabaseHandler.AbilityCollection.FindOne(x => x.AbilityName == "Cure"),
            //     DatabaseHandler.AbilityCollection.FindOne(x => x.AbilityName == "Fire")
            // },

            // Test all spells for scrolling
            MagicList = DatabaseHandler.AbilityCollection.Find("$.Type = 'Spell'").ToList(),

            NativeMagicActivationList = new Dictionary<string, int>(),

            UniqueAbilityList = new List<Ability> {
                new Ability {Name = "Morph", Type = Enums.AbilityType.Unique}
            },

            RowPosition = Enums.BattleRowPositions.FrontRow,
            IsPartyLead = false,

            RightHandEquipped = "Lightbringer"
        };

        
        // Add Native Spells
        Tina.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Cure").Name, 1);
        Tina.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Fire").Name, 3);
        Tina.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Antidote").Name, 6);
        Tina.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Drain").Name, 12);
        Tina.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Life").Name, 18);
        Tina.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Fire 2").Name, 22);
        Tina.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Warp").Name, 26);
        Tina.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Cure 2").Name, 33);
        Tina.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Dispel").Name, 37);
        Tina.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Fire 3").Name, 43);
        Tina.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Life 2").Name, 49);
        Tina.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Pearl").Name, 57);
        Tina.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Break").Name, 68);
        Tina.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Quarter").Name, 75);
        Tina.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Merton").Name, 86);
        Tina.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Ultima").Name, 99);

        AddCharacter(Tina);
        
        #endregion


        #region Locke

        var Locke = new Character
                {
                    Name = "Locke",
                    Level = 5,

                    Strength = 37,
                    Agility = 40,
                    Stamina = 31,
                    Magic = 28,
                    Attack = 24,
                    Defense = 46,
                    Evasion = 15,
                    MagicDefense = 23,
                    MagicEvasion = 2,
                    EscapeSuccess = 5,

                    ExtraHP = 48,
                    ExtraMP = 7,

                    Hp = 54,
                    HpMax = 60,
                    Mp = 35,
                    MpMax = 35,

                    InitialLevelOffset = 2,

                    InParty = true,

                    Statuses = new List<Enums.Status>(),

                    MagicList = new List<Ability>(),

                    UniqueAbilityList = new List<Ability> {
                        new Ability {Name = "Steal", IsActive = true, Type = Enums.AbilityType.Unique}
                    },

                    RowPosition = Enums.BattleRowPositions.FrontRow,
                    IsPartyLead = true,

                    RightHandEquipped = "Lightbringer"
                };
        
        AddCharacter(Locke);

        #endregion


        #region Cyan

        var Cyan = new Character
                {
                    Name = "Cyan",
                    Level = 10,

                    Strength = 40,
                    Agility = 28,
                    Stamina = 33,
                    Magic = 25,
                    Attack = 35,
                    Defense = 48,
                    Evasion = 6,
                    MagicDefense = 20,
                    MagicEvasion = 1,
                    EscapeSuccess = 3,

                    ExtraHP = 53,
                    ExtraMP = 5,

                    Hp = 120,
                    HpMax = 120,
                    Mp = 35,
                    MpMax = 35,

                    InitialLevelOffset = 2,

                    InParty = false,

                    Statuses = new List<Enums.Status>(),

                    MagicList = new List<Ability>(),

                    UniqueAbilityList = new List<Ability> {
                        new Ability {Name = "Dispatch", IsActive = true, Type = Enums.AbilityType.SwordTech,
                        Power = 120, Physical = true, IgnoreDefense = true, Unblockable = true},
                        new Ability {Name = "Retort", IsActive = true, Type = Enums.AbilityType.SwordTech,
                        Power = 56, Physical = false, IgnoreDefense = true, Unblockable = true},
                        new Ability {Name = "Slash", IsActive = true, Type = Enums.AbilityType.SwordTech,
                        Power = 8, Physical = true, IgnoreDefense = false, Unblockable = true},
                        new Ability {Name = "Quadra Slam", IsActive = true, Type = Enums.AbilityType.SwordTech,
                        Power = 72, Physical = true, IgnoreDefense = false, Unblockable = true},
                        new Ability {Name = "Empowerer", IsActive = true, Type = Enums.AbilityType.SwordTech,
                        Power = 49, Physical = false, IgnoreDefense = true, Unblockable = true},
                        // The main damage portion of Stunner is unblockable, but the special
                        // effect will only try to inflict the ailment 116/256 of the time
                        // (116 = 256 - 140).
                        new Ability {Name = "Stunner", IsActive = true, Type = Enums.AbilityType.SwordTech,
                        Power = 97, Physical = false, IgnoreDefense = false, Unblockable = true, HitRate = 140},
                        new Ability {Name = "Quadra Slice", IsActive = true, Type = Enums.AbilityType.SwordTech,
                        Power = 70, Physical = true, IgnoreDefense = true, Unblockable = true},
                        new Ability {Name = "Cleave", IsActive = true, Type = Enums.AbilityType.SwordTech,
                        Power = 0, Physical = false, IgnoreDefense = true, Unblockable = false, HitRate = 182}
                    },

                    RowPosition = Enums.BattleRowPositions.FrontRow,
                    IsPartyLead = false
                };

        
        AddCharacter(Cyan);

        #endregion


        #region Shadow

        var Shadow = new Character
                {
                    Name = "Shadow",
                    Level = 10,

                    Strength = 39,
                    Agility = 38,
                    Stamina = 30,
                    Magic = 33,
                    Attack = 33,
                    Defense = 47,
                    Evasion = 28,
                    MagicDefense = 25,
                    MagicEvasion = 9,
                    EscapeSuccess = 5,

                    ExtraHP = 51,
                    ExtraMP = 6,

                    Hp = 120,
                    HpMax = 120,
                    Mp = 35,
                    MpMax = 35,

                    InitialLevelOffset = 0,

                    InParty = false,

                    Statuses = new List<Enums.Status>(),

                    MagicList = new List<Ability>(),

                    UniqueAbilityList = new List<Ability> {
                        new Ability {Name = "Throw", IsActive = true, Type = Enums.AbilityType.Unique}
                    },

                    RowPosition = Enums.BattleRowPositions.FrontRow,
                    IsPartyLead = false
                };

        
        AddCharacter(Shadow);

        #endregion


        #region Edgar

        var Edgar = new Character
            {
                Name = "Edgar",
                Level = 10,

                Strength = 39,
                Agility = 30,
                Stamina = 34,
                Magic = 29,
                Attack = 30,
                Defense = 50,
                Evasion = 4,
                MagicDefense = 22,
                MagicEvasion = 1,
                EscapeSuccess = 4,

                ExtraHP = 49,
                ExtraMP = 6,

                Hp = 120,
                HpMax = 120,
                Mp = 35,
                MpMax = 35,

                InitialLevelOffset = 0,

                InParty = false,

                Statuses = new List<Enums.Status>(),

                MagicList = new List<Ability>(),

                UniqueAbilityList = new List<Ability> {
                    // Full damage to multiple targets
                    new Ability {Name = "AutoCrossbow", IsActive = true, Type = Enums.AbilityType.Tool,
                    Power = 125, Physical = true, IgnoreDefense = false, Unblockable = true},
                    // Confuse multiple targets
                    // Works unless target is immune
                    new Ability {Name = "NoiseBlaster", Type = Enums.AbilityType.Tool,
                    Physical = false, IgnoreDefense = false, Unblockable = true},
                    // Poison status & unblockable poison magic damage
                    // Full damage to multiple targets
                    new Ability {Name = "BioBlaster", Type = Enums.AbilityType.Tool,
                    Power = 20, Physical = false, IgnoreDefense = false, Unblockable = true},
                    // Darkness status & unblockable magic damage to multiple targets
                    // Full damage to multiple targets
                    new Ability {Name = "Flash", Type = Enums.AbilityType.Tool,
                    Power = 42, Physical = false, IgnoreDefense = false, Unblockable = true},
                    // Randomly inflict "incapacitated" (Death), immunity or stamina can "block"
                    new Ability {Name = "Chainsaw", Type = Enums.AbilityType.Tool,
                    Power = 252, Physical = true, IgnoreDefense = true, Unblockable = true},
                    // Make an enemy wak to Flame, Ice, Thunder, Water, Wind, Earth, Poison or Holy (random)
                    new Ability {Name = "Debilitator", Type = Enums.AbilityType.Tool,
                    Physical = false, IgnoreDefense = true, Unblockable = true},

                    new Ability {Name = "Drill", Type = Enums.AbilityType.Tool,
                    Power = 191, Physical = true, IgnoreDefense = true, Unblockable = true},
                    // Target dies at next action
                    // Unblockable unless target is immune
                    new Ability {Name = "AirAnchor", Type = Enums.AbilityType.Tool,
                    Power = 128, Physical = true, IgnoreDefense = true, Unblockable = true}
                    },

                    RowPosition = Enums.BattleRowPositions.FrontRow,
                    IsPartyLead = false
            };

        
        AddCharacter(Edgar);


        #endregion


        #region Sabin

        var Sabin = new Character
        {
            Name = "Sabin",
            Level = 10,

            Strength = 47,
            Agility = 37,
            Stamina = 39,
            Magic = 28,
            Attack = 36,
            Defense = 53,
            Evasion = 12,
            MagicDefense = 21,
            MagicEvasion = 4,
            EscapeSuccess = 4,

            ExtraHP = 58,
            ExtraMP = 3,

            Hp = 120,
            HpMax = 120,
            Mp = 35,
            MpMax = 35,

            InitialLevelOffset = 2,

            InParty = false,

            Statuses = new List<Enums.Status>(),

            MagicList = new List<Ability>(),

            UniqueAbilityList = new List<Ability> {
                new Ability {Name = "Pummel", Type = Enums.AbilityType.Blitz,
                Power = 110, Physical = true, IgnoreDefense = true, Unblockable = true},
                new Ability {Name = "Aurabolt", Type = Enums.AbilityType.Blitz,
                Power = 110, Physical = false, IgnoreDefense = false, Unblockable = true},
                new Ability {Name = "Suplex", Type = Enums.AbilityType.Blitz,
                Power = 110, Physical = true, IgnoreDefense = true, Unblockable = true},
                new Ability {Name = "FireDance", Type = Enums.AbilityType.Blitz,
                Power = 110, Physical = true, IgnoreDefense = true, Unblockable = true},
                new Ability {Name = "Mantra", Type = Enums.AbilityType.Blitz,
                Power = 1, Physical = false, IgnoreDefense = true, Unblockable = true},
                new Ability {Name = "AirBlade", Type = Enums.AbilityType.Blitz,
                Power = 78, Physical = false, IgnoreDefense = false, Unblockable = true},
                new Ability {Name = "Spiraler", Type = Enums.AbilityType.Blitz,
                Power = 200, Physical = false, IgnoreDefense = true, Unblockable = true},
                new Ability {Name = "BumRush", Type = Enums.AbilityType.Blitz,
                Power = 128, Physical = false, IgnoreDefense = true, Unblockable = true}
                
            },

            RowPosition = Enums.BattleRowPositions.FrontRow,
            IsPartyLead = false
        };

        AddCharacter(Sabin);

        #endregion


        #region Celes

        var Celes = new Character
        {
            Name = "Celes",
            Level = 10,

            Strength = 34,
            Agility = 34,
            Stamina = 31,
            Magic = 36,
            Attack = 26,
            Defense = 44,
            Evasion = 7,
            MagicDefense = 31,
            MagicEvasion = 9,
            EscapeSuccess = 4,

            ExtraHP = 44,
            ExtraMP = 15,

            Hp = 120,
            HpMax = 120,
            Mp = 35,
            MpMax = 35,

            InitialLevelOffset = 0,

            InParty = false,
            Statuses = new List<Enums.Status>(),

            MagicList = new List<Ability>(),
            NativeMagicActivationList = new Dictionary<string, int>(),

            UniqueAbilityList = new List<Ability> {
                new Ability {Name = "Runic", Type = Enums.AbilityType.Unique}
                
            },

            RowPosition = Enums.BattleRowPositions.FrontRow,
            IsPartyLead = false
        };
        
        // Adjust native spells as appropriate
        Celes.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Ice").Name, 1);
        Celes.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Cure").Name, 4);
        Celes.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Antidote").Name, 8);
        Celes.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Imp").Name, 13);
        Celes.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Scan").Name, 18);
        Celes.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Safe").Name, 22);
        Celes.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Ice 2").Name, 26);
        Celes.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Haste").Name, 32);
        Celes.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Muddle").Name, 32);
        Celes.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Berserk").Name, 40);
        Celes.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Ice 3").Name, 42);
        Celes.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Vanish").Name, 48);
        Celes.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Haste 2").Name, 52);
        Celes.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Pearl").Name, 72);
        Celes.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Flare").Name, 81);
        Celes.NativeMagicActivationList.Add(DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Meteor").Name, 98);        

        AddCharacter(Celes);


        #endregion


        #region Stragus

        var Stragus = new Character
        {
            Name = "Stragus",
            Level = 10,

            Strength = 28,
            Agility = 25,
            Stamina = 19,
            Magic = 34,
            Attack = 20,
            Defense = 33,
            Evasion = 6,
            MagicDefense = 27,
            MagicEvasion = 7,
            EscapeSuccess = 3,

            ExtraHP = 35,
            ExtraMP = 13,

            Hp = 120,
            HpMax = 120,
            Mp = 35,
            MpMax = 35,

            InitialLevelOffset = 2,

            InParty = false,

            Statuses = new List<Enums.Status>(),

            MagicList = new List<Ability>(),

            UniqueAbilityList = new List<Ability> {
                new Ability {Name = "Aero", Type = Enums.AbilityType.Lore,
                Power = 125, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 150},
                new Ability {Name = "Aqua Rake", IsActive = true, Type = Enums.AbilityType.Lore,
                Power = 71, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 150},
                new Ability {Name = "Big Guard", Type = Enums.AbilityType.Lore,
                Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true},
                new Ability {Name = "Blow Fish", Type = Enums.AbilityType.Lore,
                Power = 1, Physical = false, IgnoreDefense = true, Unblockable = true},
                new Ability {Name = "Cleansweep", Type = Enums.AbilityType.Lore,
                Power = 50, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 150},
                new Ability {Name = "Condemned", Type = Enums.AbilityType.Lore,
                Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true},
                new Ability {Name = "Dischord", Type = Enums.AbilityType.Lore,
                Power = 0, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 100},
                new Ability {Name = "Exploder", Type = Enums.AbilityType.Lore,
                Power = 1, Physical = false, IgnoreDefense = true, Unblockable = true},
                new Ability {Name = "ForceField", Type = Enums.AbilityType.Lore,
                Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true},
                new Ability {Name = "GrandTrain", Type = Enums.AbilityType.Lore,
                Power = 84, Physical = false, IgnoreDefense = true, Unblockable = true},
                new Ability {Name = "L.3 Muddle", Type = Enums.AbilityType.Lore,
                Power = 0, Physical = false, IgnoreDefense = false, Unblockable = false},
                new Ability {Name = "L.4 Flare", Type = Enums.AbilityType.Lore,
                Power = 66, Physical = false, IgnoreDefense = true, Unblockable = false},
                new Ability {Name = "L.5 Doom", Type = Enums.AbilityType.Lore,
                Power = 0, Physical = false, IgnoreDefense = false, Unblockable = false},
                new Ability {Name = "L? Pearl", Type = Enums.AbilityType.Lore,
                Power = 120, Physical = false, IgnoreDefense = false, Unblockable = false},
                new Ability {Name = "Pearl Wind", Type = Enums.AbilityType.Lore,
                Power = 1, Physical = false, IgnoreDefense = true, Unblockable = true},
                new Ability {Name = "Pep Up", Type = Enums.AbilityType.Lore,
                Power = 16, Physical = false, IgnoreDefense = false, Unblockable = true},
                new Ability {Name = "Quasar", Type = Enums.AbilityType.Lore,
                Power = 57, Physical = false, IgnoreDefense = true, Unblockable = true},
                new Ability {Name = "Reflect???", Type = Enums.AbilityType.Lore,
                Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true},
                new Ability {Name = "Revenge", Type = Enums.AbilityType.Lore,
                Power = 1, Physical = false, IgnoreDefense = true, Unblockable = true},
                new Ability {Name = "Rippler", Type = Enums.AbilityType.Lore,
                Power = 0, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 111},
                new Ability {Name = "Roulette", Type = Enums.AbilityType.Lore,
                Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true},
                new Ability {Name = "Sour Mouth", Type = Enums.AbilityType.Lore,
                Power = 0, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 100},
                new Ability {Name = "Step Mine", Type = Enums.AbilityType.Lore,
                Power = 32, Physical = false, IgnoreDefense = false, Unblockable = false},
                new Ability {Name = "Stone", Type = Enums.AbilityType.Lore,
                Power = 40, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 75}
            },

            RowPosition = Enums.BattleRowPositions.FrontRow,
            IsPartyLead = false
        };

        AddCharacter(Stragus);

        #endregion


        #region Relm

        var Relm = new Character
        {
            Name = "Relm",
            Level = 10,

            Strength = 26,
            Agility = 34,
            Stamina = 22,
            Magic = 44,
            Attack = 21,
            Defense = 35,
            Evasion = 13,
            MagicDefense = 30,
            MagicEvasion = 9,
            EscapeSuccess = 5,

            ExtraHP = 37,
            ExtraMP = 18,

            Hp = 120,
            HpMax = 120,
            Mp = 35,
            MpMax = 35,

            InitialLevelOffset = 0,

            InParty = false,

            Statuses = new List<Enums.Status>(),

            MagicList = new List<Ability>(),

            UniqueAbilityList = new List<Ability> {
                new Ability {Name = "Sketch", IsActive = true, Type = Enums.AbilityType.Unique},
                new Ability {Name = "Control", Type = Enums.AbilityType.Unique}
            },

            RowPosition = Enums.BattleRowPositions.FrontRow,
            IsPartyLead = false
        };

        AddCharacter(Relm);

        #endregion


        #region Setzer

        var Setzer = new Character
        {
            Name = "Setzer",
            Level = 10,

            Strength = 36,
            Agility = 32,
            Stamina = 32,
            Magic = 29,
            Attack = 28,
            Defense = 48,
            Evasion = 9,
            MagicDefense = 26,
            MagicEvasion = 1,
            EscapeSuccess = 4,

            ExtraHP = 46,
            ExtraMP = 9,

            Hp = 120,
            HpMax = 120,
            Mp = 35,
            MpMax = 35,

            InitialLevelOffset = 0,

            InParty = false,

            Statuses = new List<Enums.Status>(),

            MagicList = new List<Ability>(),

            UniqueAbilityList = new List<Ability> {
                new Ability {Name = "Slot", IsActive = true, Type = Enums.AbilityType.Unique},
                new Ability {Name = "Coin Toss", Type = Enums.AbilityType.Unique}
            },

            RowPosition = Enums.BattleRowPositions.FrontRow,
            IsPartyLead = false
        };

        AddCharacter(Setzer);

        #endregion


        #region Mog

        var Mog = new Character
        {
            Name = "Mog",
            Level = 10,

            Strength = 29,
            Agility = 36,
            Stamina = 26,
            Magic = 35,
            Attack = 26,
            Defense = 52,
            Evasion = 10,
            MagicDefense = 36,
            MagicEvasion = 12,
            EscapeSuccess = 5,

            ExtraHP = 39,
            ExtraMP = 16,

            Hp = 120,
            HpMax = 120,
            Mp = 35,
            MpMax = 35,

            InitialLevelOffset = 5,

            InParty = false,

            Statuses = new List<Enums.Status>(),

            MagicList = new List<Ability>(),

            UniqueAbilityList = new List<Ability> {
                new Ability {Name = "Wind Song", Type = Enums.AbilityType.Dance},
                new Ability {Name = "Forest Suite", Type = Enums.AbilityType.Dance},
                new Ability {Name = "Desert Aria", Type = Enums.AbilityType.Dance},
                new Ability {Name = "Love Sonata", Type = Enums.AbilityType.Dance},
                new Ability {Name = "Earth Blues", Type = Enums.AbilityType.Dance},
                new Ability {Name = "Water Rondo", Type = Enums.AbilityType.Dance},
                new Ability {Name = "Dusk Requiem", Type = Enums.AbilityType.Dance},
                new Ability {Name = "Snowman Jazz", Type = Enums.AbilityType.Dance}
            },

            RowPosition = Enums.BattleRowPositions.FrontRow,
            IsPartyLead = false
        };

        AddCharacter(Mog);

        #endregion


        #region Gau

        var Gau = new Character
        {
            Name = "Gau",
            Level = 10,

            Strength = 36,
            Agility = 32,
            Stamina = 32,
            Magic = 29,
            Attack = 28,
            Defense = 48,
            Evasion = 9,
            MagicDefense = 26,
            MagicEvasion = 1,
            EscapeSuccess = 4,

            ExtraHP = 46,
            ExtraMP = 9,

            Hp = 120,
            HpMax = 120,
            Mp = 35,
            MpMax = 35,

            InitialLevelOffset = 0,

            InParty = false,

            Statuses = new List<Enums.Status>(),

            MagicList = new List<Ability>(),

            UniqueAbilityList = new List<Ability> {
                new Ability {Name = "Guard", IsActive = true, Type = Enums.AbilityType.Rage},
                new Ability {Name = "Soldier", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Templar", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Ninja", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Samurai", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Orog", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Mag Roader", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Retainer", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Hazer", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Dahling", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Rain Man", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Brawler", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Apokryphos", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Dark Force", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Whisper", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Over-Mind", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Osteosaur", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Commander", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Rhodox", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Were-Rat", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Ursus", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Rhinotaur", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Steroidite", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Leafer", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Stray Cat", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Lobo", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Doberman", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Vomammoth", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Fidor", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Baskervor", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Suriander", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Chimera", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Behemoth", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Mesosaur", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Pterodon", Type = Enums.AbilityType.Rage},
                new Ability {Name = "FossilFang", Type = Enums.AbilityType.Rage},
                new Ability {Name = "White Dragon", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Doom Dragon", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Brachosaur", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Tyranosaur", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Dark Wind", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Beakor", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Vulture", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Harpy", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Hermit Crab", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Trapper", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Hornet", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Crasshopper", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Delta Bug", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Gilomantis", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Trilium", Type = Enums.AbilityType.Rage},
                new Ability {Name = "NightShade", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Tumbleweed", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Bloompire", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Trilobiter", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Siegfried", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Nautiloid", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Exocite", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Anguiform", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Reach Frog", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Lizard", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Chicken Lip", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Hoover", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Rider", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Chupon", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Pipsqueak", Type = Enums.AbilityType.Rage},
                new Ability {Name = "M-Tek Armor", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Sky Armor", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Telstar", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Lethal Weapon", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Vaporite", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Flan", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Ing", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Humpty", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Brainpan", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Cruller", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Cactrot", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Repo Man", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Harvester", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Bomb", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Still Life", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Boxed Set", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Slam Dancer", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Hades Gigas", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Pug", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Magic Urn", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Mover", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Figaliz", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Buffalax", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Aspik", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Ghost", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Crawler", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Sand Ray", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Areneid", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Actaneon", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Sand Horse", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Dark Side", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Mad Oscar", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Crawly", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Bleary", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Marshal", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Trooper", Type = Enums.AbilityType.Rage},
                new Ability {Name = "General", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Covert", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Ogor", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Warlock", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Madam", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Joker", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Iron Fist", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Goblin", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Apparite", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Power Demon", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Displayer", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Vector Pup", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Peepers", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Sewer Rat", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Slatter", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Rhinox", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Rhobite", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Wild Cat", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Red Fang", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Bounty Man", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Tusker", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Ralph", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Chitonid", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Wart Puck", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Rhyos", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Sr Behemoth", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Vectaur", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Wyvern", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Zombone", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Dragon", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Brontaur", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Allosaurus", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Cirpius", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Sprinter", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Gobbler", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Harpiai", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Gloom Shell", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Drop", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Mind Candy", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Weed Feeder", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Luridan", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Toe Cutter", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Over Grunk", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Exoray", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Crusher", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Uroburos", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Primordite", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Sky Cap", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Cephaler", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Maliga", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Gigan Toad", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Geckorex", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Cluck", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Land Worm", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Test Rider", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Pluto Armor", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Tom Thumb", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Heavy Armor", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Chaser", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Scullion", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Poplium", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Intangir", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Misfit", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Eland", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Enuo", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Deep Eye", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Grease Monk", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Neck Hunter", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Grenade", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Critic", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Pan Dora", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Soul Dancer", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Gigantos", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Mag Roader", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Spek Tor", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Parasite", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Earth Guard", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Coelecite", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Anemone", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Hipocampus", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Spectre", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Evil Oscar", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Slurm", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Latimeria", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Still Going", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Allo Ver", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Phase", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Outsider", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Barb-e", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Para Soul", Type = Enums.AbilityType.Rage},
                new Ability {Name = "PM Stalker", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Hemophyte", Type = Enums.AbilityType.Rage},
                new Ability {Name = "SP Forces", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Nohrabbit", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Wizard", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Scrapper", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Ceritops", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Commando", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Opinicus", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Poppers", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Lunaris", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Garm", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Vindr", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Kiwok", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Nastidon", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Rinn", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Insecare", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Vermin", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Mantodea", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Bogy", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Prussian", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Black Dragon", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Adamanchyt", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Dante", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Wirey Dragon", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Dueller", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Psychot", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Muus", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Karkass", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Punisher", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Balloon", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Gabbldegak", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Gt Behemoth", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Scorpion", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Chaos Dragon", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Spit Fire", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Vectagoyle", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Lich", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Osprey", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Bug", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Sea Flower", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Fortis", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Abolisher", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Aquila", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Junk", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Mandrake", Type = Enums.AbilityType.Rage},
                new Ability {Name = "1st Class", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Tap Dancer", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Necromancer", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Borras", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Wild Rat", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Gold Bear", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Innoc", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Trixter", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Red Wolf", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Didalos", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Woolly", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Veteran", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Sky Base", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Iron Hitman", Type = Enums.AbilityType.Rage},
                new Ability {Name = "Io", Type = Enums.AbilityType.Rage},
            },

            RowPosition = Enums.BattleRowPositions.FrontRow,
            IsPartyLead = false
        };

        AddCharacter(Gau);

        #endregion


        #region Gogo

        var Gogo = new Character
        {
            Name = "Gogo",
            Level = 10,

            Strength = 25,
            Agility = 30,
            Stamina = 20,
            Magic = 26,
            Attack = 23,
            Defense = 39,
            Evasion = 10,
            MagicDefense = 25,
            MagicEvasion = 6,
            EscapeSuccess = 4,

            ExtraHP = 36,
            ExtraMP = 12,

            Hp = 120,
            HpMax = 120,
            Mp = 35,
            MpMax = 35,

            InitialLevelOffset = 4,

            InParty = false,
            Statuses = new List<Enums.Status>(),

            MagicList = new List<Ability>(),

            UniqueAbilityList = new List<Ability> {
                new Ability {Name = "Mimic", IsActive = true, Type = Enums.AbilityType.Unique}
            },

            RowPosition = Enums.BattleRowPositions.FrontRow,
            IsPartyLead = false
        };

        AddCharacter(Gogo);

        #endregion


        #region Umaro

        var Umaro = new Character
        {
            Name = "Umaro",
            Level = 10,

            Strength = 25,
            Agility = 30,
            Stamina = 20,
            Magic = 26,
            Attack = 23,
            Defense = 39,
            Evasion = 10,
            MagicDefense = 25,
            MagicEvasion = 6,
            EscapeSuccess = 4,

            ExtraHP = 36,
            ExtraMP = 12,

            Hp = 120,
            HpMax = 120,
            Mp = 35,
            MpMax = 35,

            InitialLevelOffset = 4,

            InParty = false,

            Statuses = new List<Enums.Status>(),

            MagicList = new List<Ability>(),

            UniqueAbilityList = new List<Ability> {
                // Equip Fury Ring => Umaro flings ally at enemy, removes status ailments (confuse, sleep) to that ally
                new Ability {Name = "Body Slam", IsActive = true, Type = Enums.AbilityType.Unique,
                RandomPercentage = .38f, IgnoreDefense = true, Unblockable = true}
            },

            RowPosition = Enums.BattleRowPositions.FrontRow,
            IsPartyLead = false
        };

        AddCharacter(Umaro);

        #endregion



    }


}