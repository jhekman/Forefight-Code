using System.Drawing;
using OpenTK;
using Forefight.Drawing;
using System;

namespace Forefight.Entity{
	public class Bear : Enemy{
		int speed = 40;
		double bearcooldown = 0;

		
		public Bear(GameView view, Vector2 position) : base(view, position){

		}
		public override void draw (double delta) {
			Shapes.DrawCircle(Position.X, Position.Y, 16, 32, Color.Brown);
		}
		
		public override void update(double delta){
			bearcooldown += delta;
			Vector2 relative = Vector2.Subtract (target.Position, Position);
			relative.Normalize ();
			Velocity = Vector2.Multiply (relative, speed);

			if (relative.Length <= 20) {
				Console.WriteLine (delta);
				if (bearcooldown > 5) {
					bearcooldown = 0;
					//code for attack
					target.takeHit (5);
				}
			} else
				base.update (delta);
		}
	}
}