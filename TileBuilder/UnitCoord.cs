using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileBuilder
{
    public struct UnitCoord : IEquatable<UnitCoord>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_x">X coordinate.</param>
        /// <param name="a_y">Y coordinate.</param>
        public UnitCoord(int a_x, int a_y)
        {
            X = a_x;
            Y = a_y;
        }

        /// <summary>
        /// X coordinate.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Y coordinate.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Get coordinate one unit north of this one.
        /// </summary>
        /// <returns>North coordinate.</returns>
        public UnitCoord North()
        {
            return new UnitCoord(X, Y - 1);
        }

        /// <summary>
        /// Get coordinate one unit south of this one.
        /// </summary>
        /// <returns>North coordinate.</returns>
        public UnitCoord South()
        {
            return new UnitCoord(X, Y + 1);
        }

        /// <summary>
        /// Get coordinate one unit east of this one.
        /// </summary>
        /// <returns>North coordinate.</returns>
        public UnitCoord East()
        {
            return new UnitCoord(X + 1, Y);
        }

        /// <summary>
        /// Get coordinate one unit west of this one.
        /// </summary>
        /// <returns>North coordinate.</returns>
        public UnitCoord West()
        {
            return new UnitCoord(X - 1, Y);
        }

        /// <summary>
        /// Offset the coordinates by the given offset values (<paramref name="a_offsetX"/>, <paramref name="a_offsetY"/>).
        /// </summary>
        /// <param name="a_offsetX">Horizontal offset value.</param>
        /// <param name="a_offsetY">Vertical offset value.</param>
        /// <returns></returns>
        public UnitCoord Offset(int a_offsetX, int a_offsetY)
        {
            return new UnitCoord(X + a_offsetX, Y + a_offsetY);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false. 
        /// </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="a_other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="a_other">An object to compare with this object.</param>
        public bool Equals(UnitCoord a_other)
        {
            return X == a_other.X && Y == a_other.Y;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (X*397) ^ Y;
            }
        }
    }
}
