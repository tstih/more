/*
 * PointEx.cs
 * 
 * Extensions to the Point object.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 16.02.2020   tstih
 * 
 */
using System.Linq;
using System.Drawing;

namespace More.Windows.Forms
{
    public static class PointEx
    {
        /// <summary>
        /// Given a polygon, return bounding rectangle.
        /// </summary>
        /// <param name="points">A collection of points.</param>
        /// <returns>A rectangle representing the bounds of the polygon.</returns>
        public static Rectangle GetBoundingRect(this Point[] points)
        {
            var minX = points.Min(p => p.X);
            var minY = points.Min(p => p.Y);
            var maxX = points.Max(p => p.X);
            var maxY = points.Max(p => p.Y);

            return new Rectangle(new Point(minX, minY), new Size(maxX - minX, maxY - minY));
        }
    }
}