using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using DaS.ScriptLib.Injection;

namespace DaS.ScriptLib.Game.Data.Structures
{

	public class MapEntry
	{


		public readonly int Pointer;
		public MapEntry(int ptr)
		{
			Pointer = ptr;
		}

		public int PointerToBlockAndArea {
			get { return Hook.RInt32(Pointer + 0x4); }
		}

		public byte Block {
			get { return Hook.RByte(PointerToBlockAndArea + 0x6); }
		}

		public byte Area {
			get { return Hook.RByte(PointerToBlockAndArea + 0x7); }
		}

		public int EntityCount {
			get { return Hook.RInt32(Pointer + 0x3c); }
		}

		public int StartOfEntityStruct {
			get { return Hook.RInt32(Pointer + 0x40); }
		}

		public EntityHeader[] GetEntityHeaders()
		{
			EntityHeader[] result = new EntityHeader[EntityCount];

			for (i = 0; i <= result.Length - 1; i++) {
				result[i] = new EntityHeader(StartOfEntityStruct + (EntityHeader.Size * i));
			}

			return result;
		}

		public Entity[] GetEntities()
		{
			Entity[] result = new Entity[EntityCount];

			for (i = 0; i <= EntityCount - 1; i++) {
				result[i] = new EntityHeader(StartOfEntityStruct + (EntityHeader.Size * i)).Entity;
			}

			return result;
		}

		public EntityLocation[] GetEntityLocations()
		{
			EntityLocation[] result = new EntityLocation[EntityCount];

			for (i = 0; i <= EntityCount - 1; i++) {
				result[i] = new EntityHeader(StartOfEntityStruct + (EntityHeader.Size * i)).Location;
			}

			return result;
		}

	}

}
