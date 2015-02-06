using System;
using System.Collections.Generic;
using BaseGame;
using BaseGame.Entity;

namespace BaseGame.Stage {
	public class StartRoom : Room {
		private Pool[] pool;

		public StartRoom (Floor floor) : base(floor){
		}

		public Player SpawnPlayer (Player player){
			return null;
		}
	}
}

