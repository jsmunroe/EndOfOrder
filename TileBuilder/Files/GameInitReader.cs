using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TileBuilder.Files
{
    public class GameInitReader
    {
        private readonly IResource _resource;

        private GameInitMeta _meta;

        private static readonly Regex s_rexHeader = new Regex(@"^Game\t(?<version>\d*)", RegexOptions.Compiled);
        private static readonly Regex s_rexStart = new Regex(@"^Start\t(?<map>[\w\.]*)\t(?<x>\d*)\t(?<y>\d*)", RegexOptions.Compiled);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_resource">Game initialization data resource.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_resource"/> is null.</exception>
        public GameInitReader(IResource a_resource)
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
        public GameInitMeta Meta
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

            using (var stream = _resource.Open())
            {
                var reader = new StreamReader(stream);

                _meta = new GameInitMeta();

                ReadHeader(reader, _meta);

                if (_meta.Version == 0)
                {
                    ReadStart_0(reader, _meta);
                }
            }
        }


        /// <summary>
        /// Read the header line from the given text reader (<paramref name="a_reader"/>) into the given meta structure (<paramref name="a_meta"/>).
        /// </summary>
        /// <param name="a_reader">Text reader.</param>
        /// <param name="a_meta">Meta data structure.</param>
        private void ReadHeader(StreamReader a_reader, GameInitMeta a_meta)
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
        /// Read the meta data from the given text reader (<paramref name="a_reader"/>) into the given meta structure (<paramref name="a_meta"/>).
        /// </summary>
        /// <param name="a_reader">Text reader.</param>
        /// <param name="a_meta">Meta data structure.</param>
        private void ReadStart_0(StreamReader a_reader, GameInitMeta a_meta)
        {
            var startLine = a_reader.ReadLine();

            if (startLine == null)
                throw new TileMapSchemaException("NoStartLine"); // Header line does not exit.

            var startMatch = s_rexStart.Match(startLine);

            if (!startMatch.Success)
                throw new TileMapSchemaException("BadStartLine"); // Badly formatted header line.

            a_meta.StartMapName = startMatch.Groups["map"].Value;
            a_meta.StartRoomX = int.Parse(startMatch.Groups["x"].Value);
            a_meta.StartRoomY = int.Parse(startMatch.Groups["y"].Value);
        }

    }

    public class GameInitMeta
    {
        /// <summary>
        /// File version number.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Starting map name.
        /// </summary>
        public string StartMapName { get; set; }

        /// <summary>
        /// X coordinate of the starting room in the starting map.
        /// </summary>
        public int StartRoomX { get; set; }

        /// <summary>
        /// Y coordinate of the starting room in the starting map.
        /// </summary>
        public int StartRoomY { get; set; }

        /// <summary>
        /// Clone this instance.
        /// </summary>
        /// <returns>Clone.</returns>
        public GameInitMeta Clone()
        {
            return (GameInitMeta) MemberwiseClone();
        }
    }
}
