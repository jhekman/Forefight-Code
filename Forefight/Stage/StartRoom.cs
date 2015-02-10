using System;
using System.Collections.Generic;
using Forefight;
using Forefight.Entity;

namespace Forefight.Stage {
	public class StartRoom : Room {
		private Pool[] pool;

		public StartRoom (Floor floor) : base(floor){
		}

		public Player SpawnPlayer (Player player){
			return null;
		}
	}
}

