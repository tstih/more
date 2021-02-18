/*
 * Ruler.cs
 * 
 * Flexible ruler control. Provides default painting,
 * but allows complete customization. Also provides
 * world to screen calculations.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 31.12.2020   tstih   Happy new year!
 * 
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace More.Windows.Forms
{
    public class Ruler : Control
    {
        #region Const(s)
        private const Orientation DEFAULT_ORIENTATION = Orientation.Horizontal;
        private const StringAlignment DEFAULT_TICK_ALIGNMENT = StringAlignment.Center;
        private const int DEFAULT_MINOR_TICKS_PER_MAJOR_TICK = 10;
        private const int DEFAULT_MAJOR_TICK_PIXELS = 8;
        private const int DEFAULT_MINOR_TICK_PIXELS = 4;
        private const int DEFAULT_TICK_SIZE = 8;
        private const int DEFAULT_RULER_SIZE = 26;
        private const int DEFAULT_NEAR_MARGIN = 0;
        private const int DEFAULT_FAR_MARGIN = 0;
        private const int DEFAULT_POSITION = 0;
        private const int DEFAULT_MIN = -1000;
        private const int DEFAULT_MAX = 1000;
        #endregion // Const(s)

        #region Ctor
        public Ruler()
        {
            ResizeRedraw = true;

            // Set default property values.
            SetDefaults();
        }

        private void SetDefaults()
        {
            // Properties.
            _orientation = DEFAULT_ORIENTATION;
            _minorTicksPerMajorTick = DEFAULT_MINOR_TICKS_PER_MAJOR_TICK;
            _majorTickPixels = DEFAULT_MAJOR_TICK_PIXELS;
            _minorTickPixels = DEFAULT_MINOR_TICK_PIXELS;
            _tickSize = DEFAULT_TICK_SIZE;
            _tickAlignment = DEFAULT_TICK_ALIGNMENT;
            _nearMargin = DEFAULT_NEAR_MARGIN;
            _farMargin = DEFAULT_FAR_MARGIN;
            _border = true;
            _nearBorder = true;
            _farBorder = true;
            _borderDkColor = Color.FromKnownColor(KnownColor.ActiveBorder);
            _borderLtColor = Color.FromKnownColor(KnownColor.ActiveBorder);
            _controlBox = false;
            _position = DEFAULT_POSITION;
            _labelAlignment = StringAlignment.Near;

            // Built in props.
            Size = new Size(DEFAULT_RULER_SIZE, DEFAULT_RULER_SIZE);
        }
        #endregion

        #region Override(s)
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Create drawing layout.
            Bitmap bmp = new Bitmap(ClientRectangle.Width,ClientRectangle.Height);
            Graphics g = Graphics.FromImage(bmp);

            // Clear everything.
            g.Clear(BackColor);

            if (_border) { // Ruler has border?
                if (_controlBox) // Draw control box too background?
                    g.DrawBorder(_borderDkColor, _borderLtColor, ControlBoxRectWithBorder, 1, Borders.All);
                 // And ruler rect.
                 g.DrawBorder(_borderDkColor, BorderLtColor, RulerRectWithBorder, 1, RulerBorders);
            }
            // Now ticks.
            DrawTicks(g);

            // Finally draw bitmap.
            e.Graphics.DrawImage(bmp, Point.Empty);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Calculate first tick position.

        }

        protected override void OnDockChanged(EventArgs e)
        {
            // Help a bit...
            if (Dock == DockStyle.Left || Dock == DockStyle.Right)
                Orientation = Orientation.Vertical;
            else if (Dock == DockStyle.Top || Dock == DockStyle.Bottom)
                Orientation = Orientation.Horizontal;
        }
        #endregion // Override(s)

        #region Properties
        private int _minorTicksPerMajorTick;
        /// <summary>
        /// Each how many minor ticks do we draw a major tick.
        /// </summary>
        [Description("How many minor ticks to draw before major tick"), Category("Appearance")]
        public int MinorTicksPerMajorTick { get { return _minorTicksPerMajorTick; } set { _minorTicksPerMajorTick = value; Invalidate(); } }

        private int _minorTickPixels;
        /// <summary>
        /// When drawing minor tick, what is the length of the tick line (in pixels).
        /// </summary>
        [Description("Minor tick line length (for drawing)"), Category("Appearance")]
        public int MinorTickPixels { get { return _minorTickPixels; } set { _minorTickPixels = value; Invalidate(); } }

        private int _majorTickPixels;
        /// <summary>
        /// When drawing major tick, what is the length of the tick line (in pixels).
        /// </summary>
        [Description("Major tick line length (for drawing)"), Category("Appearance")]
        public int MajorTickPixels { get { return _majorTickPixels; } set { _majorTickPixels = value; Invalidate(); } }

        private Orientation _orientation;
        /// <summary>
        /// Vertical or horizontal ruler?
        /// </summary>
        [Description("Ruler Orientation"), Category("Layout")]
        public Orientation Orientation { get { return _orientation; } set { _orientation = value; Invalidate(); } }

        private int _tickSize;
        public int TickSize { get { return _tickSize; } set { _tickSize = value; Invalidate(); } }

        private bool _border;
        public bool Border { get { return _border; } set { _border = value; Invalidate(); } }

        private bool _nearBorder;
        public bool NearBorder { get { return _nearBorder; } set { _nearBorder = value; Invalidate(); } }

        private bool _farBorder;
        public bool FarBorder { get { return _farBorder; } set { _farBorder = value; Invalidate(); } }

        private Color _borderLtColor;
        public Color BorderLtColor { get { return _borderLtColor; } set { _borderLtColor = value; Invalidate(); } }

        private Color _borderDkColor;
        public Color BorderDkColor { get { return _borderDkColor; } set { _borderDkColor = value; Invalidate(); } }

        private StringAlignment _tickAlignment;
        public StringAlignment TickAlignment { get { return _tickAlignment; } set { _tickAlignment = value; Invalidate(); } }

        private int _nearMargin;
        public int NearMargin { get { return _nearMargin; } set { _nearMargin = value; Invalidate(); } }

        private int _farMargin;
        public int FarMargin { get { return _farMargin; } set { _farMargin = value; Invalidate(); } }

        private int _position;
        public int Position { get { return _position; } set { _position = value; Invalidate(); } }

        private bool _controlBox;
        public bool ControlBox { get { return _controlBox; } set { _controlBox = value; Invalidate(); } }


        private BiAlignment _controlBoxAlignment;
        public BiAlignment ControlBoxAlignment { get { return _controlBoxAlignment; } set { _controlBoxAlignment = value; Invalidate(); } }

        private StringAlignment _labelAlignment;
        public StringAlignment LabelAlignment { get { return _labelAlignment; } set { _labelAlignment = value; Invalidate(); } }
        #endregion // Properties

        #region Helper(s)
        private void DrawTicks(Graphics g)
        {
            // _position is our 0.
            Rectangle rr = RulerRect;

            // Draw it all.
            int firstTickAt = _orientation == Orientation.Horizontal ? rr.X : rr.Y + Math.Abs(_position % _tickSize);
            int len= _orientation==Orientation.Horizontal ? rr.Width : rr.Height;
            int firstTickIndex = _position / _tickSize; // + _position>0 ? 1 : 0;
            for(int i=firstTickAt; i<len; i+=_tickSize)
                DrawSingleTick(g, rr, i, firstTickIndex++);
        }

        private void DrawSingleTick(Graphics g, Rectangle r, int pos, int tickIndex)
        {
            if (_orientation==Orientation.Horizontal)
            { // pos is x.
                string tickLabel = TickLabel(tickIndex);
                if (tickIndex % _minorTicksPerMajorTick == 0)
                {
                    if (_tickAlignment == StringAlignment.Near)
                        g.DrawLine(Pens.Black, pos, r.Top, pos, r.Top + _majorTickPixels);
                    else if (_tickAlignment == StringAlignment.Far)
                        g.DrawLine(Pens.Black, pos, r.Bottom - _majorTickPixels, pos, r.Bottom);

                    Size labelSize = Size.Round(g.MeasureString(tickLabel, Font));
                    Rectangle labelRect;
                    if (_labelAlignment == StringAlignment.Near)
                        labelRect = new Rectangle(pos - labelSize.Width - 1, r.Top, labelSize.Width + 1, r.Height);
                    else if (_labelAlignment == StringAlignment.Far)
                        labelRect = new Rectangle(pos, r.Top, labelSize.Width + 1, r.Height);
                    else
                        labelRect = new Rectangle(pos - (labelSize.Width + 1) / 2, r.Top, labelSize.Width + 1, r.Height);

                    using (StringFormat sf = new StringFormat() { LineAlignment = StringAlignment.Center })
                        g.DrawString(
                            tickLabel,
                            Font,
                            Brushes.Black,
                            labelRect,
                            sf
                        );
                }
                else
                {
                    if (_tickAlignment == StringAlignment.Near)
                        g.DrawLine(Pens.Black, pos, r.Top, pos, r.Top + _minorTickPixels);
                    else if (_tickAlignment == StringAlignment.Far)
                        g.DrawLine(Pens.Black, pos, r.Bottom - _minorTickPixels, pos, r.Bottom);
                    else
                        g.DrawLine(Pens.Black, pos, r.Top + r.Height / 2 - _minorTickPixels / 2, pos, r.Top + r.Height / 2 + _minorTickPixels / 2);
                }
            }
        }

        private string TickLabel(int index)
        {
            return index.ToString();
        }

        private Rectangle ControlBoxRectWithBorder
        {
            get {

                // No control box?
                if (!_controlBox) return Rectangle.Empty;

                if (_orientation == Orientation.Horizontal)
                {
                    int edge = ClientRectangle.Height - 1;
                    int x = ControlBoxAlignment == BiAlignment.Near ? ClientRectangle.Left : ClientRectangle.Right - edge - 1;
                    return new Rectangle(x, ClientRectangle.Y, edge, edge);
                }
                else
                {
                    int edge = ClientRectangle.Width - 1;
                    int y = ControlBoxAlignment == BiAlignment.Near ? ClientRectangle.Top : ClientRectangle.Bottom - edge - 1;
                    return new Rectangle(ClientRectangle.X, y, edge, edge);
                }
            }
        }

        private Rectangle ControlBoxRect
        {
            get {
                Rectangle r = ControlBoxRectWithBorder;
                r.Inflate(-1, -1);
                return r;
            }
        }

        private Rectangle RulerRectWithBorder
        {
            get
            {
                Rectangle cb = ControlBoxRectWithBorder;

                if (_orientation == Orientation.Horizontal) 
                    return new Rectangle(
                        ClientRectangle.X + (ControlBoxAlignment == BiAlignment.Near ? cb.Width : 0),
                        ClientRectangle.Y,
                        ClientRectangle.Width - cb.Width - 1,
                        ClientRectangle.Height - 1);
                else
                    return new Rectangle(
                        ClientRectangle.X,
                        ClientRectangle.Y + (ControlBoxAlignment == BiAlignment.Near ? cb.Height : 0),
                        ClientRectangle.Width - 1,
                        ClientRectangle.Height - cb.Height - 1);
            }
        }

        private Rectangle RulerRect
        {
            get
            {
                Rectangle r = RulerRectWithBorder;
                r.Inflate(-1, -1);
                return r;
            }
        }

        private Borders RulerBorders
        {
            get
            {
                Borders b = Borders.None;
                if (_orientation==Orientation.Horizontal)
                {
                    b = Borders.Top | Borders.Bottom;
                    if (NearBorder) b |= Borders.Left;
                    if (FarBorder) b |= Borders.Right;
                } else
                {
                    b = Borders.Left | Borders.Right;
                    if (NearBorder) b |= Borders.Top;
                    if (FarBorder) b |= Borders.Bottom;
                }
                return b;
            }
        }
        #endregion // Helper(s)
    }

    public enum BiAlignment { Near, Far };
}
