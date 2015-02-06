//
//  Enemy.cs
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
using BaseGame.Drawing;

namespace BaseGame.Entity {
	public class Enemy : Entity {
		Player target;
		Vector2 vel;
		int radius = 16; // used for hitbox
		int speed = 60;

		public Enemy (GameView view, Vector2 position) : base(view, position){
			target = view.Player;
		}

		public Enemy (GameView view, Vector2 position, int radius) : base(view, position){
			target = view.Player;
			this.radius = radius;
		}

		public int getRadius() {
			return radius;
		}

		public void die() {
			View.Enemies.Remove(this);
			View.Effects.Add(new Boom(View, Position, radius * 2));
		}

		public override void draw (double delta){
			Shapes.DrawCircle(Position.X + Velocity.X, Position.Y + Velocity.Y, 8, 16, Color.Red);
			Shapes.DrawCircle(Position.X, Position.Y, radius, 32, Color.Blue);
		}

		public override void update (double delta){
			Vector2 tar = target.Position;
			vel = new Vector2(tar.X - Position.X, tar.Y - Position.Y);
			vel.Normalize();
			vel = Vector2.Multiply(vel, speed);

			Velocity = vel;

			base.update(delta);
		}
	}
}

