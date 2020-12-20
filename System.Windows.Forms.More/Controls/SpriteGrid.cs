/*
 * SpriteGrid.cs
 * 
 * SpriteGrid is scrollable sprite grid control.
 * Its underlying data structure is a standard Image
 * so you can use it to view any image.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 03.02.2020   tstih
 * 
 */
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.More
{
    public class SpriteGrid : ScrollableControl
    {
        #region Const(s)
        private const int DEFAULT_CELL_WIDTH = 8;
        private const int DEFAULT_CELL_HEIGHT = 8;
        private const int DEFAULT_ROWS = 8;
        private const int DEFAULT_COLUMNS = 8;
        private const int DEFAULT_RULER_WIDTH = 18;
        private const int DEFAULT_RULER_HEIGHT = 20;
        private const int DEFAULT_MINOR_TICKS_PER_MAJOR_TICK = 8;
        private const int DEFAULT_MINOR_TICK_SIZE = 4;
        private const int DEFAULT_MAJOR_TICK_SIZE = 12;
        private const int DEFAULT_MARGIN_LINE_THICKNESS = 1;
        private const int MIN_CELL_SIZE_FOR_GRID = 5;
        #endregion // Const(s)

        #region Private Member(s)

        // Vars.
        private int _firstVisibleColumn, _firstVisibleRow, _lastVisibleColumn, _lastVisibleRow;
        private bool _contentVisible;

        // Properties.
        private int _cellWidth;
        private int _cellHeight;
        private int _rows;
        private int _columns;
        private Color _rulerBackgroundColor;
        private Color _gridTickLineColor;
        private float[] _gridTickLineDashPattern;
        private Color _gridEdgeLineColor;
        private float[] _gridEdgeLineDashPattern;
        private bool _showHorzRuler;
        private bool _showVertRuler;
        private int _minorTicksPerMajorTick;
        private int _rulerWidth;
        private int _rulerHeight;
        private int _minorTickSize;
        private int _majorTickSize;
        private int _topMargin;
        private int _bottomMargin;
        private int _leftMargin;
        private int _rightMargin;
        private int _marginLineThickness;
        private Image _sourceImage;
        private GridSelection _gridSelection;

        #endregion // Private Member(s)

        #region Ctor(s)
        public SpriteGrid()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;

            // Set default property values.
            SetDefaults();
        }

        private void SetDefaults()
        {
            // New properties.
            _showHorzRuler = false;
            _showVertRuler = false;
            _minorTicksPerMajorTick = DEFAULT_MINOR_TICKS_PER_MAJOR_TICK; // Default.
            _rulerWidth = DEFAULT_RULER_WIDTH;
            _rulerHeight = DEFAULT_RULER_HEIGHT;
            _cellWidth = DEFAULT_CELL_WIDTH;
            _cellHeight = DEFAULT_CELL_HEIGHT;
            _minorTickSize = DEFAULT_MINOR_TICK_SIZE;
            _majorTickSize = DEFAULT_MAJOR_TICK_SIZE;
            _rows = DEFAULT_ROWS;
            _columns = DEFAULT_COLUMNS;
            _gridTickLineDashPattern = new float[] { 1, 1 };
            _gridTickLineColor = SystemColors.ControlDark;
            _gridEdgeLineDashPattern = new float[] { 1, 0 };
            _gridEdgeLineColor = SystemColors.WindowText;
            _rulerBackgroundColor = SystemColors.Control;
            _marginLineThickness = DEFAULT_MARGIN_LINE_THICKNESS;

            Font = new Font("Segoe UI", 7); // Default tick font.

            // Build in properties. (Don't worry, this will not cause a redraw.)
            BackColor = SystemColors.Window;
            ForeColor = SystemColors.WindowText;
        }
        #endregion // Ctor(s)

        #region Method(s)

        public bool CellAtPt(Point pt, out int col, out int row)
        {
            col = row = -1; // Assume failure.

            // Are we in ruler?
            if (pt.X - RulerOffsetX < 0 || pt.Y - RulerOffsetY < 0) return false;

            // In cell?
            if ((pt.X - RulerOffsetX) / _cellWidth + 1 <= _columns
                && (pt.Y - RulerOffsetY) / _cellHeight + 1 <= _rows)
            {
                col = (pt.X - RulerOffsetX) / _cellWidth;
                row = (pt.Y - RulerOffsetY) / _cellHeight;
                return true;
            }
            else return false; // We failed.
        }
        #endregion // Method(s)

        #region Properties
        public GridSelection GridSelection
        {
            get { return _gridSelection; }
            set { _gridSelection = value; Invalidate(); }
        }

        public Color RulerBackgroundColor
        {
            get { return _rulerBackgroundColor; }
            set { _rulerBackgroundColor = value; Invalidate(); }
        }

        public int MinorTickSize
        {
            get { return _minorTickSize; }
            set { _minorTickSize = value; Invalidate(); }
        }

        public int MajorTickSize
        {
            get { return _majorTickSize; }
            set { _majorTickSize = value; Invalidate(); }
        }

        public int MinorTicksPerMajorTick
        {
            get { return _minorTicksPerMajorTick; }
            set { _minorTicksPerMajorTick = value; Invalidate(); }
        }

        public int RulerWidth
        {
            get { return _rulerWidth; }
            set { _rulerWidth = value; UpdateScrollbars(); }
        }

        public int RulerHeight
        {
            get { return _rulerHeight; }
            set { _rulerHeight = value; UpdateScrollbars(); }
        }

        public bool ShowHorzRuler
        {
            get { return _showHorzRuler; }
            set { _showHorzRuler = value; UpdateScrollbars(); }
        }

        public bool ShowVertRuler
        {
            get { return _showVertRuler; }
            set { _showVertRuler = value; UpdateScrollbars(); }
        }

        public float[] GridEdgeLineDashPattern
        {
            get { return _gridEdgeLineDashPattern; }
            set { _gridEdgeLineDashPattern = value; Invalidate(); }
        }

        public Color GridEdgeLineColor
        {
            get { return _gridEdgeLineColor; }
            set { _gridEdgeLineColor = value; Invalidate(); }
        }

        public float[] GridTickLineDashPattern
        {
            get { return _gridTickLineDashPattern; }
            set { _gridTickLineDashPattern = value; Invalidate(); }
        }

        public Color GridTickLineColor
        {
            get { return _gridTickLineColor; }
            set { _gridTickLineColor = value; Invalidate(); }
        }

        public int CellWidth
        {
            get { return _cellWidth; }
            set { if (value <= 0) value = 1; _cellWidth = value; UpdateScrollbars(); }
        }

        public int CellHeight
        {
            get { return _cellHeight; }
            set { if (value <= 0) value = 1; _cellHeight = value; UpdateScrollbars(); }
        }

        public int Rows
        {
            get { return _rows; }
            set { if (value <= 0) value = 1; _rows = value; UpdateScrollbars(); }
        }

        public int Columns
        {
            get { return _columns; }
            set { if (value <= 0) value = 1; _columns = value; UpdateScrollbars(); }
        }

        public int RightMargin
        {
            get { return _rightMargin; }
            set { _rightMargin = value; Invalidate(); }
        }

        public int LeftMargin
        {
            get { return _leftMargin; }
            set { _leftMargin = value; Invalidate(); }
        }

        public int BottomMargin
        {
            get { return _bottomMargin; }
            set { _bottomMargin = value; Invalidate(); }
        }

        public int TopMargin
        {
            get { return _topMargin; }
            set { _topMargin = value; Invalidate(); }
        }

        public int MarginLineThickness
        {
            get { return _marginLineThickness; }
            set { _marginLineThickness = value; Invalidate(); }
        }

        public Rectangle ViewportRect
        {
            get
            {
                // Get visible cells.
                if (!_contentVisible)
                    return Rectangle.Empty;
                else
                    return new Rectangle(_firstVisibleColumn, _firstVisibleRow, _lastVisibleColumn - _firstVisibleColumn + 1, _lastVisibleRow - _firstVisibleRow + 1);
            }
        }

        // Calc. properties.
        public int TotalGridWidth { get { return RulerOffsetX + _columns * _cellWidth; } }
        public int TotalGridHeight { get { return RulerOffsetY + _rows * _cellHeight; } }

        public Image SourceImage
        {
            get { return _sourceImage; }
            set { _sourceImage = value; Invalidate(); }
        }

        #endregion // Properties

        #region Event(s)

        // Zooms.
        public event EventHandler<ZoomInArgs> ZoomIn;
        private void OnZoomIn(ZoomInArgs args)
        { if (ZoomIn != null) ZoomIn(this, args); }
        public event EventHandler<ZoomOutArgs> ZoomOut;
        private void OnZoomOut(ZoomOutArgs args)
        { if (ZoomOut != null) ZoomOut(this, args); }

        // Cell Clicked event.
        public event EventHandler<CellMouseButtonArgs> CellClicked;
        private void OnCellClicked(CellMouseButtonArgs args)
        { if (CellClicked != null) CellClicked(this, args); }

        // Mouse down... 
        public event EventHandler<CellMouseButtonArgs> CellMouseDown;
        private void OnCellMouseDown(CellMouseButtonArgs args)
        { if (CellMouseDown != null) CellMouseDown(this, args); }

        // ...and up.
        public event EventHandler<CellMouseButtonArgs> CellMouseUp;
        private void OnCellMouseUp(CellMouseButtonArgs args)
        { if (CellMouseUp != null) CellMouseUp(this, args); }

        // Cell paint event.
        public event EventHandler<CellPaintArgs> CellPaint;
        private void OnCellPaint(CellPaintArgs args)
        { if (CellPaint != null) CellPaint(this, args); }

        // Blit event.
        public event EventHandler<BlitArgs> Blit;
        private void OnBlit(BlitArgs args)
        { if (Blit != null) Blit(this, args); }

        // Cell paint event.
        public event EventHandler<CellMousePosArgs> CellMouseMove;
        private void OnCellMouseMove(CellMousePosArgs args)
        { if (CellMouseMove != null) CellMouseMove(this, args); }

        // ViewportRect changed event.
        public event EventHandler<ViewportRectChangedArgs> ViewportRectChanged;
        private void OnViewportRectChanged(ViewportRectChangedArgs args)
        { if (ViewportRectChanged != null) ViewportRectChanged(this, args); }
        #endregion // Event(s)

        #region Override(s)
        protected override void OnScroll(ScrollEventArgs e)
        {
            if (e.Type == ScrollEventType.First)
            {
                LockWindowUpdate(Handle);
            }
            else
            {
                LockWindowUpdate(IntPtr.Zero);
                Update();
                if (e.Type != ScrollEventType.Last
                    && e.Type != ScrollEventType.ThumbPosition) LockWindowUpdate(Handle);
            }
            OnViewportRectChanged(new ViewportRectChangedArgs(ViewportRect));
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (Form.ModifierKeys.HasFlag(Keys.Control))
            { // Zoom.
                if (e.Delta > 60)
                    OnZoomIn(new ZoomInArgs());
                else if (e.Delta < 60)
                    OnZoomOut(new ZoomOutArgs());
            }
            else
            { // Scroll.
                LockWindowUpdate(Handle);
                base.OnMouseWheel(e);
                LockWindowUpdate(IntPtr.Zero);
                OnViewportRectChanged(new ViewportRectChangedArgs(ViewportRect));
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            OnViewportRectChanged(new ViewportRectChangedArgs(ViewportRect));
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        { e.Graphics.Clear(BackColor); }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Respect scrolling.
            e.Graphics.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);

            // Calculate visibles.
            _contentVisible = VisibleCells(out _firstVisibleColumn, out _firstVisibleRow, out _lastVisibleColumn, out _lastVisibleRow);

            // Don't smooth enlarged pixels.
            e.Graphics.SmoothingMode = SmoothingMode.None; e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            // Minimal size of cell.
            int minCellSize = Math.Min(CellHeight, CellWidth);

            // Draw.
            BlitCells(e.Graphics);

            if (minCellSize >= MIN_CELL_SIZE_FOR_GRID) DrawGridTicks(e.Graphics);
            DrawMarginLines(e.Graphics);
            DrawGridEdges(e.Graphics);
            DrawGridSelection(e.Graphics);

            // No scrolling. Rulers are always on top.
            e.Graphics.TranslateTransform(-AutoScrollPosition.X, -AutoScrollPosition.Y);
            Rectangle rulerBounds = RulerDimensions();
            if (minCellSize >= MIN_CELL_SIZE_FOR_GRID && _showVertRuler) { DrawVertRuler(e.Graphics, rulerBounds); DrawVertTicks(e.Graphics, rulerBounds); }
            if (minCellSize >= MIN_CELL_SIZE_FOR_GRID && _showHorzRuler) { DrawHorzRuler(e.Graphics, rulerBounds); DrawHorzTicks(e.Graphics, rulerBounds); }
            if (minCellSize >= MIN_CELL_SIZE_FOR_GRID && (_showVertRuler || _showHorzRuler)) { DrawRulerSquare(e.Graphics); }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            // Generate the cell click event.
            int row, column;
            Point pt = e.Location;
            pt.Offset(-AutoScrollPosition.X, -AutoScrollPosition.Y);
            if (!CellAtPt(pt, out column, out row))
                base.OnMouseDown(e);
            else
                OnCellClicked(new CellMouseButtonArgs(pt, column, row, e.Button));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        { MousePosEvent(e, OnCellMouseMove); base.OnMouseMove(e); }

        protected override void OnMouseDown(MouseEventArgs e)
        { MouseButtonEvent(e, OnCellMouseDown); base.OnMouseDown(e); }

        protected override void OnMouseUp(MouseEventArgs e)
        { MouseButtonEvent(e, OnCellMouseUp); base.OnMouseUp(e); }
        #endregion // Override(s)

        #region Helper(s)
        private Rectangle RulerDimensions()
        {
            if (!_contentVisible)
                return Rectangle.Empty;

            // Now we know which cells are that. So let's calculate their position.
            int x, y, x2, y2;
            x = 0;
            x2 = ClientRectangle.X + RulerOffsetX + (_lastVisibleColumn - _firstVisibleColumn + 1) * _cellWidth;
            y = 0;
            y2 = ClientRectangle.Y + RulerOffsetY + (_lastVisibleRow - _firstVisibleRow + 1) * _cellHeight;
            return new Rectangle(x, y, x2 - x + 1, y2 - y + 1);
        }

        private int RulerOffsetX
        {
            get
            {
                int minCellSize = Math.Min(CellWidth, CellHeight);
                return minCellSize >= MIN_CELL_SIZE_FOR_GRID && _showVertRuler ? _rulerWidth : 0;
            }
        }

        private int RulerOffsetY
        {
            get
            {
                int minCellSize = Math.Min(CellWidth, CellHeight);
                return minCellSize >= MIN_CELL_SIZE_FOR_GRID && _showHorzRuler ? _rulerHeight : 0;
            }
        }

        private bool VisibleCells(out int startCol, out int startRow, out int endCol, out int endRow)
        {
            startCol = startRow = endCol = endRow = -1; // Assume failure.
            if (!CellAtPt(new Point(-AutoScrollPosition.X + RulerOffsetX, -AutoScrollPosition.Y + RulerOffsetY), out startCol, out startRow)) return false;
            if ((ClientRectangle.Right - AutoScrollPosition.X + RulerOffsetX) / _cellWidth + 1 <= _columns)
                endCol = (ClientRectangle.Right - AutoScrollPosition.X + RulerOffsetX) / _cellWidth;
            else
                endCol = Columns - 1;
            if ((ClientRectangle.Bottom - AutoScrollPosition.Y + RulerOffsetY) / _cellHeight + 1 <= _rows)
                endRow = (ClientRectangle.Bottom - AutoScrollPosition.Y + RulerOffsetY) / _cellHeight;
            else
                endRow = Rows - 1;

            return true;
        }

        private void UpdateScrollbars()
        {
            AutoScrollMinSize = new Size(TotalGridWidth + 1, TotalGridHeight + 1);
            HorizontalScroll.SmallChange = _cellWidth;
            HorizontalScroll.LargeChange = _cellWidth * _minorTicksPerMajorTick;
            VerticalScroll.SmallChange = _cellHeight;
            VerticalScroll.LargeChange = _cellHeight * _minorTicksPerMajorTick;
            Invalidate();
        }

        private void MousePosEvent(MouseEventArgs e, Action<CellMousePosArgs> action)
        {
            int row, column;
            Point pt = e.Location;
            pt.Offset(-AutoScrollPosition.X, -AutoScrollPosition.Y);
            CellAtPt(pt, out column, out row);
            action(new CellMousePosArgs(pt, column, row));
        }

        private void MouseButtonEvent(MouseEventArgs e, Action<CellMouseButtonArgs> action)
        {
            int row, column;
            Point pt = e.Location;
            pt.Offset(-AutoScrollPosition.X, -AutoScrollPosition.Y);
            CellAtPt(pt, out column, out row);
            action(new CellMouseButtonArgs(pt, column, row, e.Button));
        }

        private void DrawGridTicks(Graphics g)
        {
            // Interested only in visible ticks.

            using (Pen pen = new Pen(_gridTickLineColor))
            {
                pen.DashPattern = _gridTickLineDashPattern;
                for (int column = _firstVisibleColumn; column <= _lastVisibleColumn; column++)
                    g.DrawLine(pen, new Point(RulerOffsetX + column * _cellWidth, RulerOffsetY), new Point(RulerOffsetX + column * _cellWidth, TotalGridHeight));
                for (int row = _firstVisibleRow; row <= _lastVisibleRow; row++)
                    g.DrawLine(pen, new Point(RulerOffsetX, RulerOffsetY + row * _cellHeight), new Point(TotalGridWidth, RulerOffsetY + row * _cellHeight));
            }
        }

        private void DrawGridSelection(Graphics g)
        {
            if (_gridSelection != null)
            {
                using (Pen linePen = new Pen(_gridSelection.LineColor, _gridSelection.LineWidth))
                {
                    List<Point> poly = new List<Point>();

                    foreach (Point p in GridSelection.Poly)
                        poly.Add(new Point(
                            RulerOffsetX + p.X * _cellWidth,
                            RulerOffsetY + p.Y * _cellHeight
                        ));

                    g.DrawPolygon(linePen, poly.ToArray());
                }
            }
        }

        private void DrawGridEdges(Graphics g)
        {
            using (Pen pen = new Pen(_gridEdgeLineColor))
            {
                try { pen.DashPattern = _gridEdgeLineDashPattern; } catch { pen.DashStyle = DashStyle.Solid; } // Ignore bad patterns.
                g.DrawRectangle(pen, RulerOffsetX, RulerOffsetY, TotalGridWidth - RulerOffsetX, TotalGridHeight - RulerOffsetY);
            }
        }

        /*
        private void DrawCells(Graphics g)
        {

            for (int column = _firstVisibleColumn; column <= _lastVisibleColumn; column++)
                for (int row = _firstVisibleRow; row <= _lastVisibleRow; row++)
                {
                    // Calculate cell rectangle.
                    Rectangle rect = new Rectangle(
                        RulerOffsetX + column * _cellWidth + 1,
                        RulerOffsetY + row * _cellHeight + 1,
                        _cellWidth - 1,
                        _cellHeight - 1
                    );

                    // Are we inside margins?
                    bool isMargin = row < TopMargin || row >= Rows - BottomMargin || column < LeftMargin || column >= Columns - RightMargin;

                    // And raise draw event.
                    OnCellPaint(new CellPaintArgs(g, rect, column, row, isMargin));
                }
        }
        */

        private void BlitCells(Graphics g)
        {

            if (Blit != null)
            {
                // Store transforms.
                var arch = g.Save();

                // No transform.
                g.ResetTransform();

                // Create args.
                var args = new BlitArgs(
                    g,
                    new Size(_cellWidth, _cellHeight),
                    new Rectangle(_firstVisibleColumn, _firstVisibleRow, _lastVisibleColumn - _firstVisibleColumn + 1, _lastVisibleRow - _firstVisibleRow + 1),
                    RulerOffsetX + 1,
                    RulerOffsetY + 1
                );
                OnBlit(args);

                // Restore.
                g.Restore(arch);
            }
        }

        private void DrawMarginLines(Graphics g)
        {
            Rectangle marginRect = new Rectangle(
                RulerOffsetX + LeftMargin * _cellWidth,
                RulerOffsetY + TopMargin * _cellHeight,
                (Columns - RightMargin - LeftMargin) * _cellWidth,
                (Rows - BottomMargin - TopMargin) * _cellHeight
            );

            using (Pen p = new Pen(Color.Red, MarginLineThickness))
                g.DrawRectangle(p, marginRect);

        }

        private void DrawVertRuler(Graphics g, Rectangle bounds)
        {
            // Clear background.
            Rectangle fullRectangle = new Rectangle(0, 0, RulerOffsetX, ClientRectangle.Height);
            using (Brush fullBackBrush = new SolidBrush(_rulerBackgroundColor))
                g.FillRectangle(fullBackBrush, fullRectangle);

            // Now, we have to draw as if cut at the right place. 
            // So instead of limited draw we draw fill, but clip.
            var store = g.Clip;
            Rectangle cliprect = new Rectangle(0, bounds.Y, RulerWidth, bounds.Height);
            g.SetClip(cliprect);

            // Draw ruler.
            Rectangle stripInnerRectVert = new Rectangle(0, RulerOffsetY, RulerOffsetX, ClientRectangle.Height - RulerOffsetY - 1);
            stripInnerRectVert.Inflate(-2, 0);
            // Strips.
            using (Brush backBrush = new SolidBrush(BackColor))
            {
                g.FillRectangle(backBrush, stripInnerRectVert);
                ControlPaint.DrawBorder3D(g, stripInnerRectVert, Border3DStyle.SunkenInner);
            }

            // Restore clipping.
            g.Clip = store;
        }

        private void DrawHorzRuler(Graphics g, Rectangle bounds)
        {
            // Clear background.
            Rectangle fullRectangle = new Rectangle(0, 0, ClientRectangle.Width, RulerOffsetY);
            using (Brush fullBackBrush = new SolidBrush(_rulerBackgroundColor))
                g.FillRectangle(fullBackBrush, fullRectangle);

            // Now, we have to draw as if cut at the right place. 
            // So instead of limited draw we draw fill, but clip.
            var store = g.Clip;
            Rectangle cliprect = new Rectangle(bounds.X, 0, bounds.Width, RulerHeight);
            g.SetClip(cliprect);

            // Two rectangles.
            Rectangle stripInnerRectHorz = new Rectangle(RulerOffsetX, 0, ClientRectangle.Width - RulerOffsetX - 1, RulerOffsetY);
            stripInnerRectHorz.Inflate(0, -2);

            // Strips.
            using (Brush backBrush = new SolidBrush(BackColor))
            {
                g.FillRectangle(backBrush, stripInnerRectHorz);
                ControlPaint.DrawBorder3D(g, stripInnerRectHorz, Border3DStyle.SunkenInner);
            }

            // Restore clipping.
            g.Clip = store;

        }

        private void DrawHorzTicks(Graphics g, Rectangle bounds)
        {
            // Now, we have to draw as if cut at the right place. 
            // So instead of limited draw we draw fill, but clip.
            var store = g.Clip;
            Rectangle cliprect = new Rectangle(bounds.X, 0, bounds.Width, RulerHeight);
            g.SetClip(cliprect);

            // First horizontal ticks.
            int x = RulerOffsetX + AutoScrollPosition.X;
            int tick = 0;

            using (Pen pen = new Pen(ForeColor))
            using (Brush brush = new SolidBrush(ForeColor))
                while (x < ClientRectangle.Width)
                {
                    if (tick % _minorTicksPerMajorTick == 0)
                    {
                        // Draw major tick.
                        g.DrawLine(pen, new Point(x, _rulerHeight - 3), new Point(x, _rulerHeight - MajorTickSize - 3));
                        // And string, but not the first one.
                        if (tick != 0)
                        {
                            string s = tick.ToString();
                            SizeF size = g.MeasureString(s, Font);
                            g.DrawString(s, Font, brush, new Point(x - (int)Math.Round(size.Width + 1), 0));
                        }
                    }
                    else
                        g.DrawLine(pen, new Point(x, _rulerHeight - 3), new Point(x, _rulerHeight - MinorTickSize - 3));

                    x += _cellWidth;
                    tick++;
                }

            // Restore clipping.
            g.Clip = store;
        }

        private void DrawVertTicks(Graphics g, Rectangle bounds)
        {
            // Now, we have to draw as if cut at the right place. 
            // So instead of limited draw we draw fill, but clip.
            var store = g.Clip;
            Rectangle cliprect = new Rectangle(0, bounds.Y, RulerWidth, bounds.Height);
            g.SetClip(cliprect);

            // First horizontal ticks.
            int y = RulerOffsetY + AutoScrollPosition.Y;
            int tick = 0;

            using (Pen pen = new Pen(ForeColor))
            using (Brush brush = new SolidBrush(ForeColor))
            using (StringFormat sf = new StringFormat())
                while (y < ClientRectangle.Height)
                {
                    if (tick % _minorTicksPerMajorTick == 0)
                    {
                        // Major line.
                        g.DrawLine(pen, new Point(3, y), new Point(_rulerWidth - 3, y));

                        // And string, but not the first one.
                        if (tick != 0)
                        {
                            string s = tick.ToString();
                            SizeF size = g.MeasureString(s, Font);
                            RectangleF textRect = new RectangleF(new PointF(0, y), size);
                            Matrix current = g.Transform; // Store current transform.

                            g.TranslateTransform(-y - 1, y + 1);
                            g.RotateTransform(-90);
                            g.DrawString(s, Font, brush, textRect, sf);

                            g.Transform = current; // Restore current transform.
                        }
                    }
                    else
                        g.DrawLine(pen, new Point(_rulerWidth - 3, y), new Point(_rulerWidth - 3 - MinorTickSize, y));

                    y += _cellHeight;
                    tick++;
                }

            // Restore clipping.
            g.Clip = store;
        }


        private void DrawRulerSquare(Graphics g)
        {
            // Clear square.
            Rectangle square = new Rectangle(0, 0, RulerOffsetX, RulerOffsetY);
            using (Brush fullBackBrush = new SolidBrush(_rulerBackgroundColor))
                g.FillRectangle(fullBackBrush, square);
        }
        #endregion // Helper(s)

        #region Win32

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool LockWindowUpdate(IntPtr hWnd);

        #endregion // Win32
    }

    public class GridSelection
    {
        public GridSelection() { LineWidth = 3; LineColor = Color.Black; }
        public Point[] Poly { get; set; }
        public Color LineColor { get; set; }
        public int LineWidth { get; set; }
    }

    public class ZoomInArgs : EventArgs { }
    public class ZoomOutArgs : EventArgs { }

    public class ViewportRectChangedArgs : EventArgs
    {
        #region Private Member(s)
        private Rectangle _viewportRect;
        #endregion // Private Member(s)

        #region Internal Ctor
        internal ViewportRectChangedArgs(Rectangle viewportRect) { _viewportRect = viewportRect; }
        #endregion // Internal Ctor

        #region Properties
        public Rectangle ViewportRect { get { return _viewportRect; } }
        #endregion // Properties
    }

    public class CellMousePosArgs : EventArgs
    {
        #region Private Member(s)
        private int _column;
        private int _row;
        private Point _location;
        #endregion // Private Member(s)

        #region Internal Ctor
        internal CellMousePosArgs(Point location, int column, int row) { _location = location; _column = column; _row = row; }
        #endregion // Internal Ctor

        #region Properties
        public int Column { get { return _column; } }
        public int Row { get { return _row; } }
        public Point Location { get { return _location; } }
        #endregion // Properties
    }

    public class CellMouseButtonArgs : CellMousePosArgs
    {
        #region Private Member(s)
        private MouseButtons _button;
        #endregion // Private Member(s)

        #region Internal Ctor
        internal CellMouseButtonArgs(Point location, int column, int row, MouseButtons button) : base(location, column, row)
        { _button = button; }
        #endregion // Internal Ctor

        #region Properties
        public MouseButtons Button { get { return _button; } }
        #endregion // Properties
    }

    public class CellPaintArgs : EventArgs
    {
        #region Private Member(s)
        private Graphics _graphics;
        private int _column;
        private int _row;
        private Rectangle _rectangle;
        private bool _isMargin;
        #endregion // Private Member(s)

        #region Internal Ctor
        internal CellPaintArgs(Graphics graphics, Rectangle rectangle, int column, int row, bool isMargin)
        {
            _graphics = graphics; _rectangle = rectangle; _column = column; _row = row; _isMargin = isMargin;
        }
        #endregion // Internal Ctor

        #region Properties
        public Graphics Graphics { get { return _graphics; } }
        public Rectangle Rectangle { get { return _rectangle; } }
        public int Column { get { return _column; } }
        public int Row { get { return _row; } }
        public bool IsMargin { get { return _isMargin; } }
        #endregion // Properties
    }

    public class BlitArgs : EventArgs
    {
        #region Private Member(s)
        private Graphics _g;
        private int _x;
        private int _y;
        private Rectangle _sourceRectangle;
        private Size _cellSize;
        #endregion // Private Member(s)

        #region Internal Ctor
        public BlitArgs(Graphics g, Size cellSize, Rectangle sourceRectangle, int x, int y)
        {
            _g = g;
            _sourceRectangle = sourceRectangle;
            _cellSize = cellSize;
            _x = x;
            _y = y;
        }
        #endregion // Internal Ctor

        #region Properties
        public Graphics Graphics { get { return _g; } }
        public Rectangle SourceRectangle { get { return _sourceRectangle; } }
        public int X { get { return _x; } }
        public int Y { get { return _y; } }
        public Size CellSize { get { return _cellSize; } }
        #endregion // Properties
    }
}