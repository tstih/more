/*
 * Listing.cs
 * 
 * Code listing.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 03.02.2022   tstih
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace More.Windows.Forms
{
    public class Listing : Control
    {
        #region Const(s)
        private const string DEFAULT_FONT_FAMILY_NAME = "Consolas";
        private const int DEFAULT_FONT_SIZE = 12;
        private const int DEFAULT_ROW_HEIGHT = 14;
        private const int DEFAULT_CELL_WIDTH = 10;
        #endregion Const(s)

        #region Private(s)
        // Vars.
        private IListingFeed _feed;
        // Region properties.
        private int _rowHeight;
        private int _cellWidth;
        private float _topRow;
        private StringAlignment _cellAlign;
        private StringAlignment _cellLineAlign;
        private bool _integralHeight;
        #endregion // Private(s)

        #region Ctor(s)
        public Listing() : base()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw,
                true);

            // Var. props.
            _rowHeight = DEFAULT_ROW_HEIGHT;
            _cellWidth = DEFAULT_CELL_WIDTH;
            _topRow = 0;
            _cellAlign = StringAlignment.Center;
            _cellLineAlign = StringAlignment.Center;
            _integralHeight = true;

            // No tab stop.
            Font = new Font(DEFAULT_FONT_FAMILY_NAME, DEFAULT_FONT_SIZE);
            TabStop = false;
        }
        #endregion // Ctor(s)

        #region Method(s)

        /// <summary>
        /// Populate the tree by givin it data feed.
        /// </summary>
        public void SetFeed(IListingFeed feed)
        {
            // Nothing new.
            if (_feed == feed) return;
            // Store and repaint.
            _feed = feed; Invalidate();
        }
        #endregion // Method(s)

        #region Override(s)
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(BackColor);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // No feed, no fun.
            if (_feed == null) return;

            // How many rows do we fit into the client?
            int rows = ClientRectangle.Height / RowHeight;

            // And draw. Nothing to do, really.
            DrawCells(e.Graphics, TopRow, rows);
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
        /// <summary>
        /// If true then only full rows are displayed.
        /// </summary>
        [Description("If true then only full rows are displayed."), Category("Appearance")]
        public bool IntegralHeight
        {
            get { return _integralHeight; }
            set
            {
                if (value == _integralHeight) return;
                _integralHeight = value; // Override invalid.
                Invalidate();
            }
        }

        /// <summary>
        /// Horizontal alignment of char inside one cell.
        /// </summary>
        [Description("Horizontal alignment of char inside one cell."), Category("Appearance")]
        public StringAlignment CellAlign
        {
            get { return _cellAlign; }
            set
            {
                if (value == _cellAlign) return;
                _cellAlign = value; // Override invalid.
                Invalidate();
            }
        }

        /// <summary>
        /// Vertical alignment of char inside one cell.
        /// </summary>
        [Description("Vertical alignment of char inside one cell."), Category("Appearance")]
        public StringAlignment CellLineAlign
        {
            get { return _cellLineAlign; }
            set
            {
                if (value == _cellLineAlign) return;
                _cellLineAlign = value; // Override invalid.
                Invalidate();
            }
        }

        /// <summary>
        /// Row heigh. This should be sufficient to draw a char of
        /// selected font.
        /// </summary>
        [Description("Row height in pixels"), Category("Appearance")]
        public int RowHeight
        {
            get { return _rowHeight; }
            set
            {
                if (value == _rowHeight) return;
                if (value < 1) _rowHeight = DEFAULT_ROW_HEIGHT; 
                else _rowHeight = value; // Override invalid.
                Invalidate();
            }
        }

        /// <summary>
        /// Cell width. This should be sufficient to draw a char of
        /// selected font.
        /// </summary>
        [Description("Cell width in pixels"), Category("Appearance")]
        public int CellWidth
        {
            get { return _cellWidth; }
            set
            {
                if (value == _cellWidth) return;
                if (value < 1) _cellWidth = DEFAULT_CELL_WIDTH;
                else _cellWidth = value; // Override invalid.
                Invalidate();
            }
        }

        /// <summary>
        /// The top row. Manipulate this to scroll the listing.
        /// </summary>
        [Description("Top row"), Category("Appearance")]
        public float TopRow
        {
            get { return _topRow; }
            set
            {
                if (value == _topRow) return;
                if (value < 1) _topRow = 0;
                else _topRow = value; // Override invalid.
                Refresh();
            }
        }

        /// <summary>
        /// The width in characters.
        /// </summary>
        [Description("How many characters fit on current window."), Category("Appearance")]
        public float WidthInChars
        {
            get {
                return (float)Width / (float)CellWidth;
            }
            set
            {
                Width = (int)Math.Ceiling(value * (float)CellWidth);
                Invalidate();
            }
        }
        #endregion // Properties

        #region Helper(s)
        private void DrawCells(Graphics g, float topRow, int rows)
        {
            // Are we past last row?
            if (topRow > _feed.RowCount()) return;

            // Is the window too big to display all?
            if (_feed.RowCount() - topRow < rows)
                rows = _feed.RowCount() - (int)topRow;

            // Now draw it all.
            int y = ClientRectangle.Top; // TODO: Padding?
            for(int i=(int)topRow; i<topRow+rows; i++)
            {
                // Get the row.
                var row = _feed.QueryRow(i);

                // If row colors, set them. Otherwise inherit from control.
                Color foreColor= row.ForeColor.HasValue ? row.ForeColor.Value : ForeColor;
                Color backColor = row.BackColor.HasValue ? row.BackColor.Value : BackColor;

                // Draw entire row with row color only if color not inherited.
                if (row.BackColor.HasValue)
                {
                    Rectangle rowRect = new Rectangle(ClientRectangle.Left, y, ClientRectangle.Width, RowHeight);
                    using (Brush rowBackBrush = new SolidBrush(backColor))
                        g.FillRectangle(rowBackBrush, rowRect);
                }

                int x = ClientRectangle.Left;
                foreach (var cell in row.Row)
                {
                    // foreColor and backColor are inherited, so use them if cell does not have its own.
                    using (Brush backBrush = new SolidBrush(cell.BackColor.HasValue?cell.BackColor.Value:backColor),
                        foreBrush = new SolidBrush(cell.ForeColor.HasValue?cell.ForeColor.Value:foreColor))
                    {
                        Rectangle cellRect = new Rectangle(x, y, CellWidth, RowHeight);
                        using (StringFormat sf = new StringFormat(
                            StringFormatFlags.FitBlackBox | StringFormatFlags.NoWrap)
                        {
                            Alignment = CellAlign,
                            LineAlignment = CellLineAlign
                        }
                        )
                        {
                            g.FillRectangle(backBrush, cellRect);
                            g.DrawString(cell.Character.ToString(), Font, foreBrush, cellRect, sf);
                        }
                        x += CellWidth;
                    }
                }
                y += RowHeight;
            }
        }
        #endregion // Helper(s)
    }

    public class ListingCell
    {
        public ListingCell(char character, Color? foreColor=null, Color? backColor=null)
        {
            Character = character;
            ForeColor = foreColor;
            BackColor = backColor;
        }

        public char Character { get; set; }
        public Color? BackColor { get; set; }
        public Color? ForeColor { get; set; }
    }

    public class ListingRow
    {
        public ListingRow(string text, Color? foreColor = null, Color? backColor = null) {
            /* Create columns. */
            List<ListingCell> defaultRow = new List<ListingCell>();
            foreach(char character in text)
                defaultRow.Add(new ListingCell(character));
            Row = defaultRow.ToArray();
            /* Remember fore and back color. */
            ForeColor = foreColor;
            BackColor = backColor;
        }

        public ListingCell[] Row { get; set;  }

        public Color? BackColor { get; set; }
        public Color? ForeColor { get; set; }
    }

    public interface IListingFeed
    {
        ListingRow QueryRow(int rowNo);
        int RowCount();
    }
}