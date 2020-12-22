/*
 * PanelEx.cs
 * 
 * Panel that enables painted decorations.
 * Derive your panel from this class, set the Margin 
 * to offset client rectangle, and override the
 * Decorate function to paint decorations in the pseudo 
 * "non-client" area.
 * See: Frame (derived from PanelEx) for example of use.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 03.02.2020   tstih
 * 
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace More.Windows.Forms
{
    public class PanelEx : Panel
    {
        #region Ctor(s)
        public PanelEx()
        {
            // Control details.
            DoubleBuffered = true;
            ResizeRedraw = true;
        }
        #endregion // Ctor(s)

        #region Override(s)
        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle rect = new Rectangle(
                    base.DisplayRectangle.Left + Margin.Left,
                    base.DisplayRectangle.Top + Margin.Top,
                    base.DisplayRectangle.Width - Margin.Left - Margin.Right,
                    base.DisplayRectangle.Height - Margin.Top - Margin.Bottom);
                return rect;
            }
        }

        protected override void OnMarginChanged(EventArgs e)
        {
            // Relayout.
            PerformLayout();

            // And repaint.
            Invalidate();
        }

        // TODO: Rectangle calculation seems a bit complicated. Also check pixel math, look for +1/-1 pixel errors.
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.Clear(BackColor);

            // Our target rectangle is client rectangle. We'll raise events to paint each part if margin is set.
            Rectangle lt = new Rectangle(ClientRectangle.Left, ClientRectangle.Top, Margin.Left, Margin.Top);
            Rectangle rt = new Rectangle(ClientRectangle.Right - Margin.Right, ClientRectangle.Top, Margin.Right, Margin.Top);
            Rectangle lb = new Rectangle(ClientRectangle.Left, ClientRectangle.Bottom - Margin.Bottom, Margin.Left, Margin.Bottom);
            Rectangle rb = new Rectangle(ClientRectangle.Right - Margin.Right, ClientRectangle.Bottom - Margin.Bottom, Margin.Right, Margin.Bottom);
            Rectangle l = new Rectangle(ClientRectangle.Left, ClientRectangle.Top + Margin.Top, Margin.Left, ClientRectangle.Height - Margin.Top - Margin.Bottom);
            Rectangle t = new Rectangle(ClientRectangle.Left + Margin.Left, ClientRectangle.Top, ClientRectangle.Width - Margin.Left - Margin.Right, Margin.Top);
            Rectangle r = new Rectangle(ClientRectangle.Right - Margin.Right, ClientRectangle.Top + Margin.Top, Margin.Right, ClientRectangle.Height - Margin.Top - Margin.Bottom);
            Rectangle b = new Rectangle(ClientRectangle.Left + Margin.Left, ClientRectangle.Bottom - Margin.Bottom, ClientRectangle.Width - Margin.Left - Margin.Right, Margin.Bottom);

            // And paint deco.
            Decorate(e.Graphics, lt, rt, lb, rb, l, t, r, b);
        }
        #endregion // Override(s)

        #region Overridable(s)
        // Override this.
        protected virtual void Decorate(Graphics g, Rectangle lt, Rectangle rt, Rectangle lb, Rectangle rb, Rectangle l, Rectangle t, Rectangle r, Rectangle b)
        {
        }
        #endregion // Overridable(s)
    }
}