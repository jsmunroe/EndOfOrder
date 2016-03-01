namespace TileBuilder.Files
{
    public class RoomMeta
    {
        /// <summary>
        /// Position of the start of the room data within the file.
        /// </summary>
        public long FilePosition { get; set; }

        /// <summary>
        /// Room location in the parent map.
        /// </summary>
        public UnitCoord RoomLocation { get; set; }

        /// <summary>
        /// Room size.
        /// </summary>
        public UnitSize RoomSize { get; set; }

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