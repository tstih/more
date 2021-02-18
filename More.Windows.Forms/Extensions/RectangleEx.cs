/*
 * RectangleEx.cs
 * 
 * Extensions to the rectangle object.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 16.02.2020   tstih
 * 
 */
using System.Drawing;
using System.Windows.Forms;

namespace More.Windows.Forms
{
    public static class RectangleEx
    {
        /// <summary>
        /// Centers object of size into rectangle (respecting margin).
        /// </summary>
        /// <param name="outer">Outer rectangle</param>
        /// <param name="size">Object's size</param>
        /// <param name="margin">Outer rectangle's margin, use Padding.Empty for no margin</param>
        /// <returns></returns>
        public static Rectangle Center(this Rectangle outer, Size size, Padding margin)
        {
            int x, y;
            x = (outer.Width - margin.Left - margin.Right) / 2 // Width with margin divided by 2
                -size.Width / 2  // Inner width divided by 2.
                +outer.Left + margin.Left; // Finally add margin.
            y = (outer.Height - margin.Top - margin.Bottom) / 2 // Height with margin divided by 2
                -size.Height / 2 + // Inner height divided by 2.
                +outer.Top + margin.Top; // Finally add margin.
            return new Rectangle(new Point(x, y), size);
        }

        /// <summary>
        /// Calculate size to fit factor with which you have to multiply
        /// inner rect width and height to fit into outer rectangle.
        /// NOTE: Inner rectangle does not have to be smaller or larger then
        /// outer rect.
        /// </summary>
        /// <param name="inner">Inner rectangle</param>
        /// <param name="outer">Outer rectangle</param>
        /// <returns>Factor to multiply inner rect width and height with to fit into outer rect</returns>
        public static float SizeToFitFactor(this Rectangle inner, Rectangle outer)
        {
            float factor = (float)outer.Height / (float)inner.Height;
            if ((float)inner.Width * factor > outer.Width) // Switch!  
                factor = (float)outer.Width / (float)inner.Width;
            return factor;
        }

        /// <summary>
        /// Reduce rectangle size. Mostly used to draw client rectangles.
        /// </summary>
        /// <param name="r">Rectangle to reduce</param>
        /// <param name="dw">Delta width, default=1</param>
        /// <param name="dh">Delta height, default=1</param>
        /// <returns></returns>
        public static Rectangle Reduce(this Rectangle r, int dw=1, int dh=1)
        {
            return new Rectangle(r.Left, r.Top, r.Width - dw, r.Height - dh);
        }
    }
}