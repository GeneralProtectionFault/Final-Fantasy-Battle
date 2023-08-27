using System.Collections.Generic;
using Godot;

public static class Globals
{
    public static Node Overworld;

    public static Vector2 EnteredBattlePosition;
    public static bool OverworldInputEnabled = true;
    public static bool InBattle = false;
    public static bool ReturningFromBattle = false;

    public static bool CursorLocked = false;

    // Use to determine if the handcursor should show up & process input
    public static bool Battle_ActivePlayerExists = false;

    public static string OverworldSpawnNode = "Narshe";

    public static Enums.BattleMode BattleMode = Enums.BattleMode.Wait;
    

    public static Enums.GameState GameState;

    // Use this to test if "in battle," etc...
    public static readonly List<Enums.GameState> BattleStates = new List<Enums.GameState>() {
        Enums.GameState.Battle,
        Enums.GameState.Battle_Enemy_Action,
        Enums.GameState.Battle_Party_Action,

        Enums.GameState.Battle_Fight_Selecting_Target_Characters,
		Enums.GameState.Battle_Fight_Selecting_Target_Enemies,
		Enums.GameState.Battle_Magic_Selecting_Target_Characters,
		Enums.GameState.Battle_Magic_Selecting_Target_Enemies,
		Enums.GameState.Battle_Jump_Selecting_Target,
		Enums.GameState.Battle_Item_Selecting_Target_Characters,
		Enums.GameState.Battle_Item_Selecting_Target_Enemies,
		Enums.GameState.Battle_Tool_Selecting_Target,

		Enums.GameState.Battle_Fight_Selecting_Target_Multiple_Characters,
		Enums.GameState.Battle_Fight_Selecting_Target_Multiple_Enemies,
		Enums.GameState.Battle_Magic_Selecting_Target_Multiple_Characters,
		Enums.GameState.Battle_Magic_Selecting_Target_Multiple_Enemies,
		Enums.GameState.Battle_Item_Selecting_Target_Multiple_Characters,
		Enums.GameState.Battle_Item_Selecting_Target_Multiple_Enemies,
		Enums.GameState.Battle_Tool_Selecting_Target_Multiple,

        Enums.GameState.Battle_Menu_Normal,
        Enums.GameState.Battle_Magitek,

        Enums.GameState.Battle_Menu_Blitz,
        Enums.GameState.Battle_Menu_Dance,
        Enums.GameState.Battle_Menu_Item,
        Enums.GameState.Battle_Menu_Lore,
        Enums.GameState.Battle_Menu_Magic,
        Enums.GameState.Battle_Menu_Magitek,
        Enums.GameState.Battle_Menu_Rage,
        Enums.GameState.Battle_Menu_SwordTech,
        Enums.GameState.Battle_Menu_Tool,
        
        Enums.GameState.Battle_Won,
        Enums.GameState.Battle_Lost,
        Enums.GameState.Battle_End
    };

    public static readonly List<Enums.GameState> BattleSelectingObjectStates = new List<Enums.GameState>() {
        Enums.GameState.Battle_Fight_Selecting_Target_Characters,
		Enums.GameState.Battle_Fight_Selecting_Target_Enemies,
		Enums.GameState.Battle_Magic_Selecting_Target_Characters,
		Enums.GameState.Battle_Magic_Selecting_Target_Enemies,
		Enums.GameState.Battle_Jump_Selecting_Target,
		Enums.GameState.Battle_Item_Selecting_Target_Characters,
		Enums.GameState.Battle_Item_Selecting_Target_Enemies,
		Enums.GameState.Battle_Tool_Selecting_Target,

		Enums.GameState.Battle_Fight_Selecting_Target_Multiple_Characters,
		Enums.GameState.Battle_Fight_Selecting_Target_Multiple_Enemies,
		Enums.GameState.Battle_Magic_Selecting_Target_Multiple_Characters,
		Enums.GameState.Battle_Magic_Selecting_Target_Multiple_Enemies,
		Enums.GameState.Battle_Item_Selecting_Target_Multiple_Characters,
		Enums.GameState.Battle_Item_Selecting_Target_Multiple_Enemies,
		Enums.GameState.Battle_Tool_Selecting_Target_Multiple
    };


    public static readonly List<Enums.GameState> BattleSelectingCharactersStates = new List<Enums.GameState>() {
        Enums.GameState.Battle_Fight_Selecting_Target_Characters,
		Enums.GameState.Battle_Magic_Selecting_Target_Characters,
		Enums.GameState.Battle_Item_Selecting_Target_Characters,

		Enums.GameState.Battle_Fight_Selecting_Target_Multiple_Characters,
		Enums.GameState.Battle_Magic_Selecting_Target_Multiple_Characters,
		Enums.GameState.Battle_Item_Selecting_Target_Multiple_Characters,
		Enums.GameState.Battle_Tool_Selecting_Target_Multiple
    };

