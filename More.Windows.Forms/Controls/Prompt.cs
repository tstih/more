/*
 * Prompt.cs
 * 
 * Prompt panel (used to host edit controls on forms).
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 30.04.2020   tstih
 * 
 */
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace More.Windows.Forms
{
    public class Prompt : PanelEx
    {
        #region Const(s)
        private const int DEFAULT_WIDTH = 280;
        private const int DEFAULT_HEIGHT = 25;
        private const int DEFAULT_PROMPT_TEXT_WIDTH = 80;
        private const int DEFAULT_BORDER_THICKNESS = 1;
        private const int DEFAULT_GLYPH_WIDTH = 16;
        private const int DEFAULT_PROMPT_TEXT_EDGE = 8;
        #endregion // Const(s)

        #region Static(s)
        private static Size DefaultPromptSize = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
        #endregion // Static(s)

        #region Private(s)
        private Rectangle _textRect;
        private Rectangle _glyphRect;
        private ToolTip _tooltip;
        #endregion // Private(s)

        #region Ctor(s)
        public Prompt()
        {
            // Default properties.
            SetDefaults();
        }

        private void SetDefaults()
        {
            _leftBorderThickness = _rightBorderThickness = _topBorderThickness = _bottomBorderThickness = DEFAULT_BORDER_THICKNESS;
            _borderColor = Color.FromKnownColor(KnownColor.ActiveBorder);

            _contentMargin = Padding.Empty;

            _promptTextWidth = DEFAULT_PROMPT_TEXT_WIDTH;
            _promptForeColor = Color.FromKnownColor(KnownColor.ControlText);
            _promptBackColor = Color.FromKnownColor(KnownColor.Control);
            _promptTextEdge = DEFAULT_PROMPT_TEXT_EDGE;
            _promptTextFont = Control.DefaultFont;
            _promptTextLineAlignment = StringAlignment.Center;
            _promptTextAlignment = StringAlignment.Near;

            _glyph = null; // No glyph by default.
            _glyphWidth = DEFAULT_GLYPH_WIDTH;
            _glyphAlignment = ContentAlignment.MiddleCenter;
            _glyphBackColor = _promptBackColor;

            _tooltip = new ToolTip();

            UpdateMargin();

            Size = DefaultPromptSize;
        }
        #endregion // Ctor(s)

        #region Properties

        private Padding _contentMargin;
        /// <summary>
        /// Content margin within client area.
        /// </summary>
        [Description("Content margin within client area."), Category("Layout")]
        public Padding ContentMargin { get { return _contentMargin; } set { _contentMargin = value; UpdateMargin(); } }

        /// <summary>
        /// Prompt text property.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Description("Prompt text."), Category("Appearance")]
        public override string Text { get { return base.Text; } set { base.Text = value; Invalidate(); } }

        private int _topBorderThickness;
        /// <summary>
        /// Top border thickness. 0 for no border.
        /// </summary>
        [Description("Top border thickness. 0 for no border."), Category("Layout")]
        public int TopBorderThickness { get { return _topBorderThickness; } set { _topBorderThickness = value; UpdateMargin(); } }

        private int _bottomBorderThickness;
        /// <summary>
        /// Bottom border thickness. 0 for no border.
        /// </summary>
        [Description("Bottom border thickness. 0 for no border."), Category("Layout")]
        public int BottomBorderThickness { get { return _bottomBorderThickness; } set { _bottomBorderThickness = value; UpdateMargin(); } }

        private int _leftBorderThickness;
        /// <summary>
        /// Left border thickness. 0 for no border.
        /// </summary>
        [Description("Left border thickness. 0 for no border."), Category("Layout")]
        public int LeftBorderThickness { get { return _leftBorderThickness; } set { _leftBorderThickness = value; UpdateMargin(); } }

        private int _rightBorderThickness;
        /// <summary>
        /// Right border thickness. 0 for no border.
        /// </summary>
        [Description("Right border thickness. 0 for no border."), Category("Layout")]
        public int RightBorderThickness { get { return _rightBorderThickness; } set { _rightBorderThickness = value; UpdateMargin(); } }

        private int _promptTextWidth;
        /// <summary>
        /// Prompt text width. This is the width of prompt text without the glyph.
        /// </summary>
        [Description("Prompt text width. This is the width of prompt text without the glyph."), Category("Layout")]
        public int PromptTextWidth { get { return _promptTextWidth; } set { _promptTextWidth = value; UpdateMargin(); } }

        private int _promptTextEdge;
        /// <summary>
        /// Prompt text indentation (offset from the edge, depending on alignment).
        /// </summary>
        [Description("Prompt text indentation (offset from the edge, depending on alignment."), Category("Layout")]
        public int PromptTextEdge { get { return _promptTextEdge; } set { _promptTextEdge = value; Invalidate(); } }

        private Image _glyph;
        /// <summary>
        /// Image to use. If this is null no image placeholder will be reserved in the control.
        /// </summary>
        [Description("Image to use. If this is null no image placeholder will be reserved in the control."), Category("Appearance")]
        public Image Glyph { get { return _glyph; } set { _glyph = value; UpdateMargin(); } }

        private int _glyphWidth;
        /// <summary>
        /// Image width.
        /// </summary>
        [Description("Image width."), Category("Layout")]
        public int GlyphWidth { get { return _glyphWidth; } set { _glyphWidth = value; UpdateMargin(); } }

        private bool _glyphVisible;
        /// <summary>
        /// Controls show/hide image.
        /// </summary>
        [Description("Controls show/hide image."), Category("Appearance")]
        public bool GlyphVisible { get { return _glyphVisible; } set { _glyphVisible = value; Invalidate(); } }

        private string _glyphTooltip;
        /// <summary>
        /// If set this is shown when you hover over the image.
        /// </summary>
        [Description("If set this is shown when you hover over the image."), Category("Appearance")]
        public string GlyphTooltip { get { return _glyphTooltip; } set { _glyphTooltip = value; Invalidate(); } }

        private string _textTooltip;
        /// <summary>
        /// If set this is shown when you hover over the prompt text.
        /// </summary>
        [Description("If set this is shown when you hover over the prompt text."), Category("Appearance")]
        public string TextTooltip { get { return _textTooltip; } set { _textTooltip = value; Invalidate(); } }

        private Color _promptForeColor;
        /// <summary>
        /// Fore color for prompt text.
        /// </summary>
        [Description("Fore color for prompt text."), Category("Appearance")]
        public Color PromptForeColor { get { return _promptForeColor; } set { _promptForeColor = value; Invalidate(); } }

        private Color _promptBackColor;
        /// <summary>
        /// Back color (just for the prompt text part).
        /// </summary>
        [Description("Back color (just for the prompt text part)."), Category("Appearance")]
        public Color PromptBackColor { get { return _promptBackColor; } set { _promptBackColor = value; Invalidate(); } }

        private Color _glyphBackColor;
        /// <summary>
        /// Back color for the glyph.
        /// </summary>
        [Description("Back color for the glyph."), Category("Appearance")]
        public Color GlyphBackColor { get { return _glyphBackColor; } set { _glyphBackColor = value; Invalidate(); } }

        private ContentAlignment _glyphAlignment;
        /// <summary>
        /// Glyph alignment.
        /// </summary>
        [Description("Glyph alignment."), Category("Layout")]
        public ContentAlignment GlyphAlignment { get { return _glyphAlignment; } set { _glyphAlignment = value; Invalidate(); } }

        private Color _borderColor;
        /// <summary>
        /// Border color.
        /// </summary>
        [Description("Border color."), Category("Appearance")]
        public Color BorderColor { get { return _borderColor; } set { _borderColor = value; Invalidate(); } }

        private StringAlignment _promptTextAlignment;
        /// <summary>
        /// Alignment of prompt text.
        /// </summary>
        [Description("Alignment of prompt text."), Category("Appearance")]
        public StringAlignment PromptTextAlignment { get { return _promptTextAlignment; } set { _promptTextAlignment = value; Invalidate(); } }

        private StringAlignment _promptTextLineAlignment;
        /// <summary>
        /// Line alignment of prompt text.
        /// </summary>
        [Description("Line alignment of prompt text."), Category("Appearance")]
        public StringAlignment PromptTextLineAlignment { get { return _promptTextLineAlignment; } set { _promptTextLineAlignment = value; Invalidate(); } }

        private Font _promptTextFont;
        /// <summary>
        /// Font of prompt text.
        /// </summary>
        [Description("Font of prompt text."), Category("Appearance")]
        public Font PromptTextFont { get { return _promptTextFont; } set { _promptTextFont = value; Invalidate(); } }

        #endregion // Properties

        #region Override(s)
        protected override Size DefaultSize { get  { return new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT); } }

        protected override void Decorate(Graphics g, Rectangle lt, Rectangle rt, Rectangle lb, Rectangle rb, Rectangle l, Rectangle t, Rectangle r, Rectangle b)
        {
            using (Brush promptBackBrush = new SolidBrush(_promptBackColor),
                glyphBackBrush = new SolidBrush(_glyphBackColor),
                borderBrush = new SolidBrush(_borderColor),
                promptForeBrush = new SolidBrush(_promptForeColor))
            using (StringFormat sf = new StringFormat() { Alignment = _promptTextAlignment, LineAlignment = _promptTextLineAlignment })
            {
                // Paint all borders...
                g.FillRectangle(borderBrush, lt); g.FillRectangle(borderBrush, t); g.FillRectangle(borderBrush, rt);
                g.FillRectangle(borderBrush, lb); g.FillRectangle(borderBrush, b); g.FillRectangle(borderBrush, rb);
                g.FillRectangle(borderBrush, r);

                // ...except special border (left0
                Rectangle leftBorder = new Rectangle(l.X, l.Y, _leftBorderThickness, l.Height);
                g.FillRectangle(borderBrush, leftBorder);

                // Draw text.
                _textRect = new Rectangle(l.X + _leftBorderThickness, l.Y, _promptTextWidth - _leftBorderThickness, l.Height);
                Rectangle stringRect;
                if (_promptTextAlignment == StringAlignment.Near)
                    stringRect = new Rectangle(l.X + _leftBorderThickness + _promptTextEdge, l.Y, _promptTextWidth - _leftBorderThickness - _promptTextEdge, l.Height);
                else if (_promptTextAlignment == StringAlignment.Far)
                    stringRect = new Rectangle(l.X + _leftBorderThickness, l.Y, _promptTextWidth - _leftBorderThickness - _promptTextEdge, l.Height);
                else
                    stringRect = _textRect;

                g.FillRectangle(promptBackBrush, _textRect);
                g.DrawString(Text, PromptTextFont, promptForeBrush, stringRect, sf);

                // Paint glyph?
                if (_glyphVisible && _glyph!=null)
                {
                    _glyphRect = new Rectangle(_textRect.Right, _textRect.Top, _glyphWidth, _textRect.Height);
                    g.FillRectangle(glyphBackBrush, _glyphRect);
                    // Aligh glyph rect.
                    _glyphRect = CalcImageRenderBounds(_glyph, _glyphRect, _glyphAlignment);
                    g.DrawImage(_glyph, _glyphRect.Location);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Handle.
            base.OnMouseMove(e);

            // Process tooltip.
            Point loc = e.Location;
            if (_glyphRect.Contains(loc))
                _tooltip.SetToolTip(this, _glyphTooltip);
            else if (_textRect.Contains(loc))
                _tooltip.SetToolTip(this, _textTooltip);
        }
        #endregion // Override(s)

        #region Helper(s)
        private void UpdateMargin()
        {
            Margin = new Padding(
                _leftBorderThickness + _contentMargin.Left + _promptTextWidth + (_glyph == null ? 0 : _glyphWidth),
                _topBorderThickness + _contentMargin.Top,
                _rightBorderThickness + _contentMargin.Right,
                _bottomBorderThickness + _contentMargin.Bottom);
            Invalidate();
        }

        protected Rectangle CalcImageRenderBounds(Image image, Rectangle r, ContentAlignment align)
        {
            Size pointImageSize = image.Size;

            int xLoc = r.X + 2;
            int yLoc = r.Y + 2;

            if ((align & (ContentAlignment.TopRight | ContentAlignment.MiddleRight | ContentAlignment.BottomRight)) != 0)
            {
                xLoc = (r.X + r.Width - 4) - pointImageSize.Width;
            }
            else if ((align & (ContentAlignment.TopCenter | ContentAlignment.MiddleCenter | ContentAlignment.BottomCenter)) != 0)
            {
                xLoc = r.X + (r.Width - pointImageSize.Width) / 2;
            }


            if ((align & (ContentAlignment.BottomLeft | ContentAlignment.BottomCenter | ContentAlignment.BottomRight)) != 0)
            {
                yLoc = (r.Y + r.Height - 4) - pointImageSize.Height;
            }
            else if ((align & (ContentAlignment.TopLeft | ContentAlignment.TopCenter | ContentAlignment.TopRight)) != 0)
            {
                yLoc = r.Y + 2;
            }
            else
            {
                yLoc = r.Y + (r.Height - pointImageSize.Height) / 2;
            }

            return new Rectangle(xLoc, yLoc, pointImageSize.Width, pointImageSize.Height);
        }
        #endregion // Helper(s)
    }
}