/*
 * Force.cs
 * 
 * Force directed graph.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 03.02.2020   tstih
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace More.Windows.Forms
{
    public class Force : Control
    {
        #region Private(s)
        private IDirectedGraphFeed _feed;
        private Dictionary<string, int> _nodeIdx;
        private double[,] _nodePos;
        private Symtrix _edges;
        private int _edgeCount;
        private int _nodeCount;
        private Random _rnd;
        #endregion // Private(s)

        #region Ctor
        public Force()
        {
            // Control details.
            DoubleBuffered = true;
            ResizeRedraw = true;

            // Set defaults.
            SetDefaults();
        }

        private void SetDefaults()
        {
            // Initialize random generator.
            _rnd = new Random((int)DateTime.Now.Ticks);
            // Count of all nodes is 0.
            _nodeCount = 0;
        }
        #endregion // Ctor

        #region Method(s)
        /// <summary>
        /// Populate the tree by givin it data feed.
        /// </summary>
        public void SetFeed(IDirectedGraphFeed feed)
        {
            // Nothing new.
            if (_feed == feed) return;
            // Store and repaint.
            _feed = feed; 
            
            // Populate 
            Populate(); 
            
            // Calculate.
            Relayout(); 
            
            // Repaint.
            Invalidate();
        }

        public void Animate()
        {
            // Move nodes.
            Relayout();

            // And invalidate.
            Invalidate();
        }
        #endregion // Method(s)

        #region Properties
        #endregion // Properties

        #region Override(s)
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // We need nodes.
            if (_nodePos == null) return;

            // Draw all edges.
            for (int i = 1; i <= _nodeCount; i++)
                for (int j = 1; j < i; j++)
                    if (_edges[i, j] == 1)
                        DrawEdge(e.Graphics,
                            GetAbsolutePosition(_nodePos[i-1, 0], _nodePos[i-1, 1]),
                            GetAbsolutePosition(_nodePos[j-1, 0], _nodePos[j-1, 1])
                            );

            // Iterate through all nodes.
            for (int i = 0; i < _nodePos.GetLength(0); i++)
            {
                Point nodePt = GetAbsolutePosition(_nodePos[i, 0], _nodePos[i, 1]);
                DrawNode(e.Graphics, nodePt, i);
            }
        }
        #endregion // Override(s)

        #region Helper(s)
        private void DrawEdge(Graphics g, Point np1, Point np2)
        {
            g.DrawLine(Pens.Blue, np1, np2);
        }

        private void DrawNode(Graphics g, Point nodePt, int i)
        {
            int nodew = 26, nodeh = 26;
            using (StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                Rectangle nodeRect = new Rectangle(
                     nodePt.X - nodew / 2,
                     nodePt.Y - nodeh / 2,
                    nodew,
                    nodeh);

                g.FillEllipse(Brushes.White,
                    nodeRect
                );

                g.DrawEllipse(Pens.Black,
                    nodeRect
                );

                g.DrawString(i.ToString(), Font, Brushes.Blue, nodeRect, sf);
            }
        }

        private void Populate()
        {
            // Delete node index.
            _nodeIdx = new Dictionary<string, int>();

            // No feed, no fun.
            if (_feed == null) return;

            // Create node index.
            _nodeCount = 0;
            IEnumerable<string> nodes = _feed.Nodes(); // Get nodes.
            foreach (string key in nodes)
            {
                // Create node index.
                _nodeIdx.Add(key, _nodeCount++);
            }

            _edgeCount = 0;
            _edges = new Symtrix(_nodeCount);
            int i = 0;
            foreach (var ni in _nodeIdx)
                foreach (string key2 in _feed.Edges(ni.Key))
                {
                    _edges[ni.Value + 1, _nodeIdx[key2] + 1] = 1;
                    _edgeCount++;
                }

            // Random positions for each node array with columns id [x, y]
            _nodePos = new double[_nodeCount, 2];
            for (int n = 0; n < _nodeCount; n++)
            {
                _nodePos[n, 0] = 0.5 + _rnd.NextDouble() / 8 * (_rnd.NextDouble() < 0.5 ?-1 : 1);
                _nodePos[n, 1] = 0.5 + _rnd.NextDouble() / 8 * (_rnd.NextDouble() < 0.5 ? -1 : 1);
            }
        }

        private void Relayout()
        {
            // No nodes?
            if (_nodeIdx.Count == 0) return;

            // Calculate x and y distances.
            Symtrix
                x = new Symtrix(_nodeCount),
                y = new Symtrix(_nodeCount),
                repulse = new Symtrix(_nodeCount),
                attract = new Symtrix(_nodeCount);
            for (int i = 1; i <= _nodeCount; i++)
                for (int j = 1; j < i; j++)
                {
                    x[i, j] = _nodePos[i - 1, 0] - _nodePos[j - 1, 0];
                    y[i, j] = _nodePos[i - 1, 1] - _nodePos[j - 1, 1];
                    double rsquare = x[i, j] * x[i, j] + y[i, j] * y[i, j];
                    repulse[i, j] = 
                        1 / rsquare;
                    if (_edges[i, j] == 1)
                        attract[i, j] = rsquare;
                    else
                        attract[i, j] = 0;
                }

            repulse.Normalize();

            // Ponder x and y with forces.
            Symtrix kx = x * repulse, ky = y * repulse;
            Symtrix hx = x * attract, hy = y * attract;

            // Finally, move nodes!
            for (int i = 1; i <= _nodeCount; i++)
            { // Each row.
                double sumx, sumy, hsumx, hsumy; sumx = sumy = hsumx = hsumy = 0;
                for (int j = 1; j <= _nodeCount; j++)
                {
                    if (i>j)
                    { // No influence on itself.
                        sumx += kx[i, j];
                        sumy += ky[i, j];
                        hsumx -= hx[i, j];
                        hsumy -= hy[i, j];

                    } else if (i < j)
                    { // No influence on itself.
                        sumx -= kx[i, j];
                        sumy -= ky[i, j];
                        hsumx += hx[i, j];
                        hsumy += hy[i, j];
                    }
                }
                // Average...
                sumx /= _nodeCount;  sumy /= _nodeCount;

                // And change node locations.
                _nodePos[i-1, 0] += sumx+hsumx;
                _nodePos[i-1, 1] += sumy+hsumy;

                if (_nodePos[i - 1, 0] > 1) _nodePos[i - 1, 0] = 1;
                if (_nodePos[i - 1, 1] > 1) _nodePos[i - 1, 1] = 1;
                if (_nodePos[i - 1, 0] < 0) _nodePos[i - 1, 0] = 0;
                if (_nodePos[i - 1, 1] < 0) _nodePos[i - 1, 1] = 0;
            }
        }

        Point GetAbsolutePosition(double x, double y)
        {
            return new Point((int)Math.Round(x * Width), (int)Math.Round(y * Height));
        }
        #endregion // Helper(s)
    }

    public interface IDirectedGraphFeed
    {
        // Return all node keys of a graph.
        IEnumerable<string> Nodes();

        // Given node key, return edges.
        IEnumerable<string> Edges(string nodeKey);
    }

    /// <summary>
    /// Symetric data structure, similar to symmetric matrix,
    /// but without diagonal.
    /// </summary>
    internal class Symtrix
    {
        #region Private(s)
        private int _N;
        private int _dataLen;
        private double[] _data;
        #endregion // Private(s)

        #region Ctor
        public Symtrix(int N)
        {
            _N = N;
            _dataLen = _N * (_N - 1) / 2;
            _data = new double[_dataLen]; // How man connections in graph trick.
        }
        #endregion // Ctor

        #region Method(s)
        public double this[int i, int j]
        {
            get { return _data[IndexOf(i, j)]; }
            set { _data[IndexOf(i, j)] = value; }
        }

        public void Normalize()
        {
            double min = _data[0], max = _data[0];
            for(int i=1;i<_dataLen;i++)
            {
                if (_data[i] > max) max = _data[i];
                if (_data[i] < min) min = _data[i];
            }
            double spread = max - min;
            for (int i = 0; i < _dataLen; i++)
                _data[i] = (_data[i] - min)/spread;
        }
        #endregion // Method(s)

        #region Operator(s)
        public static Symtrix operator *(Symtrix a, Symtrix b)
        {
            return Symtrix.Operation(a, b, (a, b) => a * b);
        }

        public static Symtrix operator /(Symtrix a, Symtrix b)
        {
            return Symtrix.Operation(a, b, (a, b) => a / b);
        }

        public static Symtrix operator +(Symtrix a, Symtrix b)
        {
            return Symtrix.Operation(a, b, (a, b) => a + b);
        }

        public static Symtrix operator -(Symtrix a, Symtrix b)
        {
            return Symtrix.Operation(a, b, (a, b) => a - b);
        }
        #endregion // Operator(s)

        #region Properties
        public int N {get { return _N; } }
        #endregion // Properties

        #region Helper(s)
        private static Symtrix Operation(Symtrix a, Symtrix b, Func<double,double,double> fn)
        {
            Symtrix s = new Symtrix(a._N);
            for (int i = 0; i < a._dataLen; i++)
                s._data[i] = fn(a._data[i],b._data[i]);
            return s;
        }

        private int IndexOf(int i, int j)
        {
            if (j > i) Swap(ref i, ref j);
            return (i - 1) * (i - 2) / 2 + j - 1;
        }

        private void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
        #endregion // Helper(s)
    }
}
