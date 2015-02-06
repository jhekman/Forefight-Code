//
//  Player.cs
//
//  Author:
//       Timothy Oltjenbruns <timothyolt@gmail.com>
//
//  Copyright (c) 2014 Studio4 Entertainment
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
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using BaseGame.Drawing;
using System.Collections.Generic;
using System;

namespace BaseGame.Entity {
	public class Player : Entity {
		public enum Type {
			FIREBALL = 0,
			ARROW = 1,
			SHIELD = 2,
			CLOAK = 3
		}

		public bool ranged = true;
		public bool useTargeting = false;

		private const float meleeRange = 80;
		private const int coolDownTime = 20; // Change this value to change cooldown
		private int coolDown = 0;
		private Color playerColor = Color.Green; // Color changes when on cooldown

		private Vector2 target;
		private Enemy targetEnemy;
		private bool attacking;

		public Player (GameView view, Vector2 position) : base(view, position){
			target = Vector2.Zero;
			targetEnemy = null;
			attacking = false;
		}

		public Vector2 Target {
			get { return target; }
			set {
				Vector2 tar = value;
//				if (tar.Length != 0) 
//					tar.Normalize();
				target = tar;
			}
		}

		public override void update (double delta){
			List<Enemy> enemies = View.Enemies;
			if (!ranged && attacking) {
				List<Enemy> deads = new List<Enemy>();
				foreach (Enemy enemy in enemies) {
					Vector2 relative = enemy.Position - Position;
					int enemyRadius = enemy.getRadius();
					if (relative.Length <= meleeRange + enemy.getRadius() && (Math.Abs(Math.Atan2(relative.Y, relative.X) - Math.Atan2(target.Y, target.X)) < Math.PI / 4))
							deads.Add(enemy);
				}
				foreach (Enemy enemy in deads) {
					enemy.die();
				}
			}
			else {
				if (useTargeting) {
					if (attacking && targetEnemy != null) {
						View.Projectiles.Add(new Projectile(View, Position, targetEnemy.Position));
					}
					else {
						//aim assist targeting
						double lineOfSight = Math.Atan2(target.X, target.Y);
						double closest = Math.PI / 4; //no more than 90 degrees of aim assist
						foreach (Enemy enemy in enemies) {
							double dir = Math.Atan2(enemy.Position.X - Position.X, enemy.Position.Y - Position.Y);
							double margin = Math.Abs(dir - lineOfSight);
							if (margin < closest) {
								closest = margin;
								targetEnemy = enemy;
							}
						}
					}
				}
				else if (attacking) {
						View.Projectiles.Add(new Projectile(View, Position, new Vector2(View.Mouse.X, View.Mouse.Y)));
				}
			}

				if (coolDown > 0) {
					coolDown--;
					if (coolDown == 0)
						playerColor = Color.Green; 
				}

				base.update(delta);
		}

		public override void draw (double delta){
			if (!ranged) {
				double los = Math.Atan2(target.Y, target.X);
				Shapes.DrawLine(Position.X, Position.Y, Position.X + ((float) Math.Cos(los + Math.PI / 4) * meleeRange), Position.Y + ((float) Math.Sin(los + Math.PI / 4) * meleeRange), Color.Purple);
				Shapes.DrawLine(Position.X, Position.Y, Position.X + ((float) Math.Cos(los - Math.PI / 4) * meleeRange), Position.Y + ((float) Math.Sin(los - Math.PI / 4) * meleeRange), Color.Purple);
			}
			else {
			Vector2 targetExt = Vector2.Multiply(target, meleeRange);
			if (useTargeting && targetEnemy != null)
				Shapes.DrawCircle(targetEnemy.Position.X, targetEnemy.Position.Y, 8, 16, Color.Magenta);
			Shapes.DrawLine(Position.X, Position.Y, Position.X + targetExt.X, Position.Y + targetExt.Y, Color.Purple); 
			}

			Shapes.DrawCircle(Position.X, Position.Y, 16, 32, playerColor);

		}

		public void attack (bool isAttacking){
			if (isAttacking == true && coolDown == 0) {
				attacking = true;
				coolDown = coolDownTime;
				playerColor = Color.Purple;
			}
			else
				attacking = false;
		}
	}
}

