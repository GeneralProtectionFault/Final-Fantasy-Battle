using System.Collections.Generic;
using Godot;

public static class DatabaseDefaults_Weapons
{
    private static void AddWeapon(Weapon Weapon)
    {
        var WeaponResult = DatabaseHandler.WeaponCollection.FindOne(x => x.Name == Weapon.Name);

		if( !(WeaponResult is null) ) {
			GD.Print($"{Weapon.Name} is already in database, aborting add weapon.");
			return;
		}

		DatabaseHandler.WeaponCollection.Insert(Weapon);
		DatabaseHandler.WeaponCollection.EnsureIndex("Name", true);

        DatabaseHandler.GameDatabase.Commit();
        
		GD.Print($"{Weapon.Name} added to database.");
    }

    public static void AddWeapons()
    {
        #region Dirks

        var AirKnife = new Weapon { Name = "Air Knife", Attack = 76, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, Element = Enums.Elemental.Wind, WeaponType = Enums.WeaponType.Dirk};  AddWeapon(AirKnife);

        // 25 % chance of trying to instantly kill a target, but won't ever try to kill an enemy immune to instant death attacks. Will always fully restore any Undead target. 
        var AssassinsDagger = new Weapon { Name = "Assassin's Dagger", Attack = 106, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 2, Speed = 3, Evade = 10, MagicEvade = 0,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Doom"), WeaponType = Enums.WeaponType.Dirk}; AddWeapon(AssassinsDagger);

        var Dagger = new Weapon { Name = "Dagger", Attack = 26, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Dirk}; AddWeapon(Dagger);

        var Gladius = new Weapon { Name = "Gladius", Attack = 204, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, MagicEvade = 0, Element = Enums.Elemental.Thunder, WeaponType = Enums.WeaponType.Dirk}; AddWeapon(Gladius);

        // Deals Holy based elemental damage.
        var Ichigeki = new Weapon { Name = "Ichigeki", Attack = 190, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 10, MagicEvade = 0, Element = Enums.Elemental.Thunder, WeaponType = Enums.WeaponType.Dirk}; AddWeapon(Ichigeki);

        // Has a 25 % chance of casting Stop. 
        var Kagenui = new Weapon { Name = "Kagenui", Attack = 220, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Stop"), WeaponType = Enums.WeaponType.Dirk}; AddWeapon(Kagenui);

        var Kunai = new Weapon { Name = "Kunai", Attack = 82, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Dirk}; AddWeapon(Kunai);

        var MainGauche = new Weapon { Name = "Main Gauche", Attack = 59, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 4, Evade = 10, MagicEvade = 0, WeaponType = Enums.WeaponType.Dirk}; AddWeapon(MainGauche);

        // Double damage done to any Human target
        var ManEater = new Weapon { Name = "Man-Eater", Attack = 146, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 10, MagicEvade = 0, WeaponType = Enums.WeaponType.Dirk}; AddWeapon(ManEater);

        var MythrilKnife = new Weapon { Name = "Mythil Knife", Attack = 30, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Dirk}; AddWeapon(MythrilKnife);

        var Oborozuki = new Weapon { Name = "Oborozuki", Attack = 225, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 7, Stamina = 0, Magic = 0, Speed = 7, Evade = 50, MagicEvade = 10, WeaponType = Enums.WeaponType.Dirk}; AddWeapon(Oborozuki);

        var Sakura = new Weapon { Name = "Sakura", Attack = 112, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, Element = Enums.Elemental.Wind, WeaponType = Enums.WeaponType.Dirk}; AddWeapon(Sakura);

        var Sasuke = new Weapon { Name = "Sasuke", Attack = 121, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Dirk}; AddWeapon(Sasuke);

        // Shortsword that sometimes deflects enemy attacks
        var Swordbreaker = new Weapon { Name = "Swordbreaker", Attack = 164, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 30, MagicEvade = 0, WeaponType = Enums.WeaponType.Dirk}; AddWeapon(Swordbreaker);

        // This item is essentially "Steal + Attack". It has a 50% chance of stealing from the target while attacking at the same time. 
        var ThiefsKnife = new Weapon { Name = "Thief's Knife", Attack = 88, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 3, Evade = 10, MagicEvade = 10, WeaponType = Enums.WeaponType.Dirk}; AddWeapon(ThiefsKnife);

        // Barrier-piercing, adds (max HP - current HP) damage to every strike after damage is calculated. 
        var ValiantKnife = new Weapon { Name = "Valiant Knife", Attack = 145, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 10, MagicEvade = 0, WeaponType = Enums.WeaponType.Dirk}; AddWeapon(ValiantKnife);

        // Has a 25% chance of casting Sleep on target.
        var ZwillCrossblade = new Weapon { Name = "Zwill Crossblae", Attack = 220, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 3, Stamina = 3, Magic = 0, Speed = 7, Evade = 30, MagicEvade = 20, Element = Enums.Elemental.Wind,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Sleep"), WeaponType = Enums.WeaponType.Dirk}; AddWeapon(ZwillCrossblade);


        #endregion

        #region Knives

        var Ashura = new Weapon { Name = "Ashura", Attack = 57, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Knife}; AddWeapon(Ashura);

        //  Has a 50 % chance of dealing no damage with the physical strike and replacing it with a Wind Slash attack in all enemies. This is compatible with a Master's Scroll. 
        var Kazekiri = new Weapon { Name = "Kazekiri", Attack = 101, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, Element = Enums.Elemental.Wind, WeaponType = Enums.WeaponType.Knife}; AddWeapon(Kazekiri);

        var Kikuichimonji = new Weapon { Name = "Kiku-ichimonji", Attack = 81, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Knife}; AddWeapon(Kikuichimonji);

        var Kotetsu = new Weapon { Name = "Kotetsu", Attack = 66, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Knife}; AddWeapon(Kotetsu);

        var Masamune = new Weapon { Name = "Masamune", Attack = 162, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Knife}; AddWeapon(Masamune);

        var Murakumo = new Weapon { Name = "Murakumo", Attack = 199, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Knife}; AddWeapon(Murakumo);

        // Dancing blade that helps deflect enemy attacks
        var Murasame = new Weapon { Name = "Murasame", Attack = 110, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Knife}; AddWeapon(Murasame);

        var Mutsunokami = new Weapon { Name = "Mutsunokami", Attack = 215, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 20, MagicEvade = 0, WeaponType = Enums.WeaponType.Knife}; AddWeapon(Mutsunokami);

        var Zanmato = new Weapon { Name = "Zanmato", Attack = 245, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 7, Stamina = 7, Magic = 0, Speed = 0, Evade = 30, MagicEvade = 0, WeaponType = Enums.WeaponType.Knife}; AddWeapon(Zanmato);

        #endregion

        #region Swords

        var Apocalypse = new Weapon { Name = "Apocalypse", Attack = 250, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 7, Stamina = 0, Magic = 7, Speed = 0, Evade = 20, MagicEvade = 20, MpCost = new int[] {12,13,14,15,16,17,18,19}, WeaponType = Enums.WeaponType.Sword}; AddWeapon(Apocalypse);
        
        var BastardSword = new Weapon { Name = "Bastard Sword", Attack = 98, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Sword}; AddWeapon(BastardSword);

        // The wielder will absorb any damage done as HP, but will never do more damage than he can heal him/herself with. 
        var BloodSword = new Weapon { Name = "Blood Sword", Attack = 121, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Sword}; AddWeapon(BloodSword);
        
        var CrystalSword = new Weapon { Name = "Crystal Sword", Attack = 167, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Sword}; AddWeapon(CrystalSword);
        
        var Enhancer = new Weapon { Name = "Enhancer", Attack = 135, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 7, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Sword}; AddWeapon(Enhancer);
        
        var Excalibur = new Weapon { Name = "Excalibur", Attack = 217, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 2, Stamina = 1, Magic = 1, Speed = 2, Evade = 20, MagicEvade = 0, WeaponType = Enums.WeaponType.Sword}; AddWeapon(Excalibur);
        
        var Excalipoor = new Weapon { Name = "Excalipoor", Attack = 1, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Sword}; AddWeapon(Excalipoor);
        
        var Falchion = new Weapon { Name = "Falchion", Attack = 176, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 10, MagicEvade = 0, WeaponType = Enums.WeaponType.Sword}; AddWeapon(Falchion);

        // 25% chance of casting Fire
        var Flametongue = new Weapon { Name = "Flametongue", Attack = 108, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 2, Speed = 0, Evade = 0, MagicEvade = 0, 
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Fire"), WeaponType = Enums.WeaponType.Sword}; AddWeapon(Flametongue);
        
        var GreatSword = new Weapon { Name = "Great Sword", Attack = 54, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Sword}; AddWeapon(GreatSword);
        
        var Icebrand = new Weapon { Name = "Icebrand", Attack = 108, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 2, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Sword}; AddWeapon(Icebrand);

        // Will always try to use [12..19] MP to inflict a critical hit. When no MP can be used, no critical hit will be inflicted. Same amount of damage from the Back Row. Has a 25 % chance of casting Holy
        var Lightbringer = new Weapon { Name = "Lightbringer", Attack = 255, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 7, Stamina = 7, Magic = 7, Speed = 7, Evade = 50, MagicEvade = 50, MpCost = new int[] {12,13,14,15,16,17,18,19},
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Holy"), WeaponType = Enums.WeaponType.Sword}; AddWeapon(Lightbringer);
        
        var MythrilSword = new Weapon { Name = "Mythril Sword", Attack = 38, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Sword}; AddWeapon(MythrilSword);
        
        //Will always try to use [12..19] MP to inflict a critical hit. When no MP can be used, no critical hit will be inflicted. Has a average of 28% chance of breaking (depends on the last digit's of the wielder's current HP)
        var Organyx = new Weapon { Name = "Organyx", Attack = 182, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, MpCost = new int[] {12,13,14,15,16,17,18,19}, WeaponType = Enums.WeaponType.Sword}; AddWeapon(Organyx);

        // Will always try to use [12..19] MP to inflict a critical hit. When no MP can be used, no critical hit will be inflicted. Has a 25% chance of casting Flare on target. 
        var Ragnarok = new Weapon { Name = "Ragnarok", Attack = 255, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 7, Stamina = 7, Magic = 7, Speed = 3, Evade = 30, MagicEvade = 30, MpCost = new int[] {12,13,14,15,16,17,18,19},
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Flare"), WeaponType = Enums.WeaponType.Sword}; AddWeapon(Ragnarok);
        
        // Will always try to use [12..19] MP to inflict a critical hit. When no MP can be used, no critical hit will be inflicted.
        var RuneBlade = new Weapon { Name = "Rune Blade", Attack = 55, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 10, MagicEvade = 0, MpCost = new int[] {12,13,14,15,16,17,18,19}, WeaponType = Enums.WeaponType.Sword}; AddWeapon(RuneBlade);
        
        var SaveTheQueen = new Weapon { Name = "Save the Queen", Attack = 240, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 3, Magic = 7, Speed = 4, Evade = 40, MagicEvade = 40, WeaponType = Enums.WeaponType.Sword}; AddWeapon(SaveTheQueen);

        // The wielder will absorb any damage done as MP, but will never do more damage than he can heal him- or herself with. Has a 25% chance of casting Doom, but this spell won't ever appear when the target is immune to instant death attacks. 
        var SoulSabre = new Weapon { Name = "Soul Sabre", Attack = 125, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 10, MagicEvade = 0,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Doom"), WeaponType = Enums.WeaponType.Sword}; AddWeapon(SoulSabre);

        //Has a 25 % of casting Break on target, but this spell won't ever appear when the target is immune to instant death attacks.
        var Stoneblade = new Weapon { Name = "Stoneblade", Attack = 117, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Break"), WeaponType = Enums.WeaponType.Sword}; AddWeapon(Stoneblade);

        // Has a 25% of casting Thunder on target.
        var ThunderBlade = new Weapon { Name = "Thunder Blade", Attack = 108, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 2, Speed = 0, Evade = 0, MagicEvade = 0,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Thunder"), WeaponType = Enums.WeaponType.Sword}; AddWeapon(ThunderBlade);

        // Barrier-piercing, current HP/max HP ratio is factored into damage formula. Middle length is shown when dealing more than 500 damage, long length is shown when dealing more than 1000 damage.
        // Damage will decrease if not at full HP or if below level 64. To get any benefit out of this weapon, you must be above level 64. 
        var AtmaWeapon = new Weapon { Name = "Atma Weapon", Attack = 255, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Sword}; AddWeapon(AtmaWeapon);

        // 25% chance of trying to instantly kill a target by slicing, but won't ever try to slice an enemy immune to instant death attacks. 
        var Zanetetsuken = new Weapon { Name = "Zantetsuken", Attack = 208, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Doom"), WeaponType = Enums.WeaponType.Sword}; AddWeapon(Zanetetsuken);

        #endregion

        #region Lances

        // Deals twice as much damage when used with Jump as opposed to 1.5.
        var GoldenSpear = new Weapon { Name = "Golden Spear", Attack = 139, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Lance}; AddWeapon(GoldenSpear);

        // Does not get spear bonus when jumping
        var Gungnir = new Weapon { Name = "Gungnir", Attack = 240, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 7, Magic = 7, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Lance}; AddWeapon(Gungnir);

        var HeavyLance = new Weapon { Name = "Heavy Lance", Attack = 112, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Lance}; AddWeapon(HeavyLance);

        var HolyLance = new Weapon { Name = "Holy Lance", Attack = 194, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 3, Speed = 0, Evade = 0, MagicEvade = 0, Element = Enums.Elemental.Holy,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Holy"), WeaponType = Enums.WeaponType.Lance}; AddWeapon(HolyLance);

        // Only adds 1 Battle Power unless the wielder has the Imp status. Deals twice as much damage when used with Jump as opposed to 1.5.
        var Impartisan = new Weapon { Name = "Impartisan", Attack = 253, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Lance}; AddWeapon(Impartisan);

        var Longinus = new Weapon { Name = "Longinus", Attack = 235, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 7, Stamina = 3, Magic = 0, Speed = 3, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Lance}; AddWeapon(Longinus);

        // Deals twice as much damage when used with Jump as opposed to 1.5.
        var MythrilSpear = new Weapon { Name = "Mythril Spear", Attack = 70, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Lance}; AddWeapon(MythrilSpear);

        // Deals twice as much damage when used with Jump as opposed to 1.5.
        var Partisan = new Weapon { Name = "Partisan", Attack = 150, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Lance}; AddWeapon(Partisan);

        // Deals twice as much damage when used with Jump as opposed to 1.5.
        var RadiantLance = new Weapon { Name = "Radiant Lance", Attack = 227, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 3, Stamina = 1, Magic = 3, Speed = 2, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Lance}; AddWeapon(RadiantLance);

        // Deals twice as much damage when used with Jump as opposed to 1.5. Deals Water elemental damage.
        var Trident = new Weapon { Name = "Trident", Attack = 93, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, Element = Enums.Elemental.Ice, WeaponType = Enums.WeaponType.Lance}; AddWeapon(Trident);


        #endregion

        #region SpecialWeapons

        var BoneClub = new Weapon { Name = "Bone Club", Attack = 151, TwoHandCapable = true, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Special}; AddWeapon(BoneClub);

        var Boomerang = new Weapon { Name = "Boomerang", Attack = 102, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, SameDamageBackRow = true, WeaponType = Enums.WeaponType.Special}; AddWeapon(Boomerang);

        var ChainFlail = new Weapon { Name = "Chain Flail", Attack = 86, TwoHandCapable = true, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, SameDamageBackRow = true, WeaponType = Enums.WeaponType.Special}; AddWeapon(ChainFlail);

        // Has a 50 % chance of dealing 3x damage to floating targets and 1.5 to non-Floating targets.
        var Hawkeye = new Weapon { Name = "Hawkeye", Attack = 111, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, SameDamageBackRow = true, WeaponType = Enums.WeaponType.Special}; AddWeapon(Hawkeye);

        var MoonringBlade = new Weapon { Name = "Moonring Blade", Attack = 95, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, SameDamageBackRow = true, WeaponType = Enums.WeaponType.Special}; AddWeapon(MoonringBlade);

        var MorningStar = new Weapon { Name = "Morning Star", Attack = 109, TwoHandCapable = true, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, SameDamageBackRow = true, WeaponType = Enums.WeaponType.Special}; AddWeapon(MorningStar);

        var RisingSun = new Weapon { Name = "Rising Sun", Attack = 117, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, SameDamageBackRow = true, WeaponType = Enums.WeaponType.Special}; AddWeapon(RisingSun);

        // Has a 25% chance of casting Bio on target.
        var ScorpionTail = new Weapon { Name = "Scorpion Tail", Attack = 225, TwoHandCapable = true, RunicCapable = false, BushidoCapable = false,
        Strength = 4, Stamina = 4, Magic = 4, Speed = 4, Evade = 0, MagicEvade = 0, SameDamageBackRow = true, Element = Enums.Elemental.Poison,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Bio"), WeaponType = Enums.WeaponType.Special}; AddWeapon(ScorpionTail);

        // Has a 50 % chance of dealing 3x damage to Floating targets and 1.5 to non-Floating targets.
        var Sniper = new Weapon { Name = "Sniper", Attack = 172, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, SameDamageBackRow = true, WeaponType = Enums.WeaponType.Special}; AddWeapon(Sniper);

        //  25 % chance of trying to instantly kill a target, but won't ever try to kill an enemy immune to instant death attacks. Will always fully restore any Undead target
        var WingEdge = new Weapon { Name = "Wing Edge", Attack = 198, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 7, Stamina = 1, Magic = 2, Speed = 7, Evade = 0, MagicEvade = 0, SameDamageBackRow = true,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Doom"), WeaponType = Enums.WeaponType.Special}; AddWeapon(WingEdge);


        #endregion

        #region GamblerWeapons

        var Cards = new Weapon { Name = "Cards", Attack = 104, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, SameDamageBackRow = true, WeaponType = Enums.WeaponType.Gambler}; AddWeapon(Cards);

        var Darts = new Weapon { Name = "Darts", Attack = 115, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, SameDamageBackRow = true, WeaponType = Enums.WeaponType.Gambler}; AddWeapon(Darts);

        // Has a 25 % chance of casting Doom on target.
        var DeathTarot = new Weapon { Name = "Death Tarot", Attack = 187, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, SameDamageBackRow = true,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Doom"), WeaponType = Enums.WeaponType.Gambler}; AddWeapon(DeathTarot);

        // Damage = Dice Roll 1 * Dice Roll 2 * 2 * Level 
        var Dice = new Weapon { Name = "Dice", Attack = 2, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Gambler}; AddWeapon(Dice);

        // Uses anywhere from 12-19 MP to deal critical hits.
        var Trump = new Weapon { Name = "Trump", Attack = 215, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 3, Stamina = 4, Magic = 0, Speed = 4, Evade = 0, MagicEvade = 0, SameDamageBackRow = true,
         MpCost = new [] {12,13,14,15,16,17,18,19}, WeaponType = Enums.WeaponType.Gambler}; AddWeapon(Trump);

        // Damage = Dice Roll 1 * Dice Roll 2 * Dice Roll 3 * 2 * Level. If all Dice Rolls are the same, then damage = damage * Dice Roll 
        var FixedDice = new Weapon { Name = "Fixed Dice", Attack = 3, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Gambler}; AddWeapon(FixedDice);

        // 25 % chance of trying to instantly kill a target, but won't ever try to kill an enemy immune to instant death attacks. Will always fully restore any Undead target.
        var ViperDarts = new Weapon { Name = "Viper Darts", Attack = 133, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, SameDamageBackRow = true,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Doom"), WeaponType = Enums.WeaponType.Gambler}; AddWeapon(ViperDarts);


        #endregion

        #region Brushes

        // Has a 25% chance of casting Muddle on target.
        var AngelBrush = new Weapon { Name = "Angel Brush", Attack = 170, TwoHandCapable = true, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 7, Speed = 7, Evade = 0, MagicEvade = 0,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Muddle"), WeaponType = Enums.WeaponType.Brush}; AddWeapon(AngelBrush);

        // Relm's initial equipment
        var ChocoboBrush = new Weapon { Name = "Chocobo Brush", Attack = 60, TwoHandCapable = true, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 1, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Brush}; AddWeapon(ChocoboBrush);

        var DaVinciBrush = new Weapon { Name = "Da Vinci Brush", Attack = 100, TwoHandCapable = true, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 1, Speed = 1, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Brush}; AddWeapon(DaVinciBrush);

        var MagicalBrush = new Weapon { Name = "Magical Brush", Attack = 130, TwoHandCapable = true, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 1, Magic = 1, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Brush}; AddWeapon(MagicalBrush);

        var RainbowBrush = new Weapon { Name = "Rainbow Brush", Attack = 146, TwoHandCapable = true, RunicCapable = false, BushidoCapable = false,
        Strength = 1, Stamina = 1, Magic = 2, Speed = 2, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Brush}; AddWeapon(RainbowBrush);

        #endregion

        #region Claws

        // Has a 25% chance of casting Fire on target
        var BurningFist = new Weapon { Name = "Burning Fist", Attack = 122, TwoHandCapable = true, RunicCapable = true, BushidoCapable = true,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, Element = Enums.Elemental.Fire,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Fire"), WeaponType = Enums.WeaponType.Claw}; AddWeapon(BurningFist);

        var DragonClaws = new Weapon { Name = "Dragon Claws", Attack = 188, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 2, Stamina = 0, Magic = 1, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Claw}; AddWeapon(DragonClaws);

        var Godhand = new Weapon { Name = "Godhand", Attack = 220, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 7, Stamina = 7, Magic = 0, Speed = 3, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Claw}; AddWeapon(Godhand);

        var KaiserKnuckles = new Weapon { Name = "Kaiser Knuckles", Attack = 83, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, Element = Enums.Elemental.Holy, WeaponType = Enums.WeaponType.Claw}; AddWeapon(KaiserKnuckles);

        var MetalKnuckles = new Weapon { Name = "Metal Knuckles", Attack = 55, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Claw}; AddWeapon(MetalKnuckles);

        var Tigerfangs = new Weapon { Name = "Tigerfangs", TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 3, Stamina = 2, Magic = 3, Speed = 2, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Claw}; AddWeapon(Tigerfangs);

        // Has 25% chance of casting Poison on target.
        var VenomClaws = new Weapon { Name = "Venom Claws", TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, Element = Enums.Elemental.Poison,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Poison"), WeaponType = Enums.WeaponType.Claw};

        #endregion

        #region Rods

        var FlameRod = new Weapon { Name = "Flame Rod", Attack = 79, TwoHandCapable = true, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, Element = Enums.Elemental.Fire,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Fire 2"), WeaponType = Enums.WeaponType.Rod}; AddWeapon(FlameRod);

        // Has a 25 % chance of casting Graviga on the target. When used as an item, it will cast a barrier-piercing and unblockable Graviga spell after which the rod will be broken. Targets immune to instant death attacks will remain unaffected
        var GravityRod = new Weapon { Name = "Gravity Rod", Attack = 120, TwoHandCapable = true, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, Element = Enums.Elemental.Earth,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Quarter"), WeaponType = Enums.WeaponType.Rod}; AddWeapon(GravityRod);

        // Cures the target.  When selected by Throw, will auto-target your own party as oopposed to the enemy party.
        var HealingRod = new Weapon { Name = "Healing Rod", Attack = 200, TwoHandCapable = true, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Rod}; AddWeapon(HealingRod);

        // Has a 25% chance of casting Holy.  When used as an item, it will cast a barrier-piercing and unblockable Holy spell after which the rod will be broken.
        var HolyRod = new Weapon { Name = "Holy Rod", Attack = 124, TwoHandCapable = true, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, Element = Enums.Elemental.Holy,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Holy"), WeaponType = Enums.WeaponType.Rod}; AddWeapon(HolyRod);

        // Has a 25% chance of casting Ice 2.  When used as an item, it will cast a barrier-piercing and unblockable Ice 2 spell after which the rod will be broken.
        var IceRod = new Weapon { Name = "Ice Rod", Attack = 79, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, Element = Enums.Elemental.Ice,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Ice 2"), WeaponType = Enums.WeaponType.Rod}; AddWeapon(IceRod);

        var MagusRod = new Weapon { Name = "Magus Rod", Attack = 168, TwoHandCapable = true, RunicCapable = true, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 7, Speed = 0, Evade = 0, MagicEvade = 30, WeaponType = Enums.WeaponType.Rod}; AddWeapon(MagusRod);

        var MythrilRod = new Weapon { Name = "Mythril Rod", Attack = 60, TwoHandCapable = true, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 2, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Rod}; AddWeapon(MythrilRod);

        // Has a 25% chance of casting Poison.  When used as an item, it will cast a barrier-piercing and unblockable Poison spell after which the rod will be broken.
        var PoisonRod = new Weapon { Name = "Poison Rod", Attack = 86, TwoHandCapable = true, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, Element = Enums.Elemental.Poison,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Poison"), WeaponType = Enums.WeaponType.Rod}; AddWeapon(PoisonRod);

        // Draws MP to deal criticals
        var Punisher = new Weapon { Name = "Punisher", Attack = 111, TwoHandCapable = true, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, 
        MpCost = new int[] {12,13,14,15,16,17,18,19}, WeaponType = Enums.WeaponType.Rod}; AddWeapon(Punisher);

        // Has a 25% chance to cast Meteor
        var StardustRod = new Weapon { Name = "Stardust Rod", Attack = 180, TwoHandCapable = true, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 4, Magic = 7, Speed = 0, Evade = 0, MagicEvade = 0,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Meteor"), WeaponType = Enums.WeaponType.Rod}; AddWeapon(StardustRod);

        // Has a 25% chance to cast Bolt 2 on target.  When used as an item, it will cast a barrier-piercing and unblockable Bolt 2 spell after which the rod will be broken.
        var ThunderRod = new Weapon { Name = "Thunder Rod", Attack = 79, TwoHandCapable = true, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0,
        RandomAbilityCast = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == "Bolt 2"), WeaponType = Enums.WeaponType.Rod}; AddWeapon(ThunderRod);

        #endregion

        #region Shuriken

        var FumaShuriken = new Weapon { Name = "Fuma Shuriken", Attack = 132, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, SameDamageBackRow = true, WeaponType = Enums.WeaponType.Shuriken}; AddWeapon(FumaShuriken);

        var Pinwheel = new Weapon { Name = "Pinwheel", Attack =  190, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, SameDamageBackRow = true, WeaponType = Enums.WeaponType.Shuriken}; AddWeapon(Pinwheel);

        var Shuriken = new Weapon { Name = "Shuriken", Attack = 86, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, SameDamageBackRow = true, WeaponType = Enums.WeaponType.Shuriken}; AddWeapon(Shuriken);

        #endregion

        #region Skeans

        var FlameScroll = new Weapon { Name = "Flame Scroll", Attack = 0, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, Element = Enums.Elemental.Fire, 
        TargetsMultiple = true, WeaponType = Enums.WeaponType.Skean};  AddWeapon(FlameScroll);

        var InvisibilityScroll = new Weapon { Name = "Invisibility Scroll", Attack = 0, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, StatusesInduced = new List<Enums.Status> {Enums.Status.Clear}, 
        TargetsMultiple = true, WeaponType = Enums.WeaponType.Skean}; AddWeapon(InvisibilityScroll);

        var LightningScroll = new Weapon { Name = "Lightning Scroll", Attack = 0, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, Element = Enums.Elemental.Thunder,
        TargetsMultiple = true,  WeaponType = Enums.WeaponType.Skean}; AddWeapon(LightningScroll);

        var ShadowScroll = new Weapon { Name = "Shadow Scroll", Attack = 0, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, StatusesInduced = new List<Enums.Status> {Enums.Status.Image}, 
        TargetsMultiple = true, WeaponType = Enums.WeaponType.Skean}; AddWeapon(ShadowScroll);

        var WaterScroll = new Weapon { Name = "Water Scroll", Attack = 0, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, Element = Enums.Elemental.Ice, 
        TargetsMultiple = true, WeaponType = Enums.WeaponType.Skean}; AddWeapon(WaterScroll);

        #endregion

        #region Tools

        // Causes enemy to self-destruct upon moving
        var AirAnchor = new Weapon { Name = "Air Anchor", Attack = 0, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Tool}; AddWeapon(AirAnchor);

        // Multi-target physical damage (doesn't attenuate from split)
        var AutoCrossbow = new Weapon { Name = "Auto Crossbow", Attack = 0, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, TargetsMultiple = true, WeaponType = Enums.WeaponType.Tool}; AddWeapon(AutoCrossbow);

        // Poison damage and potentially poisons all enemies
        var Bioblaster = new Weapon { Name = "Bioblaster", Attack = 0, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, TargetsMultiple = true, Element = Enums.Elemental.Poison, WeaponType = Enums.WeaponType.Tool}; AddWeapon(Bioblaster);

        // Either deals damage or dispatches enemy
        var Chainsaw = new Weapon { Name = "Chainsaw", Attack = 0, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Tool}; AddWeapon(Chainsaw);

        // Assigns random elemental weakness to enemy
        var Debilitator = new Weapon { Name = "Debilitator", Attack = 0, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Tool}; AddWeapon(Debilitator);

        // Ignores defense
        var Drill = new Weapon { Name = "Debilitator", Attack = 0, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, WeaponType = Enums.WeaponType.Tool}; AddWeapon(Drill);

        // Inflicts Dark & damage to all enemies
        var Flash = new Weapon { Name = "Flash", Attack = 0, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, TargetsMultiple = true, 
        StatusesInduced = new List<Enums.Status> {Enums.Status.Dark}, WeaponType = Enums.WeaponType.Tool}; AddWeapon(Flash);

        // Confuses all enemies
        var Noiseblaster = new Weapon { Name = "Noiseblaster", Attack = 0, TwoHandCapable = false, RunicCapable = false, BushidoCapable = false,
        Strength = 0, Stamina = 0, Magic = 0, Speed = 0, Evade = 0, MagicEvade = 0, TargetsMultiple = true, WeaponType = Enums.WeaponType.Tool}; AddWeapon(Noiseblaster);

        #endregion

    }
}