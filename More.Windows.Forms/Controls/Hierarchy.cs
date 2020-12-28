/*
 * Hierarchy.cs
 * 
 * Draws a tree, delegates drawing nodes to you.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 23.12.2020   tstih       Merry Christmas.
 * 
 */
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.ComponentModel;

namespace More.Windows.Forms
{
    public class Hierarchy : ScrollableControl
    {
        #region Const(s)
        private const int DEFAULT_NODE_WIDTH = 96;
        private const int DEFAULT_NODE_HEIGHT = 26;
        private const int DEFAULT_NODE_HORZ_SPACING = 20;
        private const int DEFAULT_NODE_VERT_SPACING = 20;
        #endregion // Const(s)

        #region Private Class(es)
        private class HierarchyNode
        {
            public HierarchyNode(string key, Rectangle rectangle) { 
                Children = new List<HierarchyNode>();
                Key = key;
                Rectangle = rectangle;
            }
            public string Key { get; set; }
            public Rectangle Rectangle { get; set; }
            public List<HierarchyNode> Children { get; set; }
        }
        #endregion Private Class(es)

        #region Private(s)
        private IHierarchyFeed _feed;
        private List<HierarchyNode> _nodes;
        private List<HierarchyNode> _roots;
        #endregion // Private(s)

        #region Ctor
        public Hierarchy()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;

            // Set default property values.
            SetDefaults();
        }

        private void SetDefaults()
        {
            _nodeWidth = DEFAULT_NODE_WIDTH;
            _nodeHeight = DEFAULT_NODE_HEIGHT;
            _nodeHorzSpacing = DEFAULT_NODE_HORZ_SPACING;
            _nodeVertSpacing = DEFAULT_NODE_VERT_SPACING;
            _direction = Direction.Left2Right;

            _nodes = new List<HierarchyNode>();
            _roots = new List<HierarchyNode>();
        }
        #endregion // Ctor

        #region Method(s)

        /// <summary>
        /// Populate the tree by givin it data feed.
        /// </summary>
        public void SetFeed(IHierarchyFeed feed)
        {
            // Nothing new.
            if (_feed == feed) return;
            // Store and repaint.
            _feed = feed; Relayout(); Invalidate();
        }

        /// <summary>
        /// Get node at point.
        /// </summary>
        public string NodeAt(Point pt)
        {
            Point phyPt = new Point(pt.X - AutoScrollPosition.X, pt.Y - AutoScrollPosition.Y);
            foreach (HierarchyNode n in _nodes)
                if (n.Rectangle.Contains(phyPt))
                    return n.Key;
            // Not found.
            return null;
        }

        /// <summary>
        /// Get root nodes. 
        /// </summary>
        public IEnumerable<string> Roots 
        {
            get
            {
                return _roots.Select(r => r.Key);
            }
        }
        #endregion // Method(s)

        #region Override(s)
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            foreach(HierarchyNode n in _nodes)
            {
                // And draw node.
                Rectangle parentRect = n.Rectangle;
                parentRect.Offset(AutoScrollPosition);

                // First draw all node edges (if children?)
                foreach (HierarchyNode nc in n.Children)
                {
                    // And draw node.
                    Rectangle childRect = nc.Rectangle;
                    childRect.Offset(AutoScrollPosition);

                    // And draw edge.
                    DrawEdge.Raise(this,
                        new DrawEdgeEventArgs(
                            e.Graphics,
                            n.Key,
                            parentRect,
                            nc.Key,
                            childRect
                        ));
                }
            }

