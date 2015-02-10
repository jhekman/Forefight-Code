//
//  Boom.cs
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
	public class Boom : Entity{
		int boomRadius;
		int boom = 0;

		public Boom (GameView view, Vector2 position, int boomRadius) : base(view, position) {
			this.boomRadius = boomRadius;
		}

		public override void draw(double delta) {
			Shapes.DrawCircle(Position.X, Position.Y, boom, 32, Color.Red);
			if (boom == boomRadius)
				View.Effects.Remove(this);
		}

		public override void update(double delta) {
			if (boom < boomRadius)
				boom++;
			base.update(delta);
		}

	}
}

