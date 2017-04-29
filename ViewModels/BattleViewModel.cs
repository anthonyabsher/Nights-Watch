﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

namespace GroupProject_DD
{
	public class BattleViewModel : INotifyPropertyChanged
	{
		/******************Developer Fields**************/
		Character char1;
		Character char2;
		Character char3;
		Character char4;

		/*****************Actual Fields**********************/
		List<Item> item_dictionary;
		List<Character> CharacterList;
		List<Monster> monster_dictionary;
        List<Monster> activeMonsterList;
		public ObservableCollection<String> actions;
        public ObservableCollection<Character> CharacterReadoutList;
        public string action;
		Engine BattleEngine;
		bool steplock;

		/*****************Controllers**********************/
		CharacterController characterController = new CharacterController();
		MonsterController monsterController = new MonsterController();

		public BattleViewModel()
		{
			/**********For Developer Mode (*Fixed Character and Item List*)**********/
			devStartup();
			/**********************************************************************/
			action = "Game starting...";
			actions = new ObservableCollection<string>();
			steplock = false;
			BattleEngine = new Engine(CharacterList, item_dictionary, monster_dictionary);
			startGameLoop(true);
		}

		public void startGameLoop(bool manual_mode)
		{
			Enque(BattleEngine.IncrementDungeonLevel());
			Enque(BattleEngine.generateMonsterList(1));
			getMonsterList();

		}

		public void getMonsterList()
		{
            activeMonsterList = BattleEngine.currentMonsterList();
			foreach (Monster monster in activeMonsterList)
			{
				Enque("Level " + monster.Rating + " " + monster.Name);
			}
			Enque("");
		}


		public void devStartup()
		{
			// access to db and fetch data
			IEnumerable < Character > allCharactersInDB= characterController.GetAllItems();
			// convert ienumerable to array
			this.CharacterList = allCharactersInDB.ToList();
            CharacterReadoutList = new ObservableCollection<Character>();
            foreach (Character hero in CharacterList)
            {
                CharacterReadoutList.Add(hero);
            }
            IEnumerable<Monster> allMonstersInDB = monsterController.GetAllMonsters();
			this.monster_dictionary = allMonstersInDB.ToList();

			item_dictionary = new List<Item>()
			{
				new Item("Sword", "Typical Sword", 1, "Strength", 1),
				new Item("Leather Armor", "Torso Protection", 2, "Strength", 2),
				new Item("Stupid Helmet", "It's dumb", 1, "Defense", 0),
				new Item("Mythril Pants", "Heavy", 5, "Speed", 3),
				new Item("Silver Sword", "Elegant Sword", 4, "Strength", 1),
				new Item("Silver Helmet", "Elegant Helmet", 4, "Defense", 0),
				new Item("Bronze Armor", "Rustic", 3, "Defense", 2),
				new Item("Chainmail", "It's Bulletproof", 3, "Defense", 2),
				new Item("Feather Shorts", "It's so light", 7, "Speed", 3)
			};

		}

		public void Enque(string log)
		{
			if (actions.Count >= 11)
				actions.RemoveAt(actions.Count - 1);
			actions.Insert(0, log);
		}

		public ObservableCollection<string> Actions
		{
			get
			{
				return actions;
			}
			set
			{
				actions = value;
				OnPropertyChanged("Actions");
			}
		}

        public string MonsterCount
        {
            get
            {
                OnPropertyChanged("MonsterCount");
                return "Monsters remaining: " + BattleEngine.monsterList.Count;
            }
        }

        public ObservableCollection<Character> CharacterReadout
        {
            get
            {
                OnPropertyChanged("CharacterReadout");
                return CharacterReadoutList;
            }
        }

        public void UpdateAction(ref int counter)
		{
			if (!BattleEngine.evaluateMonsterList())
			{
				BattleEngine.IncrementDungeonLevel();
				BattleEngine.generateMonsterList(BattleEngine.dungeonLevel);
				Enque("");
				Enque("");
			}
			if (BattleEngine.evaluateCharacterList())
			{
				List<string> actionList = BattleEngine.Volley();
				foreach (string _action in actionList)
				{
					Enque(_action);
				}
			}
			else
				Enque("All characters are dead....");
			OnPropertyChanged("Actions");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			var changed = PropertyChanged;
			if (changed != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
