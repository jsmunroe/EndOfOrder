namespace TileBuilder
{
    public interface ITile
    {
        /// <summary>
        /// Tile background identifier.
        /// </summary>
        int Background { get; }

        /// <summary>
        /// Whether or not this tile is passible.
        /// </summary>
        bool IsPassible { get; }
    }
}