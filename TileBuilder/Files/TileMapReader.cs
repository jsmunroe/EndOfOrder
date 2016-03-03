using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Helpers.IO;
using TileBuilder.Contracts;

namespace TileBuilder.Files
{
    public class TileMapReader
    {
        private readonly IResource _resource;

        private TileMapMeta _meta;

        private TilesetMeta _tilesetMeta;

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

            var roomMeta = _roomMetas.FirstOrDefault(i => i.RoomLocation.X == x  && i.RoomLocation.Y == y);

            if (roomMeta == null)
                return new Room(new UnitCoord(x, y), Meta.RoomSize);

            using (var stream = _resource.Open())
            {
                var reader = new AdvancedStreamReader(stream);

                var room = ReadRoom_0(reader, roomMeta);
                return room;
            }
        }

        /// <summary>
        /// Read teh tileset.
        /// </summary>
        /// <returns></returns>
        public ITileset ReadTileset()
        {
            if (_meta == null)
                ReadMeta();

            if (_tilesetMeta == null)
                return new Tileset();

            using (var stream = _resource.Open())
            {
                var reader = new AdvancedStreamReader(stream);

                var tileset = ReadTileset_0(reader, _tilesetMeta);
                return tileset;
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
        private UnitCoord ReadNextCoord(AdvancedStreamReader a_reader, string a_lineName)
        {
            var mapSizeLine = a_reader.ReadLine();

            if (mapSizeLine == null)
                throw new TileMapSchemaException("No" + a_lineName); // Map size line does not exist.

            var mapSizeMatch = s_rexOrtho.Match(mapSizeLine);

            if (!mapSizeMatch.Success)
                throw new TileMapSchemaException("Bad" + a_lineName); // Badly formatted map size line.

            return new UnitCoord(
                int.Parse(mapSizeMatch.Groups["x"].Value),
                int.Parse(mapSizeMatch.Groups["y"].Value));

        }

        /// <summary>
        /// Read the next line and convert it to a coordinate if possible (<paramref name="a_reader"/>)
        /// </summary>
        /// <param name="a_reader">Text reader.</param>
        /// <param name="a_lineName">Line name to show if an exception is encountered.</param>
        /// <returns>Integer coordinate array of size 2.</returns>
        private UnitSize ReadNextSize(AdvancedStreamReader a_reader, string a_lineName)
        {
            var mapSizeLine = a_reader.ReadLine();

            if (mapSizeLine == null)
                throw new TileMapSchemaException("No" + a_lineName); // Map size line does not exist.

            var mapSizeMatch = s_rexOrtho.Match(mapSizeLine);

            if (!mapSizeMatch.Success)
                throw new TileMapSchemaException("Bad" + a_lineName); // Badly formatted map size line.

            return new UnitSize(
                int.Parse(mapSizeMatch.Groups["x"].Value),
                int.Parse(mapSizeMatch.Groups["y"].Value));

        }

        /// <summary>
        /// Read the meta data from the given text reader (<paramref name="a_reader"/>) into the given meta structure (<paramref name="a_meta"/>).
        /// </summary>
        /// <param name="a_reader">Text reader.</param>
        /// <param name="a_meta">Meta data structure.</param>
        private void ReadMeta_0(AdvancedStreamReader a_reader, TileMapMeta a_meta)
        {
            a_meta.MapSize = ReadNextSize(a_reader, "MapSizeLine");
            a_meta.RoomSize = ReadNextSize(a_reader, "RoomSizeLine");
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
                if (line == "*Tileset")
                    ReadTilesetMeta_0(a_reader, pos);
                else if (line == "*Room")
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

            var roomMeta = new RoomMeta
            {
                FilePosition = a_pos,
                RoomLocation = coords,
                RoomSize = Meta.RoomSize,
            };

            _roomMetas.Add(roomMeta);

            FeedToLabel(a_reader, "/Room");
        }

        /// <summary>
        /// Read the tileset meta data from the given text reader (<paramref name="a_reader"/>).
        /// </summary>
        /// <param name="a_reader">Text reader.</param>
        /// <param name="a_pos">File position.</param>
        private void ReadTilesetMeta_0(AdvancedStreamReader a_reader, long a_pos)
        {
            var tilesetMeta = new TilesetMeta
            {
                FilePosition = a_pos,
            };

            _tilesetMeta = tilesetMeta;

            FeedToLabel(a_reader, "/Tileset");
        }

        /// <summary>
        /// Feed to the given label (<paramref name="a_label"/>).
        /// </summary>
        /// <param name="a_reader">Text reader.</param>
        /// <param name="a_label">Label.</param>
        private static void FeedToLabel(AdvancedStreamReader a_reader, string a_label)
        {
            var line = a_reader.ReadLine();
            while (line != null && !line.StartsWith(a_label))
                line = a_reader.ReadLine();
        }

        /// <summary>
        /// Read the room from the given text reader (<paramref name="a_reader"/>).
        /// </summary>
        /// <param name="a_reader">Text reader.</param>
        /// <param name="a_meta">Room meta data.</param>
        /// <returns>Room that was read.</returns>
        private IRoom ReadRoom_0(AdvancedStreamReader a_reader, RoomMeta a_meta)
        {
            a_reader.SeekCharacter(a_meta.FilePosition);

            // Read past meta data
            var header = a_reader.ReadLine();
            var roomCoord = a_reader.ReadLine();

            var room = new Room(a_meta);
            var tileset = ReadTileset();

            for (var y = 0; y < Meta.RoomSize.Height; y++)
            {
                var tileLine = a_reader.ReadLine();

                if (tileLine == null)
                    return new Room(a_meta);

                var tiles = tileLine.Split('\t');

                for (var x = 0; x < tiles.Length; x++)
                {
                    int tileID;
                    if (!int.TryParse(tiles[x], out tileID))
                        continue;

                    var tile = tileset.GetTile(tileID);

                    room.SetTile(x, y, tile);
                }
            }

            return room;
        }

        /// <summary>
        /// Read the tileset from the given text reader (<paramref name="a_reader"/>).
        /// </summary>
        /// <param name="a_reader">Text reader.</param>
        /// <param name="a_meta">Tileset meta data.</param>
        /// <returns>Tileset that was read.</returns>
        private ITileset ReadTileset_0(AdvancedStreamReader a_reader, TilesetMeta a_meta)
        {
            a_reader.SeekCharacter(a_meta.FilePosition);

            // read past meta data.
            var header = a_reader.ReadLine();

            var regTileRef = new Regex(@"^(?<id>\d*)\t(?<name>[\w\.]*)\t(?<passible>[10])", RegexOptions.Compiled);

            var tileset = new Tileset();
            var line = a_reader.ReadLine();
            while (line != null && line != "/Tileset")
            {
                var match = regTileRef.Match(line);

                if (match.Success)
                {
                    var id = int.Parse(match.Groups["id"].Value);
                    var name = match.Groups["name"].Value;
                    var passible = match.Groups["passible"].Value == "1";

                    var tile = new Tile
                    {
                        ID = id,
                        Background = name,
                        IsPassible = passible,
                    };

                    tileset.SetTile(tile);
                }

                line = a_reader.ReadLine();
            }

            return tileset;
        }
    }

    public class TileMapSchemaException : Exception
    {
        public TileMapSchemaException(string a_error) 
            : base(a_error)
        {
            
        }
    }
}