    public static readonly List<Enums.GameState> BattleSelectingEnemyStates = new List<Enums.GameState>() {
        Enums.GameState.Battle_Fight_Selecting_Target_Enemies,
		Enums.GameState.Battle_Magic_Selecting_Target_Enemies,
		Enums.GameState.Battle_Jump_Selecting_Target,
		Enums.GameState.Battle_Item_Selecting_Target_Enemies,
		Enums.GameState.Battle_Tool_Selecting_Target,

		Enums.GameState.Battle_Fight_Selecting_Target_Multiple_Enemies,
		Enums.GameState.Battle_Magic_Selecting_Target_Multiple_Enemies,
		Enums.GameState.Battle_Item_Selecting_Target_Multiple_Enemies,
		Enums.GameState.Battle_Tool_Selecting_Target_Multiple
    };

    public static readonly List<Enums.GameState> BattleSelectingMenuStates = new List<Enums.GameState>() {
        Enums.GameState.Battle_Menu_Normal,
        Enums.GameState.Battle_Menu_Blitz,
        Enums.GameState.Battle_Menu_Dance,
        Enums.GameState.Battle_Menu_Item,
        Enums.GameState.Battle_Menu_Lore,
        Enums.GameState.Battle_Menu_Magic,
        Enums.GameState.Battle_Menu_Magitek,
        Enums.GameState.Battle_Menu_Rage,
        Enums.GameState.Battle_Menu_SwordTech,
        Enums.GameState.Battle_Menu_Tool

    };



    



    // When switching between targetting of characters vs. enemies, use this to simplify switching to the appropriate battle state
    public static readonly Dictionary<Enums.GameState, Enums.GameState> SelectingStateOpposites = new Dictionary<Enums.GameState, Enums.GameState> () {
        {Enums.GameState.Battle_Fight_Selecting_Target_Characters, Enums.GameState.Battle_Fight_Selecting_Target_Enemies},
		{Enums.GameState.Battle_Fight_Selecting_Target_Enemies, Enums.GameState.Battle_Fight_Selecting_Target_Characters},
		{Enums.GameState.Battle_Magic_Selecting_Target_Characters, Enums.GameState.Battle_Magic_Selecting_Target_Enemies},
		{Enums.GameState.Battle_Magic_Selecting_Target_Enemies, Enums.GameState.Battle_Magic_Selecting_Target_Characters},

        // Force no change if it is not applicable
		{Enums.GameState.Battle_Jump_Selecting_Target, Enums.GameState.Battle_Jump_Selecting_Target},
		{Enums.GameState.Battle_Item_Selecting_Target_Characters, Enums.GameState.Battle_Item_Selecting_Target_Enemies},
		{Enums.GameState.Battle_Item_Selecting_Target_Enemies, Enums.GameState.Battle_Item_Selecting_Target_Characters},
		
        // No change...
        {Enums.GameState.Battle_Tool_Selecting_Target, Enums.GameState.Battle_Tool_Selecting_Target},

        // Current behavior removes the multiple select if they switch 
		{Enums.GameState.Battle_Fight_Selecting_Target_Multiple_Characters, Enums.GameState.Battle_Fight_Selecting_Target_Enemies},
		{Enums.GameState.Battle_Fight_Selecting_Target_Multiple_Enemies, Enums.GameState.Battle_Fight_Selecting_Target_Characters},
		{Enums.GameState.Battle_Magic_Selecting_Target_Multiple_Characters, Enums.GameState.Battle_Magic_Selecting_Target_Enemies},
		{Enums.GameState.Battle_Magic_Selecting_Target_Multiple_Enemies, Enums.GameState.Battle_Magic_Selecting_Target_Characters},
		{Enums.GameState.Battle_Item_Selecting_Target_Multiple_Characters, Enums.GameState.Battle_Item_Selecting_Target_Enemies},
		{Enums.GameState.Battle_Item_Selecting_Target_Multiple_Enemies, Enums.GameState.Battle_Item_Selecting_Target_Characters},
		
        // No change...
        {Enums.GameState.Battle_Tool_Selecting_Target_Multiple, Enums.GameState.Battle_Tool_Selecting_Target_Multiple}
    };



    public static readonly List<Enums.GameState> BattleWaitStates = new List<Enums.GameState>();



    /// <summary>
    /// Globals Constructor
    /// </summary>
    static Globals ()
    {
        // Use other lists to simplify making any states in which "wait" applies (as opposed to "active")
        BattleWaitStates.AddRange(BattleSelectingObjectStates);
        BattleWaitStates.AddRange(BattleSelectingMenuStates);
        // Timer bars should still increment in the vanilla "Fight, etc..." menu
        BattleWaitStates.Remove(Enums.GameState.Battle_Menu_Normal);
        BattleWaitStates.AddRange(BattleSelectingEnemyStates);
        BattleWaitStates.Add(Enums.GameState.Battle_Won);
        BattleWaitStates.Add(Enums.GameState.Battle_Lost);

        BattleWaitStates.Add(Enums.GameState.Battle_Party_Action);
        BattleWaitStates.Add(Enums.GameState.Battle_Enemy_Action);


        // Store the Overworld in memory here.  The sprite is huge, so it will hang if we have to load it every time
        Overworld = ResourceLoader.Load<PackedScene>("res://Scenes/Overworld.tscn").Instantiate();
    }
}