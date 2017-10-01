using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
namespace DaS.ScriptLib.Game.Data.Structures
{
	public class Loc
	{
		public Vec3 Pos;

		public Heading Rot;
		public Loc()
		{
			Pos = Vec3.Zero;
			Rot = new Heading(0);
		}

		public Loc(Vec3 pos, float rot)
		{
			this.Pos = pos;
			this.Rot = new Heading(rot);
		}

		public Loc(Vec3 pos, Heading rot)
		{
			this.Pos = pos;
			this.Rot = rot;
		}

		public Loc(Vec3 pos) : this(pos, 0)
		{
		}

		public Loc(float posX, float posY, float posZ, float rotHeading)
		{
			Pos = new Vec3(posX, posY, posZ);
			this.Rot = new Heading(rotHeading);
		}

		public Loc(posX, posY, posZ)
		{
			Pos = new Vec3(posX, posY, posZ);
			Rot = new Heading(0);
		}

		public bool IsZero {
			get { return Pos.X == 0 && Pos.Y == 0 && Pos.Z == 0 && Rot.HeadingValue == 0; }
		}

		public object AngleTo(Loc other)
		{
			return Pos.GetLateralAngleTo(other.Pos);
		}


		public static readonly Loc Zero = new Loc(0, 0, 0, 0);
		public static bool operator !=(Loc left, Loc right)
		{
			return (left().Pos != right().Pos) | (left().Rot.PlanarValue != right().Rot.PlanarValue);
		}

		public static bool operator ==(Loc left, Loc right)
		{
			return (left().Pos == right().Pos) && (left().Rot.PlanarValue == right().Rot.PlanarValue);
		}

	}
}
