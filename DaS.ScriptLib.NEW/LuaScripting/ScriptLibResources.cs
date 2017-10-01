using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;

namespace DaS.ScriptLib.LuaScripting
{
    public class ScriptLibResources
    {
        private const string NamespaceString = "DaS.ScriptLib";
        private const string ResourcePathPrefix = NamespaceString + ".";
        private static readonly System.Reflection.Assembly ThisAssembly;
        // Useful for debugging lol
        static internal readonly string[] EmbeddedResourceNames;

        public static readonly string IngameFunctionsFileName = "IngameFunctions.txt";
        public static Hashtable clsBonfires = new Hashtable();
        public static Hashtable clsBonfiresIDs = new Hashtable();
        public static Hashtable clsItemCats = new Hashtable();
        public static Hashtable clsItemCatsIDs = new Hashtable();
        public static Hashtable[] cllItemCats;
        public static Hashtable[] cllItemCatsIDs;
        public static Hashtable clsWeapons = new Hashtable();
        public static Hashtable clsWeaponsIDs = new Hashtable();
        public static Hashtable clsArmor = new Hashtable();
        public static Hashtable clsArmorIDs = new Hashtable();
        public static Hashtable clsRings = new Hashtable();
        public static Hashtable clsRingsIDs = new Hashtable();
        public static Hashtable clsGoods = new Hashtable();

        public static Hashtable clsGoodsIDs = new Hashtable();

        public static List<string> listBonfireNames = new List<string>();
        static ScriptLibResources()
        {
            ThisAssembly = typeof(ScriptLibResources).Assembly;
            EmbeddedResourceNames = ThisAssembly.GetManifestResourceNames().Select(x => x.Substring(ResourcePathPrefix.Length)).ToArray();
            // Removes "DaS.ScriptLib.Resources." from beginning

            InitClls();
        }

        static internal string GetEmbeddedTextResource(string relPath)
        {
            string result = null;
            using (Stream strm = typeof(ScriptLibResources).Assembly.GetManifestResourceStream(GetRelEmbedResPath(relPath)))
            {
                using (StreamReader strmReader = new StreamReader(strm))
                {
                    result = strmReader.ReadToEnd();
                }
            }
            return result;
        }

        public static string GetBonfireName(int id)
        {
            return Convert.ToString(clsBonfires[id]);
        }

        private static string GetRelEmbedResPath(string relPath)
        {
            return ResourcePathPrefix + relPath;
        }

        public static void InitClls()
        {
            cllItemCats = new Hashtable[] {
                clsWeapons,
				clsArmor,
				clsRings,
				clsGoods

            };
            cllItemCatsIDs = new Hashtable[] {
                clsWeaponsIDs,
				clsArmorIDs,
				clsRingsIDs,
				clsGoodsIDs

            };
            //-----------------------Bonfires-----------------------
            listBonfireNames = ParseItems(ref clsBonfires, ref clsBonfiresIDs, GetEmbeddedTextResource("Resources.CL.Bonfires.txt"));
            //-----------------------Item Categories-----------------------
            clsItemCats.Clear();
            clsItemCats.Add(0, "Weapons");
            clsItemCats.Add(268435456, "Armor");
            clsItemCats.Add(536870912, "Rings");
            clsItemCats.Add(1073741824, "Goods");

            clsItemCatsIDs.Clear();
            foreach (object itemCat in clsItemCats.Keys)
            {
                clsItemCatsIDs.Add(clsItemCats[itemCat], itemCat);
            }

            //-----------------------Items-----------------------
            ParseItems(ref clsWeapons, ref clsWeaponsIDs, GetEmbeddedTextResource("Resources.CL.Weapons.txt"));
            ParseItems(ref clsArmor, ref clsArmorIDs, GetEmbeddedTextResource("Resources.CL.Armor.txt"));
            ParseItems(ref clsRings, ref clsRingsIDs, GetEmbeddedTextResource("Resources.CL.Rings.txt"));
            ParseItems(ref clsGoods, ref clsGoodsIDs, GetEmbeddedTextResource("Resources.CL.Goods.txt"));
        }

        public static List<string> ParseItems(ref Hashtable cls, ref Hashtable clsIDs, string txt, bool forceUppercaseKeys = false)
        {
            List<string> nameList = new List<string>();
            var tmpList = txt.Replace("" + (char)0xD, "").Split((char)0xA);
            int tmp1 = 0;
            string tmp2 = null;

            cls.Clear();
            for (int i = 0; i <= tmpList.Length - 1; i++)
            {
                if (tmpList[i].Contains("|"))
                {
                    tmp1 = int.Parse(tmpList[i].Split('|')[0]);
                    tmp2 = tmpList[i].Split('|')[1];
                    cls.Add(tmp1, tmp2);
                }
            }

            nameList.Clear();
            clsIDs.Clear();
            foreach (object item in cls.Keys)
            {
                var name = Convert.ToString(cls[item]);
                var nameeeee = forceUppercaseKeys ? name.ToUpper() : name;
                clsIDs.Add(nameeeee, item);
                nameList.Add(cls[item] as string);
            }

            nameList.Sort();
            return nameList;
        }
    }
}