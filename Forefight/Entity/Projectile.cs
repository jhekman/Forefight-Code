//
//  Projectile.cs
//
//  Author:
//       Timothy Oltjenbruns <timothyolt@gmail.com>
//
//  Copyright (c) 2014 Timothy Oltjenbruns
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
using System;
using OpenTK;
using BaseGame.Drawing;
using System.Drawing;

namespace BaseGame.Entity {
	public class Projectile : Entity {
		private const int radius = 8;
		private const int speed = 240;
		bool isFriendly;
		Vector2 vel;
		Color color;


		public Projectile (GameView view, Vector2 position, Vector2 target, bool _isFriendly) : base(view, position){
			vel = new Vector2(target.X - position.X, target.Y - position.Y);
			isFriendly = _isFriendly;
			if (isFriendly)
				color = Color.Yellow;
			else
				color = Color.OrangeRed;
		}

		public bool _isFriendly {
			get {return isFriendly;} 
		}

		public int getRadius() {
			return radius; 
		}

		public override void draw (double delta){
			Shapes.DrawCircle(Position.X, Position.Y, radius, 32, color);
		}

		public override void update (double delta){
			vel.Normalize();
			vel = Vector2.Multiply(vel, speed);
			Velocity = vel;

			if (isFriendly) {
				for (int i = View.Enemies.Count - 1; i > -1; i--) {
					Enemy enemy = View.Enemies[i];
					int enemyRadius = enemy.getRadius();
					if (Math.Pow((enemy.Position.X - Position.X), 2) + Math.Pow((enemy.Position.Y - Position.Y), 2) <= Math.Pow(radius + enemyRadius, 2)) {
						enemy.dead = true;
						View.Projectiles.Remove(this);
						break;
					}
				}
			}
			else {
				Vector2 relative = Vector2.Subtract(Position, View.Player.Position);
				if (relative.Length <= radius + 16) {
					View.Player.takeHit(1);
					View.Projectiles.Remove(this);
				}
			}
			base.update(delta); 
		}
	}
}

