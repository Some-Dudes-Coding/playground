using Godot;

public class Boundary : StaticBody2D {
	public Vector2 FieldSize { get; private set; }

	public override void _Ready() {
		Sprite sprite = GetNode<Sprite>("Sprite");

		FieldSize = new Vector2(sprite.Texture.GetSize().x - 20, sprite.Texture.GetSize().y - 20);
	}
}
