using Godot;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class BattleController : Node
{
	public static FontVariation FF6Font;

	private const string CharacterScenePath = "res://Scenes/Characters/";
	private const string EnemyScenePath = "res://Scenes/Enemies/";

	private static int ActiveCharacterIndex = -1; // Stores which character is active/being controlled via Index (-1 for when no character has a full ATB gauge)
	private static int CurrentMenuIndex = 0;  // When selecting a spell, item, etc..., store the index of the selected item here for cancelling and such

	private static AudioStreamPlayer MenuSwitchSound;
	private static AudioStreamPlayer EnemyDeathSound;

	public static List<Character> Characters = new List<Character>();
	public static List<Enemy> Enemies = new List<Enemy>();
	
	public static List<Node2D> CharacterObjects = new List<Node2D>();
	public static List<Node2D> EnemyObjects = new List<Node2D>();

	private static List<AnimationPlayer> CharacterAnimations = new List<AnimationPlayer>();
	private static List<int> CharactersWithFullTimerBar = new List<int>();
	private static List<TextureProgressBar> BattleTimers = new List<TextureProgressBar>();

	private static Sprite2D ActiveCharacterIcon; // The icon that appears over the head of the active character
	private static TextureRect HandCursorObject;

	// The following variables are the sprites for the menus, which is the parent of the controls
	// The "Container" variables are the child that will house the commands/spells/items/etc..., and are made variables to easily clear & populate them.
	private static Sprite2D FightMenu; // The main menu w/ Fight/Magic/Item/etc...
	private static VBoxContainer FightMenuContainer;
	private static Sprite2D MagicMenu;
	private static GridContainer MagicMenuContainer;
	private static Sprite2D ItemMenu;
	private static GridContainer ItemMenuContainer;
	private static Sprite2D EnemyMenu;
	private static VBoxContainer EnemyMenuContainer;

	private static Control CharacterGrid;


	// Keep track of what is selected when picking the target
	// private Ability AbilitySelected;
	// private Item ItemSelected;
	dynamic ActionSelected;


	// Use this even to notify the HandCursor script that we're selecting a target
	// The string argument will be "Enemy" or "Character" depending on which to start the selection with
	public static event EventHandler SelectingTarget;
	// To return to normal selection, pass in the cursor index
	public static event EventHandler NotSelectingTarget;

	private static Node DebugWindow;

	public static void Battle_UpdateGameState(Enums.GameState State)
	{
		Globals.GameState = State;
		// var DebugWindow = GetNode("BattleCanvas/Control_DebugOutput/LabelDebugOutput");
		DebugWindow.Set("text", $"Game State: {Globals.GameState.ToString()}");
	}



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Subscribe to the cursor event and handle whenever it's pressed
		HandCursor.CursorSelected += CursorPressed;
		// This event will fire when a character attacks another character
		// (Needs to be decoupled from generally damaging because of timer bars/turns/etc...)
		BattleAlgorithms.DamagingCharacter += UpdateCharacterAfterAttack;
		BattleAlgorithms.DamagingEnemy += UpdateAfterEnemyDamage;

		FF6Font = new FontVariation();
		FF6Font.Set("base_font", ResourceLoader.Load<FontFile>("res://Fonts/final_fantasy_36_font.ttf"));

		DebugWindow = GetNode("BattleCanvas/Control_DebugOutput/LabelDebugOutput");
		
		MenuSwitchSound = GetNode<AudioStreamPlayer>("BattleTemplate/MenuSwitch");
		EnemyDeathSound = GetNode<AudioStreamPlayer>("BattleTemplate/EnemyDeathSound");

		// Tracks if a player is selecting something such as a spell, ability, etc... (will affect wait and all that)
		Battle_UpdateGameState(Enums.GameState.Battle);

		// Store the hand & active cursors for controlling visibility, etc...
		HandCursorObject = GetNode<TextureRect>("BattleCanvas/HandCursor");
		ActiveCharacterIcon = GetNode<Sprite2D>("BattleCanvas/ActiveBattleCursor"); 

		FightMenu = GetNode<Sprite2D>("BattleCanvas/BlueBattlePanelPopUp");
		FightMenuContainer = FightMenu.GetNode<VBoxContainer>("Control_ActionMenuBackground/MarginContainer/VBoxContainer");
		MagicMenu = GetNode<Sprite2D>("BattleCanvas/BlueBattlePanelMagicList");
		MagicMenuContainer = MagicMenu.GetNode<GridContainer>("Control_MagicList/MarginContainer/ScrollContainer/GridContainer");
		ItemMenu = GetNode<Sprite2D>("BattleCanvas/BlueBattlePanelItemList");
		ItemMenuContainer = ItemMenu.GetNode<GridContainer>("Control_MagicList/MarginContainer/ScrollContainer/GridContainer");
		EnemyMenu = GetNode<Sprite2D>("BattleCanvas/BlueBattlePanelLeft");
		EnemyMenuContainer = EnemyMenu.GetNode<VBoxContainer>("Control_EnemyMenuBackground/MarginContainer/VBoxContainer");

		// Spawn the players in the party
		Characters = DatabaseHandler.GetCharactersInParty().ToList();
		
		// This will be used as the parent of the label stuff we need to populate in the loop below for each character.
		CharacterGrid = GetNode<Control>("BattleCanvas/BlueBattlePanelRight/Control_PartyMenuBackground/MarginContainer/GridContainer");
		// GD.Print($"Character Grid; {CharacterGrid}");


		#region CharacterSpawn

		for (var i = 0; i < Characters.Count(); i++)
		{
			// Get if the character is in the front or back
			// NAMING IMPORTANT! :D
			var RowPosition = Characters[i].RowPosition;
			var CharacterName = Characters[i].Name;

			// i + 1 because the object is not named "FrontRow_Player0" for example...
			var SpawnObject = GetNode<Node2D>($"BattleTemplate/{RowPosition}_Player{i + 1}");

			var CharacterObject = GD.Load<PackedScene>($"{CharacterScenePath}{CharacterName}.tscn").Instantiate() as Node2D;
			AddChild(CharacterObject);

			// Used in this script
			CharacterObjects.Add(CharacterObject);
			// Used for HandCursor script
			//CharactersAndEnemies.Add(CharacterObject as Node2D);

			(CharacterObject as Node2D).GlobalPosition = SpawnObject.GlobalPosition;

			// During spawning, store the animation player for each character so we can grab it from the list by an order/index :)
			var AnimPlayer = CharacterObject.GetNode<AnimationPlayer>("AnimationPlayer");
			CharacterAnimations.Add(AnimPlayer);

			// Play the battle idle animation so they each start appropriately
			AnimPlayer.Play($"{CharacterName}_IdleBattle");


			// Dynamically populate the menu crap
			var HBox = CharacterGrid.GetNode<HBoxContainer>($"HBoxContainer{i + 1}");
			HBox.Visible = true;

			var NameNode = CharacterGrid.GetNode<Label>($"Label_Player{i + 1}");
			var HPNode = CharacterGrid.GetNode<Label>($"HBoxContainer{i + 1}/HPLabel{i + 1}");
			
			var ProgressBarNode = CharacterGrid.GetNode<TextureProgressBar>($"HBoxContainer{i + 1}/TextureProgressBar{i + 1}");
			BattleTimers.Add(ProgressBarNode);

			NameNode.Text = CharacterName;
			NameNode.Visible = true;

			var HP = DatabaseHandler.GetCharacterStatAsString(CharacterName, "Hp");
			HPNode.Text = HP;
			HPNode.Visible = true;	
		}

		#endregion


		#region EnemySpawn

		// var EnemyListNode = GetNode<VBoxContainer>("BattleCanvas/BlueBattlePanelLeft/Control_EnemyMenuBackground/MarginContainer/VBoxContainer");
		
		// This will get the name of the main node, which should be named identically to the scene & database record, i.e. "BattleArea_NarshePlains1"
		var MainNode = EnemyMenuContainer.Owner.Owner;
		var BattleAreaRecord = DatabaseHandler.BattleAreaCollection.Find($"$.Name = '{MainNode.Name}'").FirstOrDefault();

		for (var i = 0; i < BattleAreaRecord.EnemyList.Count; i++)
		{	
			var EnemyName = BattleAreaRecord.EnemyList[i];

			var EnemyObject = GD.Load<PackedScene>($"{EnemyScenePath}{EnemyName}.tscn").Instantiate();
			MainNode.AddChild(EnemyObject);

			EnemyObjects.Add(EnemyObject as Node2D);

			// Used in HandCursor script
			//CharactersAndEnemies.Add(EnemyObject as Node2D);

			// Add the enemy label to the list on the left side of the screen
			var EnemyLabel = new Label();

			// Give the enemy the FF6 font :)
			EnemyLabel.Set("theme_override_fonts/font", FF6Font);
			EnemyLabel.Set("theme_override_font_sizes/font_size", 60);

			EnemyLabel.Text = EnemyName;
			EnemyMenuContainer.AddChild(EnemyLabel);
			
			// Set enemy position on screen
			// Node in format "Enemy_" (with number) to determine spawn location
			var SpawnNode = MainNode.GetNode<Node2D>($"BattleTemplate/Enemy{i + 1}");
			(EnemyObject as Node2D).GlobalPosition = SpawnNode.GlobalPosition;

			// Add actual object to list for access to the stats, etc...
			var Enemy = DatabaseHandler.EnemyCollection.FindOne(x => x.Name == EnemyName);
			GD.Print($"Adding enemy to enemy list: {Enemy.Name}");
			Enemies.Add(Enemy);

		}
		#endregion
	}




	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		#region CharacterBattleTimers

		for (var i = 0; i < Characters.Count; i++)
		{
			// Let us not bother with battle gauges of wounded characters
			// TODO:  Exclude appropriate statuses as well, like Stop
			if (Characters[i].Hp <= 0)
				continue;


			var BattleTimerIncrement = 0;

			// HASTE
			if (Characters[i].Statuses.Contains(Enums.Status.Haste))
			{
				BattleTimerIncrement += (126 * (Characters[i].Agility + 20)) / 16;
			}
			// SLOW
			else if (Characters[i].Statuses.Contains(Enums.Status.Slow))
			{
				BattleTimerIncrement += (48 * (Characters[i].Agility + 20)) / 16;
			}

			// (NORMAL)
			else
			{
				BattleTimerIncrement += (96 * (Characters[i].Agility + 20)) / 16;
			}

			// Actually set the value
			var CurrentTimerValue = BattleTimers[i].Value;
			BattleTimers[i].Value += System.Math.Min(65536 - CurrentTimerValue, BattleTimerIncrement);



			// If this player's bar is full...
			if (BattleTimers[i].Value == 65536)
			{
				// Active player logic
				// If there are no active players, make this the active player
				
				if (CharactersWithFullTimerBar.Count() == 0 || ActiveCharacterIndex == -1)
				{
					MenuSwitchSound.Play();
					SetActiveCharacter(i);
					HandCursor.AssignCursorParent(FightMenuContainer);
				}

				// Store which character is full
				// Only add this in if it isn't there, or it will get added every tick that the bar is full :-P
				if (!CharactersWithFullTimerBar.Contains(i))
					CharactersWithFullTimerBar.Add(i);
				
				Globals.Battle_ActivePlayerExists = true;
				if (Globals.GameState == Enums.GameState.Battle)
				{
					Battle_UpdateGameState(Enums.GameState.Battle_Menu_Normal);
				
					// Reveal the menu!
					if (FightMenu.Visible == false)
						FightMenu.Visible = true;
					if (HandCursorObject.Visible == false)
						HandCursorObject.Visible = true;
				}
			}
		}

		#endregion

		#region EnemyBattleTimers

		// foreach (var Enemy in Enemies)
		// {
		// 	if (Enemy.Hp > 0)
		// 	{
		// 		var EnemyBattleTimerIncrement = 0;
		// 	}

		// }

		#endregion

	}

	/// <summary>
	/// This event brought to you by HandCursor.cs
	/// We be handling all the input here.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="Command"></param>
	private void CursorPressed(object sender, string Command)
	{
		GD.Print($"{Command} pressed!");

		// Cancellation - return to prior menu...
		if (Command == "ui_cancel")
		{
			HandCursorObject.Visible = true;

			if (Globals.GameState == Enums.GameState.Battle_Fight_Selecting_Target_Characters ||
			Globals.GameState == Enums.GameState.Battle_Fight_Selecting_Target_Enemies ||
			Globals.GameState == Enums.GameState.Battle_Fight_Selecting_Target_Multiple_Characters ||
			Globals.GameState == Enums.GameState.Battle_Fight_Selecting_Target_Multiple_Enemies) // Fight & targeting back to fight menu
			{
				HandCursor.AssignCursorParent(FightMenuContainer);
				NotSelectingTarget?.Invoke(this, EventArgs.Empty);
				Battle_UpdateGameState(Enums.GameState.Battle_Menu_Normal);
			}
			else if (Globals.GameState == Enums.GameState.Battle_Item_Selecting_Target_Characters ||
			Globals.GameState == Enums.GameState.Battle_Item_Selecting_Target_Enemies ||
			Globals.GameState == Enums.GameState.Battle_Item_Selecting_Target_Multiple_Characters ||
			Globals.GameState == Enums.GameState.Battle_Item_Selecting_Target_Multiple_Enemies) // Item selected & targeting back to item menu
			{
				HandCursor.AssignCursorParent(ItemMenuContainer, CurrentMenuIndex);
				NotSelectingTarget?.Invoke(this, EventArgs.Empty);
				Battle_UpdateGameState(Enums.GameState.Battle_Menu_Item);
			}
			else if (Globals.GameState == Enums.GameState.Battle_Jump_Selecting_Target) // Jump & targeting back to fight menu
			{
				HandCursor.AssignCursorParent(FightMenuContainer, 1);
				NotSelectingTarget?.Invoke(this, EventArgs.Empty);
				Battle_UpdateGameState(Enums.GameState.Battle_Menu_Normal);
			}
			else if (Globals.GameState == Enums.GameState.Battle_Magic_Selecting_Target_Characters ||
			Globals.GameState == Enums.GameState.Battle_Magic_Selecting_Target_Enemies ||
			Globals.GameState == Enums.GameState.Battle_Magic_Selecting_Target_Multiple_Characters ||
			Globals.GameState == Enums.GameState.Battle_Magic_Selecting_Target_Multiple_Enemies) // Magic & targeting back to magic menu
			{
				HandCursor.AssignCursorParent(MagicMenuContainer, CurrentMenuIndex);
				NotSelectingTarget?.Invoke(this, EventArgs.Empty);
				Battle_UpdateGameState(Enums.GameState.Battle_Menu_Magic);
			}
			else if (Globals.GameState == Enums.GameState.Battle_Menu_Blitz)
			{

			}
			else if (Globals.GameState == Enums.GameState.Battle_Menu_Dance)
			{

			}
			else if (Globals.GameState == Enums.GameState.Battle_Menu_Lore)
			{

			}
			else if (Globals.GameState == Enums.GameState.Battle_Menu_Rage)
			{

			}
			else if (Globals.GameState == Enums.GameState.Battle_Menu_Magic) // Magic Menu back to fight menu
			{
				Battle_UpdateGameState(Enums.GameState.Battle);
				HandCursor.AssignCursorParent(FightMenuContainer, 2);
				// Hide the magic menu
				MagicMenu.Visible = false;
			}
			else if (Globals.GameState == Enums.GameState.Battle_Menu_Item) // Item Menu back to fight menu
			{
				Battle_UpdateGameState(Enums.GameState.Battle);
				HandCursor.AssignCursorParent(FightMenuContainer, 3);
				// Hide the item menu
				ItemMenu.Visible = false;
			}

		}


		#region NormalBattleMenu
		if (Globals.GameState == Enums.GameState.Battle_Menu_Normal)
		{

			// "Top" button of the main 4 buttons
			// If the icon is not visilbe, that should mean we set it invisible after a turn had commenced, and this should happen
			// irrespective of the button press
			if (Command == "top_action" || ActiveCharacterIcon.Visible == false)
			{
				MenuSwitchSound.Play();

				// Switch which character is active if we're not in a selection menu AND if an ADDITIONAL player has a full ATB gauge
				// The plain "Battle" state implies not in a menu selecting something
				if (CharactersWithFullTimerBar.Count > 1)
				{
					int ListIndex = CharactersWithFullTimerBar.FindIndex(x => x == ActiveCharacterIndex);
					int NewIndex = ListIndex + 1;

					// If we're at the end of the list, flip back to the beginning of it
					if (ListIndex == CharactersWithFullTimerBar.Count - 1)
						NewIndex = 0;

					SetActiveCharacter(CharactersWithFullTimerBar[NewIndex]);
				}
				return;
			}

			else if (Command == "Fight")
			{
				Battle_UpdateGameState(Enums.GameState.Battle_Fight_Selecting_Target_Enemies);
				SelectingTarget?.Invoke(this, EventArgs.Empty);
				return;
			}

			else if (Command == "Magic")
			{
				Battle_UpdateGameState(Enums.GameState.Battle_Menu_Magic);

				// Make the magic menu stuff visible
				MagicMenu.Visible = true;

				// Populate the magic for the ACTIVE character
				PopulateMagicList(ActiveCharacterIndex);

				// Set the Hand Cursor's parent so the hand cursor now applies to this menu!
				HandCursor.AssignCursorParent(MagicMenuContainer);
				return;
			}

			else if (Command == "Item")
			{
				Battle_UpdateGameState(Enums.GameState.Battle_Menu_Item);
				ItemMenu.Visible = true;
				PopulateItemList();
				HandCursor.AssignCursorParent(ItemMenuContainer);
				return;
			}


		}
		#endregion


		#region MagicMenu
		if (Globals.GameState == Enums.GameState.Battle_Menu_Magic)
		{
			var Spell = DatabaseHandler.AbilityCollection.FindOne(x => x.Name == Command);

			if (Spell is not null)
			{
				CurrentMenuIndex = HandCursor.GetCurrentCursorIndex();

				// Store the spell here
				ActionSelected = Spell;
				// GD.Print($"Magic selected :{ActionSelected.Name}");
				
				if (Spell.MagicClass == "White")
					Battle_UpdateGameState(Enums.GameState.Battle_Magic_Selecting_Target_Characters);
				else
					Battle_UpdateGameState(Enums.GameState.Battle_Magic_Selecting_Target_Enemies);

				// Here & now, change how the HandCursor script works!
				SelectingTarget?.Invoke(this, EventArgs.Empty);
			}
			return;
		}
		#endregion


		#region ItemMenu
		if (Globals.GameState == Enums.GameState.Battle_Menu_Item)
		{
			// The Item includes space-surrounded colon and the inventory count.  To find it, we need to strip off that stuff
			
			var SubStrIndex = Command.IndexOf(':') - 1; // -1 to axe the space

			if (SubStrIndex >= 0)
			{
				var ItemNameOnly = Command.Substring(0, SubStrIndex);
				// GD.Print($"Passed in Item name: {ItemNameOnly}");

				var Item = DatabaseHandler.ItemCollection.FindOne(x => x.Name == ItemNameOnly);
				
				if (Item is not null)
				{
					CurrentMenuIndex = HandCursor.GetCurrentCursorIndex();
					
					// ItemSelected = Item;
					ActionSelected = Item;
					GD.Print($"Item selected: {ActionSelected.Name}");
					Battle_UpdateGameState(Enums.GameState.Battle_Item_Selecting_Target_Characters);

					SelectingTarget?.Invoke(this, EventArgs.Empty);
				}
			}
		}
		#endregion


		#region SelectingTarget
		
		if (Command == "ui_accept")
		{
			// FIGHT
			// *** If we're targetting characters OR enemies! ***
			if (Globals.GameState == Enums.GameState.Battle_Fight_Selecting_Target_Characters ||
				Globals.GameState == Enums.GameState.Battle_Fight_Selecting_Target_Multiple_Characters ||
				Globals.GameState == Enums.GameState.Battle_Fight_Selecting_Target_Enemies ||
				Globals.GameState == Enums.GameState.Battle_Fight_Selecting_Target_Multiple_Enemies)
			{
				HandCursorObject.Visible = false;
				ActiveCharacterIcon.Visible = false;
				NotSelectingTarget?.Invoke(this, EventArgs.Empty);

				// TODO :Set appropriate menu visibilities to false


					// If targetting characters
					if (Globals.GameState == Enums.GameState.Battle_Fight_Selecting_Target_Characters ||
					Globals.GameState == Enums.GameState.Battle_Fight_Selecting_Target_Multiple_Characters)
					{
						Battle_UpdateGameState(Enums.GameState.Battle_Party_Action);				

						Character SelectedTarget = Characters[HandCursor.GetCurrentCursorIndex()];
						BattleAlgorithms.SetFightVariables(Characters[ActiveCharacterIndex], CharacterObjects[ActiveCharacterIndex],
						Characters[HandCursor.GetCurrentCursorIndex()], CharacterObjects[HandCursor.GetCurrentCursorIndex()]);

						// Animation plays regardless of if targeting enemies or characters.
						BattleAnimations.TriggerFightAnimation(Characters[ActiveCharacterIndex], CharacterObjects[ActiveCharacterIndex],
						Characters[HandCursor.GetCurrentCursorIndex()], CharacterObjects[HandCursor.GetCurrentCursorIndex()]);
					}
					// If targetting enemies
					else if (Globals.GameState == Enums.GameState.Battle_Fight_Selecting_Target_Enemies ||
					Globals.GameState == Enums.GameState.Battle_Fight_Selecting_Target_Multiple_Enemies)
					{
						Battle_UpdateGameState(Enums.GameState.Battle_Party_Action);

						// Set this so it can be passed back to this class so we can damage the correct enemy
						Enemies[HandCursor.GetCurrentCursorIndex()].BattleListIndex = HandCursor.GetCurrentCursorIndex();

						// Damage & Damage Text
						BattleAlgorithms.SetFightVariables(Characters[ActiveCharacterIndex], CharacterObjects[ActiveCharacterIndex],
						Enemies[HandCursor.GetCurrentCursorIndex()], EnemyObjects[HandCursor.GetCurrentCursorIndex()]);

						// Animation plays regardless of if targeting enemies or characters.
						BattleAnimations.TriggerFightAnimation(Characters[ActiveCharacterIndex], CharacterObjects[ActiveCharacterIndex],
						Enemies[HandCursor.GetCurrentCursorIndex()], EnemyObjects[HandCursor.GetCurrentCursorIndex()]);
					}
			}


		}
		#endregion
	}


	private void SetActiveCharacter(int Index)
	{
		GD.Print("Setting active character...");

		ActiveCharacterIndex = Index;

		if (ActiveCharacterIndex >= 0)
		{
			// Ensure active icon is visible and locate it above character
			if (!ActiveCharacterIcon.Visible)
				ActiveCharacterIcon.Visible = true;
			
			var CursorPlayer = ActiveCharacterIcon.GetNode<AnimationPlayer>("AnimationPlayer");

			// Trigger the animation
			CursorPlayer.Play("CursorActive");
			

			var ActiveCharacterObject = CharacterObjects[Index];
			ActiveCharacterIcon.GlobalPosition = new Vector2((ActiveCharacterObject as Node2D).GlobalPosition.X, (ActiveCharacterObject as Node2D).GlobalPosition.Y - 110);
		}
	}


	
	private void PopulateMagicList(int CharacterIndex)
	{
		GD.Print("Populating Magic...");

		// Clear any labels first
		foreach (var Label in MagicMenuContainer.GetChildren())
			Label.QueueFree();

		// Get the spells!
		var CharacterName = Characters[CharacterIndex].Name;
		var CharacterMagic = DatabaseHandler.GetCharacterMagic(CharacterName);

		// Load the Icon images
		var WhiteMagicIcon = GD.Load("res://Graphics/Icons/WhiteMagicIcon_4x.png");
		var BlackMagicIcon = GD.Load("res://Graphics/Icons/BlackMagicIcon_4x.png");
		var GrayMagicIcon = GD.Load("res://Graphics/Icons/GrayMagicIcon_4x.png");

		foreach (var Spell in CharacterMagic.Where(Spell => Spell.MagicClass == "White"))
		{
			// Container to hold the icon & spell together
			// (Otherwise the icon and spell text will be treated as separat columns and look poop)
			var SpellContainer = new HBoxContainer();

			// This will be the "circular" magic icon next to the spell, indicating if it's white, black or gray magic
			var SpellIcon = new TextureRect();
			SpellIcon.Set("texture", WhiteMagicIcon);
			// 5 = "Keep Aspect Centered"
			SpellIcon.Set("stretch_mode", 5);

			// Add the enemy label to the list on the left side of the screen
			var SpellLabel = new Label();

			// Give the enemy the FF6 font :)
			SpellLabel.Set("theme_override_fonts/font", FF6Font);
			SpellLabel.Set("theme_override_font_sizes/font_size", 60);
			SpellLabel.Text = Spell.Name;

			SpellContainer.AddChild(SpellIcon);
			SpellContainer.AddChild(SpellLabel);

			MagicMenuContainer.AddChild(SpellContainer);
		}

		foreach (var Spell in CharacterMagic.Where(Spell => Spell.MagicClass == "Black"))
		{
			// Container to hold the icon & spell together
			// (Otherwise the icon and spell text will be treated as separat columns and look poop)
			var SpellContainer = new HBoxContainer();

			// This will be the "circular" magic icon next to the spell, indicating if it's white, black or gray magic
			var SpellIcon = new TextureRect();
			SpellIcon.Set("texture", BlackMagicIcon);
			// 5 = "Keep Aspect Centered"
			SpellIcon.Set("stretch_mode", 5);

			// Add the enemy label to the list on the left side of the screen
			var SpellLabel = new Label();

			// Give the enemy the FF6 font :)
			SpellLabel.Set("theme_override_fonts/font", FF6Font);
			SpellLabel.Set("theme_override_font_sizes/font_size", 60);
			SpellLabel.Text = Spell.Name;

			SpellContainer.AddChild(SpellIcon);
			SpellContainer.AddChild(SpellLabel);

			MagicMenuContainer.AddChild(SpellContainer);
		}

		foreach (var Spell in CharacterMagic.Where(Spell => Spell.MagicClass == "Gray"))
		{
			// Container to hold the icon & spell together
			// (Otherwise the icon and spell text will be treated as separat columns and look poop)
			var SpellContainer = new HBoxContainer();

			// This will be the "circular" magic icon next to the spell, indicating if it's white, black or gray magic
			var SpellIcon = new TextureRect();
			SpellIcon.Set("texture", GrayMagicIcon);
			// 5 = "Keep Aspect Centered"
			SpellIcon.Set("stretch_mode", 5);

			// Add the enemy label to the list on the left side of the screen
			var SpellLabel = new Label();

			// Give the enemy the FF6 font :)
			SpellLabel.Set("theme_override_fonts/font", FF6Font);
			SpellLabel.Set("theme_override_font_sizes/font_size", 60);
			SpellLabel.Text = Spell.Name;

			SpellContainer.AddChild(SpellIcon);
			SpellContainer.AddChild(SpellLabel);

			MagicMenuContainer.AddChild(SpellContainer);
		}

	}
	


	private void PopulateItemList()
	{
		// Clear any labels first
		foreach (var Label in ItemMenuContainer.GetChildren())
			Label.QueueFree();

		var Items = DatabaseHandler.ItemCollection.Find("$.InventoryCount > 0");

		foreach(var Item in Items)
		{
			Label ItemLabel = new Label();

			// Give the enemy the FF6 font :)
			ItemLabel.Set("theme_override_fonts/font", FF6Font);
			ItemLabel.Set("theme_override_font_sizes/font_size", 60);
			ItemLabel.Text = Item.Name + " : " + Item.InventoryCount.ToString();

			ItemMenuContainer.AddChild(ItemLabel);
		}
	}




	/// <summary>
	/// Method to update character stats on the UI, etc...
	/// This needs to be done in the BattleController for access to the UI elements
	/// </summary>
	/// <param name="TheCharacter"></param>
	private void UpdateCharacterAfterAttack(object sender, Character TheCharacter)
	{
		BattleTimers[ActiveCharacterIndex].Value = 0;
		CharactersWithFullTimerBar.Remove(ActiveCharacterIndex);
		// The process method will set this right back appropriately if another character has a full battle gauge
		Globals.Battle_ActivePlayerExists = false;
		SetActiveCharacter(-1);

		UpdateCharacterStats(TheCharacter);

		Battle_UpdateGameState(Enums.GameState.Battle);
	}


	private void UpdateAfterEnemyDamage(object sender, Enemy TheEnemy)
	{
		// Down the Enemy's HP on the object and whatnot
		BattleTimers[ActiveCharacterIndex].Value = 0;
		CharactersWithFullTimerBar.Remove(ActiveCharacterIndex);
		// The process method will set this right back appropriately if another character has a full battle gauge
		Globals.Battle_ActivePlayerExists = false;
		SetActiveCharacter(-1);

		// Update the stats of the enemy 
		var Enemy = Enemies[TheEnemy.BattleListIndex];
		
		// NEW =D
		var EnemyObject = EnemyObjects[TheEnemy.BattleListIndex];
		
		GD.Print($"Enemy HP before: {Enemy.Hp}");
		GD.Print($"Enemy HP after: {TheEnemy.Hp}");

		// Set it to the TheEnemy w/ the updated stats
		Enemy = TheEnemy;

		// Remove upon death!
		if (TheEnemy.Hp <= 0)
		{
			Tween DelayTween = GetTree().CreateTween();

			DelayTween.TweenCallback(Callable.From(() => {
				// foreach (var Child in EnemyObject.GetChildren())
				// {
				// 	Child.QueueFree();
				// }

				EnemyDeathSound.Play();
				EnemyObjects[TheEnemy.BattleListIndex].QueueFree();
			}
			)).SetDelay(1.0f);

			
		}



		Battle_UpdateGameState(Enums.GameState.Battle);
	}


	private void UpdateCharacterStats(Character TheCharacter)
	{
		var CharacterName = TheCharacter.Name;
		var TargetIndex = "0";

		// We're just after the number here, 1-4 - the label with the character's name
		foreach (Label Label in GetTree().GetNodesInGroup("CharacterNameLabels"))
		{
			if(Label.Text.Contains(CharacterName))
			{
				var LabelName = Label.Name.ToString();
				TargetIndex = LabelName.Substring(LabelName.Length - 1);
				GD.Print($"{CharacterName} found in {Label}");
				GD.Print($"Target index is {TargetIndex}");
			}
		}

		// Update the similarly numbered label that actually corresponds tot he HP
		foreach (Label Label in GetTree().GetNodesInGroup("HPLabels"))
		{
			if (Label.Name.ToString().Contains(TargetIndex))
			{
				Label.Text = TheCharacter.Hp.ToString();
				GD.Print($"Updating text in {Label}");
			}
		}
	}
	


}
