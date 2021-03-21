using System;
using Godot;

public class Block : StaticBody2D {
	private Sprite _sprite;
	private Hardness _hardness;

	[Export]
	private Color _lightColor;
	
	[Export]
	private Color _mediumColor;
	
	[Export]
	private Color _hardColor;

	private enum Hardness {
		Light,
		Medium,
		Hard
	}

	public override void _Ready() {
		_sprite = GetNode<Sprite>("Sprite");

		_hardness = GetRandomHardness();
		SetHardnessTint(_hardness);
	}

	public Vector2 GetCenter() {
		return new Vector2(_sprite.Texture.GetWidth() / 2, _sprite.Texture.GetHeight() / 2);
	}

	private void SetHardnessTint(Hardness hardness) {
		switch(hardness) {
			case Hardness.Light:
				_sprite.Modulate = _lightColor;
				break;

			case Hardness.Medium:
				_sprite.Modulate = _mediumColor;
				break;

			case Hardness.Hard:
				_sprite.Modulate = _hardColor;
				break;
		}
	}

	private Hardness GetRandomHardness() {
		Array values = Enum.GetValues(typeof(Hardness));

		Random random = new Random();
		return (Hardness)values.GetValue(random.Next(0, values.Length));
	}

	private void OnBodyEntered(Node2D body) {
		if (_hardness == Hardness.Light)
			QueueFree();

		SetHardnessTint(--_hardness);
	}
}
