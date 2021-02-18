/*
 * LabelEx.cs
 * 
 * Label with opacity that can be rotated.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 28.12.2020   tstih
 * 
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace More.Windows.Forms
{
    public class LabelEx : Control
    {
        #region Const(s)
        private const float DEFAULT_ANGLE = 0.0f;
        private const int DEFAULT_OPACITY = 0;
        private const int DEFAULT_WIDTH = 96;
        private const int DEFAULT_HEIGHT = 26;
        private const StringAlignment DEFAULT_HORZ_ALIGNMENT = StringAlignment.Center;
        private const StringAlignment DEFAULT_VERT_ALIGNMENT = StringAlignment.Far;
        #endregion // Const(s)

        #region Private(s)
        private Size _naturalSize;
        #endregion // Private(s)

        #region Ctor
        public LabelEx() {

            // Control styles. Control double buffering not allowed.
            SetStyle(
                ControlStyles.Opaque
                | ControlStyles.ResizeRedraw, true); 

            // Defaults.
            SetDefaults();
        }

        private void SetDefaults()
        {
            // Basic 
            _angle = DEFAULT_ANGLE;
            _opacity = DEFAULT_OPACITY;
            _naturalSize = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
            _horzAlignment = DEFAULT_HORZ_ALIGNMENT;
            _vertAlignment = DEFAULT_VERT_ALIGNMENT;
        }
        #endregion // Ctor

        #region Override(s)
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | WS_EX_TRANSPARENT;
                return cp;
            }
        }

        protected override void OnTextChanged(EventArgs e) { Invalidate(); }

        protected override void OnPaint(PaintEventArgs e)
        {
            // NOTE: Clear will not work here, only FillRectangle!
            using (var brush = new SolidBrush(Color.FromArgb((100-Opacity) * 255 / 100, BackColor)))
            { e.Graphics.FillRectangle(brush, this.ClientRectangle); }

            // First measure the title.
            Size titleSize=Size.Round(e.Graphics.MeasureString(Text, Font));

            // Now calculate how much space do we need (just for title).
            Size rotatedTitleSize = RotatedSize(titleSize, Angle);

            // Autosize.
            if (_naturalSize != rotatedTitleSize)
                _naturalSize = rotatedTitleSize;

            // Create the real rotation transformation.
            Matrix mrot = new Matrix();

            // 1. Handle alignment (first!)
            int xoffs, yoffs;
            // We might need this...
            Rectangle centerRectangle = ClientRectangle.Center(rotatedTitleSize, Padding.Empty);
            // Vertical alignment...
            switch(VertAlignment)
            {
                case StringAlignment.Center:
                    yoffs = centerRectangle.Top;
                    break;
                case StringAlignment.Far:
                    yoffs = ClientRectangle.Bottom - rotatedTitleSize.Height;
                    break;
                default:
                    yoffs = 0;
                    break;
            }
            // ...and horizontal alignment.
            switch(HorzAlignment)
            {
                case StringAlignment.Center:
                    xoffs = centerRectangle.Left;
                    break;
                case StringAlignment.Far:
                    xoffs = ClientRectangle.Right- rotatedTitleSize.Width;
                    break;
                default:
                    xoffs = 0;
                    break;
            }
            // And set the matrix transform.
            mrot.Translate(xoffs, yoffs);

            // 2. Handle rotation.
            mrot.RotateAt(Angle,
                new PointF(rotatedTitleSize.Width / 2f, rotatedTitleSize.Height / 2f));
            
            e.Graphics.Transform = mrot;

            // Draw the title!
            int x = (rotatedTitleSize.Width - titleSize.Width) / 2;
            int y = (rotatedTitleSize.Height - titleSize.Height) / 2;

            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            using (Brush foreBrush = new SolidBrush(ForeColor))
                e.Graphics.DrawString(Text, Font, foreBrush, x, y);
        }

        #endregion // Override(s)

        #region Properties
        private float _angle;
        /// <summary>
        /// Angle (in degrees).
        /// </summary>
        [Description("Angle in degrees"), Category("Layout")]
        public float Angle { get { return _angle; } set { _angle = value; Invalidate(); } }

        private int _opacity;
        /// <summary>
        /// Background opacity from 0 - 100.
        /// </summary>
        [Description("Background opacity from 0-100"), Category("Appearance")]
        public int Opacity
        {
            get
            {
                if (_opacity < 0) _opacity = 0;
                else if (_opacity > 100) _opacity = 100;
                return this._opacity;
            }
            set
            {
                _opacity = value; Invalidate();
            }
        }

        private StringAlignment _horzAlignment;
        /// <summary>
        /// Text horizontal alignment
        /// </summary>
        [Description("Text horizontal alignment"), Category("Layout")]
        public StringAlignment HorzAlignment 
        { get { return _horzAlignment; } set { _horzAlignment = value; Invalidate(); } }

        private StringAlignment _vertAlignment;
        /// <summary>
        /// Text vertical alignment
        /// </summary>
        [Description("Text vertical alignment"), Category("Layout")]
        public StringAlignment VertAlignment
        { get { return _vertAlignment; } set { _vertAlignment = value; Invalidate(); } }

        /// <summary>
        /// Rotated text actual size.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Bindable(false)]
        [Browsable(false)]
        public Size NaturalSize { get { return _naturalSize; } }
        #endregion // Properties

        #region Helper(s)
        private Size RotatedSize(Size size, float angle)
        {
            // Make a Matrix to represent rotation
            // by this angle.
            Matrix mrot = new Matrix();
            mrot.Rotate(angle);

            // Rotate the image's corners to see how big
            // it will be after rotation.
            PointF[] points =
            {
                new PointF(0, 0),
                new PointF(size.Width, 0),
                new PointF(size.Width, size.Height),
                new PointF(0, size.Height),
            };
            mrot.TransformPoints(points);
            float xmin = float.MaxValue, xmax = float.MinValue, ymin = float.MaxValue, ymax = float.MinValue;
            foreach (PointF p in points)
            {
                if (p.X < xmin) xmin = p.X;
                if (p.X > xmax) xmax = p.X;
                if (p.Y < ymin) ymin = p.Y;
                if (p.Y > ymax) ymax = p.Y;
            }

            // Make a bitmap to hold the rotated result, don't +1!
            return Size.Round(
                new SizeF(xmax - xmin, ymax - ymin)
            );
        }
        #endregion // Helper(s)

        #region Win32
        private const int WS_EX_TRANSPARENT = 0x20;
        #endregion // Win32
    }
}