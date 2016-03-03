using TileBuilder.Contracts;

namespace TileBuilder
{
    public class Tile : ITile
    {
        /// <summary>
        /// Tile identifier.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Tile background sprite name.
        /// </summary>
        public string Background { get; set; }

        /// <summary>
        /// Whether or not this tile is passible.
        /// </summary>
        public bool IsPassible { get; set; }
    }
}