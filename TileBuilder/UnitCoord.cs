using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileBuilder
{
    public struct UnitCoord
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_x">X coordinate.</param>
        /// <param name="a_y">Y coordinate.</param>
        public UnitCoord(int a_x, int a_y)
        {
            X = a_x;
            Y = a_y;
        }

        /// <summary>
        /// X coordinate.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y coordinate.
        /// </summary>
        public int Y { get; set; }
    }
}
