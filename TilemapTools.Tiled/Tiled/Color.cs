using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TilemapTools.Tiled
{
    /// <summary>
    /// Represents a 32-bit color (4 bytes) in the form of ARGB (in byte order: A, R, G, B).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public struct Color : IEquatable<Color>
    {
        /// <summary>
        /// The alpha component of the color.
        /// </summary>
        public byte A;

        /// <summary>
        /// The red component of the color.
        /// </summary>
        public byte R;

        /// <summary>
        /// The green component of the color.
        /// </summary>
        public byte G;

        /// <summary>
        /// The blue component of the color.
        /// </summary>
        public byte B;

        public static readonly Color Empty = new Color();

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.  Alpha is set to 255.
        /// </summary>
        /// <param name="red">The red component of the color.</param>
        /// <param name="green">The green component of the color.</param>
        /// <param name="blue">The blue component of the color.</param>
        public Color(byte red, byte green, byte blue)
        {
            A = 255;
            R = red;
            G = green;
            B = blue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.  Alpha is set to 255.
        /// </summary>
        /// <param name="red">The red component of the color.</param>
        /// <param name="green">The green component of the color.</param>
        /// <param name="blue">The blue component of the color.</param>
        public Color(byte alpha, byte red, byte green, byte blue)
        {
            A = alpha;
            R = red;
            G = green;
            B = blue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.  Alpha is set to 255.  Passed values are clamped within byte range.
        /// </summary>
        /// <param name="red">The red component of the color.</param>
        /// <param name="green">The green component of the color.</param>
        /// <param name="blue">The blue component of the color.</param>
        public Color(int red, int green, int blue)
            : this(255, red, green, blue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.  Passed values are clamped within byte range.
        /// </summary>
        /// <param name="alpha">The alpha component of the color.</param>
        /// <param name="red">The red component of the color.</param>
        /// <param name="green">The green component of the color.</param>
        /// <param name="blue">The blue component of the color.</param>
        public Color(int alpha, int red, int green, int blue)
        {
            A = ToByte(alpha);
            R = ToByte(red);
            G = ToByte(green);
            B = ToByte(blue);
        }

        /// <summary>
        /// Converts the color into a packed integer.
        /// </summary>
        /// <returns>A packed integer containing all four color components.</returns>
        public int ToArgb()
        {
            int value = A;
            value |= R << 8;
            value |= G << 16;
            value |= B << 24;

            return (int)value;
        }

        /// <summary>
        /// Converts <see cref="string"/> to a <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The supported color formats are #AARRBBGG and #RRGGBB.</param>
        /// <returns>The converted <see cref="Color"/> or null.</returns>
        public static Color? FromString(string color)
        {
            if (!string.IsNullOrEmpty(color) && color[0] == '#')
            {
                if (color.Length == 7)
                {
                    // #RRGGBB
                    return new Color(Convert.ToInt32(color.Substring(1, 2), 16),
                                       Convert.ToInt32(color.Substring(3, 2), 16),
                                       Convert.ToInt32(color.Substring(5, 2), 16));
                }
                else if (color.Length == 9)
                {
                    // #AARRBBGG
                    return new Color(Convert.ToInt32(color.Substring(1, 2), 16),
                        Convert.ToInt32(color.Substring(3, 2), 16),
                        Convert.ToInt32(color.Substring(5, 2), 16),
                        Convert.ToInt32(color.Substring(7, 2), 16));
                }
            }

            return null;
        }

        public static byte ToByte(int value)
        {
            return (byte)(value < 0 ? 0 : value > 255 ? 255 : value);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return this.ToArgb();
        }

        /// <summary>
        /// Determines whether the specified <see cref="Color"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Color"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Color"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ref Color other)
        {
            return R == other.R && G == other.G && B == other.B && A == other.A;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Color"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Color"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Color"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Color other)
        {
            return Equals(ref other);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="value">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object value)
        {
            if (!(value is Color))
                return false;

            var strongValue = (Color)value;
            return Equals(ref strongValue);
        }
    }
}