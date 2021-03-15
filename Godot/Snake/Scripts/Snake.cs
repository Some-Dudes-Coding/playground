using System;

using Godot;

public class Snake : Node2D {
	[Signal]
	public delegate void OnBitBody();

	[Signal]
	public delegate void OnHitWall();

	[Export(PropertyHint.Range, "1, 10, 1")]
	private int _bodyLength = 3;

	private enum Direction {
		Left,
		Right,
		Up,
		Down
	}

	private Direction _direction;

	private Node2D _body;
	private KinematicBody2D _head;

	private Sprite _sprite;

	private float _timeUntilNextStep;
	private float _timeSincePreviousStep;

	public override void _Ready() {
		_body = GetNode<Node2D>("Body");
		_head = GetNode<KinematicBody2D>("Head");
		_sprite = _head.GetNode<Sprite>("Sprite");

		_timeUntilNextStep = 0.100f;
		_timeSincePreviousStep = 0f;

		ResetBody(_head.Position);
	}

	public void ResetBody(Vector2 headPosition) {
		_head.Position = headPosition;
		_direction = GetRandomDirection();

		foreach (Node bodyBlock in _body.GetChildren()) {
			_body.RemoveChild(bodyBlock);
			bodyBlock.QueueFree();
		}

		Vector2 previousPosition = Vector2.Zero;
		for (int i = 0; i < _bodyLength; i++) {
			Sprite newBodyBlock = (Sprite)_sprite.Duplicate();

			switch (_direction) {
				case Direction.Left:
					newBodyBlock.Position = new Vector2(previousPosition.x + newBodyBlock.Texture.GetWidth(), 0);
					break;

				case Direction.Right:
					newBodyBlock.Position = new Vector2(previousPosition.x - newBodyBlock.Texture.GetWidth(), 0);
					break;

				case Direction.Up:
					newBodyBlock.Position = new Vector2(0, previousPosition.y + newBodyBlock.Texture.GetHeight());
					break;

				case Direction.Down:
					newBodyBlock.Position = new Vector2(0, previousPosition.y - newBodyBlock.Texture.GetHeight());
					break;
			}

			_body.AddChild(newBodyBlock);
			previousPosition = newBodyBlock.Position;
		}
	}

	public override void _Process(float delta) {
		_timeSincePreviousStep += delta;

		if (_timeSincePreviousStep < _timeUntilNextStep)
			return;

		_timeSincePreviousStep = 0;

		if (Input.IsActionPressed("Left") && _direction != Direction.Right)
			_direction = Direction.Left;
		else if (Input.IsActionPressed("Right") && _direction != Direction.Left)
			_direction = Direction.Right;
		else if (Input.IsActionPressed("Up") && _direction != Direction.Down)
			_direction = Direction.Up;
		else if (Input.IsActionPressed("Down") && _direction != Direction.Up)
			_direction = Direction.Down;

		Vector2 previousPosition = _head.Position;
		KinematicCollision2D collision = null;

		switch (_direction) {
			case Direction.Left:
				collision = _head.MoveAndCollide(new Vector2(-_sprite.Texture.GetWidth(), 0));
				break;

			case Direction.Right:
				collision = _head.MoveAndCollide(new Vector2(_sprite.Texture.GetWidth(), 0));
				break;          

			case Direction.Up:
				collision = _head.MoveAndCollide(new Vector2(0, -_sprite.Texture.GetHeight()));
				break;

			case Direction.Down:
				collision = _head.MoveAndCollide(new Vector2(0, _sprite.Texture.GetHeight()));
				break;
		}

		UpdateBodyPosition(0, previousPosition);

		if (collision != null)
			EmitSignal(nameof(OnHitWall));

		if (IsHeadInBody())
			EmitSignal(nameof(OnBitBody));
	}

	public void IncreaseBodySize() {
		Sprite newBodyBlock = (Sprite)_sprite.Duplicate();
		newBodyBlock.Position = ((Node2D)_body.GetChild(_body.GetChildCount() - 1)).Position;

		_body.AddChild(newBodyBlock);
	}

	private void UpdateBodyPosition(int index, Vector2 newPosition) {
		if (index == _body.GetChildCount())
			return;

		Node2D currentBlock = (Node2D)_body.GetChild(index);

		UpdateBodyPosition(index + 1, currentBlock.Position);
		currentBlock.Position = newPosition;
	}

	private bool IsHeadInBody() {
		foreach (Node2D bodyBlock in _body.GetChildren())
			if (_head.Position == bodyBlock.Position)
				return true;

		return false;
	}

	private Direction GetRandomDirection() {
		System.Array values = Enum.GetValues(typeof(Direction));
		return (Direction)values.GetValue(new Random().Next(values.Length));
	}
}
