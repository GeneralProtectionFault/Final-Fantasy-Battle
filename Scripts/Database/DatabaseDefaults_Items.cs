using System.Collections.Generic;
using Godot;

public static class DatabaseDefaults_Items
{
    private static void AddItem(Item Item)
	{
		var ItemResult = DatabaseHandler.ItemCollection.FindOne(x => x.Name == Item.Name);

		if( !(ItemResult is null) ) {
			GD.Print($"{Item.Name} is already in database, aborting add item.");
			return;
		}

		DatabaseHandler.ItemCollection.Insert(Item);
		DatabaseHandler.ItemCollection.EnsureIndex("Name", true);

		DatabaseHandler.GameDatabase.Commit();

		GD.Print($"{Item.Name} added to database.");
	}


    public static void AddItems()
    {
        var Antidote = new Item { Name = "Antidote", Cost = 50, StatusesRemoved = new List<Enums.Status> {Enums.Status.Poisoned} , InventoryCount = 4}; AddItem(Antidote);
		var Cider = new Item { Name = "Cider", IsRareItem = true }; AddItem(Cider);
		var Coral = new Item { Name = "Coral", IsRareItem = true }; AddItem(Coral);
		var DriedMeat = new Item { Name = "Dried Meat", Cost = 150, HpIncrease = 150 }; AddItem(DriedMeat);
		var EchoScreen = new Item { Name = "Echo Screen", Cost = 120, StatusesRemoved = new List<Enums.Status> {Enums.Status.Mute} }; AddItem(EchoScreen);
		var Elixir = new Item { Name = "Elixir", HpIncrease = 9999, MpIncrease = 9999 }; AddItem(Elixir);
		var EmperorsLetter = new Item { Name = "Emperor's Letter", IsRareItem = true }; AddItem(EmperorsLetter);
		var Ether = new Item { Name = "Ether", Cost = 1500, MpIncrease = 50 }; AddItem(Ether);
		var EyeDrops = new Item { Name = "Eye Drops", Cost = 50, StatusesRemoved = new List<Enums.Status> {Enums.Status.Dark} }; AddItem(EyeDrops);
		var Fish = new Item { Name = "Fish", IsRareItem = true } ; AddItem(Fish);
		var GoldNeedle = new Item { Name = "Gold Needle", Cost = 200, StatusesRemoved = new List<Enums.Status> {Enums.Status.Petrified} }; AddItem(GoldNeedle);
		var GreenCherry = new Item { Name = "Green Cherry", Cost = 150, StatusesRemoved = new List<Enums.Status> {Enums.Status.Imp} }; AddItem(GreenCherry);
		var HiEther = new Item { Name = "Hi-Ether",  MpIncrease = 150 }; AddItem(HiEther);
		var HiPotion = new Item { Name = "Hi-Potion", Cost = 300, HpIncrease = 250 }; AddItem(HiPotion);
		var HolyWater = new Item { Name = "Holy Water", Cost = 300, StatusesRemoved = new List<Enums.Status> {Enums.Status.Zombie} }; AddItem(HolyWater);
		var LolasLetter = new Item { Name = "Lola's Letter", IsRareItem = true }; AddItem(LolasLetter);
		var LumpOfMetal = new Item { Name = "Lump of Metal", IsRareItem = true }; AddItem(LumpOfMetal);
		var MagiciteShard = new Item { Name = "Magicite"}; AddItem(MagiciteShard);
		var Megalixir = new Item { Name = "Megalixir", TargetsMultiple = true }; AddItem(Megalixir);
		var OldClockKey = new Item { Name = "Old Clock Key", IsRareItem = true }; AddItem(OldClockKey);
		var Pendant = new Item { Name = "Pendant", IsRareItem = true }; AddItem(Pendant);
		var Potion = new Item { Name = "Potion", Cost = 50, HpIncrease = 50, InventoryCount = 10 }; AddItem(Potion);
		var Remedy = new Item { Name = "Remedy", Cost = 1000, 
			StatusesRemoved = new List<Enums.Status> {
				Enums.Status.Berserk, Enums.Status.Float, Enums.Status.Poisoned, Enums.Status.Confused, Enums.Status.Condemned, Enums.Status.Undead, 
				Enums.Status.Imp, Enums.Status.Mute, Enums.Status.Seizure, Enums.Status.Sleep, Enums.Status.Slow, Enums.Status.Dark, Enums.Status.Petrified, 
				Enums.Status.Stop	
			}}; AddItem(Remedy);
		var RustRid = new Item { Name = "Rust-Rid", IsRareItem = true }; AddItem(RustRid);
		var SleepingBag = new Item { Name = "Sleeping Bag", Cost = 500, HpIncrease = 9999, MpIncrease = 9999 }; AddItem(SleepingBag);
		var SmokeBomb = new Item { Name = "Smoke Bomb", Cost = 300, WarpsFromBattle = true }; AddItem(SmokeBomb);
		var StoneTablet = new Item { Name = "Stone Tablet", IsRareItem = true }; AddItem(StoneTablet);
		var SuperBall = new Item { Name = "Super Ball", Cost = 10000 }; AddItem(SuperBall);
		var WarpStone = new Item { Name = "Warp Stone", Cost = 700, WarpsFromBattle = true }; AddItem(WarpStone);
		var Tent = new Item { Name = "Tent", TargetsMultiple = true, HpIncrease = 9999, MpIncrease = 9999 }; AddItem(Tent);
		var XEther = new Item { Name = "X-Ether", MpIncrease = 9999 }; AddItem(XEther);
		var XPotion = new Item { Name = "X-Potion", HpIncrease = 9999 }; AddItem(XPotion);
    }

}