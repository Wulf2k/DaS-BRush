using System.Collections.Generic;
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

        public Loc(Vec3 pos) : this(pos, 0) { }

		public Loc(float posX, float posY, float posZ, float rotHeading)
		{
			Pos = new Vec3(posX, posY, posZ);
			this.Rot = new Heading(rotHeading);
		}

        public Loc(double posX, double posY, double posZ, double rotHeading) : this((float)posX, (float)posY, (float)posZ, (float)rotHeading) { }


        public Loc(float posX, float posY, float posZ)
		{
			Pos = new Vec3(posX, posY, posZ);
			Rot = new Heading(0);
		}

        public Loc(double posX, double posY, double posZ) : this((float)posX, (float)posY, (float)posZ) { }


        public bool IsZero {
			get { return Pos.X == 0 && Pos.Y == 0 && Pos.Z == 0 && Rot.HeadingValue == 0; }
		}

		public object AngleTo(Loc other)
		{
			return Pos.GetLateralAngleTo(other.Pos);
		}

        public override bool Equals(object obj)
        {
            var loc = obj as Loc;
            return loc != null &&
                   EqualityComparer<Vec3>.Default.Equals(Pos, loc.Pos) &&
                   EqualityComparer<Heading>.Default.Equals(Rot, loc.Rot);
        }

        public static readonly Loc Zero = new Loc(0, 0, 0, 0);

        public static bool operator !=(Loc left, Loc right) => !left.Equals(right);
        public static bool operator ==(Loc left, Loc right) => left.Equals(right);
    }
}
