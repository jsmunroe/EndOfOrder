using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileBuilder.Contracts;
using TileBuilder.Contracts.Ui;
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
        /// <param name="a_finder">Resource finder.</param>
        public static void Load(IResourceFinder a_finder)
        {
            ResourceFinder = a_finder;

            var gameInit = ResourceFinder.FindGameInit();
            var gameInitReader = new GameInitReader(gameInit);

            var gameInitMeta = gameInitReader.Meta;
            var startMap = ResourceFinder.FindTileMap(gameInitMeta.StartMapName);

            World.GotoMap(startMap, gameInitMeta.StartRoomX, gameInitMeta.StartRoomY);

            var character = new Character
            {
                Sprite = "Sprites.Player",
                SpriteFrame = 1,
                Position = new UnitCoord(3, 3),
            };

            Player.SetCharacter(character);
            World.PlaceCharacter(character);
        }

        /// <summary>
        /// Player context.
        /// </summary>
        public static PlayerContext Player { get; private set; } = new PlayerContext();

        /// <summary>
        /// World context.
        /// </summary>
        public static WorldContext World { get; private set; } = new WorldContext();
























        public class PlayerContext
        {
            private ICharacter _character;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="a_character"></param>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_character"/> is null.</exception>
            public void SetCharacter(ICharacter a_character)
            {
                #region Argument Validation

                if (a_character == null)
                    throw new ArgumentNullException(nameof(a_character));

                #endregion

                _character = a_character;
            }

            /// <summary>
            /// Move the player one space to the north.
            /// </summary>
            public void MoveNorth()
            {
                if (_character == null) return;

                _character.SpriteFrame = 10; // TODO: Get sprite frame from file.
                MoveTo(_character.Position.North());
            }

            /// <summary>
            /// Move the player one space to the sourth.
            /// </summary>
            public void MoveSouth()
            {
                if (_character == null) return;

                _character.SpriteFrame = 1; // TODO: Get sprite frame from file.
                MoveTo(_character.Position.South());
            }

            /// <summary>
            /// Move the player one space to the east.
            /// </summary>
            public void MoveEast()
            {
                if (_character == null) return;

                _character.SpriteFrame = 7; // TODO: Get sprite frame from file.
                MoveTo(_character.Position.East());
            }

            /// <summary>
            /// Move the player one space to the west.
            /// </summary>
            public void MoveWest()
            {
                if (_character == null) return;

                _character.SpriteFrame = 4; // TODO: Get sprite frame from file.
                MoveTo(_character.Position.West());
            }

            /// <summary>
            /// Move the player to the given coordinates (<paramref name="a_coord"/>).
            /// </summary>
            /// <param name="a_coord">Destination coordiantes.</param>
            private void MoveTo(UnitCoord a_coord)
            {
                if (World.CurrentRoom.InRoom(a_coord))
                {
                    var tile = World.CurrentRoom.GetTile(a_coord);

                    if (tile.IsPassible)
                        _character.Position = a_coord;

                    World.Update(_character);
                }
                else
                {
                    var roomWidth = World.CurrentRoom.Size.Width;
                    var roomHeight = World.CurrentRoom.Size.Height;

                    var offsetX = 0;
                    var offsetY = 0;

                    if (a_coord.X < 0)
                        offsetX = -1;
                    else if (a_coord.X >= roomWidth)
                        offsetX = 1;

                    if (a_coord.Y < 0)
                        offsetY = -1;
                    else if (a_coord.Y >= roomHeight)
                        offsetY = 1;

                    var newLocation = World.CurrentRoom.Location.Offset(offsetX, offsetY);

                    World.GotoRoom(newLocation);

                    var newCoord = a_coord.Offset(-offsetX*roomWidth, -offsetY*roomHeight);

                    MoveTo(newCoord);
                    World.PlaceCharacter(_character);
                }
            }

        }

        public class WorldContext
        {
            private TileMapReader _reader;

            private UnitCoord _roomLocation;

            private IRoom _room;

            private IRoomView _roomView;

            /// <summary>
            /// Current room.
            /// </summary>
            public IRoom CurrentRoom => _room;

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
            /// <param name="a_coord">Coordinates of the room to which to go.</param>
            public void GotoRoom(UnitCoord a_coord)
            {
                GotoRoom(a_coord.X, a_coord.Y);
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

                _roomLocation = new UnitCoord(a_roomX, a_roomY);
                _room = _reader.ReadRoom(a_roomX, a_roomY);

                _roomView?.ShowRoom(_room);
            }

            /// <summary>
            /// Goto the room to the north within the current map.
            /// </summary>
            public void GotoNorth()
            {
                GotoRoom(_roomLocation.North());
            }

            /// <summary>
            /// Goto the room to the south within the current map.
            /// </summary>
            public void GotoSouth()
            {
                GotoRoom(_roomLocation.South());
            }

            /// <summary>
            /// Goto the room to the east within the current map.
            /// </summary>
            public void GotoEast()
            {
                GotoRoom(_roomLocation.East());
            }

            /// <summary>
            /// Goto the room to the west within the current map.
            /// </summary>
            public void GotoWest()
            {
                GotoRoom(_roomLocation.West());
            }

            /// <summary>
            /// Register the given room view (<paramref name="a_view"/>) for the game.
            /// </summary>
            /// <param name="a_view">Room view.</param>
            public void RegisterRoomView(IRoomView a_view)
            {
                _roomView = a_view;
            }

            /// <summary>
            /// Place the character (<paramref name="a_character"/>) in the current room.
            /// </summary>
            /// <param name="a_character">Character.</param>
            public void PlaceCharacter(ICharacter a_character)
            {
                _roomView?.ShowCharacter(a_character);
            }

            /// <summary>
            /// Update the given character (<paramref name="a_character"/>).
            /// </summary>
            /// <param name="a_character">Character.</param>
            public void Update(ICharacter a_character)
            {
                _roomView?.UpdateCharacter(a_character);
            }
        }

    }

}
