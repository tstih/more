/*
 * Navigator.cs
 * 
 * Classic left collapsible NavBar for WinForms.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 05.02.2022   tstih
 * 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace More.Windows.Forms
{
    public class Navigator : ControlBase
    {
        #region Const(s)
        private const int DEFAULT_ITEM_HEIGHT = 32;
        private const int DEFAULT_TITLE_HEIGHT = 48;
        private const int DEFAULT_FONT_SIZE = 11;
        private const int DEFAULT_TITLE_FONT_SIZE = 14;
        private static readonly Color DEFAULT_BACK_COLOR = 
            Color.FromArgb(64,16,64);
        private static readonly Color DEFAULT_FORE_COLOR =
            Color.FromArgb(192, 176, 192);
        private static readonly Color DEFAULT_TITLE_FORE_COLOR =
            Color.FromArgb(224, 224, 224);
        private static readonly Color DEFAULT_TITLE_SEPARATOR_COLOR =
            Color.FromArgb(96, 32, 96);
        private static readonly Color DEFAULT_HOVER_BACK_COLOR =
            Color.FromArgb(96, 32, 96);
        private static readonly Color DEFAULT_HOVER_FORE_COLOR =
            Color.FromArgb(192, 176, 192);
        private static readonly Color DEFAULT_SELECTED_BACK_COLOR =
    Color.FromArgb(16, 100, 164);
        private static readonly Color DEFAULT_SELECTED_FORE_COLOR =
            Color.FromArgb(255,255,255);
        private const int DEFAULT_COLLAPSE_WIDTH = 48;
        private const int DEFAULT_ITEM_INDENT = 8;
        #endregion // Const(s)

        #region Private(s)
        // Rect 1=hit rect, Rect 2=draw rect
        private List<Tuple<Rectangle, Rectangle, NavigatorItemBase>> _itemsRects;
        private Rectangle? _titleRect;
        private NavigatorItemBase _hoverItem;
        private NavigatorItemBase _selectedItem;
        #endregion // Private(s)

        #region Ctor(s)
        public Navigator()
        {
            // Default properties.
            SetDefaults();
        }

        private void SetDefaults()
        {
            // Status vars.
            _hoverItem = null;
            _selectedItem = null;

            // Property vars.
            _itemHeight = DEFAULT_ITEM_HEIGHT;
            _titleHeight = DEFAULT_TITLE_HEIGHT;
            _collapseWidth = DEFAULT_COLLAPSE_WIDTH;
            _itemIndent = DEFAULT_ITEM_INDENT;
            _itemsRects = new List<Tuple<Rectangle, Rectangle, NavigatorItemBase>>();
            _hoverBackColor = DEFAULT_HOVER_BACK_COLOR;
            _hoverForeColor = DEFAULT_HOVER_FORE_COLOR;
            _selectedBackColor = DEFAULT_SELECTED_BACK_COLOR;
            _selectedForeColor = DEFAULT_SELECTED_FORE_COLOR;
            _activeCategories = true;

            // Derived properties.
            Font = new Font(Font.FontFamily, DEFAULT_FONT_SIZE);
            TitleFont = new Font(Font.FontFamily, DEFAULT_TITLE_FONT_SIZE, FontStyle.Bold);
            ForeColor = DEFAULT_FORE_COLOR;
            BackColor = DEFAULT_BACK_COLOR;
            TitleForeColor = DEFAULT_TITLE_FORE_COLOR;
            TitleSeparatorColor = DEFAULT_TITLE_SEPARATOR_COLOR;
        }
        #endregion // Ctor(s)

        #region Method(s)
        private IEnumerable<NavigatorItemBase> _root;
        public void SetRoot(IEnumerable<NavigatorItemBase> root)
        {
            _root = root;
            Invalidate();
        }
        #endregion // Method(s)

        #region Overrides(s)
        protected override void OnMarginChanged(EventArgs e)
        {
            // Refresh.
            Invalidate();
            // And call base.
            base.OnMarginChanged(e);
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            // Refresh.
            Invalidate();
            // And call base.
            base.OnPaddingChanged(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            // Refresh.
            Invalidate();
            // And call base.
            base.OnTextChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            PaintTitle(e.Graphics);
            if (_root != null)
                DrawItems(e.Graphics, _root);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            // Not hovering over anything.
            _hoverItem = null;
            Invalidate();
            // Call base.
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Are we hovering over an item?
            NavigatorItemBase hoverItem = null;
            foreach (var itemRect in _itemsRects)
                if (itemRect.Item1.Contains(e.Location)
                    && (
                        !(itemRect.Item3 is NavigatorCategory)
                        || ActiveCategories
                    )
                )
                    hoverItem = itemRect.Item3;

            // Invalidate.
            if (_hoverItem != hoverItem)
            {
                _hoverItem = hoverItem;
                Invalidate();
            }

            // Pass.
            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            // Are we hovering over an item?
            NavigatorItemBase selectedItem = null;
            foreach (var itemRect in _itemsRects)
                if (itemRect.Item1.Contains(e.Location)
                    && (
                        !(itemRect.Item3 is NavigatorCategory)
                        || ActiveCategories
                    )
                )
                    selectedItem = itemRect.Item3;

            // Invalidate.
            if (_selectedItem != selectedItem)
            {
                // Set new selection.
                _selectedItem = selectedItem;
                // Repait navigator.
                Invalidate();
                // Do we need to call the callback?
                if (_selectedItem != null && _selectedItem.Callback != null)
                    _selectedItem.Callback();
            }

            // Pass...
            base.OnMouseDown(e);
        }
        #endregion // Overrides(s)

        #region Properties
        private bool _activeCategories;
        public bool ActiveCategories
        {
            get { return _activeCategories; }
            set
            {
                if (value == _activeCategories) return;
                _activeCategories = value;
                Invalidate();
            }
        }

        private int _itemIndent;
        public int ItemIndent
        {
            get { return _itemIndent; }
            set
            {
                if (value == _itemIndent) return;
                _itemIndent = value;
                Invalidate();
            }
        }

        private int _collapseWidth;
        public int CollapseWidth
        {
            get { return _collapseWidth; }
            set
            {
                if (value == _collapseWidth) return;
                _collapseWidth = value;
                Invalidate();
            }
        }

        private int _itemHeight;
        public int ItemHeight
        {
            get { return _itemHeight; }
            set
            {
                if (value == _itemHeight) return;
                _itemHeight = value;
                Invalidate();
            }
        }

        private int _titleHeight;
        public int TitleHeight
        {
            get { return _titleHeight; }
            set
            {
                if (value == _titleHeight) return;
                _titleHeight = value;
                Invalidate();
            }
        }

        private Font _titleFont;
        public Font TitleFont
        {
            get { return _titleFont; }
            set
            {
                // No change?
                if (value == _titleFont) return;
                if (value == null ) // Reset to default?
                    _titleFont= new Font(
                        DefaultFont.FontFamily, 
                        DEFAULT_TITLE_FONT_SIZE, 
                        FontStyle.Bold);
                else
                    _titleFont = value;
                Invalidate();
            }
        }

        private Color _titleForeColor;
        public Color TitleForeColor
        {
            get { return _titleForeColor; }
            set
            {
                // No change?
                if (value == _titleForeColor) return;
                _titleForeColor = value;
                Invalidate();
            }
        }

        private Color _titleSeparatorColor;
        public Color TitleSeparatorColor
        {
            get { return _titleSeparatorColor; }
            set
            {
                // No change?
                if (value == _titleSeparatorColor) return;
                _titleSeparatorColor = value;
                Invalidate();
            }
        }

        private Color _hoverForeColor;
        public Color HoverForeColor
        {
            get { return _hoverForeColor; }
            set
            {
                // No change?
                if (value == _hoverForeColor) return;
                _hoverForeColor = value;
                Invalidate();
            }
        }

        private Color _hoverBackColor;
        public Color HoverBackColor
        {
            get { return _hoverBackColor; }
            set
            {
                // No change?
                if (value == _hoverBackColor) return;
                _hoverBackColor = value;
                Invalidate();
            }
        }

        private Color _selectedForeColor;
        public Color SelectedForeColor
        {
            get { return _selectedForeColor; }
            set
            {
                // No change?
                if (value == _selectedForeColor) return;
                _selectedForeColor = value;
                Invalidate();
            }
        }

        private Color _selectedBackColor;
        public Color SelectedBackColor
        {
            get { return _selectedBackColor; }
            set
            {
                // No change?
                if (value == _selectedBackColor) return;
                _selectedBackColor = value;
                Invalidate();
            }
        }
        #endregion // Properties

        #region Helper(s)
        private Rectangle AddOffsets(Rectangle rect, Padding padding)
        {
            return new Rectangle(
                rect.Left + padding.Left,
                rect.Top + padding.Top,
                rect.Width - padding.Left - padding.Right,
                rect.Height - padding.Top - padding.Bottom);
        }

        private void DrawItems(
            Graphics g, 
            IEnumerable<NavigatorItemBase> root)
        {
            // Layout items.
            int x = ValidArea.Left
                , y = _titleRect.HasValue?_titleRect.Value.Bottom + 1: ValidArea.Top;
            _itemsRects.Clear();
            LayoutItems(_root, x, y, ValidArea.Width);
            foreach(var itemRect in _itemsRects)
            {
                // Selecting fore and back color for items.
                Color foreColor = ForeColor, backColor = BackColor;
                if (itemRect.Item3==_selectedItem)
                {
                    foreColor = SelectedForeColor;
                    backColor = SelectedBackColor;
                } else if (itemRect.Item3==_hoverItem)
                {
                    foreColor = HoverForeColor;
                    backColor = HoverBackColor;
                }

                // Background rect is not indented.
                using (Brush itemBrush = new SolidBrush(foreColor),
                    backBrush=new SolidBrush(backColor))
                using (StringFormat sf = new StringFormat(
                    StringFormatFlags.NoWrap | StringFormatFlags.FitBlackBox)
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                })
                {
                    g.FillRectangle(backBrush, itemRect.Item1);
                    g.DrawString(
                        itemRect.Item3.Text, 
                        Font, 
                        itemBrush, 
                        itemRect.Item2, 
                        sf);
                }
            }
        }

        private int LayoutItems(IEnumerable<NavigatorItemBase> items, int x, int y, int width)
        {
            foreach(var item in items)
            {
                // Hit rect.
                Rectangle hitRect = new Rectangle(
                    ValidArea.Left,
                    y, 
                    ValidArea.Width, 
                    ItemHeight);
                // item rect.
                Rectangle itemRect = AddOffsets(
                    new Rectangle(x, y, width, ItemHeight)
                    ,Padding);
                AddItemRects(hitRect,itemRect, item);
                y += ItemHeight;
                // If navigation with children?
                NavigatorCategory cat = item as NavigatorCategory;
                if (cat!=null && cat.Children.Count>0)
                    // Recurse.
                    y = LayoutItems(cat.Children,x+ItemIndent,y,width-ItemIndent);
            }
            return y;
        }

        private void AddItemRects(
            Rectangle hitRect, 
            Rectangle itemRect, 
            NavigatorItemBase item)
        {
            _itemsRects.Add(new Tuple<Rectangle, Rectangle, NavigatorItemBase>(
                hitRect,itemRect,item
                ));
        }

        private void PaintTitle(Graphics g)
        {
            if (string.IsNullOrEmpty(Text))
            {
                _titleRect = null;
                return;
            }
            // If we're here, we have a title.
            _titleRect = AddOffsets(
                new Rectangle(
                    ValidArea.Left,
                    ValidArea.Top,
                    ValidArea.Width, 
                    TitleHeight)
                ,Padding);
            using(Brush titleBrush=new SolidBrush(TitleForeColor))
            using(Pen titleSeparatorPen=new Pen(TitleSeparatorColor))
            using(StringFormat sf=new StringFormat(StringFormatFlags.NoWrap|StringFormatFlags.FitBlackBox)
            {
                 Alignment=StringAlignment.Near,
                 LineAlignment=StringAlignment.Center
            })
            {
                g.DrawString(Text, TitleFont, titleBrush, _titleRect.Value, sf);
                g.DrawLine(titleSeparatorPen, 
                    _titleRect.Value.Left, _titleRect.Value.Bottom,
                    _titleRect.Value.Right, _titleRect.Value.Bottom);
            }
        }

        private Rectangle ValidArea
        {
            get
            {
                return AddOffsets(ClientRectangle, Margin);
            }
        }
        #endregion // Helper(s)
    }

    public interface IHasEnd<T>
    {
        T End();
    }

    public class NavigatorBuilder
    {
        private List<NavigatorItemBase> _children;

        public static NavigatorBuilder Fluent()
        { return new NavigatorBuilder(); }

        internal NavigatorBuilder()
        {
            _children = new List<NavigatorItemBase>();
        }

        public FluentNavigatorCategory Begin(string text, Action callBack=null, string key=null)
        {
            FluentNavigatorCategory fluentCategory 
                = new FluentNavigatorCategory(this, key, text, callBack);
            _children.Add(fluentCategory.Category);
            return fluentCategory;
        }

        public IEnumerable<NavigatorItemBase> GetRoot()
        { return _children; }

        public NavigatorBuilder Add(string text, Action callBack=null, Image glyph = null, string key=null)
        {
            _children.Add(new NavigatorItem()
            { Key=key, Text=text, Glyph=glyph, Callback=callBack });
            return this;
        }
    }   

    public class FluentNavigatorCategory
    {
        internal FluentNavigatorCategory(NavigatorBuilder builder, string key, string text, Action callBack=null)
        {
            _builder = builder;
            _category = new NavigatorCategory()
            {
                Key = key,
                Text = text,
                Callback=callBack
            };
        }

        public FluentNavigatorCategory Add(string text, Action callBack=null, string key = null, Image glyph = null)
        {
            _category.Children.Add(new NavigatorItem()
            { Key = key, Text = text, Glyph = glyph, Callback=callBack});
            return this;
        }

        private NavigatorBuilder _builder;
        public NavigatorBuilder End()
        {
            return _builder;
        }

        private NavigatorCategory _category;
        internal NavigatorCategory Category { get { return _category; } }
    }

    public class NavigatorItemBase
    {
        public string Text { get; set; }
        public Action Callback { get; set; }
        public string Key { get; set; }
        public bool Disabled { get; set; }
    }

    public class NavigatorItem : NavigatorItemBase
    {
        public Image Glyph { get; set; }
    }

    public class NavigatorCategory : NavigatorItemBase
    {
        public NavigatorCategory()
        { Children=new List<NavigatorItem>(); }
        public List<NavigatorItem> Children { get; }
    }
}