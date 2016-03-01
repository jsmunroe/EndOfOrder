using System;
using System.Linq;
using System.Reflection;
using TileBuilder;

namespace EndOfOrder.Story
{
    public class ResourceFinder : IResourceFinder
    {
        /// <summary>
        /// Find the game initialization data and return it.
        /// </summary>
        /// <returns>Game initialization data stream.</returns>
        public IResource FindGameInit()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var names = assembly.GetManifestResourceNames();

            var name = $"EndOfOrder.Story.Data.Game.gi";
            name = names.FirstOrDefault(i => i.Equals(name, StringComparison.OrdinalIgnoreCase)); // Correct case.

            if (name == null)
                return null;

            return new AssemblyResource(assembly, name);
        }

        /// <summary>
        /// Find a tile map stream for the given name (<paramref name="a_name"/>) and return it.
        /// </summary>
        /// <param name="a_name">Tile map resource name.</param>
        /// <returns>Tile map stream.</returns>
        public IResource FindTileMap(string a_name)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var names = assembly.GetManifestResourceNames();

            var name = $"EndOfOrder.Story.Data.{a_name}";
            name = names.FirstOrDefault(i => i.Equals(name, StringComparison.OrdinalIgnoreCase)); // Correct case.

            if (name == null)
                return null;

            return new AssemblyResource(assembly, name);
        }

        /// <summary>
        /// Find a background brush for the given name (<paramref name="a_name"/>) and return it.
        /// </summary>
        /// <param name="a_name">Background brush name.</param>
        /// <returns>Background brush.</returns>
        public IResource FindBackground(string a_name)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var names = assembly.GetManifestResourceNames();

            var name = $"EndOfOrder.Story.Images.{a_name}.png";
            name = names.FirstOrDefault(i => i.Equals(name, StringComparison.OrdinalIgnoreCase)); // Correct case.

            if (name == null)
                return null;

            return new AssemblyResource(assembly, name);


        }
    }

}
