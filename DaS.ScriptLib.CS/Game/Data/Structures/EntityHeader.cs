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

	[Flags]
	public enum EntityHeaderFlagsA : byte
	{
		None = 0x0,
		Disabled = 0x1,
		PlayerHide = 0x4
	}

	public class EntityHeader
	{


		public const int Size = 0x20;

		public readonly int Pointer;
		public EntityHeader(int ptr)
		{
			Pointer = ptr;
		}

		public void CopyFrom(EntityHeader other)
		{
			//Write ENTIRE STRUCT properly
			Hook.WBytes(Pointer, Hook.RBytes(other.Pointer, Size));
		}

		public int EntityPtr {
			get { return Hook.RInt32(Pointer + 0x0); }
			set { Hook.WInt32(Pointer + 0x0, value); }
		}

		public Entity Entity {
			get { return new Entity(() => EntityPtr); }
		}

		public int CloneValue {
			get { return Hook.RInt32(Pointer + 0x8); }
			set { Hook.WInt32(Pointer + 0x8, value); }
		}

		public int UnknownPtr1 {
			get { return Hook.RInt32(Pointer + 0x10); }
		}

		public EntityHeaderFlagsA FlagsA {
			get { return (EntityHeaderFlagsA)Hook.RByte(Pointer + 0x14); }
			set { Hook.WByte(Pointer + 0x14, Convert.ToByte(value)); }
		}


		public int LocationPtr {
			get { return Hook.RInt32(Pointer + 0x18); }
			set { Hook.WInt32(Pointer + 0x18, value); }
		}

		public EntityLocation Location {
			get { return new EntityLocation(LocationPtr); }
			set { new EntityLocation(LocationPtr).CopyFrom(value); }
		}

		public int UnknownPtr2 {
			get { return Hook.RInt32(Pointer + 0x1c); }
		}

		//TODO: See if writeable, also see wtf it even is lol
		public bool DeadFlag {
			get { return Hook.RBool(UnknownPtr2 + 0x14); }
		}
		#region "Flags"
		public bool GetFlagA(EntityHeaderFlagsA flg)
		{
			return FlagsA.HasFlag(flg);
		}

		public void SetFlagA(EntityHeaderFlagsA flg, bool state)
		{
			if (state) {
				FlagsA = FlagsA | flg;
			} else {
				FlagsA = FlagsA & (!flg);
			}
		}

		public bool PlayerHide {
			get { return GetFlagA(EntityHeaderFlagsA.PlayerHide); }
			set { SetFlagA(EntityHeaderFlagsA.PlayerHide, value); }
		}

		public bool Disabled {
			get { return GetFlagA(EntityHeaderFlagsA.Disabled); }
			set { SetFlagA(EntityHeaderFlagsA.Disabled, value); }
		}
		#endregion


	}

}
