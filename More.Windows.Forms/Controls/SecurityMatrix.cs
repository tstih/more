/*
 * SecurityMatrix.cs
 * 
 * A common matrix with roles on top and permissions
 * on the left. You can map role to permission by using
 * checks (default) or your own mechanism.
 * 
 * TODO:
 *  - extract data structures to feeder
 *  - connect clicks to feeder
 *  - enable custom drawing events
 *  - document events
 *  - constants for default values
 *  - structure drawing functions
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
    public class SecurityMatrix : Control
    {
        #region Const(s)
        private const int ROLE_OFFSET = 6;
        private const int CATEGORY_OFFSET = 6;
        private const int PERMISSION_OFFSET = 12;
        #endregion // Const(s)

        #region Private(s)
        private List<Tuple<SecurityRole, Point[]>> _rolePolygons;
        private List<Tuple<SecurityCategory, Rectangle>> _categoryRectangles;
        private List<Tuple<SecurityCategory, SecurityPermission, Rectangle>> _permissionRectangles;
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
            _rolesAngle = 45.0; // degrees.
            _cellSize = new Size(32, 32);
            _categoryCellHeight = 26;
            _rolesHeight = 80;
            _categoriesWidth = 160;

            // Create categories collection and subscribe to it.
            _categories = new BindingList<SecurityCategory>();
            _categories.ListChanged += _categories_ListChanged;

            // create roles and subscribe to it.
            _roles = new BindingList<SecurityRole>();
            _roles.ListChanged += _roles_ListChanged;

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
            _categoryForeColor = Color.FromKnownColor(KnownColor.WindowText);
            _categoryCellForeColor = Color.FromKnownColor(KnownColor.InactiveCaption);
        }
        #endregion // Ctor

        #region Override(s)
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(BackColor);

            // First draw categories.
            DrawGrid(g);
        }
        #endregion // Override(s)

        #region Properties
        private Font _roleFont;
        public Font RoleFont { get { return _roleFont; } set { _roleFont = value; Invalidate(); } }

        private Color _roleForeColor;
        public Color RoleForeColor { get { return _roleForeColor; } set { _roleForeColor = value; Invalidate(); } }

        private Color _roleBackColor;
        public Color RoleBackColor { get { return _roleBackColor; } set { _roleBackColor = value; Invalidate(); } }

        private Color _roleBorderColor;
        public Color RoleBorderColor { get { return _roleBorderColor; } set { _roleBorderColor = value; Invalidate(); } }

        private Font _categoryFont;
        public Font CategoryFont { get { return _categoryFont; } set { _categoryFont = value; Invalidate(); } }

        private Color _categoryForeColor;
        public Color CategoryForeColor { get { return _categoryForeColor; } set { _categoryForeColor = value; Invalidate(); } }

        private Color _categoryBackColor;
        public Color CategoryBackColor { get { return _categoryBackColor; } set { _categoryBackColor = value; Invalidate(); } }

        private Font _permissionFont;
        public Font PermissionFont { get { return _permissionFont; } set { _permissionFont = value; Invalidate(); } }

        private Color _permissionForeColor;
        public Color PermissionForeColor { get { return _permissionForeColor; } set { _permissionForeColor = value; Invalidate(); } }

        private Color _permissionBackColor;
        public Color PermissionBackColor { get { return _permissionBackColor; } set { _permissionBackColor = value; Invalidate(); } }

        private Color _permissionCellForeColor;
        public Color PermissionCellForeColor { get { return _permissionCellForeColor; } set { _permissionCellForeColor = value; Invalidate(); } }

        private Color _permissionCellBackColor;
        public Color PermissionCellBackColor { get { return _permissionCellBackColor; } set { _permissionCellBackColor = value; Invalidate(); } }

        private Color _permissionTickColor;
        public Color PermissionTickColor { get { return _permissionTickColor; } set { _permissionTickColor = value; Invalidate(); } }


        private Color _categoryCellForeColor;
        public Color CategoryCellForeColor { get { return _categoryCellForeColor; } set { _categoryCellForeColor = value; Invalidate(); } }

        private Color _categoryCellBackColor;
        public Color CategoryCellBackColor { get { return _categoryCellBackColor; } set { _categoryCellBackColor = value; Invalidate(); } }

        private int _categoryCellHeight;
        public int CategoryCellHeight { get { return _categoryCellHeight; } set { _categoryCellHeight = value; Invalidate(); } }

        private Size _cellSize;
        public Size CellSize { get { return _cellSize; } set { _cellSize = value; Invalidate(); } }

        private int _rolesHeight;
        public int RolesHeight { get { return _rolesHeight; } set { _rolesHeight = value; Invalidate(); } }

        private double _rolesAngle;
        public double RolesAngle {
            get { return _rolesAngle; }
            set {
                if (value < 25) _rolesAngle = 25;
                else if (value > 90) _rolesAngle = 90;
                else _rolesAngle = value;
                Invalidate();
            }
        }

        private int _categoriesWidth;
        public int CategoriesWidth { get { return _categoriesWidth; } set { _categoriesWidth = value; Invalidate(); } }

        private BindingList<SecurityCategory> _categories;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Bindable(false)]
        [Browsable(false)]
        public BindingList<SecurityCategory> Categories { get { return _categories; } set { _categories = value; } }

        private BindingList<SecurityRole> _roles;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Bindable(false)]
        [Browsable(false)]
        public BindingList<SecurityRole> Roles { get { return _roles; } set { _roles = value; } }
        #endregion // Properties

        #region Event Handler(s)
        private void _roles_ListChanged(object sender, ListChangedEventArgs e)
        { Invalidate(); }

        private void _categories_ListChanged(object sender, ListChangedEventArgs e)
        { Invalidate(); }
        #endregion // Event Handler(s)

        #region Helper(s)
        private void DrawGrid(Graphics g)
        {
            bool exit = false;

            // Data there?
            if (_categories == null || _roles == null) exit=true;

            // Space there?
            if (_rolesHeight <= 0 || _categoriesWidth <= 0) exit=true;

            // Just tell the world you can't make it.
            if (exit)
            {
                using (Brush foreBrush = new SolidBrush(ForeColor))
                using (StringFormat sf = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                })
                {
                    g.DrawString(
                        "This control is only visible in runtime mode.", 
                        Font, 
                        foreBrush, 
                        ClientRectangle, 
                        sf);
                }
                return;
            }

            // Draw role backgrounds.
            int x = ContentRect.X + _categoriesWidth,
                y = ContentRect.Y + _rolesHeight;
            _rolePolygons = new List<Tuple<SecurityRole, Point[]>>();
            foreach (var r in _roles)
            {
                int offset = (int)Math.Round((double)_rolesHeight / Math.Tan(_rolesAngle * Math.PI / 180f));
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

            // Draw roles.
            x = ContentRect.X + _categoriesWidth;
            y = ContentRect.Y + _rolesHeight;
            int maxx, maxy;

            // Role text length.
            int d = (int)Math.Round((double)_rolesHeight / Math.Sin(_rolesAngle * Math.PI / 180f));

            foreach (var r in _roles)
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

            // Rremember max x.
            maxx = x;
            _categoryRectangles = new List<Tuple<SecurityCategory, Rectangle>>();
            _permissionRectangles = new List<Tuple<SecurityCategory, SecurityPermission, Rectangle>>();
            foreach (var c in _categories)
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
                foreach (var p in c.Permissions)
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
                    DrawPermissionCellBackground(g, cellRectangle, rp.Item1, pr.Item1, pr.Item2);
                    DrawPermissionCellForeground(g, cellRectangle, rp.Item1, pr.Item1, pr.Item2);
                }
            }
            
        }

        private void DrawCheck(Graphics g, Rectangle rect)
        {
            // Vector tick.
            float[,] relpts= new float[,]
            {
                { 0.3f, 0.55f },
                { 0.4f, 0.7f },
                { 0.7f, 0.4f }
            };
            List<Point> pts = new List<Point>();
            for (int i = 0; i < 3; i++)
                pts.Add(new Point(
                    (int)Math.Round(rect.Left + (float)relpts[i,0]*rect.Width),
                    (int)Math.Round(rect.Top + (float)relpts[i, 1] * rect.Height)
                    ));

            float sz = Math.Min(rect.Width, rect.Height);

            using(Pen checkPen=new Pen(_permissionTickColor, (int)Math.Round((0.08f * sz)) ))
                g.DrawLines(checkPen, pts.ToArray());
        }

        private void DrawPermissionCellBackground(
            Graphics g, 
            Rectangle rect, 
            SecurityRole r, 
            SecurityCategory c,
            SecurityPermission p)
        {
            using (Brush backBrush = new SolidBrush(PermissionCellBackColor))
                g.FillRectangle(backBrush, rect);
        }

        private void DrawPermissionCellForeground(
            Graphics g, 
            Rectangle rect, 
            SecurityRole r, 
            SecurityCategory c,
            SecurityPermission p)
        {
            using (Pen forePen = new Pen(PermissionCellForeColor))
                g.DrawRectangle(forePen, rect);

            DrawCheck(g, rect);
        }

        private void DrawCategoryCellBackground(Graphics g, Rectangle rect, SecurityRole r, SecurityCategory c)
        {
            using (Brush backBrush = new SolidBrush(CategoryCellBackColor))
                g.FillRectangle(backBrush, rect);
        }

        private void DrawCategoryCellForeground(Graphics g, Rectangle rect, SecurityRole r, SecurityCategory c)
        { }

        private void DrawPermissionBackground(
            Graphics g, 
            Rectangle r, 
            SecurityCategory category, 
            SecurityPermission permission)
        {
            using (Brush permissionBrush = new SolidBrush(_permissionBackColor))
                g.FillRectangle(permissionBrush, r);
        }

        private void DrawPermissionForeground(
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

        private void DrawRoleBackground(Graphics g, Point[] pts, SecurityRole role)
        {
            using (Brush roleBackBrush = new SolidBrush(_roleBackColor))
            using (Pen roleBorderPen = new Pen(_roleBorderColor))
            {
                g.FillPolygon(roleBackBrush, pts);
                g.DrawPolygon(roleBorderPen, pts);
            }
        }

        private void DrawRoleForeground(Graphics g, Rectangle r, SecurityRole role)
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

        private void DrawCategoryBackground(Graphics g, Rectangle r, SecurityCategory c)
        {
            using (Pen categoryBackPen = new Pen(_categoryBackColor))
            using (Brush categoryBackBrush = new SolidBrush(_categoryBackColor))
            {
                g.FillRectangle(categoryBackBrush, r);
                g.DrawRectangle(categoryBackPen, r);
            }
        }

        private void DrawCategoryForeground(Graphics g, Rectangle r, SecurityCategory category)
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

        private Rectangle ContentRect
        {
            get
            {
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

    public class Identifiable : INotifyPropertyChanged
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                FireChanged("Id");
            }
        }

        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                FireChanged("DisplayName");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void FireChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class SecurityCategory : Identifiable
    {
        public SecurityCategory()
        {
            _permissions = new BindingList<SecurityPermission>();
            _permissions.ListChanged += _permissions_ListChanged;
        }

        private void _permissions_ListChanged(object sender, ListChangedEventArgs e)
        {
            FireChanged("Permissions");
        }

        private BindingList<SecurityPermission> _permissions;
        public BindingList<SecurityPermission> Permissions { get { return _permissions; } }
    }

    public class SecurityPermission : Identifiable
    {
    }

    public class SecurityRole : Identifiable
    {
    }
}
