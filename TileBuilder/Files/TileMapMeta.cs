namespace TileBuilder.Files
{
    public class TileMapMeta
    {
        /// <summary>
        /// File version number.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Size of map in rooms.
        /// </summary>
        public UnitSize MapSize { get; set; }

        /// <summary>
        /// Size of rooms in tiles.
        /// </summary>
        public UnitSize RoomSize { get; set; }

        /// <summary>
        /// Count of rooms in map.
        /// </summary>
        public int RoomCount { get; set; }

        /// <summary>
        /// Clone this instance.
        /// </summary>
        /// <returns>Cloned instance.</returns>
        public TileMapMeta Clone()
        {
            return (TileMapMeta)MemberwiseClone();
        }
    }
}