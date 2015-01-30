using System;
using System.Collections.Generic;
using BaseGame;
using BaseGame.Entity;
using System.Runtime.InteropServices.WindowsRuntime;

namespace BaseGame.Stage {
	public class PuzzleRoom : Room {
		public PuzzleRoom (Floor floor) : base(floor){
		}

		public Key DropKey (){
			return null;
		}
	}
}

