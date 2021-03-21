using System;

using Godot;

public class Ball : KinematicBody2D {
	public bool IsMoving { get; private set; }

	[Export]
	private Vector2 _defaultSpeed;
	private Vector2 _speed;

	public override void _Ready() {}

	public override void _PhysicsProcess(float delta) {
		if (!IsMoving)
			return;

		KinematicCollision2D collision = MoveAndCollide(_speed * delta);
		if (collision == null)
			return;

		if (collision.Normal.x != 0)
			_speed = new Vector2(_speed.x * -1, _speed.y);
		else
			_speed = new Vector2(_speed.x, _speed.y * -1);
	}

	public void StartMoving() {
		IsMoving = true;

		int angle = new Random().Next(45, 135);

		_speed = new Vector2(
			_defaultSpeed.x * Mathf.Cos(angle * (float)Math.PI / 180f), 
			-_defaultSpeed.y * Mathf.Sin(angle * (float)Math.PI / 180f)
		);
	}

	public void StopMoving() {
		IsMoving = false;
	}
}
