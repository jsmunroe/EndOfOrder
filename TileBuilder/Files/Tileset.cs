using System.Collections.Generic;
using System.Linq;
using TileBuilder.Contracts;

namespace TileBuilder.Files
{
    public class Tileset : ITileset
    {
        private readonly List<ITile> _tiles = new List<ITile>(); 

        /// <summary>
        /// Get the tile with the given identifier (<paramref name="a_tileId"/>).
        /// </summary>
        /// <param name="a_tileId">Tile identifier.</param>
        /// <returns>Tile.</returns>
        public ITile GetTile(int a_tileId)
        {
            return _tiles.FirstOrDefault(i => i.ID == a_tileId);
        }

        /// <summary>
        /// Set the given tile (<paramref name="a_tile"/>) into this tileset.
        /// </summary>
        /// <param name="a_tile">Tile.</param>
        public void SetTile(ITile a_tile)
        {
            var tile = _tiles.FirstOrDefault(i => i.ID == a_tile.ID);

            if (tile != null)
                _tiles.Remove(tile);

            _tiles.Add(a_tile);
        }
    }
}