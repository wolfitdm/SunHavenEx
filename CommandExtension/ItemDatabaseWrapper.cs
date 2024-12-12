using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;
using QFSW.QC;
using QFSW.QC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using System.IO;
using Wish;
using System.Runtime.Remoting.Messaging;

namespace CommandExtension
{
        public class ItemDatabaseWrapper
        {
            public static ItemDatabaseWrapper Instance { get; private set; }
            private static Dictionary<int, ItemSellInfo> itemDatabase = ItemInfoDatabase.Instance.allItemSellInfos;
            private static List<int> keys = getItemDatabaseKeys();
            private static List<ItemSellInfo> values = getItemDatabaseValues();
            private static List<int> getItemDatabaseKeys()
            {
                List<int> keys__ = new List<int>();
                foreach (int item in itemDatabase.Keys)
                {
                    keys__.Add(item);
                }
                return keys__;
            }

            private static List<ItemSellInfo> getItemDatabaseValues()
            {
                List<ItemSellInfo> values__ = new List<ItemSellInfo>();
                foreach (ItemSellInfo item_info in itemDatabase.Values)
                {
                    values__.Add(item_info);
                }
                return values__;
            }

            public Dictionary<int, ItemSellInfo> GetItemDatabase() { return itemDatabase; }

            public List<int> GetKeys()
            {
                return keys;
            }

            public List<ItemSellInfo> GetValues()
            {
                return values;
            }

            public ItemDatabaseWrapper()
            {
            }
        }
}
