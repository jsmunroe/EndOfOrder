namespace TileBuilder.Files
{
    public class RoomMeta
    {
        /// <summary>
        /// Position of the start of the room data within the file.
        /// </summary>
        public long FilePosition { get; set; }

        /// <summary>
        /// Horizontal coordinate of the room in the parent map.
        /// </summary>
        public int RoomX { get; set; }

        /// <summary>
        /// Vertical coordinate of the room in the parent map.
        /// </summary>
        public int RoomY { get; set; }

        /// <summary>
        /// Width of the room in tiles.
        /// </summary>
        public int RoomWidth { get; set; }

        /// <summary>
        /// Height of the room in tiles.
        /// </summary>
        public int RoomHeight { get; set; }


        /// <summary>
        /// Clone this instance.
        /// </summary>
        /// <returns>Cloned instance.</returns>
        public RoomMeta Clone()
        {
            return (RoomMeta)MemberwiseClone();
        }
    }
}