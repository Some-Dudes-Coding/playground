using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake {
    public class Snake {
        public static event Action OnBitBody;

        private Vector2 _headPosition;
        private Texture2D _texture;

        private enum Direction { 
            Left,
            Right,
            Up,
            Down
        };

        private Direction _direction;
        private float _timeUntilNextStep;
        private float _timeSincePreviousStep;

        private List<Vector2> _body;

        public Snake(Vector2 position) {
            _headPosition = position;
        }

        public void Initialize() {
            _direction = GetRandomDirection();

            _timeUntilNextStep = 0.100f;
            _timeSincePreviousStep = 0;

            _body = new List<Vector2>() { _headPosition };
        }

        public void LoadContent(ContentManager contentManager) {
            _texture = contentManager.Load<Texture2D>("Body Block/Body Block");

            Vector2 currentPosition = _headPosition;
            for (int i = 0; i < 3; i++) {
                switch (_direction) {
                    case Direction.Right:
                        currentPosition -= new Vector2(_texture.Width, 0);
                        break;

                    case Direction.Left:
                        currentPosition += new Vector2(_texture.Width, 0);
                        break;

                    case Direction.Up:
                        currentPosition += new Vector2(0, _texture.Height);
                        break;

                    case Direction.Down:
                        currentPosition -= new Vector2(0, _texture.Height);
                        break;
                }

                _body.Add(currentPosition);
            }
        }

        public void Update(float delta) {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Left) && _direction != Direction.Right)
                _direction = Direction.Left;

            if (state.IsKeyDown(Keys.Right) && _direction != Direction.Left)
                _direction = Direction.Right;

            if (state.IsKeyDown(Keys.Up) && _direction != Direction.Down)
                _direction = Direction.Up;

            if (state.IsKeyDown(Keys.Down) && _direction != Direction.Up)
                _direction = Direction.Down;

            _timeSincePreviousStep += delta;
            if (_timeSincePreviousStep < _timeUntilNextStep)
                return;

            _timeSincePreviousStep = 0;

            switch (_direction) {
                case Direction.Right:
                    _headPosition += new Vector2(_texture.Width, 0);
                    break;

                case Direction.Left:
                    _headPosition -= new Vector2(_texture.Width, 0);
                    break;

                case Direction.Up:
                    _headPosition -= new Vector2(0, _texture.Height);
                    break;

                case Direction.Down:
                    _headPosition += new Vector2(0, _texture.Height);
                    break;
            }

            UpdateBodyBlockPosition(0, _headPosition);

            if (IsPositionInBody(_headPosition))
                OnBitBody?.Invoke();
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (Vector2 bodyBlock in _body)
                spriteBatch.Draw(_texture, bodyBlock, Color.White);
        }

        public bool IsPositionInBody(Vector2 position) {
            foreach (Vector2 bodyBlock in _body)
                if (position.Equals(bodyBlock))
                    return true;

            return false;
        }

        public void AddTail() {
            _body.Add(_body[_body.Count - 1]);
        }

        private Direction GetRandomDirection() {
            Array values = Enum.GetValues(typeof(Direction));
            return (Direction)values.GetValue(new Random().Next(values.Length));
        }

        private void UpdateBodyBlockPosition(int index, Vector2 newPosition) {
            if (index >= _body.Count)
                return;

            UpdateBodyBlockPosition(index + 1, _body[index]);
            _body[index] = newPosition;
        }
    }
}