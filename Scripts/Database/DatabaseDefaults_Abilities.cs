using System.Collections.Generic;
using LiteDB;
using System.Linq;
using Godot;


public static class DatabaseDefaults_Abilities
{

    private static void AddAbility(Ability Ability)
	{
		var AbilityResult = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == Ability.Name);

		if( !(AbilityResult is null) ) {
			GD.Print($"{Ability.Name} is already in database, aborting add ability.");
			return;
		}

		DatabaseHandler.AbilityCollection.Insert(Ability);
		DatabaseHandler.AbilityCollection.EnsureIndex("Name", true);

		GD.Print($"{Ability.Name} added to database.");
	}


    public static void AddAbilities()
    {
        #region EnemyAbilities

        // Standard enemy physical attach, stats will vary/be based on strength, etc...
        var Attack = new Ability {Name = "Attack", Type = Enums.AbilityType.EnemyNormal, RandomPercentage = .8f}; AddAbility(Attack);
        var Critical = new Ability {Name = "Critical", Type = Enums.AbilityType.EnemySpecial, RandomPercentage = .8f}; AddAbility(Critical);

        var Dagger = new Ability {Name = "Dagger", Type = Enums.AbilityType.EnemySpecial}; AddAbility(Dagger);
        var DragonClaws = new Ability {Name = "Dragon Claws", Type = Enums.AbilityType.EnemySpecial}; AddAbility(DragonClaws);
        var Chomp = new Ability {Name = "Chomp", Type = Enums.AbilityType.EnemySpecial}; AddAbility(Chomp);

        var MegaVolt = new Ability {Name = "Mega Volt", Type = Enums.AbilityType.EnemySpecial}; AddAbility(MegaVolt);
        var GigaVolt = new Ability {Name = "Giga Volt", Type = Enums.AbilityType.EnemySpecial}; AddAbility(GigaVolt);
        var Rush = new Ability {Name = "Rush", Type = Enums.AbilityType.EnemySpecial}; AddAbility(Rush);

        #endregion


        #region Spells

        List<Ability> BlackMagic = new List<Ability> {
            new Ability {Name = "Bio", Power = 53, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 120, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Bolt", Power = 20, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 150, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Bolt 2", Power = 61, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 150, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Bolt 3", Power = 120, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 150, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Break", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 120, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Demi", Power = 8, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 120, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Doom", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 95, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Drain", Power = 38, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 120, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Fire", Power = 21, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 150, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Fire 2", Power = 60, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 150, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Fire 3", Power = 121, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 150, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Flare", Power = 60, Physical = false, IgnoreDefense = true, Unblockable = false, HitRate = 150, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Ice", Power = 22, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 150, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Ice 2", Power = 62, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 150, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Ice 3", Power = 122, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 150, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Merton", Power = 138, Physical = false, IgnoreDefense = true, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Meteor", Power = 36, Physical = false, IgnoreDefense = true, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Pearl", Power = 108, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 150, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Poison", Power = 25, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 100, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Quake", Power = 111, Physical = false, IgnoreDefense = true, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Quarter", Power = 12, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 100, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "Ultima", Power = 150, Physical = false, IgnoreDefense = true, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "W Wind", Power = 15, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 100, Type = Enums.AbilityType.Spell, MagicClass = "Black"},
            new Ability {Name = "X-Zone", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 85, Type = Enums.AbilityType.Spell, MagicClass = "Black"}
        };


        List<Ability> GrayMagic = new List<Ability> {
            new Ability {Name = "Berserk", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 150, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Dispel", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Float", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Haste", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Haste 2", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Imp", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 100, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Muddle", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 94, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Mute", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 100, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Osmose", Power = 26, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 150, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Quick", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Rasp", Power = 10, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 150, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Reflect", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Safe", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Scan", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 222, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Shell", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Sleep", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 111, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Slow", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 120, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Slow 2", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 150, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Stop", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = false, HitRate = 100, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Vanish", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "Gray"},
            new Ability {Name = "Warp", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "Gray"}
        };



        List<Ability> WhiteMagic = new List<Ability> {
            new Ability {Name = "Antidote", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "White"},
            new Ability {Name = "Cure", Power = 10, Physical = false, IgnoreDefense = true, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "White"},
            new Ability {Name = "Cure 2", Power = 28, Physical = false, IgnoreDefense = true, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "White"},
            new Ability {Name = "Cure 3", Power = 66, Physical = false, IgnoreDefense = true, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "White"},
            new Ability {Name = "Life", Power = 2, Physical = false, IgnoreDefense = true, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "White"},
            new Ability {Name = "Life 2", Power = 16, Physical = false, IgnoreDefense = true, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "White"},
            new Ability {Name = "Life 3", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "White"},
            new Ability {Name = "Regen", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "White"},
            new Ability {Name = "Remedy", Power = 0, Physical = false, IgnoreDefense = false, Unblockable = true, Type = Enums.AbilityType.Spell, MagicClass = "White"}
        };


        foreach(var ability in BlackMagic)
            AddAbility(ability);
        foreach(var ability in GrayMagic)
            AddAbility(ability);
        foreach(var ability in WhiteMagic)
            AddAbility(ability);

        #endregion
    }


}