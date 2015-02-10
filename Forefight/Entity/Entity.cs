using OpenTK;

namespace Forefight.Entity {
	public abstract class Entity {
		private readonly GameView view;

		protected Entity (GameView view, Vector2 position){
			this.view = view;
			this.Position = position;
			Velocity = Vector2.Zero;
		}

		public Vector2 Position { get; set; }

		public Vector2 Velocity { get; set; }

		public GameView View {
			get { return view; }
		}

		public abstract void draw (double delta);

		public virtual void update (double delta){
			Position += Vector2.Multiply(Velocity, (float) delta);
		}
	}
}