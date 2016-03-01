using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileBuilder
{
    public interface IResourceFinder
    {
        /// <summary>
        /// Find the game initialization data and return it.
        /// </summary>
        /// <returns>Game initialization data stream.</returns>
        IResource FindGameInit();

        /// <summary>
        /// Find a tile map stream for the given name (<paramref name="a_name"/>).
        /// </summary>
        /// <param name="a_name">Tile map resource name.</param>
        /// <returns>Tile map stream.</returns>
        IResource FindTileMap(string a_name);

        /// <summary>
        /// Find a background brush for the given name (<paramref name="a_name"/>).
        /// </summary>
        /// <param name="a_name">Background brush name.</param>
        /// <returns>Background brush.</returns>
        IResource FindBackground(string a_name);
    }

    public interface IResource
    {
        /// <summary>
        /// Open the resource stream and return it.
        /// </summary>
        /// <returns>Resource stream.</returns>
        Stream Open();
    }
}
