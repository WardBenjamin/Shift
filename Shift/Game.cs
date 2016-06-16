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
            GameTime gameTime = new GameTime(TimeSpan.Zero);

            while (Running)
            {
                Platform.ProcessEvents();
                Update(gameTime);
                Draw(gameTime);
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
        }

        public abstract void Update(GameTime time);
        public abstract void Draw(GameTime time);
    }
}
