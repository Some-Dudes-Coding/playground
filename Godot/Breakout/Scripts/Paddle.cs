using Godot;

public class Paddle : KinematicBody2D {
	public Vector2 Extents { get; private set; }
	
	[Export(PropertyHint.Range, "100, 400, 20")]
	private float _speed;

	private bool _readyToResetInputs;

	private bool _leftPressed;
	private bool _rightPressed;

	public override void _Ready() {
		RectangleShape2D collisionShape = (RectangleShape2D)GetNode<CollisionShape2D>("CollisionShape2D").Shape;
		Extents = collisionShape.Extents;
	}

	public override void _Process(float delta) {
		if (_readyToResetInputs)
			ResetInputs();

		_leftPressed |= Input.IsActionPressed("Left");
		_rightPressed |= Input.IsActionPressed("Right");
 	}

	public override void _PhysicsProcess(float delta) {
		if (_leftPressed)
			MoveAndCollide(new Vector2(-_speed * delta, 0));
		else if (_rightPressed)
			MoveAndCollide(new Vector2(_speed * delta, 0));

		_readyToResetInputs = true;
	}

	private void ResetInputs() {
		_readyToResetInputs = false;

		_leftPressed = false;
		_rightPressed = false;
	}
}
