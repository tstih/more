/*
 * Frame.cs
 * 
 * Panel with external border, internal border,
 * and title. All optional.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 03.02.2020   tstih
 * 
 */
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace More.Windows.Forms
{
    public class Frame : PanelEx
    {
        #region Const(s)
        private const int DEFAULT_TITLE_HEIGHT = 24;
        private const int DEFAULT_OUTER_BORDER_THICKNESS = 1;
        private const int DEFAULT_INNER_BORDER_THICKNESS = 0;
        private const int DEFAULT_BORDER_THICKNESS = 2;
        private const int DEFAULT_TITLE_OFFSET = 8;
        #endregion // Const(s)

        #region Ctor(s)
        public Frame()
        {
            // Default properties.
            SetDefaults();
        }

        private void SetDefaults()
        {
            // Borders.
            _outerBorderLightColor = Color.FromKnownColor(KnownColor.WindowFrame);
            _outerBorderDarkColor = Color.FromKnownColor(KnownColor.WindowFrame);
            _outerBorderThickness = DEFAULT_OUTER_BORDER_THICKNESS;
            _innerBorderLightColor = Color.FromKnownColor(KnownColor.WindowFrame);
            _innerBorderDarkColor = Color.FromKnownColor(KnownColor.WindowFrame);
            _innerBorderThickness = DEFAULT_INNER_BORDER_THICKNESS;
            _titleHeight = DEFAULT_TITLE_HEIGHT;
            _borderThickness = DEFAULT_BORDER_THICKNESS;

            // Title.
            _titleBackColor = Color.FromKnownColor(KnownColor.Control);
            _titleForeColor = Color.FromKnownColor(KnownColor.ControlText);
            _titleAlignment = StringAlignment.Near;
            _titleOffset = DEFAULT_TITLE_OFFSET;
            _title = string.Empty; // No title by default!

            // And margins.
            UpdateMargins();
        }
        #endregion // Ctor(s)

        #region Overridable(s)
        // Override this.
        protected override void Decorate(Graphics g, Rectangle lt, Rectangle rt, Rectangle lb, Rectangle rb, Rectangle l, Rectangle t, Rectangle r, Rectangle b)
        {
            g.SmoothingMode = SmoothingMode.None;

            using (Brush titleBackBrush = new SolidBrush(TitleBackColor),
                titleForeBrush = new SolidBrush(TitleForeColor))
            using (StringFormat sf = new StringFormat() { Alignment = TitleAlignment, LineAlignment = StringAlignment.Center })
            {
                // Paint title background.
                Rectangle titleRect = new Rectangle(lt.X, lt.Y, lt.Width + t.Width + rt.Width, t.Height);
                g.FillRectangle(titleBackBrush, titleRect);

                // Paint inner frame.
                if (InnerBorderThickness > 0)
                {
                    int inx = l.Right - InnerBorderThickness, iny = t.Bottom - InnerBorderThickness,
                        inx2 = r.Left + InnerBorderThickness, iny2 = b.Top + InnerBorderThickness;
                    Rectangle innerRect = new Rectangle(inx, iny, inx2 - inx - 1, iny2 - iny - 1);
                    g.DrawBorder(InnerBorderDarkColor, InnerBorderLightColor, innerRect, InnerBorderThickness);
                }

                // Paint outer.
                if (OuterBorderThickness > 0)
                {
                    Rectangle outerRect = new Rectangle(l.Left, t.Top, r.Right-l.Left-1, b.Bottom-t.Top-1 );
                    g.DrawBorder(OuterBorderDarkColor, OuterBorderLightColor, outerRect, OuterBorderThickness);
                }

                // Paint title text.
                titleRect.Inflate(-TitleOffset, 0);
                g.DrawString(Title, Font, titleForeBrush, titleRect, sf);
            }
        }
        #endregion // Overridable(s)

        #region Properties

        // Outer border.
        private Color _outerBorderLightColor;
        [Description("Outer border light color"), Category("Appearance")]
        public Color OuterBorderLightColor { get { return _outerBorderLightColor; } set { _outerBorderLightColor = value; Invalidate(); } }
        private Color _outerBorderDarkColor;
        [Description("Outer border dark color"), Category("Appearance")]
        public Color OuterBorderDarkColor { get { return _outerBorderDarkColor; } set { _outerBorderDarkColor = value; Invalidate(); } }
        private int _outerBorderThickness;
        [Description("Outer border thickness in pixels"), Category("Appearance")]
        public int OuterBorderThickness { get { return _outerBorderThickness; } set { _outerBorderThickness = value; UpdateMargins();  } }

        // Inner border.
        private Color _innerBorderLightColor;
        [Description("Inner border light color"), Category("Appearance")]
        public Color InnerBorderLightColor { get { return _innerBorderLightColor; } set { _innerBorderLightColor = value; Invalidate(); } }
        private Color _innerBorderDarkColor;
        [Description("Inner border dark color"), Category("Appearance")]
        public Color InnerBorderDarkColor { get { return _innerBorderDarkColor; } set { _innerBorderDarkColor = value; Invalidate(); } }
        private int _innerBorderThickness;
        [Description("Inner border thickness in pixels"), Category("Appearance")]
        public int InnerBorderThickness { get { return _innerBorderThickness; } set { _innerBorderThickness = value; UpdateMargins(); } }

        // Inner 2 outer space.
        private int _borderThickness;
        [Description("Pixels between outer and inner border"), Category("Appearance")]
        public int BorderThickness { get { return _borderThickness; } set { _borderThickness = value; UpdateMargins();} }

        // Title.
        private Color _titleBackColor;
        [Description("Title back color"), Category("Appearance")]
        public Color TitleBackColor { get { return _titleBackColor; } set { _titleBackColor = value; Invalidate(); } }
        private Color _titleForeColor;
        [Description("Title fore color"), Category("Appearance")]
        public Color TitleForeColor { get { return _titleForeColor; } set { _titleForeColor = value; Invalidate(); } }
        private StringAlignment _titleAlignment;
        [Description("Title alignment"), Category("Appearance")]
        public StringAlignment TitleAlignment { get { return _titleAlignment; } set { _titleAlignment = value; Invalidate(); } }
        private string _title;
        [Description("Title text"), Category("Appearance")]
        public string Title { get { return _title; } set { _title = value; Invalidate(); } }
        private int _titleOffset;
        [Description("Title text offset (left and right, from start of title space)"), Category("Appearance")]
        public int TitleOffset { get { return _titleOffset; } set { _titleOffset = value; Invalidate(); } }
        private int _titleHeight;
        [Description("Title height in pixels"), Category("Appearance")]
        public int TitleHeight { get { return _titleHeight; } set { _titleHeight = value; UpdateMargins();  } }

        // Margin hack (overload margin).
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Bindable(false)]
        [Browsable(false)]
        public new Padding Margin { get { return base.Margin; } set { base.Margin = value; } }
        #endregion // Properties

        #region Helper(s)
        public void UpdateMargins()
        {
            // Border margins.
            int m= OuterBorderThickness + BorderThickness + InnerBorderThickness;
            Margin = new Padding(m, m + TitleHeight, m, m);
            Invalidate();
        }
        #endregion // Helper(s)
    }
}