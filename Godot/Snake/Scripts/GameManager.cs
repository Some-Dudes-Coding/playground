using System;

using Godot;

public class GameManager : Node2D {
	private Food _food;
	private Snake _snake;

	private Boundary _boundary;
	private Vector2 _boundaryOffset;

	private int _score;
	private Label _scoreLabel;

	private bool _gameOver;
	private Control _gameOverScreen;

	public override void _Ready() {
		_food = GetNode<Food>("Food");
		_food.Connect(nameof(Food.OnEaten), this, nameof(Food_HandleEaten));

		_snake = GetNode<Snake>("Snake");
		_snake.Connect(nameof(Snake.OnBitBody), this, nameof(Snake_GameOver));
		_snake.Connect(nameof(Snake.OnHitWall), this, nameof(Snake_GameOver));

		_boundary = GetNode<Boundary>("Boundary");
		_boundaryOffset = new Vector2(20, 80);

		_scoreLabel = GetNode<Label>("Score Label");
		_score = 0;

		_gameOverScreen = GetNode<Control>("Game Over Screen");
		
		SetRandomFoodLocation();
	}

	private void ResetGame() {
		_gameOver = false;
		_score = 0;
		_scoreLabel.Text = "Score: 0";
		_gameOverScreen.Visible = false;

		_snake.ResetBody(Vector2.Zero);
		SetRandomFoodLocation();

		GetTree().Paused = false;
	}

	private void Food_HandleEaten() {
		_score++;
		_scoreLabel.Text = "Score: " + _score.ToString();

		_snake.IncreaseBodySize();

		SetRandomFoodLocation();
	}

	private void SetRandomFoodLocation() {
		Vector2 grid = _boundary.FieldSize / _food.Size;
		Random random = new Random();

		_food.Position = new Vector2(
			random.Next(0, (int)grid.x) * _food.Size.x + _boundaryOffset.x, 
			random.Next(0, (int)grid.y) * _food.Size.y + _boundaryOffset.y
		);
	}

	private void Snake_GameOver() {
		_gameOver = true;

		GetTree().Paused = true;
		_gameOverScreen.Visible = true;
	}

	public override void _UnhandledInput(InputEvent @event) {
		if (!_gameOver)
			return;
		
		if (@event.IsActionPressed("Space"))
			ResetGame();
	}
}
