using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileBuilder;

namespace EndOfOrder
{
    public class SampleRoom : IRoom
    {
        /// <summary>
        /// Room width in tiles.
        /// </summary>
        public int Width { get; } = 20;

        /// <summary>
        /// Room height in tiles.
        /// </summary>
        public int Height { get; } = 15;


        /// <summary>
        /// Get the tile at the given coordinates (<paramref name="x"/>, <paramref name="y"/>).
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns>Tile of the room.</returns>
        public ITile GetTile(int x, int y)
        {
            var background = 0;
            if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                background = 1;

            return new SampleTile
            {
                 Background = background,
                 IsPassible = true,
            };
        }
    }
}
