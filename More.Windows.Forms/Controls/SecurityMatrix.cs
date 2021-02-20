/*
 * SecurityMatrix.cs
 * 
 * A common matrix with roles on top and permissions
 * on the left. You can map role to permission by using
 * checks (default) or your own mechanism.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 31.12.2020   tstih   Happy new year!
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace More.Windows.Forms
{
    public class SecurityMatrix : ScrollableControl
    {
        #region Const(s)
        // Hardcoded values.
        private const int ROLE_OFFSET = 6;
        private const int CATEGORY_OFFSET = 6;
        private const int PERMISSION_OFFSET = 12;
        // Default values.
        private const float DEFAULT_ROLES_ANGLE = 45.0f;
        private const int DEFAULT_CELL_SIZE = 32;
        private const int DEFAULT_CATEGORY_CELL_HEIGHT = 24;
        private const int DEFAULT_ROLES_HEIGHT = 80;
        private const int DEFAULT_CATEGORIES_WIDTH = 160;
        #endregion // Const(s)

        #region Private Class(es)
        private class DemoFeed : ISecurityMatrixFeed
        {
            public bool this[SecurityRole r, SecurityCategory c, SecurityPermission p]
            {
                get { return true; }
                set { }
            }

            public IEnumerable<SecurityCategory> QueryCategories()
            {
                yield return new SecurityCategory() { Id = "1", DisplayName = "Category 1" };
                yield return new SecurityCategory() { Id = "1", DisplayName = "Category 2" };
            }

            public IEnumerable<SecurityPermission> QueryPermissions(SecurityCategory category)
            {
                if ("1".Equals(category.Id))
                {
                    yield return new SecurityPermission() { Id = "1", DisplayName = "Permission 1.1" };
                    yield return new SecurityPermission() { Id = "2", DisplayName = "Permission 1.2" };
                    yield return new SecurityPermission() { Id = "3", DisplayName = "Permission 1.3" };
                }
                else if ("2".Equals(category.Id))
                {
                    yield return new SecurityPermission() { Id = "1", DisplayName = "Permission 2.1" };
                    yield return new SecurityPermission() { Id = "2", DisplayName = "Permission 2.2" };

                }
            }

            public IEnumerable<SecurityRole> QueryRoles()
            {
                yield return new SecurityRole() { Id = "1", DisplayName = "Role 1" };
                yield return new SecurityRole() { Id = "2", DisplayName = "Role 2" };
                yield return new SecurityRole() { Id = "3", DisplayName = "Role 3" };
                yield return new SecurityRole() { Id = "4", DisplayName = "Role 4" };
                yield return new SecurityRole() { Id = "5", DisplayName = "Role 5" };
            }
        }
        #endregion // Private Class(es)

        #region Private(s)
        private List<Tuple<SecurityRole, Point[]>> _rolePolygons;
        private List<Tuple<SecurityCategory, Rectangle>> _categoryRectangles;
        private List<Tuple<SecurityCategory, SecurityPermission, Rectangle>> _permissionRectangles;
        private List<Tuple<SecurityRole, SecurityCategory, SecurityPermission, Rectangle>> _permissionCellRectangles;
        private ISecurityMatrixFeed _feed;
        private SecurityRole _activeRole;
        private SecurityCategory _activeCategory;
        private SecurityPermission _activePermission;
        #endregion // Private(s)

        #region Ctor
        public SecurityMatrix()
        {
            // Window style.
            SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.ResizeRedraw
                | ControlStyles.OptimizedDoubleBuffer,
                true);

            // Default props and vars.
            SetDefaults();
        }

        private void SetDefaults()
        {
            // Variables.
            _activeCategory = null;
            _activePermission = null;
            _activeRole = null;

            // Properties.
            _rolesAngle = DEFAULT_ROLES_ANGLE; // degrees.
            _cellSize = new Size(DEFAULT_CELL_SIZE, DEFAULT_CELL_SIZE);
            _categoryCellHeight = DEFAULT_CATEGORY_CELL_HEIGHT;
            _rolesHeight = DEFAULT_ROLES_HEIGHT;
            _categoriesWidth = DEFAULT_CATEGORIES_WIDTH;

            // Font properties.
            _roleFont = Font;
            _permissionFont = Font;
            _categoryFont = Font;

            // Role colors.
            _roleBackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
            _roleForeColor = Color.FromKnownColor(KnownColor.WindowText);
            _roleBorderColor = Color.FromKnownColor(KnownColor.Window);

            // Permission colors.
            _permissionForeColor = Color.FromKnownColor(KnownColor.WindowText);
            _permissionCellForeColor = Color.FromKnownColor(KnownColor.ScrollBar);
            _permissionBackColor  = Color.FromKnownColor(KnownColor.Window);
            _permissionCellBackColor = Color.FromKnownColor(KnownColor.Window);
            _permissionTickColor = Color.FromKnownColor(KnownColor.ActiveCaption);
            
            // Category colors.
            _categoryBackColor = Color.FromKnownColor(KnownColor.ScrollBar);
            _categoryCellBackColor = Color.FromKnownColor(KnownColor.ScrollBar);
            _permissionActiveCellBackColor = Color.FromKnownColor(KnownColor.ControlLight);
            _categoryForeColor = Color.FromKnownColor(KnownColor.WindowText);
            _categoryCellForeColor = Color.FromKnownColor(KnownColor.InactiveCaption);

            // Control properties.
            AutoScroll = true;
        }
        #endregion // Ctor

        #region Method(s)
        /// <summary>
        /// Populate the matrix.
        /// </summary>
        public void SetFeed(ISecurityMatrixFeed feed)
        {
            // Nothing new.
            if (_feed == feed) return;
            // Store and repaint.
            _feed = feed; 
            Invalidate();
        }
        #endregion // Method(s)

        #region Properties
        private int _rolesHeight;
        [Description("Height of the roles section (at the top)."), Category("Appearance")]
        public int RolesHeight { get { return _rolesHeight; } set { _rolesHeight = value; Invalidate(); } }

        private double _rolesAngle;
        [Description("Angle of the category. Valid values are between 25 and 90 degrees."), Category("Appearance")]
        public double RolesAngle
        {
            get { return _rolesAngle; }
            set
            {
                if (value < 25) _rolesAngle = 25;
                else if (value > 90) _rolesAngle = 90;
                else _rolesAngle = value;
                Invalidate();
            }
        }

        private Font _roleFont;
        [Description("Font for the role text."), Category("Appearance")]
        public Font RoleFont { get { return _roleFont; } set { _roleFont = value; Invalidate(); } }

        private Color _roleForeColor;
        [Description("Fore color of the role. Used for role text."), Category("Appearance")]
        public Color RoleForeColor { get { return _roleForeColor; } set { _roleForeColor = value; Invalidate(); } }

        private Color _roleBackColor;
        [Description("Back color of the role."), Category("Appearance")]
        public Color RoleBackColor { get { return _roleBackColor; } set { _roleBackColor = value; Invalidate(); } }

        private Color _roleBorderColor;
        [Description("Color of border around role."), Category("Appearance")]
        public Color RoleBorderColor { get { return _roleBorderColor; } set { _roleBorderColor = value; Invalidate(); } }

        private Font _categoryFont;
        [Description("Font for the category text."), Category("Appearance")]
        public Font CategoryFont { get { return _categoryFont; } set { _categoryFont = value; Invalidate(); } }

        private int _categoryCellHeight;
        [Description("Height of the category."), Category("Appearance")]
        public int CategoryCellHeight { get { return _categoryCellHeight; } set { _categoryCellHeight = value; Invalidate(); } }

        private int _categoriesWidth;
        [Description("Width of the categories (on the left)."), Category("Appearance")]
        public int CategoriesWidth { get { return _categoriesWidth; } set { _categoriesWidth = value; Invalidate(); } }

        private Color _categoryForeColor;
        [Description("Fore color of the category. Used for category text."), Category("Appearance")]
        public Color CategoryForeColor { get { return _categoryForeColor; } set { _categoryForeColor = value; Invalidate(); } }

        private Color _categoryBackColor;
        [Description("Back color of the category."), Category("Appearance")]
        public Color CategoryBackColor { get { return _categoryBackColor; } set { _categoryBackColor = value; Invalidate(); } }

        private Color _categoryCellForeColor;
        [Description("Fore color of the category cell."), Category("Appearance")]
        public Color CategoryCellForeColor { get { return _categoryCellForeColor; } set { _categoryCellForeColor = value; Invalidate(); } }

        private Color _categoryCellBackColor;
        [Description("Back color of the category cell."), Category("Appearance")]
        public Color CategoryCellBackColor { get { return _categoryCellBackColor; } set { _categoryCellBackColor = value; Invalidate(); } }

        private Font _permissionFont;
        [Description("Font for the permission text."), Category("Appearance")]
        public Font PermissionFont { get { return _permissionFont; } set { _permissionFont = value; Invalidate(); } }

        private Color _permissionForeColor;
        [Description("Fore color of the permission."), Category("Appearance")]
        public Color PermissionForeColor { get { return _permissionForeColor; } set { _permissionForeColor = value; Invalidate(); } }

        private Color _permissionBackColor;
        [Description("Back color of the permission (title)."), Category("Appearance")]
        public Color PermissionBackColor { get { return _permissionBackColor; } set { _permissionBackColor = value; Invalidate(); } }

        private Color _permissionActiveCellBackColor;
        [Description("Back color of the permission cell, when active."), Category("Appearance")]
        public Color PermissionActiveCellBackColor { get { return _permissionActiveCellBackColor; } set { _permissionActiveCellBackColor = value; Invalidate(); } }

        private Color _permissionCellForeColor;
        [Description("Fore color of the permission cell."), Category("Appearance")]
        public Color PermissionCellForeColor { get { return _permissionCellForeColor; } set { _permissionCellForeColor = value; Invalidate(); } }

        private Color _permissionCellBackColor;
        [Description("Back color of the permission cell."), Category("Appearance")]
        public Color PermissionCellBackColor { get { return _permissionCellBackColor; } set { _permissionCellBackColor = value; Invalidate(); } }

        private Color _permissionTickColor;
        [Description("If permission is set on, what color is the tick?"), Category("Appearance")]
        public Color PermissionTickColor { get { return _permissionTickColor; } set { _permissionTickColor = value; Invalidate(); } }

        private Size _cellSize;
        [Description("Size of the permission cell."), Category("Appearance")]
        public Size CellSize { get { return _cellSize; } set { _cellSize = value; Invalidate(); } }
        #endregion // Properties

        #region Override(s)
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);
            // Draw categories.
            DrawGrid(g);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(BackColor);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Only process when we have a feed.
            if (_feed != null)
            {
                Point pt = new Point(
                    e.Location.X - AutoScrollPosition.X,
                    e.Location.Y - AutoScrollPosition.Y);

                foreach (var pcrect in _permissionCellRectangles)
                    if (pcrect.Item4.Contains(pt))
                    {
                        _activeCategory = pcrect.Item2;
                        _activePermission = pcrect.Item3;
                        _activeRole = pcrect.Item1;
                        Invalidate();
                        break;
                    }
            }
            // Call base.
            base.OnMouseMove(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            // Only process when we have a feed.
            if (_feed != null)
            {
                Point pt = new Point(
                e.Location.X - AutoScrollPosition.X,
                e.Location.Y - AutoScrollPosition.Y);

                foreach (var pcrect in _permissionCellRectangles)
                    if (pcrect.Item4.Contains(pt))
                    {
                        _feed[pcrect.Item1, pcrect.Item2, pcrect.Item3] =
                            !_feed[pcrect.Item1, pcrect.Item2, pcrect.Item3];
                        Invalidate();
                        break;
                    }
            }

            // Base.
            base.OnMouseClick(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            // Only process when we have a feed.
            if (_feed != null)
            {
                // Deactivate.
                _activeCategory = null;
                _activePermission = null;
                _activeRole = null;

                // Repaint.
                Invalidate();
            }

            // Call base.
            base.OnMouseLeave(e);
        }
        #endregion // Override(s)

        #region Overridable(s)
        /// <summary>
        /// Override this to draw the role (title) background.
        /// </summary>
        protected virtual void DrawRoleBackground(Graphics g, Point[] pts, SecurityRole role)
        {
            using (Brush roleBackBrush = new SolidBrush(_roleBackColor))
            using (Pen roleBorderPen = new Pen(_roleBorderColor))
            {
                g.FillPolygon(roleBackBrush, pts);
                g.DrawPolygon(roleBorderPen, pts);
            }
        }

        /// <summary>
        /// Override this to draw the role (title) foreground.
        /// </summary>
        protected virtual void DrawRoleForeground(Graphics g, Rectangle r, SecurityRole role)
        {
            r.Inflate(-ROLE_OFFSET, 0);
            using (StringFormat sf = new StringFormat()
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            })
            using (Brush roleBrush = new SolidBrush(_roleForeColor))
                // And draw it.
                g.DrawString(
                role.DisplayName,
                RoleFont,
                roleBrush,
                r,
                sf
            );
        }

        /// <summary>
        /// Override this to draw the permission (title) background.
        /// </summary>
        protected virtual void DrawPermissionBackground(
            Graphics g,
            Rectangle r,
            SecurityCategory category,
            SecurityPermission permission)
        {
            using (Brush permissionBrush = new SolidBrush(_permissionBackColor))
                g.FillRectangle(permissionBrush, r);
        }

        /// <summary>
        /// Override this to draw the permission (title) foreground.
        /// </summary>
        protected virtual void DrawPermissionForeground(
            Graphics g,
            Rectangle r,
            SecurityCategory category,
            SecurityPermission permission)
        {
            r.Inflate(-PERMISSION_OFFSET, 0);
            using (StringFormat sf = new StringFormat()
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            })
            using (Brush permissionBrush = new SolidBrush(_permissionForeColor))
                // And draw it.
                g.DrawString(
                permission.DisplayName,
                PermissionFont,
                permissionBrush,
                r,
                sf
            );
        }

        /// <summary>
        /// Override this to draw the tick on selected permission.
        /// </summary>
        protected virtual void DrawTick(Graphics g, Rectangle rect)
        {
            // Vector tick.
            float[,] relpts = new float[,]
            {
                { 0.3f, 0.55f },
                { 0.4f, 0.7f },
                { 0.7f, 0.4f }
            };
            List<Point> pts = new List<Point>();
            for (int i = 0; i < 3; i++)
                pts.Add(new Point(
                    (int)Math.Round(rect.Left + (float)relpts[i, 0] * rect.Width),
                    (int)Math.Round(rect.Top + (float)relpts[i, 1] * rect.Height)
                    ));

            float sz = Math.Min(rect.Width, rect.Height);

            using (Pen checkPen = new Pen(_permissionTickColor, (int)Math.Round((0.08f * sz))))
                g.DrawLines(checkPen, pts.ToArray());
        }

        /// <summary>
        /// Override this to draw the background of the permission cell.
        /// </summary>
        protected virtual void DrawPermissionCellBackground(
            Graphics g,
            Rectangle rect,
            SecurityRole r,
            SecurityCategory c,
            SecurityPermission p)
        {
            // Is the cell active?
            Color backColor;
            if (_activeRole != null && r.Id.Equals(_activeRole.Id)
                && _activeCategory != null && c.Id.Equals(_activeCategory.Id)
                && _activePermission != null && p.Id.Equals(_activePermission.Id))
                backColor = _permissionActiveCellBackColor;
            else
                backColor = _permissionCellBackColor;

            using (Brush backBrush = new SolidBrush(backColor))
                g.FillRectangle(backBrush, rect);
        }

        /// <summary>
        /// Override this to draw the foregraound of the permission cell.
        /// NOTES: Tick is drawn separately.
        /// </summary>
        protected virtual void DrawPermissionCellForeground(
            Graphics g,
            Rectangle rect,
            SecurityRole r,
            SecurityCategory c,
            SecurityPermission p)
        {
            using (Pen forePen = new Pen(PermissionCellForeColor))
                g.DrawRectangle(forePen, rect);
        }

        /// <summary>
        /// Override this to draw the foreground of the category (title).
        /// </summary>
        protected virtual void DrawCategoryForeground(Graphics g, Rectangle r, SecurityCategory category)
        {
            r.Inflate(-CATEGORY_OFFSET, 0);
            using (StringFormat sf = new StringFormat()
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            })
            using (Brush categoryBrush = new SolidBrush(_categoryForeColor))
                // And draw it.
                g.DrawString(
                    category.DisplayName,
                    CategoryFont,
                    categoryBrush,
                    r,
                    sf
                );
        }

        /// <summary>
        /// Override this to draw the background of the category (title).
        /// </summary>
        protected virtual void DrawCategoryBackground(Graphics g, Rectangle r, SecurityCategory c)
        {
            using (Pen categoryBackPen = new Pen(_categoryBackColor))
            using (Brush categoryBackBrush = new SolidBrush(_categoryBackColor))
            {
                g.FillRectangle(categoryBackBrush, r);
                g.DrawRectangle(categoryBackPen, r);
            }
        }

        /// <summary>
        /// Override this to draw the background of the category cell.
        /// </summary>
        protected virtual void DrawCategoryCellBackground(Graphics g, Rectangle rect, SecurityRole r, SecurityCategory c)
        {
            using (Brush backBrush = new SolidBrush(CategoryCellBackColor))
                g.FillRectangle(backBrush, rect);
        }

        /// <summary>
        /// Override this to draw the foregroundof the category cell.
        /// </summary>
        protected virtual void DrawCategoryCellForeground(Graphics g, Rectangle rect, SecurityRole r, SecurityCategory c)
        { }
        #endregion // Overridable(s)

        #region Helper(s)
        private void DrawGrid(Graphics g)
        {
            // Do we have a feed?
            ISecurityMatrixFeed feed;
            if (_feed == null)
                feed = new DemoFeed();
            else
                feed = _feed;

            // Get categories and roles.
            var categories = feed.QueryCategories();
            var roles = feed.QueryRoles();

            // Data there?
            if (categories == null || roles == null) return;

            // Space there?
            if (_rolesHeight <= 0 || _categoriesWidth <= 0) return;

            // We shall need this for scrolling.
            int maxx, maxy;

            // Draw role backgrounds.
            int x = ContentRect.X + _categoriesWidth,
                y = ContentRect.Y + _rolesHeight;
            _rolePolygons = new List<Tuple<SecurityRole, Point[]>>();
            int offset = (int)Math.Round((double)_rolesHeight / Math.Tan(_rolesAngle * Math.PI / 180f));
            foreach (var r in roles)
            {
                Point[] pts = new Point[]
                {
                    new Point(x + offset, ContentRect.Y),
                    new Point(x + _cellSize.Width + offset, ContentRect.Y),
                    new Point(x + _cellSize.Width, y),
                    new Point(x, y)
                };
                DrawRoleBackground(g, pts, r);
                _rolePolygons.Add(new Tuple<SecurityRole, Point[]>(r, pts));
                x += _cellSize.Width;
            }

            // Rremember max x.
            maxx = x+offset;

            // Draw roles.
            x = ContentRect.X + _categoriesWidth;
            y = ContentRect.Y + _rolesHeight;

            // Role text length.
            int d = (int)Math.Round((double)_rolesHeight / Math.Sin(_rolesAngle * Math.PI / 180f));

            foreach (var r in roles)
            {
                // Measure string.
                Size textSize = Size.Round(g.MeasureString(r.DisplayName, Font));

                // Save current transform.
                var state = g.Save();

                // We'll need to start drawing from back of string.
                g.TranslateTransform(x + _cellSize.Width / 2f, y);
                g.ScaleTransform(-1f, -1f);
                g.RotateTransform((float)(180f - _rolesAngle));
                g.TranslateTransform(0, -CellSize.Height / 2);

                // Text rectangle to the center point.
                Rectangle textRect = new Rectangle(
                    0,
                    0,
                    d,
                    _cellSize.Height + 1);

                // Now draw the role.
                DrawRoleForeground(g, textRect, r);

                // Next cell.
                x += _cellSize.Width;

                // Restore the transform.
                g.Restore(state);
            }

            _categoryRectangles = new List<Tuple<SecurityCategory, Rectangle>>();
            _permissionRectangles = new List<Tuple<SecurityCategory, SecurityPermission, Rectangle>>();
            foreach (var c in categories)
            {
                y++; // Start on next line.

                // Draw category.
                Size textSize = Size.Round(g.MeasureString(c.DisplayName, Font));
                Rectangle targetRect = new Rectangle(ContentRect.X, y, _categoriesWidth, _categoryCellHeight);
                DrawCategoryBackground(g, targetRect, c);
                DrawCategoryForeground(g, targetRect, c);

                // Add category cells to the drawing queue.
                _categoryRectangles.Add(new Tuple<SecurityCategory, Rectangle>(c, targetRect));

                // Next.
                y += _categoryCellHeight;

                // If category has any permissions, draw them.
                var permissions = feed.QueryPermissions(c);
                foreach (var p in permissions)
                {
                    // Permission rectangle.
                    Rectangle permissionRect = new Rectangle(
                        ContentRect.X,
                        y,
                        _categoriesWidth,
                        _cellSize.Height);

                    // Draw permission.
                    DrawPermissionBackground(g, permissionRect, c, p);
                    DrawPermissionForeground(g, permissionRect, c, p);

                    // Add permission cells to the drawing queue
                    _permissionRectangles.Add(new Tuple<SecurityCategory, SecurityPermission, Rectangle>
                        (
                            c, p, permissionRect
                        ));

                    // Next row.
                    y += _cellSize.Height;
                }

            }

            // Remember max y.
            maxy = y;

            // And, finally, grid lines.
            _permissionCellRectangles = new List<Tuple<SecurityRole, SecurityCategory, SecurityPermission, Rectangle>>();
            foreach (var rp in _rolePolygons)
            {
                // All categories.
                foreach (var cr in _categoryRectangles)
                {
                    // Get category rectangle.
                    Rectangle cellRectangle = new Rectangle(
                        rp.Item2[3].X,
                        cr.Item2.Top,
                        rp.Item2[2].X - rp.Item2[3].X + 1,
                        cr.Item2.Height
                    );
                    DrawCategoryCellBackground(g, cellRectangle, rp.Item1, cr.Item1);
                    DrawCategoryCellForeground(g, cellRectangle, rp.Item1, cr.Item1);
                }
                // And permissions.
                foreach (var pr in _permissionRectangles)
                {
                    // Get category rectangle.
                    Rectangle cellRectangle = new Rectangle(
                        rp.Item2[3].X,
                        pr.Item3.Top,
                        rp.Item2[2].X - rp.Item2[3].X,
                        pr.Item3.Height
                    );
                    _permissionCellRectangles.Add(
                        new Tuple<SecurityRole, SecurityCategory, SecurityPermission, Rectangle>(
                            rp.Item1,
                            pr.Item1,
                            pr.Item2,
                            cellRectangle
                        ));
                    DrawPermissionCellBackground(g, cellRectangle, rp.Item1, pr.Item1, pr.Item2);
                    DrawPermissionCellForeground(g, cellRectangle, rp.Item1, pr.Item1, pr.Item2);
                    
                    // Checked?
                    if (feed[rp.Item1, pr.Item1, pr.Item2])
                        DrawTick(g, cellRectangle);
                }
            }

            // Set scroll size.
            AutoScrollMinSize = new Size(maxx, maxy);
            
        }

        private Rectangle ContentRect
        {
            get
            { // Respect margins.
                return new Rectangle(
                    ClientRectangle.X + Margin.Left,
                    ClientRectangle.Y + Margin.Top,
                    ClientRectangle.Width - Margin.Left - Margin.Right,
                    ClientRectangle.Height - Margin.Top - Margin.Bottom
                ); 
            }
        }
#endregion // Helper(s)
    }

    public interface ISecurityMatrixFeed
    {
        IEnumerable<SecurityRole> QueryRoles();
        IEnumerable<SecurityCategory> QueryCategories();
        IEnumerable<SecurityPermission> QueryPermissions(SecurityCategory category);
        bool this[SecurityRole r, SecurityCategory c, SecurityPermission p] { get; set; } 
    }

    public class KeyNamePair
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
    }

    public class SecurityCategory : KeyNamePair
    {}

    public class SecurityPermission : KeyNamePair
    {}

    public class SecurityRole : KeyNamePair
    {}
}
