/*
 * Line.cs
 * 
 * Simple patterned line control.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 03.02.2020   tstih
 * 
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace More.Windows.Forms
{
    public class Line : Control
    {
        #region Const(s)
        private const int DEFAULT_LINE_WIDTH = 96;
        private const int DEFAULT_LINE_HEIGHT = 26;
        private const int DEFAULT_LINE_THICKNESS = 1;
        private const StringAlignment DEFAULT_TEXT_ALIGNMENT = StringAlignment.Near;
        private const int DEFAULT_TEXT_OFFSET = 8;
        private static readonly Color DEFAULT_LINE_COLOR = Color.FromKnownColor(KnownColor.WindowText);
        private static readonly float[] DEFAULT_DASH_PATTERN = new float[] { 1, 0 };
        #endregion Const(s)

        #region Private(s)

        // Region properties.
        private Orientation _orientation;
        private int _thickness;
        private float[] _dashValues;
        private StringAlignment _textAlignment;
        private int _textOffset;
        private Color _lineColor;

        #endregion // Private(s)

        #region Ctor(s)
        public Line()
        {
            // Control details.
            DoubleBuffered = true;
            ResizeRedraw = true;

            // Var. props.
            _orientation = Orientation.Horizontal;
            _thickness = DEFAULT_LINE_THICKNESS;
            _dashValues = DEFAULT_DASH_PATTERN;
            _textAlignment = DEFAULT_TEXT_ALIGNMENT;
            _textOffset = DEFAULT_TEXT_OFFSET;
            _lineColor = DEFAULT_LINE_COLOR;

            // Set size.
            Size = new Size(DEFAULT_LINE_WIDTH, DEFAULT_LINE_HEIGHT);

            // No tab stop.
            TabStop = false;
        }
        #endregion // Ctor(s)

        #region Override(s)
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(BackColor);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using (Pen linePen = new Pen(LineColor, Thickness))
            {
                // Try dash pattern. But don't die if invalid.
                try { linePen.DashPattern = _dashValues; }
                catch { _dashValues = DEFAULT_DASH_PATTERN; }

                // Draw line.
                if (Orientation == Orientation.Horizontal)
                    g.DrawLine(linePen, 0, Height / 2, Width, Height / 2);
                else
                    g.DrawLine(linePen, Width / 2, 0, Width / 2, Height);

                // And text.
                if (!string.IsNullOrEmpty(Text))
                {
                    // Create text bitmap.
                    Bitmap textBmp = TextToBitmap(g);

                    // Do we need to rotate it?
                    if (Orientation == Orientation.Vertical)
                    {
                        g.TranslateTransform(0, Height); // Rotate around center.
                        g.RotateTransform(-90); // 90 degrees.
                    }

                    // Draw it.
                    g.DrawImage(textBmp, 0, 0);

                    // Get rid of the bitmap.
                    textBmp.Dispose();

                    // And reset transforms.
                    g.ResetTransform();
                }
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            // Proceed...
            base.OnTextChanged(e);
            // ...and redraw.
            Invalidate();
        }
        #endregion // Override(s)

        #region Properties
        [Description("Line orientation: horizontal or vertical"), Category("Layout")]
        public Orientation Orientation
        {
            get { return _orientation; }
            set
            {
                if (value == _orientation) return;
                _orientation = value; Invalidate();
            }
        }

        [Description("Line thickness in pixels"), Category("Appearance")]
        public int Thickness
        {
            get { return _thickness; }
            set
            {
                if (value == _thickness) return;
                if (value < 1) _thickness = 1; else _thickness = value; // Override invalid.
                Invalidate();
            }
        }

        [Description("Line dash style"), Category("Appearance")]
        public float[] DashValues
        {
            get { return _dashValues; }
            set
            {
                if (value == _dashValues) return;
                if (value == null || value.Length < 1) return;
                else
                {
                    _dashValues = value;
                    Invalidate();
                }
            }
        }

        [Description("Text alignment"), Category("Appearance")]
        public StringAlignment TextAlignment
        {
            get { return _textAlignment; }
            set
            {
                if (value == _textAlignment) return;
                _textAlignment = value;
                Invalidate();
            }
        }

        [Description("If text alignment is far or near, this is the text margin from start or end of line"), Category("Appearance")]
        public int TextOffset
        {
            get { return _textOffset; }
            set
            {
                if (value == _textOffset) return;
                _textOffset = value;
                Invalidate();
            }
        }

        [Description("Line color"), Category("Appearance")]
        public Color LineColor
        {
            get { return _lineColor; }
            set
            {
                if (value == _lineColor) return;
                _lineColor = value;
                Invalidate();
            }
        }
        #endregion // Properties

        #region Helper(s)
        private Bitmap TextToBitmap(Graphics g)
        {
            using (Brush backbrush = new SolidBrush(BackColor),
                textBrush = new SolidBrush(ForeColor))
            using (StringFormat sf = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.NoWrap
            })
            {
                // Set control w and h based on orientation.
                int w = Orientation == Orientation.Horizontal ? Width : Height;
                int h = Orientation == Orientation.Horizontal ? Height : Width;

                // We will not use default alignment, but do it ourselves.
                var textSize = g.MeasureString(Text, Font);

                // First determine text width. Make sure you don't fall out of line.
                float textSpread = textSize.Width + 1;
                if (textSpread + 1 > w - 2 * TextOffset)
                    textSpread = w - 2 * TextOffset;

                // Now the x position.
                float x = 0;
                switch (TextAlignment)
                {
                    case StringAlignment.Near:
                        x = TextOffset;
                        break;
                    case StringAlignment.Far:
                        x = w - textSpread - TextOffset;
                        break;
                    default:
                        x = (w - textSpread) / 2;
                        break;
                }
                // Create rectangle.
                Rectangle textRect = Rectangle.Round(new RectangleF(x, 0, textSpread, h));

                // Draw all to bitmap.
                Bitmap bmp = new Bitmap(w, h, g);
                Graphics gbmp = Graphics.FromImage(bmp);
                gbmp.FillRectangle(backbrush, textRect);
                gbmp.DrawString(Text, Font, textBrush, textRect, sf);

                // And return it.
                return bmp;
            }
        }
        #endregion // Helper(s)
    }
}