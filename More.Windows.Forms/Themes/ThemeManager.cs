/*
 * ThemeManager.cs
 * 
 * Manages the themes. When the current theme is changed, it 
 * raises static event that all controls supporting themes 
 * subscribe to. 
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2021 Tomaz Stih
 * 
 * 22.05.2022   tstih   
 * 
 */
using System;

namespace More.Windows.Forms
{
    public class ThemeManager
    {
        // Current theme property.
        private static Theme _currentTheme;
        public static Theme CurrentTheme
        {
            get { return _currentTheme; }
            set
            {
                // Same theme?
                if (value == _currentTheme) return;

                if (value == null) 
                    _currentTheme = new Theme(); // DEfault theme.
                else if (value != _currentTheme) // It's a different theme than last time.
                    _currentTheme = value;

                // And apply.
                ThemeChange.Raise(null, new ThemeChangeEventArgs(_currentTheme));
            }
        }

        // Theme change event.
        public static event EventHandler<ThemeChangeEventArgs> ThemeChange;
    }

    // Event arguments for the ThemeChange event.
    public class ThemeChangeEventArgs : EventArgs
    {
        private Theme _theme;
        internal ThemeChangeEventArgs(Theme theme)
        {
            _theme = theme;
        }
        public Theme Theme { get { return _theme; } }
    }
}