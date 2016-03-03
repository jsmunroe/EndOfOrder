using System.IO;

namespace TileBuilder.Contracts
{
    public interface IResourceFinder
    {
        /// <summary>
        /// Find the game initialization data and return it.
        /// </summary>
        /// <returns>Game initialization data stream.</returns>
        IResource FindGameInit();

        /// <summary>
        /// Find a tile map data for the given name (<paramref name="a_name"/>) and return it.
        /// </summary>
        /// <param name="a_name">Tile map resource name.</param>
        /// <returns>Tile map stream.</returns>
        IResource FindTileMap(string a_name);

        /// <summary>
        /// Find a image resource for the given name (<paramref name="a_name"/>) and return it.
        /// </summary>
        /// <param name="a_name">Image name.</param>
        /// <returns>Image resource.</returns>
        IResource FindImage(string a_name);
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
