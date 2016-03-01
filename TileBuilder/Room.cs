using System;
using TileBuilder.Files;

namespace TileBuilder
{
    public class Room : IRoom
    {
        private static readonly ITile NullTile = new Tile {};

        private readonly ITile[,] _tiles;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Room(UnitCoord a_roomLocation, UnitSize a_roomSize)
        {
            Location = a_roomLocation;
            Size = a_roomSize;

            _tiles = new ITile[a_roomSize.Width, a_roomSize.Height];
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_meta">Room meta data.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_meta"/> is null.</exception>
        public Room(RoomMeta a_meta)
        {
            #region Argument Validation

            if (a_meta == null)
                throw new ArgumentNullException(nameof(a_meta));

            #endregion

            Location = a_meta.RoomLocation;
            Size = a_meta.RoomSize;

            _tiles = new ITile[a_meta.RoomSize.Width, a_meta.RoomSize.Height];
        }

        /// <summary>
        /// Room location.
        /// </summary>
        public UnitCoord Location { get; }

        /// <summary>
        /// Room size.
        /// </summary>
        public UnitSize Size { get; }


        /// <summary>
        /// Get the tile at the given coordinates (<paramref name="x"/>, <paramref name="y"/>).
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns>Tile of the room.</returns>
        public ITile GetTile(int x, int y)
        {
            if (x < 0 || x >= Size.Width || y < 0 || y >= Size.Height)
                return NullTile;

            return _tiles[x, y] ?? NullTile;
        }

        /// <summary>
        /// Set the given tile to the given coordinates (<paramref name="x"/>, <paramref name="y"/>).
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="a_tile">Tile of the room.</param>
        public void SetTile(int x, int y, ITile a_tile)
        {
            if (x < 0 || x >= Size.Width || y < 0 || y >= Size.Height)
                return;

            _tiles[x, y] = a_tile;
        }
    }
}