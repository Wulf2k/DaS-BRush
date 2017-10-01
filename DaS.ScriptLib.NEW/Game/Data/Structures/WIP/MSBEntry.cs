namespace DaS.ScriptLib.Game.Data.Structures
{

    //UNDER CONSTRUCTION
    public class MSBEntry
	{


		public readonly int Pointer;
		public MSBEntry(int i)
		{
			Pointer = MSB.FirstEntryPointer + (4 * i);
		}

	}

}
