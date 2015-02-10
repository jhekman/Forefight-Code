//
//  Main.cs
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
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Forefight.Drawing;

namespace Forefight {
	class Program : IDisposable {
		private GameWorld world;
		private GameView view;

		public Program (){
			world = new GameWorld();
			view = new GameView();
		}

		public void Dispose (){
			world.Dispose();
			view.Dispose();
		}

		public static void Main (string[] args){
			using (Program p = new Program()) {
				p.view.Run();
				Console.WriteLine("End");
			}
		}
	}
}
