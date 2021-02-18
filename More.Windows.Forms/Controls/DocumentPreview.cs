/*
 * DocumentPreview.cs
 * 
 * WYSIWYG document preview. Enables basic operations
 * on the document such as showing margins, headers, 
 * footers, etc.
 * 
 * TODO:
 *  - background image
 *  - set margin lines (horz. and vert. lines)
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2021 Tomaz Stih
 * 
 * 03.02.2021   tstih   
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace More.Windows.Forms
{
    public class DocumentPreview : Control
    {
        #region Const(s)
        private const Corner DEFAULT_FOLD = Corner.RightTop;
        private const int DEFAULT_FOLD_PERCENT = 20;
        private const string DEFAULT_UNIT = "mm";
        private readonly static Size DEFAULT_DOCUMENT_SIZE = new Size(210, 297);
        private readonly static Color DEFAULT_PAPER_COLOR = Color.FromKnownColor(KnownColor.Window);
        private readonly static Color DEFAULT_BACK_COLOR = Color.FromKnownColor(KnownColor.AppWorkspace);
        private readonly static Color DEFAULT_BORDER_COLOR = Color.FromKnownColor(KnownColor.WindowText);
        #endregion // Const(s)

        #region Private(s)
        private float _scaleFactor;
        #endregion // Private(s)

        #region Ctor
        public DocumentPreview()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;

            // Set default property values.
            SetDefaults();
        }

        private void SetDefaults()
        {
            // Folding.
            _fold = DEFAULT_FOLD;
            _foldPercent = DEFAULT_FOLD_PERCENT;

            // Document size and units.
            _documentSize = DEFAULT_DOCUMENT_SIZE;
            _unit = DEFAULT_UNIT;

            // Colors.
            _paperColor = DEFAULT_PAPER_COLOR;
            _borderColor = DEFAULT_BORDER_COLOR;
            BackColor = DEFAULT_BACK_COLOR; // Property.
        }
        #endregion // Ctor

        #region Override(s)
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics; // Easier to use.
            g.Clear(BackColor);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics; // For ease of use.

            // Draw doc.
            Rectangle dr = DocumentRect;
            int foldLen = (int)Math.Round(dr.Width > dr.Height ? (float)dr.Height / 100.0f * _foldPercent : (float)dr.Width / 100.0f * _foldPercent);
            Point[] docPoly = GetDocumentOutlinePoly(dr,foldLen);

            using (Brush paperBrush=new SolidBrush(PaperColor))
            using(Pen borderPen=new Pen(BorderColor))
            using(GraphicsPath path=new GraphicsPath())
            {
                // First draw outline.
                if (_shadow)
                {
                    Color shadowColor = Color.FromArgb(32, BorderColor);
                    using (Pen shadowPen = new Pen(shadowColor, 5))
                        g.DrawPolygon(shadowPen, docPoly);
                }
                g.FillPolygon(paperBrush, docPoly);
                g.DrawPolygon(borderPen, docPoly);

                // Add poly to path.
                path.AddPolygon(docPoly);

                // Document content drawing here.
                var rgn=g.Clip; // Store clipping region.
                var grstate = g.Save();
                Rectangle drInner = dr; 
                drInner.Inflate(-1, -1); // Get inner region (drawing area!) 
                g.SetClip(path); // Set it as clipping region.
                g.TranslateTransform(drInner.Left, drInner.Top);
                g.ScaleTransform(_scaleFactor, _scaleFactor);
                OnDocumentDraw.Raise(this, new DocumentDrawEventArgs() { Graphics = g });
                g.Restore(grstate);
                g.Clip = rgn; // Restore clipping region.

                // Now draw every fold.
                if (_fold.HasFlag(Corner.LeftTop))
                {
                    Point[] ltPoly = new Point[] {
                        new Point(dr.Left, dr.Top + foldLen),
                        new Point(dr.Left + foldLen, dr.Top + foldLen),
                        new Point(dr.Left + foldLen, dr.Top)
                    };
                    g.FillPolygon(paperBrush, ltPoly);
                    g.DrawPolygon(borderPen, ltPoly);
                }
                if (_fold.HasFlag(Corner.RightTop))
                {
                    Point[] rtPoly = new Point[] {
                        new Point(dr.Right-foldLen, dr.Top),
                        new Point(dr.Right - foldLen, dr.Top + foldLen),
                        new Point(dr.Right, dr.Top + foldLen)
                    };
                    g.FillPolygon(paperBrush, rtPoly);
                    g.DrawPolygon(borderPen, rtPoly);
                }
                if (_fold.HasFlag(Corner.RightBottom))
                {
                    Point[] rbPoly = new Point[] {
                        new Point(dr.Right, dr.Bottom - foldLen),
                        new Point(dr.Right - foldLen, dr.Bottom - foldLen),
                        new Point(dr.Right - foldLen, dr.Bottom)
                    };
                    g.FillPolygon(paperBrush, rbPoly);
                    g.DrawPolygon(borderPen, rbPoly);
                }
                if (_fold.HasFlag(Corner.LeftBottom))
                {
                    Point[] lbPoly = new Point[] {
                        new Point(dr.Left + foldLen, dr.Bottom),
                        new Point(dr.Left + foldLen, dr.Bottom - foldLen),
                        new Point(dr.Left, dr.Bottom - foldLen)
                    };
                    g.FillPolygon(paperBrush, lbPoly);
                    g.DrawPolygon(borderPen, lbPoly);
                }

            }
        }
        #endregion // Override(s)

        #region Properties
        private Corner _fold;
        [Description("Folded corters?"), Category("Appearance")]
        public Corner Fold { get { return _fold; } set { _fold = value; Invalidate(); } }

        private int _foldPercent;
        [Description("Folder corner size as % of shorter side."), Category("Appearance")]
        public int FoldPercent { get { return _foldPercent; } set { _foldPercent = value; Invalidate(); } }

        private bool _shadow;
        [Description("Document has shadow."), Category("Appearance")]
        public bool Shadow { get { return _shadow; } set { _shadow = value; Invalidate(); } }

        private Size _documentSize;
        [Description("Document size in any unit. When drawing, document proportions shall be preserved i.e. A4 will look like A4, landscape will be landscape..."), Category("Appearance")]
        public Size DocumentSize { get { return _documentSize; } set { _documentSize = value; Invalidate(); } }

        private string _unit;
        [Description("Document unit. This is a name, such as cm, px, or empty string."), Category("Appearance")]
        public string Unit { get { return _unit; } set { _unit = value; Invalidate(); } }

        private Color _paperColor;
        [Description("Document is drawn respecting margins on background. This is color for the document (paper color)."), Category("Appearance")]
        public Color PaperColor { get { return _paperColor; } set { _paperColor = value; Invalidate(); } }

        private Color _borderColor;
        [Description("Document border color."), Category("Appearance")]
        public Color BorderColor { get { return _borderColor; } set { _borderColor = value; Invalidate(); } }
        #endregion // Properties

        #region Event(s)
        [Description("Called when the document content needs to redraw."), Category("Misc")]
        public event EventHandler<DocumentDrawEventArgs> OnDocumentDraw;
        #endregion // Event(s)

        #region Helper(s)
        private Rectangle WorkspaceRect
        {
            get
            {
                return new Rectangle(
                    ClientRectangle.Left + Margin.Left,
                    ClientRectangle.Top + Margin.Top,
                    ClientRectangle.Width - Margin.Left - Margin.Right - 1,
                    ClientRectangle.Height - Margin.Top - Margin.Bottom - 1
                );
            }
        }

        private Rectangle DocumentRect { 
            get
            {
                // Fit doc inside margins and size.
                Rectangle realDocumentRect = new Rectangle(Point.Empty, _documentSize); // Size in real units.
                _scaleFactor = realDocumentRect.SizeToFitFactor(WorkspaceRect);
                Size calcDocumentSize = new Size(
                    (int)Math.Round((float)_documentSize.Width * _scaleFactor),
                    (int)Math.Round((float)_documentSize.Height * _scaleFactor)
                );
                return WorkspaceRect.Center(calcDocumentSize, Padding.Empty);
            }
        }

        private Point[] GetDocumentOutlinePoly(Rectangle dr, int foldLen)
        {
                List<Point> pts = new List<Point>();
                
                // Top-left corder.
                pts.Add(new Point(dr.Left, dr.Top+foldLen));
                if (!_fold.HasFlag(Corner.LeftTop))
                    pts.Add(new Point(dr.Left, dr.Top));
                pts.Add(new Point(dr.Left + foldLen, dr.Top));

                // Top-right corder.
                pts.Add(new Point(dr.Right-foldLen, dr.Top));
                if (!_fold.HasFlag(Corner.RightTop))
                    pts.Add(new Point(dr.Right, dr.Top));
                pts.Add(new Point(dr.Right, dr.Top+foldLen));

                // Bottom right.
                pts.Add(new Point(dr.Right, dr.Bottom - foldLen));
                if (!_fold.HasFlag(Corner.RightBottom))
                    pts.Add(new Point(dr.Right, dr.Bottom));
                pts.Add(new Point(dr.Right - foldLen, dr.Bottom));

                // Bottom left.
                pts.Add(new Point(dr.Left + foldLen, dr.Bottom));
                if (!_fold.HasFlag(Corner.LeftBottom))
                    pts.Add(new Point(dr.Left, dr.Bottom));
                pts.Add(new Point(dr.Left, dr.Bottom - foldLen));

                return pts.ToArray();
        }
        #endregion // Helper(s)
    }

    public class DocumentDrawEventArgs : EventArgs
    {
        public DocumentDrawEventArgs() {}
        public Graphics Graphics { get; set; }
    }

    [Flags]
    public enum Corner { 
        None            = 0, 
        LeftTop         = 1, 
        RightTop        = 2,
        LeftBottom      = 4,     
        RightBottom     = 8 };
}