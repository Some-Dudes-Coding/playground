using Godot;

public class GameManager : Node2D {
	private Paddle _paddle;
	private Ball _ball;
	private Node2D _blocks;

	[Export(PropertyHint.Range, "0, 10, 1")]
	private float _ballOffset;

	[Export]
	private float _blockPadding;

	public override void _Ready() {
		_paddle = GetNode<Paddle>("Paddle");
		_ball = GetNode<Ball>("Ball");

		_blocks = GetNode<Node2D>("Blocks");
		SetupBlocks();
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Space") && !_ball.IsMoving)
			_ball.StartMoving();
	}

	public override void _PhysicsProcess(float delta) {
		if (!_ball.IsMoving)
			_ball.Position = new Vector2(_paddle.Position.x, (_paddle.Position.y - _paddle.Extents.y * 2) - _ballOffset);
	}

	private void SetupBlocks() {
		PackedScene blockScene = GD.Load<PackedScene>("res://Scenes/Block.tscn");

		Vector2 previousRowPosition = Vector2.Zero;
		for (int i = 0; i < 4; i++) {
			Block rowBlock = (Block)blockScene.Instance();
			_blocks.AddChild(rowBlock);

			rowBlock.Position = new Vector2(
				rowBlock.GetCenter().x + _blockPadding, 
				rowBlock.GetCenter().y * 2 + _blockPadding + previousRowPosition.y
			);
			
			Vector2 previousColumnPosition = rowBlock.Position;
			for (int j = 0; j < 9; j++) {
				Block columnBlock = (Block)rowBlock.Duplicate();
				_blocks.AddChild(columnBlock);

				columnBlock.Position = new Vector2(previousColumnPosition.x + columnBlock.GetCenter().x * 2 + _blockPadding, previousColumnPosition.y);
				previousColumnPosition = columnBlock.Position;
			}

			previousRowPosition = rowBlock.Position;
		}
	}

	private void DeadZone_OnBallEnter(Node2D body) {
		_ball.StopMoving();
	}
}
