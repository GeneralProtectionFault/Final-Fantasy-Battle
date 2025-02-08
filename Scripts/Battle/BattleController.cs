using Godot;
// using LiteDB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


public partial class BattleController : Node
{
	public static FontVariation FF6Font;
	// As set in game's config menu
	public static int BattleSpeed = 3;

	private const string CharacterScenePath = "res://Scenes/Characters/";
	private const string EnemyScenePath = "res://Scenes/Enemies/";

	private static int ActiveCharacterIndex = -1; // Stores which character is active/being controlled via Index (-1 for when no character has a full ATB gauge)
	private static int CurrentMenuIndex = 0;  // When selecting a spell, item, etc..., store the index of the selected item here for cancelling and such

	private static AudioStreamPlayer MenuSwitchSound;
	private static AudioStreamPlayer EnemyDeathSound;
	private static AudioStreamPlayerIntro Fanfare;

	// These are the objects with the character/enemy stats
	public static List<Character> Characters = new List<Character>();
	public static List<Enemy> Enemies = new List<Enemy>();
	
	// These are the Godot objects
	public static List<Node2D> CharacterObjects = new List<Node2D>();
	public static List<Node2D> EnemyObjects = new List<Node2D>();

	private static List<AnimationPlayer> CharacterAnimations = new List<AnimationPlayer>();
	private static List<int> CharactersWithFullTimerBar = new List<int>();
	private static List<int> EnemiesWithFullTimerBar = new List<int>();
	private static List<TextureProgressBar> CharacterBattleTimers = new List<TextureProgressBar>();
	private static List<TextureProgressBar> EnemyBattleTimers = new List<TextureProgressBar>();

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

	private static Sprite2D VictoryTextSprite;
	private static Label VictoryTextLabel;

	// The "bounty" from the battle is displayed incrementally as the player presses the button
	// Use this to store which one we're on when handling the button press (End of the Process method)
	private static int BattleWonTextIndex = 0;
	// This will be populated when the battle is won, and contain each thing that will get displayed @ the top of the screen after the battle.
	List<string> VictoryTextList = new List<string>();

	// Keep track of what is selected when picking the target
	// private Ability AbilitySelected;
	// private Item ItemSelected;
	dynamic ActionSelected;


	// Use this even to notify the HandCursor script that we're selecting a target
	// The string argument will be "Enemy" or "Character" depending on which to start the selection with
	public static event EventHandler SelectingTarget;
	// To return to normal selection, pass in the cursor index
	public static event EventHandler NotSelectingTarget;

	public static Node DebugWindow;

	



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Reset stuff...
		ActiveCharacterIndex = -1;
		CurrentMenuIndex = 0;

		CharacterObjects.Clear();
		EnemyObjects.Clear();
		Enemies.Clear();
		CharacterBattleTimers.Clear();
		VictoryTextList.Clear();
		BattleWonTextIndex = 0;


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
		Fanfare = GetNode<AudioStreamPlayerIntro>("BattleTemplate/Fanfare");

		// Tracks if a player is selecting something such as a spell, ability, etc... (will affect wait and all that)
		Globals.Battle_UpdateGameState(Enums.GameState.Battle);

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

		VictoryTextSprite = GetNode<Sprite2D>("BattleCanvas/BlueBattleVictoryPanel");
		VictoryTextLabel = VictoryTextSprite.GetNode<Label>("Control_VictoryText/Label_VictoryText");

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

			// The animation player used on the overworld will eff with the animations in a different animation player, so disable it here
			// (and re-enable it upon going back to the overworld)
			CharacterObject.GetNode<AnimationTree>("AnimationTree").Active = false;

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
			CharacterBattleTimers.Add(ProgressBarNode);

			NameNode.Text = CharacterName;
			NameNode.Visible = true;

