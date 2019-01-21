﻿using System;
using System.Collections.Generic;
using System.Linq;
using Enemy = SoulsFormats.MSB3.Part.Enemy;

namespace StraySouls
{
    [Flags]
    public enum EnemyRandomizerAddMode { None = 0, MainBoss = 1, OptionalBoss = 2, AggressiveNPC = 4, FriendlyNPC = 8 }

    public class EnemyRandomizer : MapRandomizerBase<Enemy, EnemyRandomProperties>
    {
        private static readonly string[] ID_MUST_SKIP = new string[]
        {
            //These might be bonfires and etc.I haven't tested
            "c0100_0000","c0100_0001","c0100_0002","c0100_0003","c0100_0004",
            "c0100_0005","c0100_0006","c0100_0007","c0100_0009","c0100_0010",
            "c0100_0011","c0100_0012","c0100_0013","c0100_0014","c0100_0015",
            "c0100_0016","c0100_0017","c1000_0000","c1000_0001","c1000_0002",
            "c1000_0003","c1000_0004","c1000_0005","c1000_0006","c1000_0007",
            "c1000_0008","c1000_0009","c1000_0010","c1000_0011","c1000_0012",
            "c1000_0013","c1000_0014","c1000_0015","c1000_0016","c1000_0017",
            "c1000_0018","c5020_0000","c5021_0000","c5022_0000","c5022_0001",
            "c5022_0002","c5022_0003","c5250_0000",
        };

        private static readonly string[] ID_MAIN_BOSS = new string[]
        {
            "c5110_0000", //Ludex Gundyr
            "c5110_0001", //Ludex Gundyr (P2)
            "c2240_0000", //Vordt of the Boreal Valley
            "c1320_0000", //Crystal Sage
            "c5220_0000", //Deacons of the Deep ()
            "c3040_0000", //Abyss Wathcers (Both P1 and P2)
            "c3040_0001", //Abyss Wathcers (P1, normal)
            "c3040_0002", //Abyss Wathcers (P1, red eyes)
            "c5160_0000", //High Lord Wolnir
            "c5140_0000", //Pontiff Sulyvahn
            "c5140_0001", //Pontiff Sulyvahn (Phantom)
            "c5150_0001", //Aldrich, Devourer of Gods
            "c5260_0000", //Yhorm the Giant
            "c5270_0001", //Dancer of the Boreal Valley
            "c3160_0000", //Dragonslayer Armour
            "c5250_0001", //Lorian, Elder Prince (P1)
            "c5250_0000", //Lorian, Elder Prince (P2)
          //"Unknown", //Lothric, Younger Prince
            "c5280_0000", //Soul of Cinder
            "c6020_0001", //Sister Friede (P1)
            "c6010_0001", //Sister Friede (P2)
            "c6020_0000", //Blackflame Friede (P3)
            "c6010_0000", //Father Ariandel (P2)
            "c5020_0001", //Demon in Pain
            "c5020_0002", //Demon from Below
            "c6280_0000", //Judicator Argo
            "c0000_0010", //Halflight, Spear of the Church
            "c6201_0000", //Slave Knight Gael
        };

        private static readonly string[] ID_OPTIONAL_BOSS = new string[]
        {
            "c5180_0000", //Curse-rotted Greatwood*
            "c3050_0000", //Old Demon King
            "c2090_0000", //Oceiros, the Consumed King
            "c5110_0000", //Champion Gundyr*
            "c3141_0000", //Ancient Wyvern
            "c5030_0000", //The Nameless King (P1)
            "c5010_0000", //The Nameless King (P2)
            "c0000_0009", //Champion's Gravetender
            "c6030_0004", //Gravetender Greatwolf
            "c6210_0000", //Darkeater Midir (Welcomer)
            "c6211_0000", //Darkeater Midir (Boss)
        };

        private static readonly string[] ID_AGGRESSIVE_NPC = new string[]
        {
          //"c", //Alva, Seeker of the Spurned
          //"c", //Black Hand Kamui
            "c0000_0015", //Brigand
          //"c", //Court Sorcerer
          //"c", //Creighton the Wanderer
            "c0000_0015", //Daughter of Crystal Kriemhild
          //"c", //Desert Pyromancer Zoey
          //"c", //Dragonblood Knight
          //"c", //Drang Knight
            "c0000_0016", //Fallen Knight
          //"c", //Havel the Rock
            "c0000_0015", //Isabella the Mad
          //"c", //Knight Slayer Tsorig
          //"c", //Livid Pyromancer Dunnel
          //"c", //Longfinger Kirk
          //"c", //Moaning Knight
          //"c", //Rapier Champion
          //"c", //Silver Knight Ledo
          //"c0000_0012", //Sir Vilhelm
        };