            foreach (HierarchyNode n in _nodes)
            {
                // And draw node.
                Rectangle nodeRect = n.Rectangle;
                nodeRect.Offset(AutoScrollPosition);
                DrawNode.Raise(this, new DrawNodeEventArgs(e.Graphics, n.Key, nodeRect));
            }
        }
        #endregion // Override(s)

        #region Event(s)
        public event EventHandler<DrawNodeEventArgs> DrawNode;
        public event EventHandler<DrawEdgeEventArgs> DrawEdge;
        #endregion // Event(s)

        #region Properties
        int _nodeWidth;
        /// <summary>
        /// Width of each node.
        /// </summary>
        [Description("Node width"), Category("Layout")]
        public int NodeWidth { get { return _nodeWidth; } set { _nodeWidth = value; Relayout(); } }

        int _nodeHeight;
        /// <summary>
        /// Height of each node.
        /// </summary>
        [Description("Node height"), Category("Layout")]
        public int NodeHeight{ get { return _nodeHeight; } set { _nodeHeight = value; Relayout(); } }

        int _nodeHorzSpacing;
        /// <summary>
        /// Minimal horizontal space between two nodes.
        /// </summary>
        [Description("Min. horizontal space between two nodes"), Category("Layout")]
        public int NodeHorzSpacing { get { return _nodeHorzSpacing; } set { _nodeHorzSpacing = value; Relayout(); } }

        int _nodeVertSpacing;
        /// <summary>
        /// Minimal vertical space between two nodes.
        /// </summary>
        [Description("Min. vertical space between two nodes"), Category("Layout")]
        public int NodeVertSpacing { get { return _nodeVertSpacing; } set { _nodeVertSpacing = value; Relayout(); } }

        Direction _direction;
        /// <summary>
        /// Left to right, right to left, top to bottom or bottom to top.
        /// </summary>
        [Description("Graph direction"), Category("Layout")]
        public Direction Direction { get { return _direction; } set { _direction = value; Relayout(); } }
        #endregion // Properties

        #region Helper(s)
        private void Relayout()
        {
            // No feed?
            if (_feed == null)
            {
                _nodes.Clear();
                _roots.Clear();
                return;
            }

            // First level.
            int coord = 0;
            AddNodes(ref coord);
            NeedReverse();

            // Update scrollbars.
            Rectangle graphRect = GetTreeBoundingRect();
            AutoScrollMinSize = graphRect.Size;

            // And, finally, invalidate.
            Invalidate();
        }

        // Basic algorithm of tree drawing in three steps.
        // 1) Allocate new slot for every node without children
        // 2) Ident children
        // 2) Don't allocate slot for nodes with children, simply center them on top of children
        private HierarchyNode AddNodes(ref int coord, string pkey=null, int level=0)
        {
            // Get children.
            var children = _feed.Query(pkey);

            if (children.Count() == 0)
            { // Parent has no children.
                var hierarchyNode = new HierarchyNode(pkey,CalcNodeRectangle(level, coord));
                _nodes.Add(hierarchyNode);
                NextCoord(ref coord, hierarchyNode); // Allocate slot.
                return hierarchyNode;
            }
            else
            { // Parent has children. Reserve no slot.
                HierarchyNode hierarchyNode = null;
                if (pkey != null)
                {
                    hierarchyNode = new HierarchyNode(pkey,CalcNodeRectangle(level, coord));
                    _nodes.Add(hierarchyNode);
                }
                // And recurse.
                List<HierarchyNode> hierarchyChildren = new List<HierarchyNode>();
                foreach (var ckey in children)
                    hierarchyChildren.Add(AddNodes(ref coord, ckey, level + 1));
                // Only in case there is a parent.
                if (hierarchyNode != null)
                {
                    // Now center parent!
                    hierarchyNode.Children = hierarchyChildren;
                    hierarchyNode.Rectangle = CenterOverChildren(hierarchyNode, hierarchyChildren);
                }
                // If root node, store them to roots.
                if (pkey == null) _roots = hierarchyChildren;
                // And return.
                return hierarchyNode;
            }
        }

        private void NextCoord(ref int coord, HierarchyNode node)
        {
            if (IsHorizontal)
                coord = node.Rectangle.Bottom + NodeVertSpacing;
            else
                coord = node.Rectangle.Right + NodeHorzSpacing;

        }

        private Rectangle CenterOverChildren(HierarchyNode parent, IEnumerable<HierarchyNode> children)
        {
            int start = IsHorizontal ? children.First().Rectangle.Top : children.First().Rectangle.Left, 
                end = IsHorizontal ? children.Last().Rectangle.Bottom : children.Last().Rectangle.Right;

            Rectangle areaOfInterest;
            
            if(IsHorizontal)
                areaOfInterest = new Rectangle(
                    parent.Rectangle.Left,
                    start,
                    parent.Rectangle.Width,
                    end - start + 1
                    );
            else
                areaOfInterest = new Rectangle(
                    start,
                    parent.Rectangle.Top,
                    end-start + 1,
                    parent.Rectangle.Height
                    );

            return areaOfInterest.Center(parent.Rectangle.Size, Padding.Empty);
        }

        private Rectangle CalcNodeRectangle(int level, int coord)
        {
            // Horizontal?
            if (IsHorizontal)
                return new Rectangle(
                    (NodeWidth + NodeHorzSpacing) * (level - 1),
                    coord,
                    NodeWidth,
                    NodeHeight
                );
            else
                return new Rectangle(
                    coord,
                    (NodeHeight + NodeVertSpacing) * (level - 1),
                    NodeWidth,
                    NodeHeight
                );
        }

        private bool IsHorizontal
        { 
            get { return Direction == Direction.Left2Right || Direction == Direction.Right2Left; }
        }

        // Takes left to right graph and reverse it to right to left.
        private void NeedReverse()
        {
            Rectangle bounds = GetTreeBoundingRect();
            foreach (HierarchyNode n in _nodes)
                if(Direction==Direction.Right2Left)
                    n.Rectangle = new Rectangle(
                        bounds.Right-n.Rectangle.Left-n.Rectangle.Width,
                        n.Rectangle.Top,
                        n.Rectangle.Width,
                        n.Rectangle.Height
                    );
                else if (Direction==Direction.Bottom2Top)
                    n.Rectangle = new Rectangle(
                        n.Rectangle.Left,
                        bounds.Bottom - n.Rectangle.Top-n.Rectangle.Height,
                        n.Rectangle.Width,
                        n.Rectangle.Height
                    );
        }

        private Rectangle GetTreeBoundingRect()
        {
            int x = _nodes.Min(n => n.Rectangle.Left),
                y = _nodes.Min(n => n.Rectangle.Top),
                x2 = _nodes.Max(n => n.Rectangle.Right),
                y2 = _nodes.Max(n => n.Rectangle.Bottom);
            return new Rectangle(x,y,x2-x+1, y2-y+1);
        }
        #endregion // Helper(s)
    }

    public interface IHierarchyFeed
    {
        IEnumerable<string> Query(string key=null);
    }

    public class DrawNodeEventArgs: EventArgs
    {
        public DrawNodeEventArgs(Graphics g, string key, Rectangle rect) {
            Graphics = g;
            Key = key;
            Rectangle = rect;
        }
        public Graphics Graphics { private set; get; }
        public Rectangle Rectangle { private set; get; }
        public string Key { private set; get; }

    }
    
    public class DrawEdgeEventArgs: EventArgs
    {
        public DrawEdgeEventArgs(Graphics g, string parentKey, Rectangle parentRect, string childKey, Rectangle childRect)
        {
            Graphics = g;
            ParentRectangle = parentRect;
            ParentKey = parentKey;
            ChildRectangle = childRect;
            ChildKey = childKey;
        }
        public Graphics Graphics { private set; get; }
        public Rectangle ParentRectangle { private set; get; }
        public string ParentKey { private set; get; }
        public Rectangle ChildRectangle { private set; get; }
        public string ChildKey { private set; get; }
    }
}
