using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Helpers.IO;

namespace TileBuilder.Files
{
    public class TileMapReader
    {
        private readonly IResource _resource;

        private TileMapMeta _meta;

        private readonly List<RoomMeta> _roomMetas = new List<RoomMeta>();

        private static readonly Regex s_rexHeader = new Regex(@"^TileMap\t(?<version>\d*)", RegexOptions.Compiled);
        private static readonly Regex s_rexOrtho = new Regex(@"^(?<x>\d*)\t(?<y>\d*)", RegexOptions.Compiled);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_resource">Tile map resource.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_resource"/> is null.</exception>
        public TileMapReader(IResource a_resource)
        {
            #region Argument Validation

            if (a_resource == null)
                throw new ArgumentNullException(nameof(a_resource));

            #endregion

            _resource = a_resource;
        }

        /// <summary>
        /// Metadata. 
        /// </summary>
        public TileMapMeta Meta
        {
            get
            {
                if (_meta == null)
                    ReadMeta();

                return _meta?.Clone();
            }
        }

        /// <summary>
        /// Read the meta data.
        /// </summary>
        private void ReadMeta()
        {
            _meta = null;
            _roomMetas.Clear();

            using (var stream = _resource.Open())
            {
                var reader = new AdvancedStreamReader(stream);

                _meta = new TileMapMeta();

                ReadHeader(reader, _meta);

                if (_meta.Version == 0)
                {
                    ReadMeta_0(reader, _meta);
                    ReadObjects_0(reader);
                }
                else
                    throw new TileMapSchemaException("BadSchemaVersion"); // Unrecognized schema version.

            }
        }

        /// <summary>
        /// Read the room at the given room coordinate (<paramref name="x"/> and <paramref name="y"/>).
        /// </summary>
        /// <param name="x">X room coordinate.</param>
        /// <param name="y">Y room coordinate.</param>
        /// <returns>Room.</returns>
        public IRoom ReadRoom(int x, int y)
        {
            if (_meta == null)
                ReadMeta();

            var roomMeta = _roomMetas.FirstOrDefault(i => i.RoomX == x && i.RoomY == y);

            if (roomMeta == null)
                return new NullRoom();

            using (var stream = _resource.Open())
            {
                var reader = new AdvancedStreamReader(stream);

                var room = ReadRoom_0(reader, roomMeta);
                return room;
            }
        }

        /// <summary>
        /// Read the header line from the given text reader (<paramref name="a_reader"/>) into the given meta structure (<paramref name="a_meta"/>).
        /// </summary>
        /// <param name="a_reader">Text reader.</param>
        /// <param name="a_meta">Meta data structure.</param>
        private void ReadHeader(AdvancedStreamReader a_reader, TileMapMeta a_meta)
        {
            var headerLine = a_reader.ReadLine();

            if (headerLine == null)
                throw new TileMapSchemaException("NoHeaderLine"); // Header line does not exit.

            var headerMatch = s_rexHeader.Match(headerLine);

            if (!headerMatch.Success)
                throw new TileMapSchemaException("BadHeaderLine"); // Badly formatted header line.

            a_meta.Version = int.Parse(headerMatch.Groups["version"].Value);
        }

        /// <summary>
        /// Read the next line and convert it to a coordinate if possible (<paramref name="a_reader"/>)
        /// </summary>
        /// <param name="a_reader">Text reader.</param>
        /// <param name="a_lineName">Line name to show if an exception is encountered.</param>
        /// <returns>Integer coordinate array of size 2.</returns>
        private int[] ReadNextCoord(AdvancedStreamReader a_reader, string a_lineName)
        {
            var mapSizeLine = a_reader.ReadLine();

            if (mapSizeLine == null)
                throw new TileMapSchemaException("No" + a_lineName); // Map size line does not exist.

            var mapSizeMatch = s_rexOrtho.Match(mapSizeLine);

            if (!mapSizeMatch.Success)
                throw new TileMapSchemaException("Bad" + a_lineName); // Badly formatted map size line.

            return new[]
            {
                int.Parse(mapSizeMatch.Groups["x"].Value),
                int.Parse(mapSizeMatch.Groups["y"].Value)
            };
        }

        /// <summary>
        /// Read the meta data from the given text reader (<paramref name="a_reader"/>) into the given meta structure (<paramref name="a_meta"/>).
        /// </summary>
        /// <param name="a_reader">Text reader.</param>
        /// <param name="a_meta">Meta data structure.</param>
        private void ReadMeta_0(AdvancedStreamReader a_reader, TileMapMeta a_meta)
        {
            var mapSize = ReadNextCoord(a_reader, "MapSizeLine");

            a_meta.MapWidth = mapSize[0];
            a_meta.MapHeight = mapSize[1];

        }

