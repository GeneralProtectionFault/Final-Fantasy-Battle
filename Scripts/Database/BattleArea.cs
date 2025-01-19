using System.Collections;
using System.Collections.Generic;
using LiteDB;


public class BattleArea 
{
    public ObjectId _id { get; set; }

    public string Name { get; set; }
    public List<string> EnemyList { get; set; }

    
}
