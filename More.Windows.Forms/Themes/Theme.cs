/*
 * Theme.cs
 * 
 * Control theme class. Derive from this class
 * to add your own theme.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2021 Tomaz Stih
 * 
 * 05.02.2022   tstih   
 * 
 */
using System.Drawing;

namespace More.Windows.Forms
{
    public class Theme
    {
        #region Color Properties
        // Control back color.
        public virtual Color BackColor { get { return Color.FromKnownColor(KnownColor.Control); } }
        // Control fore color.
        public virtual Color ForeColor { get { return Color.FromKnownColor(KnownColor.ControlText); } }
        // Control meta data fore color. Meta data are labels, prompts, etc.
        public virtual Color MetaForeColor { get { return Color.FromKnownColor(KnownColor.InactiveCaption); } }
        // Important control meta data fore color.
        public virtual Color MetaEmphasizeForeColor { get { return Color.FromKnownColor(KnownColor.ActiveCaption); } }
        // Control data fore color. 
        public virtual Color DataForeColor { get { return Color.FromKnownColor(KnownColor.ControlDarkDark); } }
        // Important control data.
        public virtual Color DataEmphasizeForeColor { get { return Color.FromKnownColor(KnownColor.ControlText); } }
        // Control standard border.
        public virtual Color BorderForeColor { get { return Color.FromKnownColor(KnownColor.InactiveCaption); } }
        // Important control border.
        public virtual Color BorderEmphasizeForeColor { get { return Color.FromKnownColor(KnownColor.ActiveBorder); } }
        // Line colors (when not border lines)
        public virtual Color LineForeColor { get { return Color.FromKnownColor(KnownColor.ControlText); } }
        #endregion // Color Properties
    }
}