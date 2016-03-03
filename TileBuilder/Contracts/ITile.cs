namespace TileBuilder.Contracts
{
    public interface ITile
    {
        /// <summary>
        /// Tile identifier.
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Tile background sprite name.
        /// </summary>
        string Background { get; }

        /// <summary>
        /// Whether or not this tile is passible.
        /// </summary>
        bool IsPassible { get; }
    }
}