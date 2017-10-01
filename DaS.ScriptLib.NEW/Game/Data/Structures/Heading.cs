using System;
namespace DaS.ScriptLib.Game.Data.Structures
{

    public class Heading
	{
		private float _ingameValue;

        public Heading() { }

		public Heading(float headingAngle)
		{
			HeadingValue = headingAngle;
		}

		public double PlanarValue {
			get { return _ingameValue; }
			set { _ingameValue = (float)value; }
		}

		//Value converted as a double to prevent any noticable loss of precision in the conversion.
		public double HeadingValue {
			get { return (_ingameValue / Math.PI * 180.0) + 180.0; }
			set { _ingameValue = (float)((value * Math.PI / 180.0) - Math.PI); }
		}

	}

}
