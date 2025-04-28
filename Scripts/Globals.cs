using System.Collections.Generic;
using System.Diagnostics;
using Godot;

public static class Globals
{
    public static Node Overworld;
    public static Vector2 OverworldPosition;
    public static bool OverworldInputEnabled = true;
    public static bool InBattle = false;
    public static bool ReturningFromBattle = false;




    public static bool CursorLocked = false;

    // Use to determine if the handcursor should show up & process input
    public static bool Battle_ActivePlayerExists = false;
    public static bool Battle_ActiveEnemyExists = false;

    // public static string OverworldSpawnNode = "Narshe";

    public static Enums.BattleMode BattleMode = Enums.BattleMode.Wait;
    

    public static Enums.GameState GameState;
    public static Enums.GameState PreviousGameState;
    public static Enums.SelectionState SelectionState;
    public static Enums.SelectionState PreviousSelectionState;




    // Use this to test if "in battle," etc...
    public static readonly List<Enums.GameState> BattleStates = new() {
        Enums.GameState.Battle,
        Enums.GameState.Battle_Enemy_Action,
        Enums.GameState.Battle_Party_Action,

        Enums.GameState.Battle_Magitek,
        
        Enums.GameState.Battle_Won,
        Enums.GameState.Battle_Lost,
        Enums.GameState.Battle_End
    };

    public static readonly List<Enums.SelectionState> BattleSelectingObjectStates = new() {
        Enums.SelectionState.Battle_Fight_Selecting_Target_Characters,
		Enums.SelectionState.Battle_Fight_Selecting_Target_Enemies,
		Enums.SelectionState.Battle_Magic_Selecting_Target_Characters,
		Enums.SelectionState.Battle_Magic_Selecting_Target_Enemies,
		Enums.SelectionState.Battle_Jump_Selecting_Target,
		Enums.SelectionState.Battle_Item_Selecting_Target_Characters,
		Enums.SelectionState.Battle_Item_Selecting_Target_Enemies,
		Enums.SelectionState.Battle_Tool_Selecting_Target,

		Enums.SelectionState.Battle_Fight_Selecting_Target_Multiple_Characters,
		Enums.SelectionState.Battle_Fight_Selecting_Target_Multiple_Enemies,
		Enums.SelectionState.Battle_Magic_Selecting_Target_Multiple_Characters,
		Enums.SelectionState.Battle_Magic_Selecting_Target_Multiple_Enemies,
		Enums.SelectionState.Battle_Item_Selecting_Target_Multiple_Characters,
		Enums.SelectionState.Battle_Item_Selecting_Target_Multiple_Enemies,
		Enums.SelectionState.Battle_Tool_Selecting_Target_Multiple
    };


    public static readonly List<Enums.SelectionState> BattleSelectingCharactersStates = new() {
        Enums.SelectionState.Battle_Fight_Selecting_Target_Characters,
		Enums.SelectionState.Battle_Magic_Selecting_Target_Characters,
		Enums.SelectionState.Battle_Item_Selecting_Target_Characters,

		Enums.SelectionState.Battle_Fight_Selecting_Target_Multiple_Characters,
		Enums.SelectionState.Battle_Magic_Selecting_Target_Multiple_Characters,
		Enums.SelectionState.Battle_Item_Selecting_Target_Multiple_Characters,
		Enums.SelectionState.Battle_Tool_Selecting_Target_Multiple
    };

    public static readonly List<Enums.SelectionState> BattleSelectingEnemyStates = new() {
        Enums.SelectionState.Battle_Fight_Selecting_Target_Enemies,
		Enums.SelectionState.Battle_Magic_Selecting_Target_Enemies,
		Enums.SelectionState.Battle_Jump_Selecting_Target,
		Enums.SelectionState.Battle_Item_Selecting_Target_Enemies,
		Enums.SelectionState.Battle_Tool_Selecting_Target,

		Enums.SelectionState.Battle_Fight_Selecting_Target_Multiple_Enemies,
		Enums.SelectionState.Battle_Magic_Selecting_Target_Multiple_Enemies,
		Enums.SelectionState.Battle_Item_Selecting_Target_Multiple_Enemies,
		Enums.SelectionState.Battle_Tool_Selecting_Target_Multiple
    };

    public static readonly List<Enums.SelectionState> BattleSelectingMenuStates = new() {
        Enums.SelectionState.Battle_Menu_Normal,
        Enums.SelectionState.Battle_Menu_Blitz,
        Enums.SelectionState.Battle_Menu_Dance,
        Enums.SelectionState.Battle_Menu_Item,
        Enums.SelectionState.Battle_Menu_Lore,
        Enums.SelectionState.Battle_Menu_Magic,
        Enums.SelectionState.Battle_Menu_Magitek,
        Enums.SelectionState.Battle_Menu_Rage,
        Enums.SelectionState.Battle_Menu_SwordTech,
        Enums.SelectionState.Battle_Menu_Tool
    };

    public static readonly List<Enums.Status> BattleInactiveStatuses = new() {
        Enums.Status.Wounded,
        Enums.Status.Petrified,
        Enums.Status.Zombie,
        Enums.Status.InstantDeath,
        Enums.Status.Seizure,
        Enums.Status.Stop
    };

    

