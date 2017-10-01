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

	public class EntityLocation
	{


		public readonly int Pointer;
		public EntityLocation(int ptr)
		{
			Pointer = ptr;
		}

		public void CopyFrom(EntityLocation other)
		{
			var oAngle = other.Angle;
			var oX = other.X;
			var oY = other.Y;
			var oZ = other.Z;

			Angle = oAngle;
			X = oX;
			Y = oY;
			Z = oZ;
		}

		public double Heading {
			get { return (Hook.RFloat(Pointer + 0x4) / Math.PI * 180) + 180; }
			set { Hook.WFloat(Pointer + 0x4, (float)(value * Math.PI / 180) - (float)Math.PI); }
		}

		public double Angle {
			get { return Hook.RFloat(Pointer + 0x4); }
			set { Hook.WFloat(Pointer + 0x4, (float)value); }
		}

		public float X {
			get { return Hook.RFloat(Pointer + 0x10); }
			set { Hook.WFloat(Pointer + 0x10, value); }
		}

		public float Y {
			get { return Hook.RFloat(Pointer + 0x14); }
			set { Hook.WFloat(Pointer + 0x14, value); }
		}

		public float Z {
			get { return Hook.RFloat(Pointer + 0x18); }
			set { Hook.WFloat(Pointer + 0x18, value); }
		}

	}

}
