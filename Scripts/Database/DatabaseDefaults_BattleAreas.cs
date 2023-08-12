using Godot;
using System.Collections.Generic;


public static class DatabaseDefaults_BattleAreas 
{
    private static void AddBattleArea(BattleArea BattleArea)
	{
		var BattleAreaResult = DatabaseHandler.BattleAreaCollection.FindOne(x => x.Name == BattleArea.Name);

		if( !(BattleAreaResult is null) ) {
			GD.Print($"{BattleArea.Name} is already in database, aborting add ability.");
			return;
		}

		DatabaseHandler.BattleAreaCollection.Insert(BattleArea);
		DatabaseHandler.BattleAreaCollection.EnsureIndex("Name", true);

		GD.Print($"{BattleArea.Name} added to database.");
	}



    public static void AddBattleAreas()
    {
        var NarshePlains = new BattleArea 
        {Name = "BattleArea_NarshePlains1", EnemyList = new List<string>() {"Rhinotaur"}};
		AddBattleArea(NarshePlains);


    }

}