        /// <summary>
        /// Read the object meta data from the given text reader (<paramref name="a_reader"/>).
        /// </summary>
        /// <param name="a_reader">Text reader.</param>
        private void ReadObjects_0(AdvancedStreamReader a_reader)
        {
            var pos = a_reader.CharacterPosition;
            var line = a_reader.ReadLine();
            while (line != null)
            {
                if (line == "ROOM")
                    ReadRoomMeta_0(a_reader, pos);

                pos = a_reader.CharacterPosition;
                line = a_reader.ReadLine();
            }
        }

        /// <summary>
        /// Read the room meta data from the given text reader (<paramref name="a_reader"/>).
        /// </summary>
        /// <param name="a_reader">Text reader.</param>
        /// <param name="a_pos">File position.</param>
        private void ReadRoomMeta_0(AdvancedStreamReader a_reader, long a_pos)
        {
            var coords = ReadNextCoord(a_reader, "RoomCoordLine");
            var size = ReadNextCoord(a_reader, "RoomSizeLine");

            var room = new RoomMeta
            {
                FilePosition = a_pos,
                RoomX = coords[0],
                RoomY = coords[1],
                RoomWidth = size[0],
                RoomHeight = size[1],
            };

            _roomMetas.Add(room);
        }

        /// <summary>
        /// Read the room from the given text reader (<paramref name="a_reader"/>).
        /// </summary>
        /// <param name="a_reader">Text reader.</param>
        /// <param name="a_meta">File position.</param>
        private IRoom ReadRoom_0(AdvancedStreamReader a_reader, RoomMeta a_meta)
        {
            a_reader.SeekCharacter(a_meta.FilePosition);

            // Read past meta data
            var header = a_reader.ReadLine();
            var roomCoord = a_reader.ReadLine();
            var roomSize = a_reader.ReadLine();

            var room = new Room(a_meta);

            for (var y = 0; y < a_meta.RoomHeight; y++)
            {
                var tileLine = a_reader.ReadLine();

                if (tileLine == null)
                    return new NullRoom();

                var tiles = tileLine.Split('\t');

                for (var x = 0; x < tiles.Length; x++)
                {
                    int tileID;
                    if (!int.TryParse(tiles[x], out tileID))
                        continue;

                    var tile = new Tile
                    {
                        Background = tileID,
                    };

                    room.SetTile(x, y, tile);
                }
            }

            return room;
        }







        public class Room : IRoom
        {
            private readonly ITile[,] _tiles;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="a_meta">Room meta data.</param>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_meta"/> is null.</exception>
            public Room(RoomMeta a_meta)
            {
                #region Argument Validation

                if (a_meta == null)
                    throw new ArgumentNullException(nameof(a_meta));

                #endregion

                Width = a_meta.RoomWidth;
                Height = a_meta.RoomHeight;

                _tiles = new ITile[a_meta.RoomWidth, a_meta.RoomHeight];
            }

            /// <summary>
            /// Room width in tiles.
            /// </summary>
            public int Width { get; }

            /// <summary>
            /// Room height in tiles.
            /// </summary>
            public int Height { get; }

            /// <summary>
            /// Get the tile at the given coordinates (<paramref name="x"/>, <paramref name="y"/>).
            /// </summary>
            /// <param name="x">X coordinate.</param>
            /// <param name="y">Y coordinate.</param>
            /// <returns>Tile of the room.</returns>
            public ITile GetTile(int x, int y)
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    return new NullTile();

                return _tiles[x, y] ?? new NullTile();
            }

            /// <summary>
            /// Set the given tile to the given coordinates (<paramref name="x"/>, <paramref name="y"/>).
            /// </summary>
            /// <param name="x">X coordinate.</param>
            /// <param name="y">Y coordinate.</param>
            /// <param name="a_tile">Tile of the room.</param>
            public void SetTile(int x, int y, ITile a_tile)
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    return;

                _tiles[x, y] = a_tile;
            }
        }

        public class Tile : ITile
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






        #region NullRoom

        /// <summary>
        /// Room returned if the room coordinates are invalid.
        /// </summary>
        public class NullRoom : IRoom
        {
            /// <summary>
            /// Room width in tiles.
            /// </summary>
            public int Width { get; } = 10;

            /// <summary>
            /// Room height in tiles.
            /// </summary>
            public int Height { get; } = 10;

            /// <summary>
            /// Get the tile at the given coordinates (<paramref name="x"/>, <paramref name="y"/>).
            /// </summary>
            /// <param name="x">X coordinate.</param>
            /// <param name="y">Y coordinate.</param>
            /// <returns>Tile of the room.</returns>
            public ITile GetTile(int x, int y)
            {
                return new NullTile();
            }
        }

        /// <summary>
        /// Tile returned from a <see cref="NullRoom"/>.
        /// </summary>
        public class NullTile : ITile
        {
            /// <summary>
            /// Tile background identifier.
            /// </summary>
            public int Background { get; set; } = 0;

            /// <summary>
            /// Whether or not this tile is passible.
            /// </summary>
            public bool IsPassible { get; set; } = false;
        } 

        #endregion

    }

    public class TileMapSchemaException : Exception
    {
        public TileMapSchemaException(String a_error) 
            : base(a_error)
        {
            
        }
    }
}