			var HP = DatabaseHandler.GetCharacterStatAsString(CharacterName, "Hp");
			HPNode.Text = HP;
			HPNode.Visible = true;	
		}

		#endregion


		#region EnemySpawn
		// This will get the name of the main node, which should be named identically to the scene & database record, i.e. "BattleArea_NarshePlains1"
		var MainNode = EnemyMenuContainer.Owner.Owner;
		var BattleAreaRecord = DatabaseHandler.BattleAreaCollection.Find($"$.Name = '{MainNode.Name}'").FirstOrDefault();

		for (var i = 0; i < BattleAreaRecord.EnemyList.Count; i++)
		{	
			var EnemyName = BattleAreaRecord.EnemyList[i];

			var EnemyObject = GD.Load<PackedScene>($"{EnemyScenePath}{EnemyName}.tscn").Instantiate();
			MainNode.AddChild(EnemyObject);

			EnemyObjects.Add(EnemyObject as Node2D);

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

			// Dynamically create enemy progress bar that will track & available for debugging
			var Bar = new TextureProgressBar();
			// Use the same resources as the character bar
			var UnderTexture = (Texture2D)GD.Load("res://Graphics/MenuStuff/TimerBarEmpty.png");
			var ProgressTexture = (Texture2D)GD.Load("res://Graphics/MenuStuff/TimerBarFull.png");
			Bar.TextureUnder = UnderTexture;
			Bar.TextureProgress = ProgressTexture;
			Bar.MaxValue = 65536;
			Bar.Value = 10000;
			
			// This would default to invisible, but on for debugging
			// Bar.Visible = false;

			EnemyBattleTimers.Add(Bar);
			EnemyObject.AddChild(Bar);
		}
		#endregion
	}



	private void EndBattle()
	{
		Globals.OverworldInputEnabled = true;
		Globals.InBattle = false;
		Globals.ReturningFromBattle = true;

		HandCursor.CursorSelected -= CursorPressed;
		BattleAlgorithms.DamagingCharacter -= UpdateCharacterAfterAttack;
		BattleAlgorithms.DamagingEnemy -= UpdateAfterEnemyDamage;


		GetTree().Root.AddChild(Globals.Overworld);
		EncounterController.ResetEncounter();
		this.QueueFree();
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		#region BattleWon!
		if (Globals.GameState == Enums.GameState.Battle_Won)
		{
			if (Input.IsActionJustPressed("ui_accept"))
			{
				// If we've reached the end of the list of text stuff to display after the battle...
				// END THE BATTLE IN THIS CASE =D (Tween out, whatever...)
				if (BattleWonTextIndex == VictoryTextList.Count)
				{
					GD.Print("Battle over!");
					Globals.GameState = Enums.GameState.Battle_End;
					EndBattle();
				}
				else
				{
					// Change the text to the next "thing"
					VictoryTextLabel.Text = VictoryTextList[BattleWonTextIndex];
					BattleWonTextIndex += 1;
				}
			}
		}

		#endregion

		// Set to this from Process so as to not be "processing" when changing the scene out of battle
		if (Globals.GameState == Enums.GameState.Battle_End)
			return;

		if (Globals.BattleMode == Enums.BattleMode.Wait)
		{
			if (Globals.BattleWaitStates.Contains(Globals.GameState))
			{
				return;
			}
		}
		
		#region CharacterBattleTimers
		for (var i = 0; i < Characters.Count; i++)
		{
			// Let us not bother with battle gauges of wounded/screwed characters
			if (Characters[i].Statuses.Intersect(Globals.BattleInactiveStates).Any())
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
			// Debug.WriteLine($"CHARACTER INCREMENT: {BattleTimerIncrement}");

			// Actually set the value
			var CurrentTimerValue = CharacterBattleTimers[i].Value;
			CharacterBattleTimers[i].Value += System.Math.Min(65536 - CurrentTimerValue, BattleTimerIncrement);


			// If this player's bar is full...
			if (CharacterBattleTimers[i].Value == 65536)
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
					Globals.Battle_UpdateGameState(Enums.GameState.Battle_Menu_Normal);
				
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

		for (var i = 0; i < Enemies.Count; i++)
		{
			if (Enemies[i].Hp > 0)
			{
				var BattleTimerIncrement = 0;
				// var BattleSpeedOffset = 255 - ((BattleSpeed - 1) * 24);
				var BattleSpeedOffset = 1;

				// HASTE
				if (Enemies[i].Statuses.Contains(Enums.Status.Haste))
				{
					BattleTimerIncrement += (126 * (Enemies[i].Agility + 20)) * BattleSpeedOffset / 16;
				}
				// SLOW
				else if (Enemies[i].Statuses.Contains(Enums.Status.Slow))
				{
					BattleTimerIncrement += (48 * (Enemies[i].Agility + 20)) * BattleSpeedOffset / 16;
				}
				// (NORMAL)
				else
				{
					BattleTimerIncrement += (96 * (Enemies[i].Agility + 20)) * BattleSpeedOffset / 16;
				}
				// Debug.WriteLine($"ENEMY INCREMENT: {BattleTimerIncrement}");

				// Actually set the value
				var CurrentTimerValue = EnemyBattleTimers[i].Value;
				EnemyBattleTimers[i].Value += System.Math.Min(65536 - CurrentTimerValue, BattleTimerIncrement);
			}

			#region EnemyAction
			// If this player's bar is full...
			if (EnemyBattleTimers[i].Value == 65536)
			{
				// Active enemy logic
				// Store which enemy is full
				// Only add this in if it isn't there, or it will get added every tick that the bar is full :-P
				if (!EnemiesWithFullTimerBar.Contains(i))
					EnemiesWithFullTimerBar.Add(i);
				
				Globals.Battle_ActiveEnemyExists = true;
				// if (Globals.GameState == Enums.GameState.Battle)
				// {
					// ** QUEUE ENEMY ATTACK HERE ** - NOTE:  If we're on a "WAIT" menu, we won't even get to this part of the code
					if (Globals.GameState != Enums.GameState.Battle_Party_Action)
					{
						
						Globals.Battle_UpdateGameState(Enums.GameState.Battle_Enemy_Action);
						
						var FlashUniformMaterial = EnemyObjects[i].GetNode<Sprite2D>("Sprite2D").Material;
						
						FlashUniformMaterial.Set("shader_parameter/attack_flash", true);
						Tween FlashTween = GetTree().CreateTween();
						FlashTween.TweenCallback(Callable.From(() => {
							FlashUniformMaterial.Set("shader_parameter/attack_flash", false);
						}
						)).SetDelay(0.2f);


						var EnemyNode = EnemyObjects[i];
						(EnemyNode as IEnemyAction).ExecuteTurn(i);
						EnemyBattleTimers[i].Value = 0;
						EnemiesWithFullTimerBar.Remove(i);

					}			

					
				// }
			}
			#endregion

		}

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
				Globals.Battle_UpdateGameState(Enums.GameState.Battle_Menu_Normal);
			}
			else if (Globals.GameState == Enums.GameState.Battle_Item_Selecting_Target_Characters ||
			Globals.GameState == Enums.GameState.Battle_Item_Selecting_Target_Enemies ||
			Globals.GameState == Enums.GameState.Battle_Item_Selecting_Target_Multiple_Characters ||
			Globals.GameState == Enums.GameState.Battle_Item_Selecting_Target_Multiple_Enemies) // Item selected & targeting back to item menu
			{
				HandCursor.AssignCursorParent(ItemMenuContainer, CurrentMenuIndex);
				NotSelectingTarget?.Invoke(this, EventArgs.Empty);
				Globals.Battle_UpdateGameState(Enums.GameState.Battle_Menu_Item);
			}
			else if (Globals.GameState == Enums.GameState.Battle_Jump_Selecting_Target) // Jump & targeting back to fight menu
			{
				HandCursor.AssignCursorParent(FightMenuContainer, 1);
				NotSelectingTarget?.Invoke(this, EventArgs.Empty);
				Globals.Battle_UpdateGameState(Enums.GameState.Battle_Menu_Normal);
			}
			else if (Globals.GameState == Enums.GameState.Battle_Magic_Selecting_Target_Characters ||
			Globals.GameState == Enums.GameState.Battle_Magic_Selecting_Target_Enemies ||
			Globals.GameState == Enums.GameState.Battle_Magic_Selecting_Target_Multiple_Characters ||
			Globals.GameState == Enums.GameState.Battle_Magic_Selecting_Target_Multiple_Enemies) // Magic & targeting back to magic menu
			{
				HandCursor.AssignCursorParent(MagicMenuContainer, CurrentMenuIndex);
				NotSelectingTarget?.Invoke(this, EventArgs.Empty);
				Globals.Battle_UpdateGameState(Enums.GameState.Battle_Menu_Magic);
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
				Globals.Battle_UpdateGameState(Enums.GameState.Battle);
				HandCursor.AssignCursorParent(FightMenuContainer, 2);
				// Hide the magic menu
				MagicMenu.Visible = false;
			}
			else if (Globals.GameState == Enums.GameState.Battle_Menu_Item) // Item Menu back to fight menu
			{
				Globals.Battle_UpdateGameState(Enums.GameState.Battle);
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
				Globals.Battle_UpdateGameState(Enums.GameState.Battle_Fight_Selecting_Target_Enemies);
				SelectingTarget?.Invoke(this, EventArgs.Empty);
				return;
			}

			else if (Command == "Magic")
			{
				Globals.Battle_UpdateGameState(Enums.GameState.Battle_Menu_Magic);

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
				Globals.Battle_UpdateGameState(Enums.GameState.Battle_Menu_Item);
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
					Globals.Battle_UpdateGameState(Enums.GameState.Battle_Magic_Selecting_Target_Characters);
				else
					Globals.Battle_UpdateGameState(Enums.GameState.Battle_Magic_Selecting_Target_Enemies);

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
					Globals.Battle_UpdateGameState(Enums.GameState.Battle_Item_Selecting_Target_Characters);

					SelectingTarget?.Invoke(this, EventArgs.Empty);
				}
			}
		}
		#endregion


		#region CharacterAction
		
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
						Globals.Battle_UpdateGameState(Enums.GameState.Battle_Party_Action);				

						Character SelectedTarget = Characters[HandCursor.GetCurrentCursorIndex()];
						BattleAlgorithms.SetFightVariables(Characters[HandCursor.GetCurrentCursorIndex()], CharacterObjects[HandCursor.GetCurrentCursorIndex()]);

						// Animation plays regardless of if targeting enemies or characters.
						BattleAnimations.TriggerFightAnimation(Characters[ActiveCharacterIndex], CharacterObjects[ActiveCharacterIndex],
						Characters[HandCursor.GetCurrentCursorIndex()], CharacterObjects[HandCursor.GetCurrentCursorIndex()]);
					}
					// If targetting enemies
					else if (Globals.GameState == Enums.GameState.Battle_Fight_Selecting_Target_Enemies ||
					Globals.GameState == Enums.GameState.Battle_Fight_Selecting_Target_Multiple_Enemies)
					{
						Globals.Battle_UpdateGameState(Enums.GameState.Battle_Party_Action);

						// Set this so it can be passed back to this class so we can damage the correct enemy
						Enemies[HandCursor.GetCurrentCursorIndex()].BattleListIndex = HandCursor.GetCurrentCursorIndex();

						// Damage & Damage Text
						BattleAlgorithms.SetFightVariables(Enemies[HandCursor.GetCurrentCursorIndex()], EnemyObjects[HandCursor.GetCurrentCursorIndex()]);

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




	#region MethodsFromEvents
	
	/// <summary>
	/// Method to update character stats on the UI, etc...
	/// This needs to be done in the BattleController for access to the UI elements
	/// </summary>
	/// <param name="TheCharacter"></param>
	private void UpdateCharacterAfterAttack(object sender, Character TheCharacter)
	{
		FightMenu.Visible = false;
		HandCursorObject.Visible = false;

		CharacterBattleTimers[ActiveCharacterIndex].Value = 0;
		CharactersWithFullTimerBar.Remove(ActiveCharacterIndex);
		// The process method will set this right back appropriately if another character has a full battle gauge
		Globals.Battle_ActivePlayerExists = false;
		SetActiveCharacter(-1);

		UpdateCharacterStats(TheCharacter);

		Globals.Battle_UpdateGameState(Enums.GameState.Battle);
	}



	private void UpdateAfterEnemyDamage(object sender, Enemy TheEnemy)
	{
		FightMenu.Visible = false;
		HandCursorObject.Visible = false;

		GD.Print($"Size: {CharacterBattleTimers.Count}");
		GD.Print($"Active Character Index: {ActiveCharacterIndex}");

		// Down the Enemy's HP on the object and whatnot
		CharacterBattleTimers[ActiveCharacterIndex].Value = 0;
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
				EnemyDeathSound.Play();
				EnemyObjects[TheEnemy.BattleListIndex].QueueFree();
				
				// Remove enemy from the list (UI on the left)
				foreach (Label EnemyLabel in EnemyMenuContainer.GetChildren())
				{
					if (EnemyLabel.Text == TheEnemy.Name)
					{
						EnemyLabel.QueueFree();
						break;
					}
				}

			}
			)).SetDelay(1.0f);
		}


		// If all enemies have <= 0 HP
		if (Enemies.Where(x => x.Hp > 0).Count() <= 0)
		{
			Globals.Battle_UpdateGameState(Enums.GameState.Battle_Won);
			ActiveCharacterIcon.Visible = false;

			Tween BattleEndTween = GetTree().CreateTween();

			BattleEndTween.TweenCallback(Callable.From(()=> {
				GetNode<AudioStreamPlayer>("BattleTemplate/BattleTheme").Stop();
				Fanfare.Play();

				var UnwoundedCharacters = Characters.Where(x => x.Hp > 0).ToList();

				// Play victory animation if the character is still standing :)
				foreach (Character Character in Characters.Where(x => x.Hp > 0))
				{
					// Get the list index so we can get the Node2D from that list (indices will be the same)
					var Index = Characters.FindIndex(x => x == Character);

					var Player = CharacterObjects[Index].GetNode<AnimationPlayer>("Battle_AnimationPlayer");
					Player.Queue("Spin");
					Player.Queue("Fanfare");
				}


				// Experience ********************************
				var Exp = 0;
				foreach(var Enemy in Enemies)
				{
					Exp += Enemy.ExperienceGiven;
				}

				// I THINK the experience in FF6 is displaying PER CHARACTER, ergo...
				Exp = Exp / UnwoundedCharacters.Count;
				VictoryTextList.Add($"Got {Exp} Exp. point(s)");

				foreach	(var Character in UnwoundedCharacters)
				{
					Character.Experience += Exp;
					DatabaseHandler.UpdateCharacter(Character);
				}
				
				// Espers / Magic points


				// Items **************************************
				// Create this as a dictionary to account for cases in which there's more than 1 of a particular item.
				// The dictionary will yield the number of items to display
				var ItemList = new Dictionary<string, int>();

				foreach (var Enemy in Enemies)
				{
					foreach(KeyValuePair<string,float> Item in Enemy.DroppedItems)
					{
						GD.Print($"Iterating over {Enemy} dropped item: {Item.Key}");

						Random gen = new Random();
						double TriggerValue = gen.NextDouble();
						double ItemProbability = Item.Value;

						GD.Print($"Trigger: {TriggerValue}");
						GD.Print($"Prob. of Item: {ItemProbability}");

						// If the random value (between 0 and 1) is greater than the stored %, they get the item
						if (TriggerValue <= ItemProbability)
						{
							GD.Print($"{Item.Key} dropped");

							var ItemData = DatabaseHandler.ItemCollection.FindOne(x => x.Name == Item.Key);
							ItemData.InventoryCount += 1;
							DatabaseHandler.UpdateItem(ItemData);


							// Check if we already have this item
							if (ItemList.ContainsKey(Item.Key))
								ItemList[Item.Key] += 1;
							else
								ItemList.Add(Item.Key, 1);
						}
					}
				}

				foreach (var Item in ItemList)
					VictoryTextList.Add($"Got {Item.Key} x {Item.Value}");

				// GP ********************************************
				var GP = 0;
				foreach (var Enemy in Enemies)
				{
					GP += Enemy.Gil;
				}

				VictoryTextList.Add($"Got {GP} GP");

				VictoryTextSprite.Visible = true;
				VictoryTextLabel.Text = VictoryTextList[0];
				BattleWonTextIndex += 1;
 
			})).SetDelay(2.0f);


		}
		else
			Globals.Battle_UpdateGameState(Enums.GameState.Battle);
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

	#endregion


}
