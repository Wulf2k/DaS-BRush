using System;
namespace DaS.ScriptLib.Game.Data.Structures
{
    public class Vec3
	{
		public float X = 0;
		public float Y = 0;

		public float Z = 0;
		public Vec3(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		public Vec3 Plus(Vec3 v)
		{
			return new Vec3(X + v.X, Y + v.Y, Z + v.Z);
		}

		public Vec3 Minus(Vec3 v)
		{
			return new Vec3(X - v.X, Y - v.Y, Z - v.Z);
		}

		public Vec3 Times(Vec3 v)
		{
			return new Vec3(X * v.X, Y * v.Y, Z * v.Z);
		}

		public Vec3 DividedBy(Vec3 v)
		{
			return new Vec3(X / v.X, Y / v.Y, Z / v.Z);
		}

		public float MagnitudeSquared()
		{
			return (float)(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
		}

		public float Magnitude()
		{
			return (float)Math.Sqrt(MagnitudeSquared());
		}

		public float DistanceSquaredTo(Vec3 other)
		{
			return other.Minus(this).MagnitudeSquared();
		}

		public float DistanceTo(Vec3 other)
		{
			return (float)Math.Sqrt(DistanceSquaredTo(other));
		}

		public Heading GetLateralAngleTo(Vec3 other)
		{
			return new Heading { PlanarValue = Math.Atan2(other.Z - Z, other.X - X) };
		}

		public static Vec3 GetLateralUnit(float angle)
		{
			return new Vec3((float)Math.Cos(angle), 0, (float)Math.Sin(angle));
		}

		public static readonly Vec3 Zero = new Vec3(0, 0, 0);
		public static readonly Vec3 One = new Vec3(1, 1, 1);
		public static readonly Vec3 Up = new Vec3(0, 1, 0);
		public static readonly Vec3 Down = new Vec3(0, -1, 0);
		public static readonly Vec3 Left = new Vec3(-1, 0, 0);
		public static readonly Vec3 Right = new Vec3(1, 0, 0);
		public static readonly Vec3 Front = new Vec3(0, 0, 1);

		public static readonly Vec3 Back = new Vec3(0, 0, -1);
		public static bool operator !=(Vec3 left, Vec3 right)
		{
			return (Left.X != Right.X) | (Left.Y != Right.Y) | (Left.Z != Right.Z);
		}

		public static bool operator ==(Vec3 left, Vec3 right)
		{
			return (Left.X == Right.X) && (Left.Y == Right.Y) && (Left.Z == Right.Z);
		}

	}
}
