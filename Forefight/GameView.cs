//
//  GameView.cs
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
using System;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using BaseGame.Entity;
using System.Collections.Generic;
using System.Collections.Specialized;


//Tim is now here


namespace BaseGame {
	public class GameView : GameWindow {
		GameWorld world;
		readonly Player player;
		readonly List<Enemy> enemies;
		readonly List<Projectile> projectiles;
		readonly List<Boom> effects;
		//readonly MouseState[] mouseBuffer;
		//int mouseBuffIndex;
		double spawnTime;

		private KeyboardState previousKeys, keys;

		public GameView (){
			world = new GameWorld();

			Size = new Size(1024, 768);
			GL.LineWidth(4f);

			player = new Player(this, new Vector2(50, 50));
			enemies = new List<Enemy>();
			projectiles = new List<Projectile>();
			effects = new List<Boom>();
			keys = OpenTK.Input.Keyboard.GetState();

			//mouseBuffer = new MouseState[16];
			//mouseBuffIndex = 0;

			spawnTime = 0;
		}

		public Player Player {
			get { return player; }
		}

		public List<Enemy> Enemies {
			get { return enemies; }
		}

		public List<Projectile> Projectiles {
			get { return projectiles; }
		}

		public List<Boom> Effects {
			get { return effects; }
		}

		protected override void OnResize (EventArgs e){
			// Set orthographic rendering (useful when you want 2D)
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(ClientRectangle.Left, ClientRectangle.Right,
			         ClientRectangle.Bottom, ClientRectangle.Top, -1.0, 1.0);
			GL.Viewport(ClientRectangle.Size);
		}

		protected override void OnRenderFrame (FrameEventArgs e){
			//Input states
		    previousKeys = keys;
			keys = OpenTK.Input.Keyboard.GetState();
			MouseState mouse = OpenTK.Input.Mouse.GetState();

			// Timothy, Do we need this code anymore??
			/*MouseState oldMouse = mouseBuffer[mouseBuffIndex % mouseBuffer.Length];
			Vector2 mouseDelta = new Vector2(mouse.X - oldMouse.X, mouse.Y - oldMouse.Y);
			if (mouseDelta.Length > 1E-8F)
				mouseBuffer[++mouseBuffIndex % mouseBuffer.Length] = mouse;

			MouseState startMouse = mouseBuffer[mouseBuffIndex % mouseBuffer.Length];
			MouseState endMouse = mouseBuffer[(mouseBuffIndex + 1) % mouseBuffer.Length]; */

			float xdelta = Mouse.X - Player.Position.X;
			float ydelta = Mouse.Y - Player.Position.Y;
			//int zdelta = newMouse.Wheel - oldMouse.Wheel;

			//Enemy handling
			Random rand = new Random();
			int posx = rand.Next(0, this.Width);
			int posy = rand.Next(0, this.Height);
			int size = rand.Next(2, 9);

			spawnTime += e.Time;
			if (enemies.Count <= 12 && spawnTime >= 2) {
				spawnTime = 0;
				enemies.Add(new Enemy(this, new Vector2(posx, posy), size * size));
			}

			foreach (Enemy enemy in enemies) {
				enemy.update(e.Time);
			}
			foreach (Projectile projectile in projectiles) {
				projectile.update(e.Time); 
			}
			foreach (Boom boom in effects) {
				boom.update(e.Time);
			}

			//Player input
			Vector2 vel = new Vector2();
			if (keys[OpenTK.Input.Key.S])
				vel.Y = 1;
			else if (keys[OpenTK.Input.Key.W])
				vel.Y = -1;
			else
				vel.Y = 0;
			if (keys[OpenTK.Input.Key.D])
				vel.X = 1;
			else if (keys[OpenTK.Input.Key.A])
				vel.X = -1;
			else
				vel.X = 0;
			if (vel.Length > 0)
				vel.Normalize();
			//Toggle Melee/Ranged
			if (keys[OpenTK.Input.Key.R] && !previousKeys[OpenTK.Input.Key.R])
				Player.ranged = !Player.ranged;
			//Toggle Targeting (only affects ranged)
			if (keys[OpenTK.Input.Key.T] && !previousKeys[OpenTK.Input.Key.T])
				Player.useTargeting = !Player.useTargeting;
			//Boost button
			if (keys[OpenTK.Input.Key.Space])
				vel = Vector2.Multiply(vel, 20);
			player.Velocity = vel;
			player.update(200 * e.Time);

			if (mouse[MouseButton.Left])
				player.attack(true);
			else
				player.attack(false);

			//melee
			if (xdelta != 0 && ydelta != 0) {
				Vector2 target = new Vector2(xdelta, ydelta);
				target.Normalize();
				target = Vector2.Multiply(target, 50);
				player.Target = target;
			}
			//ranged
			//Vector2 target = new Vector2(Mouse.X - player.Position.X, Mouse.Y - player.Position.Y);
			//player.Target = target;

			//Actual draw
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			player.draw(e.Time);

			foreach (Enemy enemy in enemies) {
				enemy.draw(e.Time);
			}
			foreach (Projectile projectile in projectiles) {
				projectile.draw(e.Time);
			}
			for (int i = effects.Count - 1; i > -1; i--) {
				effects[i].draw(e.Time);
			}

			SwapBuffers();
		}
	}
}

