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
        private TimeSpan _targetElapsedTime = TimeSpan.FromTicks(166667); // 60 FPS, used for fixed framerate timing
        private TimeSpan _accumulatedElapsedTime = TimeSpan.Zero; // Used to track elapsed time

        private Stopwatch _gameTimer;

        public GameWindow Window;

        public bool Running = false;

        public Game() : this(800, 600, "Shift") { }

        public Game(int width, int height, string title)
        {
            Window = new GameWindow(width, height, title);
            Running = true;
        }

        public Game(Vector2 size, string title) : this((int)size.X, (int)size.Y, title) { }

        public void Run(bool isFixedTimeStep = false)
        {
            Window.Show();

            _gameTimer = Stopwatch.StartNew();
            GameTime gameTime = new GameTime(TimeSpan.Zero);

            if(isFixedTimeStep)
            {
                while(Running)
                {

                }
            }
            else
            {
                while (Running)
                {
                    Update(gameTime);
                    Draw(gameTime);
                }
            }
        }

        /// <summary>
        /// Resets the elapsed time reported by the game timer.
        /// </summary>
        public void ResetElapsedTime()
        {
            _gameTimer.Restart();
            _accumulatedElapsedTime = TimeSpan.Zero;
        }
        
        public abstract void Update(GameTime time);
        public abstract void Draw(GameTime time);
    }
}
