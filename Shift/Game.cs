using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shift
{
    public abstract class Game
    {
        private Stopwatch _gameTimer;
        private GameTime _gameTime;

        public GameWindow Window;

        /// <summary>
        /// True if the game loop is running.
        /// </summary>
        public bool Running = false;

        /// <summary>
        /// Indicates whether the game is currently the active application.
        /// </summary>
        public bool IsActive
        {
            get { return Platform.IsActive; }
        }

        public Game() : this(800, 600, "Shift") { }

        public Game(int width, int height, string title)
        {
            Platform.Init(this);
            Window = new GameWindow(width, height, title);
            Running = true;
        }

        public Game(Vector2 size, string title) : this((int)size.X, (int)size.Y, title) { }

        public void Run(bool useVSync = false)
        {
            if (useVSync)
                throw new ArgumentException("VSync currently not supported.");

            Window.Create();
            Window.Show();

            _gameTimer = Stopwatch.StartNew();
            _gameTime = new GameTime(TimeSpan.Zero, TimeSpan.Zero);

            Console.WriteLine("Version: " + OpenGL.Gl.GetString(OpenGL.StringName.Version) + "!");

            Init();

            while (Running)
            {
                _gameTime.Delta = _gameTimer.Elapsed - _gameTime.Total;
                _gameTime.Total = _gameTimer.Elapsed;

                Platform.ProcessEvents();
                Update(_gameTime);
                Draw(_gameTime);
                Window.SwapBuffer();
            }

            Window.Delete();
        }

        /// <summary>
        /// Resets the elapsed time reported by the game timer.
        /// </summary>
        public void ResetElapsedTime()
        {
            _gameTimer.Restart();
            _gameTime.Total = TimeSpan.Zero;
        }

        public abstract void Init();
        public abstract void Update(GameTime time);
        public abstract void Draw(GameTime time);
    }
}
