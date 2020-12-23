/*
 * Hierarchy.cs
 * 
 * Draws a tree, delegates drawing nodes to you.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 21.12.2020   tstih
 * 
 */
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System;

namespace More.Windows.Forms
{
    public class Hierarchy : Control
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

            _nodes = new List<HierarchyNode>();
        }
        #endregion // Ctor

        #region Method(s)
        public void SetFeed(IHierarchyFeed feed)
        {
            // Nothing new.
            if (_feed == feed) return;
            // Store and repaint.
            _feed = feed; Relayout(); Invalidate();
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
                // First draw all node edges (if children?)
                foreach (HierarchyNode nc in n.Children)
                    DrawEdge.Raise(this, 
                        new DrawEdgeEventArgs(
                            e.Graphics,
                            n.Key,
                            n.Rectangle,
                            nc.Key,
                            nc.Rectangle
                        ));
            }

            foreach(HierarchyNode n in _nodes)
                // And draw node.
                DrawNode.Raise(this, new DrawNodeEventArgs(e.Graphics, n.Key, n.Rectangle));
        }
        #endregion // Override(s)

        #region Event(s)
        public event EventHandler<DrawNodeEventArgs> DrawNode;
        public event EventHandler<DrawEdgeEventArgs> DrawEdge;
        #endregion // Event(s)

        #region Properties
        int _nodeWidth;
        public int NodeWidth { get { return _nodeWidth; } set { _nodeWidth = value; Invalidate(); } }

        int _nodeHeight;
        public int NodeHeight{ get { return _nodeHeight; } set { _nodeHeight = value; Invalidate(); } }

        int _nodeHorzSpacing;
        public int NodeHorzSpacing { get { return _nodeHorzSpacing; } set { _nodeHorzSpacing = value; Invalidate(); } }

        int _nodeVertSpacing;
        public int NodeVertSpacing { get { return _nodeVertSpacing; } set { _nodeVertSpacing = value; Invalidate(); } }

        #endregion // Properties

        #region Helper(s)
        private void Relayout()
        {
            // No feed?
            if (_feed == null)
            {
                _nodes.Clear();
                return;
            }

            // First level.
            Dictionary<int, int> levelx = new Dictionary<int, int>();
            AddNode(levelx);
        }

        private HierarchyNode AddNode(Dictionary<int, int> levelx, string pkey=null, int level=-1)
        {
            var children = _feed.Query(pkey);
            if (children.Count() > 0)
            { // Iterate through all children.
                var hierarchyNodes = new List<HierarchyNode>();
                foreach (string key in _feed.Query(pkey)) // Go all the way.
                    hierarchyNodes.Add(AddNode(levelx, key, level + 1));
                if (pkey == null) return null;
                // Create me on top of them.
                int startx = hierarchyNodes.First().Rectangle.Left,
                    endx = hierarchyNodes.Last().Rectangle.Right;
                Rectangle targetRect = new Rectangle(
                    startx,
                    level * (NodeHeight + NodeVertSpacing),
                    endx-startx+1,
                    NodeHeight);
                var nodeRect=targetRect.Center(new Size(NodeWidth, NodeHeight), Padding.Empty);
                if (levelx.ContainsKey(level) && nodeRect.Left < levelx[level])
                {   // Place taken. Propagate down the hierarchy.
                    int diffx = levelx[level] - nodeRect.Left + NodeHorzSpacing;
                    nodeRect = new Rectangle(levelx[level], nodeRect.Top, nodeRect.Width, nodeRect.Height);
                    MoveChildrenRight(hierarchyNodes, diffx, level+1,levelx);
                }
                var n = new HierarchyNode(pkey,nodeRect);
                n.Children = hierarchyNodes;
                levelx[level] = nodeRect.Right + NodeHorzSpacing;
                _nodes.Add(n);
                return n;
            }
            else
            { // We have no children.
                if (!levelx.ContainsKey(level)) levelx.Add(level, 0);
                // Position the node.
                var n = new HierarchyNode(
                    pkey,
                    new Rectangle(
                        levelx[level],
                        level * (NodeHeight + NodeVertSpacing),
                        NodeWidth,
                        NodeHeight
                    )
                );
                levelx[level] = levelx[level] + NodeWidth + NodeHorzSpacing;
                _nodes.Add(n);
                return n;
            }
        }

        private void MoveChildrenRight(List<HierarchyNode> children, int diffx, int level, Dictionary<int, int> levelx)
        {
            foreach(HierarchyNode n in children)
            {
                n.Rectangle = new Rectangle(n.Rectangle.X + diffx, n.Rectangle.Top, n.Rectangle.Width, n.Rectangle.Height);
                if (levelx.ContainsKey(level) && n.Rectangle.X + diffx > levelx[level])
                    levelx[level] = n.Rectangle.X + diffx;
                MoveChildrenRight(n.Children, diffx, level + 1, levelx);
            }
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
