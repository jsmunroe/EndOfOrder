using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileBuilder;

namespace EndOfOrder
{
    public class SampleTile : ITile
    {
        /// <summary>
        /// Tile background identifier.
        /// </summary>
        public int Background { get; set; }

        /// <summary>
        /// Whether or not this tile is passible.
        /// </summary>
        public bool IsPassible { get; set; }
    }
}
