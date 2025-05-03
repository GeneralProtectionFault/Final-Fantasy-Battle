using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public partial class HandCursor : TextureRect
{
	[Export]
	NodePath MenuParentPath;
	[Export]
	Vector2 CursorOffset;

	private static HandCursor ThisHandCursor; // Need singleton functionality in a few spots :)

	public static event EventHandler<string> CursorSelected; // Fire off whenever any cursor input from this script happens.

	// Store the cursor position - This will need to be reset whenever changing the MenuParent!
	// The menu items, as well as characters/enemies, will be stored as child incices & C# list objects respectively.
	// This means this variable will indicate the position/selection for both menu items and characters/enemies. 
	private static int CursorIndex = 0; 

	private static ScrollContainer Scroller; // This will hold the ScrollContainer object when applicable (Magic/Item menus, etc...)
	private static Control MenuItem; // The individual item being selected in a menu
	private static Node MenuParent;	// The direct parent of the MenuItem (this will be populated per character spells, items in inventory, etc...)
	
	private static Enums.HandCursorMode HandCursorMode; // For delineating behavior between selecting menu items & characters/enemies
	private static AudioStreamPlayer FingerSoundPlayer;

	private static BattleGameObject SelectedObject;


	// This is public so that the hand cursor can be applied to other menus/nodes from outside the script
	// Probably needs a set method to move parenting (as opposed to just assigning)
	public static void AssignCursorParent(Node Parent, int CursorPosition = 0)
	{
		CursorIndex = CursorPosition;
		MenuParent = Parent;
		
		if (MenuParent.GetParent().GetType().Equals(new ScrollContainer().GetType()))
		{
			Scroller = MenuParent.GetParent() as ScrollContainer;
		}

		ThisHandCursor.SetCursorFromIndex(CursorIndex);
	}
	

	#region EventFiredMethods

	private void EnableObjectSelectMode(object sender, EventArgs e)
	{
		Debug.WriteLine($"Enabling object selection mode; {e}");
		HandCursorMode = Enums.HandCursorMode.Object;
		CursorIndex = 0; // Since we will be moving from a menu to selecting characters & enemies, this needs to reset

		// Set the hand cursor to the initial target
		TargetCurrent();
	}


	private void EnableMenuSelectMode(object sender, EventArgs e)
	{
		Debug.WriteLine($"Enabling menu selection mode; {e}");
		HandCursorMode = Enums.HandCursorMode.Menu;
	}

	#endregion


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ThisHandCursor = this;

		BattleController.SelectingTarget += EnableObjectSelectMode;
		BattleController.NotSelectingTarget += EnableMenuSelectMode;

		HandCursorMode = Enums.HandCursorMode.Menu;

		MenuParent = GetNode<Node>(MenuParentPath);

		
		SetCursorFromIndex(CursorIndex);
		// This sucky piece of code is here because putting it in _Ready borks if this cursor starts off invisible :(
		if (FingerSoundPlayer is null || !IsInstanceValid(FingerSoundPlayer))
			FingerSoundPlayer = GetNode<AudioStreamPlayer>("FingerSound");

		(this as TextureRect).Visible = false;
	}


	public void TreeExiting()
	{
		// GD.Print("HandCursor Exiting...");
		BattleController.SelectingTarget -= EnableObjectSelectMode;
		BattleController.NotSelectingTarget -= EnableMenuSelectMode;
	}


	public override void _Process(double delta)
	{
		var Input = Vector2.Zero;

		// Debug.WriteLine($"ACTIVE CHARACTER? {BattleController.Characters.Where(x => x.IsActiveCharacter).Any()}");
		
		// If in a battle and if there ought be a menu...
		if (!Globals.CursorLocked && BattleController.Characters.Where(x => x.IsActiveCharacter).Any())
		{
			#region Universal Selection

			if (Godot.Input.IsActionJustPressed("ui_cancel"))
			{
				CursorSelected?.Invoke(this, "ui_cancel");
				return;
			}

			#endregion


			#region Menu Selection
			if (HandCursorMode == Enums.HandCursorMode.Menu)
			{
				// Input handling
				if (Godot.Input.IsActionJustPressed("ui_up"))
				{
					Input.Y -= 1;
					// Also play the sound for moving (or trying to move) selection
					FingerSoundPlayer.Play();
				}
				else if (Godot.Input.IsActionJustPressed("ui_down"))
				{
					Input.Y += 1;
					// Also play the sound for moving (or trying to move) selection
					FingerSoundPlayer.Play();
				}
				else if (Godot.Input.IsActionJustPressed("ui_left"))
				{
					Input.X -= 1;
					// Also play the sound for moving (or trying to move) selection
					FingerSoundPlayer.Play();
				}
				else if (Godot.Input.IsActionJustPressed("ui_right"))
				{
					Input.X += 1;
					// Also play the sound for moving (or trying to move) selection
					FingerSoundPlayer.Play();
				}
				

				else if (Godot.Input.IsActionJustPressed("ui_select"))
				{
					CursorSelected?.Invoke(this, "top_action");
					return;
				}
				

				// *** Confirming any choice ***
				else if (Godot.Input.IsActionJustPressed("ui_accept"))
				{
					Debug.WriteLine($"UI ACCEPT IN MENU MODE!");
					var CurrentMenuItem = GetMenuItemAtIndex(CursorIndex);
					if (CurrentMenuItem is not null)
					{
						FingerSoundPlayer.Play();

						// This mess accouts for if the finger is pointing at a Label or HBoxContainer (since we use the latter for icon + magic spell)
						// In either case, nfires the event off so the BattleController can receive the selected item/spell/whatever and act on it
						if (CurrentMenuItem is Label)
							CursorSelected?.Invoke(this, (CurrentMenuItem as Label).Text);
						else if (CurrentMenuItem is HBoxContainer)
							// This will be the MagicList HBoxContainer w/ icon & label
							CursorSelected?.Invoke(this, ((CurrentMenuItem as Control).GetChild(1) as Label).Text);
					}
				}
				
				// Skip if no input :)
				else
					return;
			

				// Also play the sound for moving (or trying to move) selection
				FingerSoundPlayer.Play();
				
				// This will result in the cursor index incrementing/decrementing, and we don't want that to happen twice if we're not in "menu" mode, and vice versa
				if (Globals.BattleSelectingMenuStates.Contains(Globals.SelectionState))
				{
					// Handle (d-pad, not the ui_accept bit) depending on type of container
					if (MenuParent is VBoxContainer)
						SetCursorFromIndex(CursorIndex + (int)Input.Y);
					else if (MenuParent is HBoxContainer)
						SetCursorFromIndex(CursorIndex + (int)Input.X);
					else if (MenuParent is GridContainer)
						SetCursorFromIndex(CursorIndex + (int)Input.X + (int)Input.Y * (MenuParent as GridContainer).Columns);
				}
			}
			#endregion
		


			#region Object Selection
			// OBJECT INPUT (selecting characters, monsters, etc...)
			else if (HandCursorMode == Enums.HandCursorMode.Object)
			{
				// Debug.WriteLine("HAND CURSOR OBJECT MODE");
				// BATTLE
				if (Globals.BattleSelectingObjectStates.Contains(Globals.SelectionState))
				{
					// Debug.WriteLine("HAND CURSOR AND SELECTION STATE CONFIRMED");
					if (Godot.Input.IsActionJustPressed("ui_down"))
					{
						// Also play the sound for moving (or trying to move) selection
						FingerSoundPlayer.Play();
						TargetPrevious();
					}
					else if (Godot.Input.IsActionJustPressed("ui_up"))
					{
						// Also play the sound for moving (or trying to move) selection
						FingerSoundPlayer.Play();
						TargetNext();
					}

					
					// To switch between characters & enemies
					else if (Godot.Input.IsActionJustPressed("ui_left") || Godot.Input.IsActionJustPressed("ui_right"))
					{
						CursorIndex = 0;

						FingerSoundPlayer.Play();
						// If we're targetting characters, switch to enemies & vice versa
						Globals.Battle_UpdateSelectionState(this, Globals.SelectingStateOpposites[Globals.SelectionState]);
						TargetCurrent();
					};

			
				}


				if (Godot.Input.IsActionJustPressed("ui_accept"))
				{
					CursorSelected?.Invoke(this, "ui_accept");
				}
			}
			#endregion
		}
		
	}


	/// <summary>
	/// Call from other script(s) to get the CursorIndex
	/// </summary>
	/// <returns></returns>
	public static int GetCurrentCursorIndex()
	{
		return CursorIndex;
	}


	/// <summary>
	/// Primarily used when first selecting a spell/item/etc...
	/// </summary>
	private void TargetCurrent()
	{
		Debug.WriteLine("TARGETTING CURRENT (FIRST TARGET)");
		Sprite2D ObjectSprite;

		if (Globals.BattleSelectingEnemyStates.Contains(Globals.SelectionState))
		{
			SelectedObject = BattleController.Enemies[0];
		}
		else if (Globals.BattleSelectingCharactersStates.Contains(Globals.SelectionState))
		{
			SelectedObject = BattleController.Characters[0];
		}

		ObjectSprite = SelectedObject.EntityNode.GetNode<Sprite2D>("Sprite2D");
		SetCursorPosition_Object(ObjectSprite);
	}


	private void TargetNext()
	{
		if (Globals.BattleSelectingEnemyStates.Contains(Globals.SelectionState))
		{
			// Only target the next enemy if there is more than 1
			if (BattleController.Enemies.Count > 1)
			{
				SelectedObject = BattleController.Enemies.Concat(BattleController.Enemies).SkipWhile(x => x == SelectedObject).First();
			}
		}
		else if (Globals.BattleSelectingCharactersStates.Contains(Globals.SelectionState))
		{
			// Only target the next character if there is a next character that isn't wounded, zombied, etc...
			if (BattleController.Characters.Where(x => Globals.BattleInactiveStatuses.Intersect(x.EntityData.Statuses).Any()).Count() > 1)
			{
				SelectedObject = BattleController.Characters.Concat(BattleController.Characters).SkipWhile(x => x == SelectedObject).First();
			}
		}

		
		// Node2D NextObject = BattleController.CharactersAndEnemies[CursorIndex];
		var ObjectSprite = SelectedObject.EntityNode.GetNode<Sprite2D>("Sprite2D");
		SetCursorPosition_Object(ObjectSprite);
	}

	private void TargetPrevious()
	{
		if (Globals.BattleSelectingEnemyStates.Contains(Globals.SelectionState))
		{
			// Only target the next enemy if there is more than 1
			if (BattleController.Enemies.Count > 1)
			{
				List<BattleGameObject> reversedEnemies = BattleController.Enemies.ToList();
				reversedEnemies.Reverse();
				SelectedObject = reversedEnemies.Concat(reversedEnemies).Reverse().SkipWhile(x => x == SelectedObject).First();
			}
		}
		else if (Globals.BattleSelectingCharactersStates.Contains(Globals.SelectionState))
		{
			// Only target the next character if there is a next character that isn't wounded, zombied, etc...
			if (BattleController.Characters.Where(x => Globals.BattleInactiveStatuses.Intersect(x.EntityData.Statuses).Any()).Count() > 1)
			{
				List<BattleGameObject> reversedCharacters = BattleController.Characters.ToList();
				reversedCharacters.Reverse();
				SelectedObject = reversedCharacters.Concat(reversedCharacters).Reverse().SkipWhile(x => x == SelectedObject).First();
			}
		}

		
		// Node2D NextObject = BattleController.CharactersAndEnemies[CursorIndex];
		var ObjectSprite = SelectedObject.EntityNode.GetNode<Sprite2D>("Sprite2D");
		SetCursorPosition_Object(ObjectSprite);
	}



	private Control GetMenuItemAtIndex(int Index)
	{
		if (MenuParent is null)
			return null;

		if (Index >= MenuParent.GetChildCount() || Index < 0)
			return null;

		return MenuParent.GetChild(Index) as Control;
	}


	private void SetCursorFromIndex(int Index)
	{
		MenuItem = GetMenuItemAtIndex(Index);
		
		if (MenuItem is null)
			return;

		// var GlobalPosition = (MenuItem as Control).GlobalPosition;
        // GD.Print($"Menu Item: {MenuItem}, Global Position Before: {GlobalPosition}");

        CursorIndex = Index;

		// If it's a scroll menu, this should not be null and this will keep the selected item in view
		// if (Scroller is not null && MenuParent.GetParent().GetType().Equals(new ScrollContainer().GetType()))
		if (Scroller is not null && MenuParent.GetParent() is ScrollContainer)
        	Scroller.EnsureControlVisible(MenuItem);
		
		CallDeferred("SetCursorPosition");
	}


	private void SetCursorPosition()
	{
		var Selected_GlobalPosition = (MenuItem as Control).GlobalPosition;
		
		// MenuItem is the Labels
		// Casting these as Control because we also want to use it on HBoxContainer (for the icons & spells together),
		// and since they both inherit from Control, and Control has a Size property, we can get the size whichever it is.
		var Size = MenuItem.Size;
		var NewPosition = new Vector2(Selected_GlobalPosition.X, Selected_GlobalPosition.Y + Size.Y / 2.0f) - ThisHandCursor.Size / 2.0f - CursorOffset;
		// GD.Print($"New Cursor Position: {NewPosition}");
		ThisHandCursor.SetGlobalPosition(NewPosition);	
	}


	private void SetCursorPosition_Object(Sprite2D ObjectSprite)
	{
		var SpriteOrigin = ObjectSprite.GetGlobalTransform().Origin;
		var SpriteSize = ObjectSprite.GetRect().Size;

		// Origin.....subtract half the width (to the left edge)....and the offset (the Y offset doesn't make as much sense on the characters/enemies)
		var NewCursorPosition = SpriteOrigin - new Vector2((SpriteSize.X / 2f + 25), 0) - new Vector2(CursorOffset.X, 0);

		ThisHandCursor.SetGlobalPosition(NewCursorPosition);
	}

}


