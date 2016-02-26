using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileBuilder
{
    public interface IRoom
    {
        /// <summary>
        /// Room width in tiles.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Room height in tiles.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Get the tile at the given coordinates (<paramref name="x"/>, <paramref name="y"/>).
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns>Tile of the room.</returns>
        ITile GetTile(int x, int y);
    }
}
