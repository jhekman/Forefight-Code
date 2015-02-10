﻿//
//  Skeleton.cs
//
//  Author:
//       Trevor <${AuthorEmail}>
//
//  Copyright (c) 2014 Trevor
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
using System.Drawing;
using OpenTK;
using Forefight.Drawing;

namespace Forefight.Entity {
	public class Skeleton : Enemy {
		int speed = 80;
		int keepDistance = 300;
		const int coolDownTime = 100;
		int coolDown = 0;

		public Skeleton (GameView view, Vector2 position) : base(view, position){
		}

		public override void draw (double delta) {
			Shapes.DrawCircle(Position.X, Position.Y, 16, 32, Color.BlanchedAlmond);
		}

		public override void update (double delta) {
			Vector2 relative = Vector2.Subtract(target.Position, Position);
			Vector2 Velocity2 = Vector2.Zero;

			if (coolDown == 0) {
				View.Projectiles.Add(new Projectile(View, Position, target.Position, false));
				coolDown = coolDownTime;
			}
			else
				coolDown--;

			if (View.Projectiles.Count != 0) {
				Projectile closest = View.Projectiles[0]; // Dodge away from whatever projectile is closest
				float closestLength = (Vector2.Subtract(closest.Position, Position)).Length;
				foreach (Projectile projectile in View.Projectiles) {
					Vector2 rel = Vector2.Subtract(projectile.Position, Position);
					if (projectile._isFriendly && rel.Length < closestLength) {
						closest = projectile; 
						closestLength = Vector2.Subtract(closest.Position, Position).Length;
					}
				}
				if (closest._isFriendly) { //Dodging Projectiles
					Vector2 rel = Vector2.Subtract(closest.Position, Position);
					int totalRadius = this.getRadius() + closest.getRadius();
					double adjustedValue = totalRadius / Math.Sqrt(1 - closest.Velocity.Y * closest.Velocity.Y / closest.Velocity.LengthSquared);
					if (rel.Length < 200) { // When the Skeleton Sees incoming Projectiles; Detection of whether dodging is needed or not
						if ((Position.Y - closest.Position.Y - adjustedValue) <= (closest.Velocity.Y / closest.Velocity.X) * (Position.X - closest.Position.X) &&
							(Position.Y - closest.Position.Y + adjustedValue) >= (closest.Velocity.Y / closest.Velocity.X) * (Position.X - closest.Position.X)) {
							Vector2 run = Vector2.Zero;
							//View.Effects.Add(new Boom(View, Position, 50)); // For testing detection of whether incoming projectiles will hit or miss
							if ((Position.Y - closest.Position.Y) < (Position.X - closest.Position.X) * (closest.Velocity.Y) / (closest.Velocity.X)) { // Deciding which direction to dodge
								if (closest.Velocity.X > 0)
									run = new Vector2(closest.Velocity.Y, -1 * closest.Velocity.X);
								else
									run = new Vector2(-1 * closest.Velocity.Y, closest.Velocity.X);
							}
							else {
								if (closest.Velocity.X > 0)
									run = new Vector2(-1 * closest.Velocity.Y, closest.Velocity.X);
								else
									run = new Vector2(closest.Velocity.Y, -1 * closest.Velocity.X);
							}
							run.Normalize();
							Velocity2 = Vector2.Multiply(run, speed);
						}
					}
				}
			}

			if (relative.Length < keepDistance) { // Run From Player
				Vector2 vel = relative;
				vel.Normalize();
				Velocity = -1 * Vector2.Multiply(vel, speed);
			}
			else
				Velocity = Vector2.Zero;

			Position += Vector2.Multiply(Vector2.Add(Velocity, Velocity2), (float) delta);
		}
	}
}