    // When switching between targetting of characters vs. enemies, use this to simplify switching to the appropriate battle state
    public static readonly Dictionary<Enums.SelectionState, Enums.SelectionState> SelectingStateOpposites = new () {
        {Enums.SelectionState.Battle_Fight_Selecting_Target_Characters, Enums.SelectionState.Battle_Fight_Selecting_Target_Enemies},
		{Enums.SelectionState.Battle_Fight_Selecting_Target_Enemies, Enums.SelectionState.Battle_Fight_Selecting_Target_Characters},
		{Enums.SelectionState.Battle_Magic_Selecting_Target_Characters, Enums.SelectionState.Battle_Magic_Selecting_Target_Enemies},
		{Enums.SelectionState.Battle_Magic_Selecting_Target_Enemies, Enums.SelectionState.Battle_Magic_Selecting_Target_Characters},

        // Force no change if it is not applicable
		{Enums.SelectionState.Battle_Jump_Selecting_Target, Enums.SelectionState.Battle_Jump_Selecting_Target},
		{Enums.SelectionState.Battle_Item_Selecting_Target_Characters, Enums.SelectionState.Battle_Item_Selecting_Target_Enemies},
		{Enums.SelectionState.Battle_Item_Selecting_Target_Enemies, Enums.SelectionState.Battle_Item_Selecting_Target_Characters},
		
        // No change...
        {Enums.SelectionState.Battle_Tool_Selecting_Target, Enums.SelectionState.Battle_Tool_Selecting_Target},

        // Current behavior removes the multiple select if they switch 
		{Enums.SelectionState.Battle_Fight_Selecting_Target_Multiple_Characters, Enums.SelectionState.Battle_Fight_Selecting_Target_Enemies},
		{Enums.SelectionState.Battle_Fight_Selecting_Target_Multiple_Enemies, Enums.SelectionState.Battle_Fight_Selecting_Target_Characters},
		{Enums.SelectionState.Battle_Magic_Selecting_Target_Multiple_Characters, Enums.SelectionState.Battle_Magic_Selecting_Target_Enemies},
		{Enums.SelectionState.Battle_Magic_Selecting_Target_Multiple_Enemies, Enums.SelectionState.Battle_Magic_Selecting_Target_Characters},
		{Enums.SelectionState.Battle_Item_Selecting_Target_Multiple_Characters, Enums.SelectionState.Battle_Item_Selecting_Target_Enemies},
		{Enums.SelectionState.Battle_Item_Selecting_Target_Multiple_Enemies, Enums.SelectionState.Battle_Item_Selecting_Target_Characters},
		
        // No change...
        {Enums.SelectionState.Battle_Tool_Selecting_Target_Multiple, Enums.SelectionState.Battle_Tool_Selecting_Target_Multiple}
    };



    public static readonly List<Enums.SelectionState> BattleWaitStates = new();

    public static readonly List<Enums.GameState> BattleActionStates = new() {
        Enums.GameState.Battle_Party_Action,
        Enums.GameState.Battle_Enemy_Action
    };

    public static readonly List<Enums.Status> AlwaysHitStatuses = new() {
        Enums.Status.Sleep,
        Enums.Status.Petrified,
        Enums.Status.Frozen,
        Enums.Status.Stop
    };




    /// <summary>
	/// Literally update the game state
	/// </summary>
	/// <param name="State"></param>
	public static void UpdateGameState(Enums.GameState State)
	{
		Globals.PreviousGameState = Globals.GameState;
		Globals.GameState = State;
	}

    // <summary>
	/// Literally update the selection (menus, etc...) state
	/// </summary>
	/// <param name="State"></param>
	public static void UpdateSelectionState(Enums.SelectionState State)
	{
		Globals.PreviousSelectionState = Globals.SelectionState;
		Globals.SelectionState = State;
	}




	/// <summary>
	/// Update the game state if in battle.
	/// This will also update the debug information on the battle screen
	/// </summary>
	/// <param name="State"></param>
	public static void Battle_UpdateGameState(object sender, Enums.GameState State)
	{
        // Debug.WriteLine($"Updating game state to --- {State} -- from: {sender}");
        UpdateGameState(State);
		BattleController.BattleDebugWindow.Set("text", $"Battle State: {GameState}");
	}


    /// <summary>
	/// Update the selection (menus) state if in battle.
	/// This will also update the debug information on the battle screen
	/// </summary>
	/// <param name="State"></param>
	public static void Battle_UpdateSelectionState(object sender, Enums.SelectionState State)
	{
        UpdateSelectionState(State);
		BattleController.SelectionDebugWindow.Set("text", $"Select State: {SelectionState}");
	}


    /// <summary>
    /// Globals Constructor
    /// </summary>
    static Globals ()
    {
        // Use other lists to simplify making any states in which "wait" applies (as opposed to "active")
        BattleWaitStates.AddRange(BattleSelectingObjectStates);
        BattleWaitStates.AddRange(BattleSelectingMenuStates);
        BattleWaitStates.AddRange(BattleSelectingEnemyStates);

        // Timer bars should still increment in the vanilla "Fight, etc..." menu
        BattleWaitStates.Remove(Enums.SelectionState.Battle_Menu_Normal);
        
        // BattleWaitStates.Add(Enums.GameState.Battle_Won);
        // BattleWaitStates.Add(Enums.GameState.Battle_Lost);

        // BattleWaitStates.Add(Enums.GameState.Battle_Party_Action);
        // BattleWaitStates.Add(Enums.GameState.Battle_Enemy_Action);

        // Store the Overworld in memory here.  The sprite is huge, so it will hang if we have to load it every time
        Overworld = ResourceLoader.Load<PackedScene>("res://Scenes/Overworld.tscn").Instantiate();
    }

}