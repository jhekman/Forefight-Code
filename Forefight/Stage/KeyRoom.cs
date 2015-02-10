using System;
using System.Collections.Generic;
using BaseGame;
using BaseGame.Entity;

namespace Forefight.Stage {
	public class KeyRoom : Room {
		private Key key;

		public KeyRoom (Floor floor, RoomType type) : base(floor){
			this.type = type;
		}

		public Key DropKey (){
			return null;
		}
	}
}

