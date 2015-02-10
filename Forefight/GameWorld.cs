using System;
using Box2DX;
using Box2DX.Dynamics;
using Box2DX.Collision;
using Box2DX.Common;

namespace Forefight {
	public class GameWorld : World {

		public GameWorld () : base (prepareAABB(), new Vec2(0f, 0f), true){
		}

		private static AABB prepareAABB(){
			AABB worldAABB = new AABB();
			worldAABB.LowerBound.Set(-100.0f, -100.0f);
			worldAABB.UpperBound.Set(100.0f, 100.0f);
			return worldAABB;
		}
	}
}

