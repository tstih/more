/*
 * ScrollableControlBase.cs
 * 
 * Base control for all More controls. This class is used
 * to inject same control palette to different controls.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2021 Tomaz Stih
 * 
 * 05.02.2022   tstih   
 * 
 */
using System.Linq;
using System.Windows.Forms;

namespace More.Windows.Forms
{
    public class ScrollableControlBase : ScrollableControl
    {
        #region Ctor
        public ScrollableControlBase()
        {
            // Control details.
            DoubleBuffered = true;
            ResizeRedraw = true;

            // Default theme.
            _theme = Theme.AvailableThemes.Values.First();
        }
        #endregion // Ctor

        #region Overridable(s)
        public virtual void OnApplyTheme(Theme theme) { }
        #endregion // Overridable(s)

        #region Properties
        private Theme _theme;
        public Theme Theme
        {
            get { return _theme; }
            set
            {
                // Same theme?
                if (value == _theme) return;

                if (value == null)
                    _theme = Theme.AvailableThemes.Values.First();
                else if (value != _theme) // It's a different theme than last time.
                    _theme = value;

                // And apply.
                OnApplyTheme(_theme);
            }
        }
        #endregion // Properties
    }
}
