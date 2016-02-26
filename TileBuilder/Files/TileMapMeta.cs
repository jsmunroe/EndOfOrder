namespace TileBuilder.Files
{
    public class TileMapMeta
    {
        /// <summary>
        /// File version number.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Width of the map in rooms.
        /// </summary>
        public int MapWidth { get; set; }

        /// <summary>
        /// Width of the map in rooms.
        /// </summary>
        public int MapHeight { get; set; }

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