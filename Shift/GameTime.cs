using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shift
{
    public class GameTime
    {
        /// <summary>
        /// Represents the total elapsed time since the beginning of the game loop.
        /// </summary>
        public TimeSpan Total;

        /// <summary>
        /// Represents the change in time since the last game loop iteration.
        /// </summary>
        public TimeSpan Delta;

        public GameTime(TimeSpan total, TimeSpan delta)
        {
            Total = total;
            Delta = delta;
        }
    }
}
