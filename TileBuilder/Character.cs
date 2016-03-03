using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileBuilder.Contracts.Ui;

namespace TileBuilder
{
    public class Character : ICharacter
    {
        /// <summary>
        /// Character sprite image.
        /// </summary>
        public string Sprite { get; set; }

        /// <summary>
        /// Character sprite frame.
        /// </summary>
        public int SpriteFrame { get; set; }

        /// <summary>
        /// Position of this character in the room.
        /// </summary>
        public UnitCoord Position { get; set; } = new UnitCoord();
    }
}
