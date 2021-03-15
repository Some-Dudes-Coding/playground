using Godot;

public class Food : Area2D {
	[Signal]
	public delegate void OnEaten();

	public Vector2 Size { get; private set; }

	public override void _Ready() {
		Size = GetNode<Sprite>("Sprite").Texture.GetSize();
	}

	private void OnBodyEntered(Node2D body) {
		EmitSignal(nameof(OnEaten));
	}
}
