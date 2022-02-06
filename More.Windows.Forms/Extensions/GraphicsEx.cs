/*
 * GraphicsEx.cs
 * 
 * Extensions to the graphics object.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * THANK YOU:
 *  https://stackoverflow.com/questions/33853434/how-to-draw-a-rounded-rectangle-in-c-sharp
 * 
 * 15.02.2020   tstih
 * 
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace More.Windows.Forms
{
    public enum Direction { Left2Right, Top2Bottom, Right2Left, Bottom2Top }
    
    [Flags]
    public enum Borders { 
        None    = 0x00, 
        Top     = 0x01, 
        Bottom  = 0x02, 
        Right   = 0x04, 
        Left    = 0x08,
        All     = Top + Bottom + Right + Left};

    public static class GraphicsEx
    {
        #region Extension Method(s)
        public static void DrawBorder(
           this Graphics graphics,
           Color dkColor,
           Color ltColor,
           Rectangle bounds,
           int thickness,
           Borders sides=Borders.All)
        {
            // Can't draw border without thickness.
            if (thickness < 1) return;

            using (Pen ltPen = new Pen(ltColor), dkPen = new Pen(dkColor))
                for (int i = 0; i < thickness; i++)
                {
                    // Light (top, left).
                    if (sides.HasFlag(Borders.Top)) graphics.DrawLine(ltPen, bounds.Left, bounds.Top+i, bounds.Right - i, bounds.Top+i);
                    if (sides.HasFlag(Borders.Left)) graphics.DrawLine(ltPen, bounds.Left+i, bounds.Top, bounds.Left+i, bounds.Bottom - i);
                    // Dark (bottom, right) - this is the dominating color.
                    if (sides.HasFlag(Borders.Bottom)) graphics.DrawLine(dkPen, bounds.Left+i, bounds.Bottom-i, bounds.Right, bounds.Bottom - i);
                    if (sides.HasFlag(Borders.Right)) graphics.DrawLine(dkPen, bounds.Right-i, bounds.Top+i, bounds.Right-i, bounds.Bottom);
                }
        }

        /// <summary>
        /// Draw grip on canvas.
        /// </summary>
        /// <param name="dkColor">Grip dark color</param>
        /// <param name="ltColor">Grip light color</param>
        /// <param name="bounds">Rectangle</param>
        public static void DrawGrip(
            this Graphics graphics,
            Color dkColor,
            Color ltColor,
            Rectangle bounds)
        {
            using (HatchBrush dkBrush=new HatchBrush(HatchStyle.Percent20, dkColor, Color.Transparent),
                ltBrush = new HatchBrush(HatchStyle.Percent20, ltColor, Color.Transparent))
            {
                graphics.FillRectangle(dkBrush, bounds);
                graphics.RenderingOrigin = new Point(-1, -1);
                graphics.FillRectangle(ltBrush, bounds);
            }
        }

        /// <summary>
        /// Draw rounded rectangle.
        /// </summary>
        /// <param name="pen">Border pen</param>
        /// <param name="bounds">Rectangle bounds</param>
        /// <param name="lt_radius">Left top radius</param>
        /// <param name="rt_radius">Right top radius</param>
        /// <param name="lb_radius">Left bottom radius</param>
        /// <param name="rb_radius">Right bottom radius</param>
        public static void DrawRectangle(
            this Graphics graphics, 
            Pen pen, 
            Rectangle bounds,
            int lt_radius,
            int rt_radius,
            int lb_radius,
            int rb_radius)
        {
            using (GraphicsPath path = RoundedRect(bounds, lt_radius, rt_radius, lb_radius, rb_radius))
                graphics.DrawPath(pen, path);
        }

        /// <summary>
        /// Fill rounded rectangle.
        /// </summary>
        /// <param name="brush">Fill brush</param>
        /// <param name="bounds">Rectangle bounds</param>
        /// <param name="lt_radius">Left top radius</param>
        /// <param name="rt_radius">Right top radius</param>
        /// <param name="lb_radius">Left bottom radius</param>
        /// <param name="rb_radius">Right bottom radius</param>
        public static void FillRectangle(
            this Graphics graphics, 
            Brush brush, 
            Rectangle bounds, 
            int lt_radius,
            int rt_radius,
            int lb_radius,
            int rb_radius)
        {
            using (GraphicsPath path = RoundedRect(bounds, lt_radius, rt_radius, lb_radius, rb_radius))
                graphics.FillPath(brush, path);
        }

        /// <summary>
        /// Draw triangle inside rect.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="pen">Line pen</param>
        /// <param name="bounds">Rectangle bounds</param>
        /// <param name="direction">Rectangle direction, from wide to narrow i.e. Rectangle in the shape of V would be 'Top'.</param>
        public static void DrawTriangle(this Graphics graphics, Pen pen, Rectangle bounds, Direction direction)
        {
            using (GraphicsPath path = Triangle(bounds, direction))
                graphics.DrawPath(pen, path);
        }

        /// <summary>
        /// Fill triangle inside rect.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="brush">Fill brush</param>
        /// <param name="bounds">Rectangle bounds</param>
        /// <param name="direction">Rectangle direction, from wide to narrow i.e. Rectangle in the shape of V would be 'Top'.</param>
        public static void FillTriangle(this Graphics graphics, Brush brush, Rectangle bounds, Direction direction)
        {
            using (GraphicsPath path = Triangle(bounds, direction))
                graphics.FillPath(brush, path);
        }

        /// <summary>
        /// Draw string that glows.
        /// Trick by Bob Powell (Text Halo).
        /// TODO: Does not work!
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="text">Text to draw</param>
        /// <param name="font">Font</param>
        /// <param name="brush">Text brush</param>
        /// <param name="glowColor">Color used for glow</param>
        /// <param name="rect">Target rectangle</param>
        /// <param name="stringFormat">Text formatting</param>
        public static void DrawGlowString(
            this Graphics graphics, 
            string text,
            Font font,
            Brush brush,
            Color glowColor,
            Rectangle rect,
            StringFormat stringFormat)
        {
            const float div = 0.9f;
            //Create a bitmap in a fixed ratio to the original drawing area.
            using (Bitmap bmp = new Bitmap(
                (int)Math.Round((float)rect.Width / div),
                (int)Math.Round((float)rect.Height / div)))
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddString(text,
                    font.FontFamily,
                    (int)font.Style,
                    font.Size,
                    rect,
                    stringFormat);
                using (Graphics gimg = Graphics.FromImage(bmp))
                using (Pen gpen = new Pen(glowColor))
                using (Brush gbrush = new SolidBrush(glowColor))
                {
                    Matrix mx = new Matrix(1.0f / div, 0, 0, 1.0f / div, -(1.0f / div), -(1.0f / div));
                    gimg.SmoothingMode = SmoothingMode.AntiAlias;
                    gimg.Transform = mx;
                    gimg.DrawPath(gpen, path);
                    gimg.FillPath(gbrush, path);
                }
                graphics.Transform = new Matrix(1, 0, 0, 1, 10 * div, 10 * div);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(bmp, rect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel);
                graphics.FillPath(brush, path);
            }
        }
        #endregion // Extension Method(s)

        #region Static Helper(s)
        private static GraphicsPath RoundedRect(
            Rectangle bounds,
            int lt_radius,
            int rt_radius,
            int lb_radius,
            int rb_radius
            )
        {
            GraphicsPath path = new GraphicsPath();

            // top left
            if (lt_radius == 0)
                path.AddLines(new Point[] {
                    new Point(bounds.Left,bounds.Top),
                    new Point(bounds.Left+lt_radius*2,bounds.Top),
                    new Point(bounds.Left,bounds.Top+lt_radius*2)
                });
            else path.AddArc(new Rectangle(bounds.Location, new Size(lt_radius * 2, lt_radius * 2)), 180, 90);

            // top right
            if (rt_radius == 0)
                path.AddLines(new Point[] {
                    new Point(bounds.Right,bounds.Top),
                    new Point(bounds.Right-rt_radius*2,bounds.Top),
                    new Point(bounds.Right,bounds.Top+rt_radius*2)
                });
            else path.AddArc(new Rectangle(new Point(bounds.Right-rt_radius*2,bounds.Top), new Size(rt_radius * 2, rt_radius * 2)), 270, 90);

            // bottom right
            if (rb_radius == 0)
                path.AddLines(new Point[] {
                    new Point(bounds.Right,bounds.Bottom),
                    new Point(bounds.Right-rb_radius*2,bounds.Bottom),
                    new Point(bounds.Right,bounds.Bottom - rb_radius*2)
                });
            else path.AddArc(new Rectangle(new Point(bounds.Right - rb_radius * 2, bounds.Bottom - rb_radius * 2), new Size(rb_radius * 2, rb_radius * 2)), 0, 90);

            // bottom left 
            if (lb_radius == 0)
                path.AddLines(new Point[] {
                    new Point(bounds.Left,bounds.Bottom),
                    new Point(bounds.Left+lb_radius*2,bounds.Bottom),
                    new Point(bounds.Left,bounds.Bottom - lb_radius*2)
                });
            else path.AddArc(new Rectangle(new Point(bounds.Left, bounds.Bottom - lb_radius * 2), new Size(lb_radius * 2, lb_radius * 2)), 90, 90);

            path.CloseFigure();
            return path;
        }

        private static GraphicsPath Triangle(Rectangle bounds, Direction direction)
        {
            GraphicsPath path = new GraphicsPath();
            int midx = bounds.Width / 2 + bounds.Left;
            int midy = bounds.Height / 2 + bounds.Top;
            switch (direction)
            {
                case Direction.Bottom2Top:
                    path.AddLine(bounds.Left, bounds.Bottom, bounds.Right, bounds.Bottom);
                    path.AddLine(bounds.Right, bounds.Bottom, midx, bounds.Top);
                    break;
                case Direction.Top2Bottom:
                    path.AddLine(bounds.Left, bounds.Top, bounds.Right, bounds.Top);
                    path.AddLine(bounds.Right, bounds.Top, midx, bounds.Bottom);
                    break;
                case Direction.Left2Right:
                    path.AddLine(bounds.Left, bounds.Top, bounds.Left, bounds.Bottom);
                    path.AddLine(bounds.Left, bounds.Bottom, bounds.Right, midy);
                    break;
                case Direction.Right2Left:
                    path.AddLine(bounds.Right, bounds.Top, bounds.Right, bounds.Bottom);
                    path.AddLine(bounds.Right, bounds.Bottom, bounds.Left, midy);
                    break;
            }
            path.CloseFigure();
            return path;
        }
        #endregion // Static Helper(s)
    }
}