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
