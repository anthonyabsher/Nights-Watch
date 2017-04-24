﻿using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinProject.Models
{
    class Engine
    {
        /***********************resource declarations***************************/
        public List<Character> characterList;
        public List<Monster> monsterList;
        public List<Character> characterDeadList;
        public List<Item> item_dictionary;
        public List<Monster> monster_dictionary;
        public int dungeonLevel;
        Random rand;
        /**********************Game Start**************************************/
        public Engine(List<Character> charList, List<Item> itemDictionary, List<Monster> monsterDictionary)
        {
            characterList = charList;
            item_dictionary = itemDictionary;
            monster_dictionary = monsterDictionary;
            monsterList = new List<Monster>();
            characterDeadList = new List<Character>();
            dungeonLevel = 0;
            rand = new Random();
        }

        public string IncrementDungeonLevel()
        {
            dungeonLevel++;
            return "********* Entering Dungeon Level = " + dungeonLevel + " *********";
        }

        public string generateMonsterList(int dungeonLevel)
        {
            
            int num_monsters = rand.Next(1, 4) + dungeonLevel;
            for (int i = 0; i < num_monsters; i++)
            {
                //will randomly pick a monster from the monster dictionary
                monsterList.Add(new Monster(monster_dictionary[rand.Next(monster_dictionary.Count)]));
                monsterList[i].setMonsterLevel(dungeonLevel);
            }
            return "There are " + num_monsters.ToString() + " monsters in this dungeon.";
        }

        public List<Monster> currentMonsterList()
        {
            return monsterList;
        }

        public bool evaluateCharacterList()
        {
            if (characterList.Count > 0)
                return true;
            else
                return false;
        }

        public bool evaluateMonsterList()
        {
            if (monsterList.Count > 0)
                return true;
            else
                return false;
        }

        public List<string> Volley()
        {
            List<string> actions = new List<string>();
            string action;
            Character hero = characterList[0];
            Monster monster = monsterList[0];
            characterList.RemoveAt(0);
            monsterList.RemoveAt(0);
            int heroAttk = hero.Attack();
            int monsterAttk = monster.Attack();
            monster.takeDamage(heroAttk);
            if (monster.isDead())
            {
                action = hero.Name + " killed " + monster.Name + " with " + heroAttk.ToString() + " damage.";
                actions.Add(action);
            }
            else
            {
                action = hero.Name + " hit " + monster.Name + " with " + heroAttk.ToString() + " damage.";
                actions.Add(action);
                hero.takeDamage(monsterAttk);
                if (hero.isDead())
                {
                    action = monster.Name + " killed " + hero.Name + " with " + monsterAttk.ToString() + " damage.";
                    actions.Add(action);
                    characterDeadList.Add(hero);
                }
                else
                {
                    action = monster.Name + " hit " + hero.Name + " with " + monsterAttk.ToString() + " damage.";
                    actions.Add(action);
                    characterList.Add(hero);
                }
                monsterList.Add(monster);
            }
            return actions;
        }

         /*
         public eplore_dungeon(dungeon_level)
         {
         generate a bunch of dungeon_level monster and put them in the list
         while (monster list is isn't empty and character list isn't empty)
                battle(characterlist top, monsterlist top)
                if characterlist top is dead
                    add character to dead list
                else
                    put character at back of the list
         start(dungeon_level++)
         }
         
         public battle(character, monster)
         {
            monster.health -= charater.strength
            if (monster isn't dead)
                character.health -= monster.health
            else
                character.addexp(monster.exp)
                character.addItem(monster.item)
         }
         
         
         
         */

    }
}
