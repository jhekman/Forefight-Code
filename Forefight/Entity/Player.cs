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
using Forefight.Drawing;
using System.Collections.Generic;
using System;
using OpenTK.Graphics.OpenGL;

namespace Forefight.Entity {
	public class Player : Entity {
		public enum Type {
			FIREBALL = 0,
			ARROW = 1,
			SHIELD = 2,
			CLOAK = 3
		}

		public bool ranged = true;
		public bool useTargeting = false;
		private const float meleeRange = 70;
		private const double meleeAngle = Math.PI / 4;
		private const int coolDownTime = 40; // Change this value to change cooldown
		private int coolDown = 0; // Do not change this value
		private int speed = 240;
		private const int maxHP = 40;
		private int HP;
		private Color playerColor = Color.Green; // Color changes when on cooldown
		private Vector2 target;
		private Enemy targetEnemy;
		private bool attacking;

		public Player (GameView view, Vector2 position) : base(view, position){
			target = Vector2.Zero;
			targetEnemy = null;
			attacking = false;
			HP = maxHP;
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

		public void takeHit (int damage) {
			HP -= damage;
			if (HP <= 0)
				die();
		}

		public void die() { // Player Dies
			View.Effects.Add(new Boom(View, Position, 500)); // Just for now 
		}

		public float getHealthRatio() {
			return (float) HP / maxHP;
		}

		public override void update (double delta){
			List<Enemy> enemies = View.Enemies;
			if (!ranged) {
				if (attacking) {
					List<Enemy> deads = new List<Enemy>();
					foreach (Enemy enemy in enemies) {
						Vector2 relative = enemy.Position - Position;
						float relDist = relative.Length;
						double relAngle = Math.Atan2(relative.Y, relative.X) - Math.Atan2(target.Y, target.X);
						relative.X = (float) Math.Cos(relAngle) * relDist;
						relative.Y = (float) Math.Sin(relAngle) * relDist;
						int enemyRadius = enemy.getRadius();
						//Melee Cone
						if (relative.Length <= meleeRange + enemy.getRadius() && //Within kill radius
							((meleeAngle >= Math.PI / 2) ? ((meleeAngle == Math.PI / 2) ? //crazy conditional shit
								//if meleeAngle == Math.PI / 2
								(relative.X + enemyRadius >= 0) :
								//if meleeAngle > Math.PI / 2
								((relative.Y <= Math.Tan(-meleeAngle) * relative.X + enemyRadius) || (relative.Y >= Math.Tan(meleeAngle) * relative.X - enemyRadius))) :
								//if meleeAngle < Math.PI / 2
								((relative.Y <= Math.Tan(meleeAngle) * relative.X + enemyRadius) && (relative.Y >= Math.Tan(-meleeAngle) * relative.X - enemyRadius)))) {
							deads.Add(enemy);
						}
					}
					foreach (Enemy enemy in deads) {
						enemy.dead = true;
					}
				}
			}
			else {
				if (useTargeting) {
					if (targetEnemy != null) {
						if (targetEnemy.dead == true)
							targetEnemy = null;
						else if (attacking) {
							View.Projectiles.Add(new Projectile(View, Position, targetEnemy.Position, true));
						}
					}
					double lineOfSight = Math.Atan2(target.X, target.Y);
					double closest = meleeAngle; //no more than 90 degrees of aim assist


					foreach (Enemy enemy in enemies) {
						double dir = Math.Atan2(enemy.Position.X - Position.X, enemy.Position.Y - Position.Y);
						double margin = Math.Abs(dir - lineOfSight);
						if (margin < closest) {
							closest = margin;
							targetEnemy = enemy;
						}
					}
				}
				else if (attacking) {
					View.Projectiles.Add(new Projectile(View, Position, new Vector2(View.Mouse.X, View.Mouse.Y), true));
				}
			}

			if (coolDown > 0) {
				coolDown--;
				if (coolDown == 0)
					playerColor = Color.Green; 
			}

			base.update(speed * delta);
		}

		public override void draw (double delta){
			if (!ranged) {
				double los = Math.Atan2(target.Y, target.X);
				Shapes.DrawLine(Position.X, Position.Y, Position.X + ((float) Math.Cos(los + meleeAngle) * meleeRange), Position.Y + ((float) Math.Sin(los + meleeAngle) * meleeRange), Color.Purple);
				Shapes.DrawLine(Position.X, Position.Y, Position.X + ((float) Math.Cos(los - meleeAngle) * meleeRange), Position.Y + ((float) Math.Sin(los - meleeAngle) * meleeRange), Color.Purple);
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


