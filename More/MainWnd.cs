/*
 * MainWnd.cs
 * 
 * Playground for testing controls.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 20.12.2020   tstih
 * 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using More.Windows.Forms;

namespace More
{
    public partial class MainWnd : Form
    {
        public MainWnd()
        {
            InitializeComponent();
        }

        private void MainWnd_Load(object sender, EventArgs e)
        {
            Feeder feed = new Feeder(6,4);
            _hierarchy.SetFeed(feed);
        }

        private void _hierarchy_DrawEdge(object sender, DrawEdgeEventArgs e)
        {
            Graphics g = e.Graphics;
            Point
                start = new Point(
                    e.ParentRectangle.Left + e.ParentRectangle.Width / 2,
                    e.ParentRectangle.Top + e.ParentRectangle.Height / 2),
                end = new Point(
                    e.ChildRectangle.Left + e.ChildRectangle.Width / 2,
                    e.ChildRectangle.Top + e.ChildRectangle.Height / 2);
            using (Pen p = new Pen(ForeColor)) 
                g.DrawLine(p,start,end);
        }

        private void _hierarchy_DrawNode(object sender, DrawNodeEventArgs e)
        {
            Graphics g = e.Graphics;
            using (Pen p = new Pen(ForeColor))
            using (Brush backBrush = new SolidBrush(BackColor), foreBrush = new SolidBrush(ForeColor))
            {
                g.FillEllipse(backBrush, e.Rectangle);
                g.DrawEllipse(p, e.Rectangle);
            }
        }
    }

    public class Feeder : IHierarchyFeed
    {
        private class Node
        {
            public Node(int index)
            {
                Index = index;
                Children = new List<Node>();
            }
            public List<Node> Children;
            public int Index;
        }

        Dictionary<int, Node> _allNodes;
        private int _maxLevel;
        private int _maxChildren;

        public Feeder(int maxLevel = 3, int maxChildren = 8)
        {
            _maxLevel = maxLevel;
            _maxChildren = maxChildren;
            _allNodes = new Dictionary<int, Node>();

            // Init random generator.
            Random rnd = new Random((int)DateTime.Now.Ticks);
            Node root = new Node(1);
            _allNodes.Add(root.Index, root);
            Generate(rnd, root, 1);
        }

        void Generate(Random rnd, Node n, int level = 0)
        {
            if (level >= _maxLevel) return;
            int children = rnd.Next(0, _maxChildren); // How many children.
            for (int i = 0; i < children; i++)
            {
                // New node.
                int index = n.Index * 10 + i + 1;
                Node nx = new Node(index);
                n.Children.Add(nx);
                _allNodes.Add(index,nx);
                Generate(rnd, nx, level + 1);
            }
        }

        public IEnumerable<string> Query(string key = null)
        {
            List<string> results = new List<string>();
            if (key != null)
            {
                int index = int.Parse(key);
                Node n = _allNodes[index];
                foreach (Node nc in n.Children)
                    results.Add(nc.Index.ToString());
            }
            else
                results.Add("1"); // Root node.      
            return results;
        }
    }
}