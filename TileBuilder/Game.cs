using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileBuilder.Files;

namespace TileBuilder
{
    /// <summary>
    /// Game state object.
    /// </summary>
    public static class Game
    {
        private static IResourceFinder s_finder;

        /// <summary>
        /// Load the game with the given resource finder (<paramref name="a_finder"/>).
        /// </summary>
        /// <param name="a_finder"></param>
        public static void Load(IResourceFinder a_finder)
        {
            s_finder = a_finder;

            var gameInit = s_finder.FindGameInit();
            var gameInitReader = new GameInitReader(gameInit);

            var gameInitMeta = gameInitReader.Meta;
            var startMap = s_finder.FindTileMap(gameInitMeta.StartMapName);
            

        }

        public static World World { get; private set; }
    }

    public class World
    {
        /// <summary>
        /// Goto the map within the given map resource (<paramref name="a_map"/>).
        /// </summary>
        /// <param name="a_map">Map resource.</param>
        /// <param name="a_roomX">X coordinate of the room to which to go.</param>
        /// <param name="a_roomY">Y coordinate of the room to which to go.</param>
        public void GotoMap(IResource a_map, int a_roomX, int a_roomY)
        {
            
        }
    }

    public interface IRoomView
    {
        /// <summary>
        /// Show the given room (<paramref name="a_room"/>) in this view.
        /// </summary>
        /// <param name="a_room">Room to show.</param>
        void ShowRoom(IRoom a_room);
    }
}
