using System;
using OpenTK;
using System.Collections.Generic;
using Forefight;
using Forefight.Entity;

namespace Forefight.Stage {
	public class Room {
		public enum RoomType {
			BIG_ROOM,
			LONG_ROOM,
			DEAD_END,
			STANDARD_ROOM,
			LOCKED_ROOM,
			LOOT_ROOM,
			START_ROOM,
			PUZZLE_ROOM
		}

		private List<Vector2> Walls;
		private List<Enemy> enemy;
		private List<Chest> chest;
		private List<Projectile> projectile;
		private Floor floor;
		protected RoomType type;
		private Door[] doors;

		public Room (Floor floor){
		}

		public void draw (double delta){
		}

		public void update (double delta){
		}
	}
}