        private static readonly string[] ID_FRIENDLY_NPC = new string[]
        {
            //(Some of them might be aggressive at some cases)
            "c1400_0000", //Fire Keeper
            "c0000_0026", //Anri of Astora (Road of Sacrifice)
            "c0000_0007", //Anri of Astora (Cathedral of Deep)
            "c3190_0000", //Blacksmith Andre
          //"c", //Company Captain Yorshka
          //"c", //Cornyx of the Great Swanp
            "c3250_0000", //Emma, High Priestess of Lothric Castle
            "c0000_0014", //Eygon of Carim
            "c0000_0004", //Eygon of Carim (Irina death?)
          //"c", //Filianore
          //"c", //Forlorn Corvian Settler (Slab holder)
          //"c", //Greirat of the Undead Settlement
            "c0000_0033", //Hawkwood the Deserter
            "c0000_0022", //Hawkwood the Deserter (Farron Keep)
            "c0000_0023", //Horace the Hushed
            "c0000_0002", //Holy Knight Hodrick
            "c0000_0033", //Holy Knight Hodrick (Road of Sacrifice)
          //"c", //Irina of Carim
          //"c", //Karla
            "c0000_0007", //Lion Knight Albert
          //"c", //Londor Pale Shade
            "c1450_0000", //Ludleth of Courland
          //"c", //Old Woman of Londor
            "c0000_0017", //Orbeck of Vinheim
          //"c", //Ringfinger Leonhard
          //"c", //Rosaria, Mother of Rebirth
          //"c", //Shira, Knight of Filianore
            "c3200_0000", //Shrine Handmaid
            "c3200_0000", //Shrine Handmaid (Past)
            "c0000_0007", //Siegward of Catarina (Meet at elevator)
            "c0000_0016", //Siegward of Catarina (Yhorm)
            "c0000_0006", //Sirris of the Sunless Realms
            "c0000_0019", //Sirris of the Sunless Realms (Cathedral of Deep)
            "c0000_0021", //Slave Knight Gael (Cathedral of Deep)
            "c0000_0012", //Sword Master
            "c0000_0008", //Sword Master (Phantom, Vordt)
            "c0000_0013", //Sword Master (Phantom, Gundyr)
          //"c", //The Painter
            "c0000_0013", //Unbreakable Patches
          //"c", //Yellowfinger Heysel
            "c2160_0000", //Yoel of Londor
          //"c", //Yuria of Londor
        };

        private readonly List<string> _skipIDs = new List<string>();

        private readonly List<string> _additionIDs = new List<string>();

        private readonly List<Enemy> _additionEnemies = new List<Enemy>();

        private EnemyRandomizerAddMode _additionMode = EnemyRandomizerAddMode.None;

        public void SetAdditionMode(EnemyRandomizerAddMode mode)
        {
            _additionMode = mode;
            _skipIDs.AddRange(ID_MAIN_BOSS);
            _skipIDs.AddRange(ID_OPTIONAL_BOSS);
            _skipIDs.AddRange(ID_AGGRESSIVE_NPC);
            _skipIDs.AddRange(ID_FRIENDLY_NPC);
            GetAdditionEnemies();
        }

        protected override IEnumerable<Enemy> ModifyBeforeRandomize(IEnumerable<Enemy> entries)
        {
            _additionEnemies.Clear();

            entries = entries.Where(entry =>
             {
                 if (ID_MUST_SKIP.Contains(entry.Name))
                     return false;

                 if (_skipIDs.Contains(entry.Name))
                     if (_additionIDs.Contains(entry.Name))
                         _additionEnemies.Add(new Enemy(entry) { Name = entry.Name + "_c" });
                     else
                         return false;

                 return true;
             }).Concat(_additionEnemies);
            return entries;
        }

        private void GetAdditionEnemies()
        {
            _additionIDs.Clear();

            if ((_additionMode & EnemyRandomizerAddMode.MainBoss) != 0)
                _additionIDs.AddRange(ID_MAIN_BOSS);
            if ((_additionMode & EnemyRandomizerAddMode.OptionalBoss) != 0)
                _additionIDs.AddRange(ID_OPTIONAL_BOSS);
            if ((_additionMode & EnemyRandomizerAddMode.AggressiveNPC) != 0)
                _additionIDs.AddRange(ID_AGGRESSIVE_NPC);
            if ((_additionMode & EnemyRandomizerAddMode.FriendlyNPC) != 0)
                _additionIDs.AddRange(ID_FRIENDLY_NPC);
        }
    }
}