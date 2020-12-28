/*
 * BitmapEx.cs
 * 
 * Extensions to the Bitmap object.
 * 
 * NOTE:
 *  Function to rotate the image from -
 *  http://csharphelper.com/blog/2016/03/rotate-images-in-c/
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 28.12.2020   tstih
 * 
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace More.Windows.Forms
{
    public static class BitmapEx
    {
        /// <summary>
        /// Rotates a bitmap. Creates an image of different size of not square.
        /// </summary>
        /// <param name="angle">Angle (in degrees) to rotate.</param>
        /// <param name="fillBackColor">Because it resizes an image, this is the fill color for new areas</param>
        /// <returns></returns>
        public static Bitmap Rotate(this Bitmap bm, float angle, Color fillBackColor)
        {
            // Make a Matrix to represent rotation
            // by this angle.
            Matrix rotate_at_origin = new Matrix();
            rotate_at_origin.Rotate(angle);

            // Rotate the image's corners to see how big
            // it will be after rotation.
            float xmin=0, xmax=bm.Width, ymin=0, ymax=bm.Height;
            PointF[] points =
            {
                new PointF(xmin, ymin),
                new PointF(xmax, ymin),
                new PointF(xmax, ymax),
                new PointF(xmin, ymax),
            };
            rotate_at_origin.TransformPoints(points);
            foreach (PointF p in points) {
                if (p.X < xmin) xmin = p.X;
                if (p.X > xmax) xmax = p.X;
                if (p.Y < ymin) ymin = p.Y;
                if (p.Y > ymax) ymax = p.Y;
            }

            // Make a bitmap to hold the rotated result.
            int wid = (int)Math.Round(xmax - xmin);
            int hgt = (int)Math.Round(ymax - ymin);
            Bitmap result = new Bitmap(wid, hgt);

            // Create the real rotation transformation.
            Matrix rotate_at_center = new Matrix();
            rotate_at_center.RotateAt(angle,
                new PointF(wid / 2f, hgt / 2f));

            // Draw the image onto the new bitmap rotated.
            using (Graphics gr = Graphics.FromImage(result))
            {
                // Use smooth image interpolation.
                gr.InterpolationMode = InterpolationMode.High;

                // Clear with the color in the image's upper left corner.
                gr.Clear(fillBackColor);

                // Set up the transformation to rotate.
                gr.Transform = rotate_at_center;

                // Draw the image centered on the bitmap.
                int x = (wid - bm.Width) / 2;
                int y = (hgt - bm.Height) / 2;
                gr.DrawImage(bm, x, y);
            }

            // Return the result bitmap.
            return result;
        }
    }
}
