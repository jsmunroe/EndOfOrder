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
        /// <summary>
        /// Resource finder.
        /// </summary>
        public static IResourceFinder ResourceFinder { get; private set; }

        /// <summary>
        /// Load the game with the given resource finder (<paramref name="a_finder"/>).
        /// </summary>
        /// <param name="a_finder"></param>
        public static void Load(IResourceFinder a_finder)
        {
            ResourceFinder = a_finder;

            var gameInit = ResourceFinder.FindGameInit();
            var gameInitReader = new GameInitReader(gameInit);

            var gameInitMeta = gameInitReader.Meta;
            var startMap = ResourceFinder.FindTileMap(gameInitMeta.StartMapName);

            World.GotoMap(startMap, gameInitMeta.StartRoomX, gameInitMeta.StartRoomY);
        }

        /// <summary>
        /// Game world.
        /// </summary>
        public static World World { get; private set; } = new World();
    }

    public class World
    {
        private TileMapReader _reader;

        private int _roomX;
        private int _roomY;

        private IRoom _room;

        private IRoomView _roomView;

        /// <summary>
        /// Goto the map within the given map resource (<paramref name="a_map"/>).
        /// </summary>
        /// <param name="a_map">Map resource.</param>
        /// <param name="a_roomX">X coordinate of the room to which to go.</param>
        /// <param name="a_roomY">Y coordinate of the room to which to go.</param>
        public void GotoMap(IResource a_map, int a_roomX, int a_roomY)
        {
            #region Argument Validation

            if (a_map == null)
                throw new ArgumentNullException(nameof(a_map));

            #endregion

            _reader = new TileMapReader(a_map);

            GotoRoom(a_roomX, a_roomY);
        }

        /// <summary>
        /// Goto the room within the current map.
        /// </summary>
        /// <param name="a_roomX">X coordinate of the room to which to go.</param>
        /// <param name="a_roomY">Y coordinate of the room to which to go.</param>

        public void GotoRoom(int a_roomX, int a_roomY)
        {
            if (_reader == null)
                return;

            _roomX = a_roomX;
            _roomY = a_roomY;
            _room = _reader.ReadRoom(a_roomX, a_roomY);

            _roomView?.ShowRoom(_room);
        }

        /// <summary>
        /// Register a room view for the game.
        /// </summary>
        /// <param name="a_view"></param>
        public void RegisterRoomView(IRoomView a_view)
        {
            _roomView = a_view;
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
