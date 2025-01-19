using Godot;
using System.Collections.Generic;
using System.IO;
using LiteDB;
using Newtonsoft.Json;
using System.Linq;
using System.ComponentModel;


public partial class DatabaseHandler : Node
{
	public static LiteDatabase GameDatabase;

	public static ILiteCollection<Character> CharacterCollection;
	public static ILiteCollection<Enemy> EnemyCollection;
	public static ILiteCollection<Ability> AbilityCollection;
	public static ILiteCollection<Item> ItemCollection;
	public static ILiteCollection<Weapon> WeaponCollection;
	public static ILiteCollection<Armor> ArmorCollection;

	// Store battle areas and the enemies, probability of pincer, etc...
	public static ILiteCollection<BattleArea> BattleAreaCollection;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitializeDatabase();
		InitializeCollections();
		
		// TESTING: Character
		// var TestCharacterJson = GetCharacterJson("Tina");
		// GD.Print(TestCharacterJson);
		
		// var CharacterObject = GetCharacterObject("Tina");

		// foreach(var item in CharacterObject.MagicList)
		// {
		//     GD.Print($"Spell: {item.Name}");
		// }


		// TESTING: Enemy => Ability list
		// var TestEnemy = DatabaseHandler.EnemyCollection.FindOne(x => x.Name == "Silver Lobo");
		
		// foreach(var ability in TestEnemy.RageAbilities)
		// {
		// 	GD.Print(ability.Name);
		// }
		


	}	


	private void InitializeDatabase()
	{
		GD.Print("Initializing database...");

		var current_directory = System.IO.Directory.GetCurrentDirectory();
		var database_path = Path.Combine(current_directory, "Scripts", "Database", "game_database.db");

		var connection_string = $"Filename = {database_path}; Password = xyz123; Connection Type = Shared;";
		GameDatabase = new LiteDatabase(connection_string);

	}


	private void InitializeCollections()
	{
		
		AbilityCollection = GameDatabase.GetCollection<Ability>("abilities");
		CharacterCollection = GameDatabase.GetCollection<Character>("characters");
		EnemyCollection = GameDatabase.GetCollection<Enemy>("enemies");
		ItemCollection = GameDatabase.GetCollection<Item>("items");
		WeaponCollection = GameDatabase.GetCollection<Weapon>("weapons");
		ArmorCollection = GameDatabase.GetCollection<Armor>("armor");
		BattleAreaCollection = GameDatabase.GetCollection<BattleArea>("battle_areas");
		
		DatabaseDefaults_Abilities.AddAbilities();
		DatabaseDefaults_Characters.AddCharacters();
		DatabaseDefaults_Enemies.AddEnemies();
		DatabaseDefaults_Items.AddItems();
		DatabaseDefaults_Weapons.AddWeapons();
		DatabaseDefaults_Armor.AddAllArmor();
		DatabaseDefaults_BattleAreas.AddBattleAreas();
	}



	public static string GetCharacterJson (string CharacterName)
	{
		var Result = BsonMapper.Global.ToDocument(
			CharacterCollection.FindOne(x => x.Name == CharacterName)
		);

		return LiteDB.JsonSerializer.Serialize(Result);
	}



	public static Character GetCharacterObject(string CharacterName)
	{
		var json = GetCharacterJson(CharacterName);
		return JsonConvert.DeserializeObject<Character>(json);
	}


	
	public static void UpdateCharacter(Character Character)
	{
		var CharacterToUpdate = CharacterCollection.FindOne(x => x.Name == Character.Name);

		BsonValue ItemID = CharacterToUpdate._id;

		CharacterToUpdate = Character;
		CharacterToUpdate._id = ItemID;

		var UpdateResult = CharacterCollection.Update(CharacterToUpdate);
		GD.Print($"Database update result: {UpdateResult}");
	}

	public static void UpdateItem(Item Item)
	{
		var ItemToUpdate = ItemCollection.FindOne(x => x.Name == Item.Name);
		BsonValue ItemID = ItemToUpdate._id;

		ItemToUpdate = Item;
		ItemToUpdate._id = ItemID;

		var UpdateResult = ItemCollection.Update(ItemToUpdate);
		GD.Print($"Item updated, {Item.Name} quantity: {ItemToUpdate.InventoryCount}");
		
	}



	public static Character GetPartyLeader()
	{
		var LeadPartyMember = CharacterCollection.FindOne(x => x.IsPartyLead == true);
		return LeadPartyMember;
	}


	public static IEnumerable<Character> GetCharactersInParty()
	{
		var Result = CharacterCollection.Find("$.InParty = true");
		return Result;
	}


	public static string GetCharacterStatAsString(string CharacterName, string StatName)
	{
		var Character = CharacterCollection.FindOne(x => x.Name == CharacterName);

		var PropertyInfo = Character.GetType().GetProperty(StatName);
		var PropertyValue = PropertyInfo.GetValue(Character, null).ToString();
		return PropertyValue;
	}


	public static List<Ability> GetCharacterMagic(string CharacterName)
	{
		var Result = CharacterCollection.Find($"$.Name = '{CharacterName}'").FirstOrDefault();
		
		List<Ability> MagicList = new List<Ability>();
		if (Result is not null)
			MagicList = Result.MagicList;
		
		return MagicList;
	}

}
