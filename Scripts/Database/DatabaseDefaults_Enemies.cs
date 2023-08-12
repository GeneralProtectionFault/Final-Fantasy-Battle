using System.Collections.Generic;
using Godot;
using System.Linq;
using LiteDB;


public static class DatabaseDefaults_Enemies
{

    private static void AddEnemy(Enemy Enemy)
	{
		var EnemyResult = DatabaseHandler.EnemyCollection.FindOne(x => x.Name == Enemy.Name);
        
		if( !(EnemyResult is null) ) {
			GD.Print($"{Enemy.Name} is already in database, aborting add enemy.");
			return;
		}

		DatabaseHandler.EnemyCollection.Insert(Enemy);
		DatabaseHandler.EnemyCollection.EnsureIndex("Name", true);

		GD.Print($"{Enemy.Name} added to database.");
	}



    public static void AddEnemies()
    {
        #region Guard

        var Guard = new Enemy
        {
            Name = "Guard",
            Level = 5,

            Attack = 16,
            Magic = 6,
            Defense = 100,
            MagicDefense = 140,
            MagicEvasion = 0,
            Agility = 30,
            HitRate = 100,
            Evasion = 0,
            Experience = 48,
            Gil = 48,

            ElementsWeak = new Dictionary<Enums.Elemental, float> () 
            {
                {Enums.Elemental.Poison, .5f}
            },

            BattleAbilities = new List<Ability>
            {
                DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Dagger"),
                DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Critical")
            },

            RageAbilities = new List<Ability>
            {
                DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Attack"),
                DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Critical")
            },

            StolenItems = new Dictionary<string, float> ()
            {
                {"Potion", .875f},
                {"Hi-Potion", .125f}
            },

            DroppedItems = new Dictionary<string, float> ()
            {
                {"Potion", .875f}
            },

            RagnarokItems = new Dictionary<string, float> ()
            {
                {"Antidote", .25f},
                {"Green Cherry", .25f},
                {"Eyedrops", .25f},
                {"Gold Needle", .25f}
            }
        };

        AddEnemy(Guard);

        #endregion

        #region SilverLobo
        
        var result = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Dragon Claws");

        var SilverLobo = new Enemy
        {
            Name = "Silver Lobo",
            Level = 5,

            Attack = 20,
            Magic = 3,
            Defense = 80,
            MagicDefense = 120,
            MagicEvasion = 0,
            Agility = 35,
            HitRate = 100,
            Evasion = 0,
            Experience = 37,
            Gil = 30,

            ElementsWeak = new Dictionary<Enums.Elemental, float> () 
            {
                {Enums.Elemental.Fire, .5f}
            },

            
            
            BattleAbilities = new List<Ability>
            {
                DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Dragon Claws"),
                DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Chomp")
            },

            RageAbilities = new List<Ability>
            {
                DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Attack"),
                DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Chomp")
            },

            SketchAbilities = new Dictionary<string, float>
            {
                {DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Mega Volt").Name, .75f},
                {DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Giga Volt").Name, .25f}
            },

            ControlConfuseAbilities = new Dictionary<string, float>
            {
                {DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Mega Volt").Name, .25f},
                {DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Attack").Name, .75f}
            },

            StolenItems = new Dictionary<string, float> ()
            {
                {"Tonic", .875f},
                {"Mithril Claw", .125f}
            },

            DroppedItems = new Dictionary<string, float> ()
            {
                {"Potion", .875f}
            },

            RagnarokItems = new Dictionary<string, float> ()
            {
                {"Dried Meat", 1f}
                // {"Dried Meat", .25f},
                // {"Dried Meat", .25f},
                // {"Dried Meat", .25f}
            }
        };

        AddEnemy(SilverLobo);

        #endregion


        #region Rhinotaur
        
        // Delivers Critical hits when in Imp status
        var Rhinotaur = new Enemy
        {
            Name = "Rhinotaur",
            Level = 8,

            Attack = 25,
            Magic = 10,
            Defense = 100,
            MagicDefense = 155,
            MagicEvasion = 0,
            Agility = 35,
            HitRate = 100,
            Evasion = 0,
            Experience = 246,
            Gil = 186,

            Hp = 232,
            HpMax = 232,
            Mp = 100,
            MpMax = 100,


            ElementsWeak = new Dictionary<Enums.Elemental, float> () 
            {
                {Enums.Elemental.Fire, .5f}
            },

            
            
            BattleAbilities = new List<Ability>
            {
                DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Dragon Claws"),
                DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Rush")
            },

            RageAbilities = new List<Ability>
            {
                DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Dragon Claws"),
                DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Mega Volt")
            },

            StolenItems = new Dictionary<string, float> ()
            {
                {"Potion", .875f}
            },

            DroppedItems = new Dictionary<string, float> ()
            {
                {"Potion", .875f}
            },

            RagnarokItems = new Dictionary<string, float> ()
            {
                {"Dried Meat", 1f}
                // {"Dried Meat", .25f},
                // {"Dried Meat", .25f},
                // {"Dried Meat", .25f}
            }
        };

        AddEnemy(Rhinotaur);
        #endregion

    }


}