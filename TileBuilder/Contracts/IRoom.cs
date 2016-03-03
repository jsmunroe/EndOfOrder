namespace TileBuilder.Contracts
{
    public interface IRoom
    {
        /// <summary>
        /// Room location.
        /// </summary>
        UnitCoord Location { get; }

        /// <summary>
        /// Room size.
        /// </summary>
        UnitSize Size { get; }

        /// <summary>
        /// Get the tile at the given coordinates (<paramref name="a_coord"/>).
        /// </summary>
        /// <param name="a_coord">Coordiantes.</param>
        /// <returns>Tile of the room.</returns>
        ITile GetTile(UnitCoord a_coord);

        /// <summary>
        /// Get the tile at the given coordinates (<paramref name="x"/>, <paramref name="y"/>).
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns>Tile of the room.</returns>
        ITile GetTile(int x, int y);

        /// <summary>
        /// Get whether the given tile coordinates is in this room (<paramref name="a_coord"/>).
        /// </summary>
        /// <param name="a_coord">Coordinates.</param>
        /// <returns>True if the coordinate is still in the room.</returns>
        bool InRoom(UnitCoord a_coord);

        /// <summary>
        /// Get whether the given tile coordinates is in this room (<paramref name="x"/>, <paramref name="y"/>).
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns>True if the coordinate is still in the room.</returns>
        bool InRoom(int x, int y);
    }
}
