using Forefight.Drawing;
using System.Drawing;
using Forefight.Entity;

namespace Forefight {
	public class Hud {
		private GameView view;
		private const int lengthOfHealthBar = 50; //length of health bar in pixels 
		private const int heightOfHealthBar = 20; //height of health bar in pixels
		private float healthRatio = 1; //How full the bar should be
		private int xPos = 10; // X coordinate of top left vertex of health bar
		private int yPos = 10; // Y coordinate of top left vertex of health bar

		public Hud (GameView view){
			this.view = view;
		}

		public void draw() {
			// Health bar box
			Shapes.DrawLine (xPos, yPos, xPos + lengthOfHealthBar + 1, yPos, Color.White);
			Shapes.DrawLine (xPos, yPos + heightOfHealthBar + 1, xPos + lengthOfHealthBar + 1, yPos + heightOfHealthBar + 1, Color.White);
			Shapes.DrawLine (xPos, yPos, xPos, yPos + heightOfHealthBar + 1, Color.White);
			Shapes.DrawLine (xPos + lengthOfHealthBar + 1, yPos, xPos + lengthOfHealthBar + 1, yPos + heightOfHealthBar + 1, Color.White);

			int length = (int) ((float) lengthOfHealthBar * healthRatio);
			if (length == 0 && healthRatio > 0)
				length = 1;
				
			for(int i = 0; i < length; i++) {
				Shapes.DrawLine (xPos + 1 + i, yPos + 1, xPos + 1 + i, yPos + heightOfHealthBar + 1, Color.Red);
			}

		}

		public void update() {
			healthRatio = view.Player.getHealthRatio ();
		}
	}
}

