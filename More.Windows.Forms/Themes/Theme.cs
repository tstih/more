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
using System.Collections.Generic;
using System.Drawing;

namespace More.Windows.Forms
{
    public class Theme
    {
        #region Const(s)
        private const string BASIC_THEME_NAME = "Basic";
        #endregion // Const(s)

        #region Static(s)
        private static Dictionary<string, Theme> _availableThemes;
        public static Dictionary<string, Theme> AvailableThemes { get { return _availableThemes; } }
        static Theme()
        {
            // First theme.
            _availableThemes = new Dictionary<string, Theme>();
            // Add yourself.
            AvailableThemes.Add(BASIC_THEME_NAME, new Theme());
        }
        #endregion // Static(s)

        #region Meta Properties
        public virtual string Name { get { return BASIC_THEME_NAME; } }
        #endregion // Meta Properties

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
        #endregion // Color Properties
    }
}