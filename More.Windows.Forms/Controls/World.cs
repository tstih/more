/*
 * World.cs
 * 
 * The world control maps things into the 2D world.
 *  - It enables you to draw into the world using world coordinates
 *  - It handles zoom
 *  - It manages viewport (part of the world that you're viweing)
 *  - It converts from world to pixels and vice versa
 * This control can be used for numerous tasks:
 *  - Maps
 *  - Drawings
 *  - Math graphs
 *  - ...
 *  The control enables you to override coordinate mapping
 *  which enables implementation such as longitude/latitude mapping.
 *  
 * TODO: Integrate open street maps.
 *  https://wiki.openstreetmap.org/wiki/Slippy_map_tilenames#C.23
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 31.12.2020   tstih   Happy new year!
 * 
 */
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace More.Windows.Forms
{
    public class World : Control
    {
        #region Const(s)
        private const string DEFAULT_WORLD_UNIT = "m"; // meter
        private const int DEFAULT_WORLD_SIZE = 10000; // Default size is 0,0,10000,10000
        private const CoordinateType DEFAULT_COORD_TYPE = CoordinateType.Cartesian;
        #endregion // Const(s)

        #region Ctor
        public World()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            _worldUnit = DEFAULT_WORLD_UNIT;
            _worldRect = new Rectangle(0, 0, DEFAULT_WORLD_SIZE, DEFAULT_WORLD_SIZE);
        }
        #endregion // Ctor

        #region Properties
        private Rectangle _worldRect;
        /// <summary>
        /// Rectangle for the world. i.e. min x, min y, max x, max y.
        /// Polar worlds adjust these corodinates.
        /// </summary>
        [Description("Rectangle for the world.i.e.min x, min y, max x, max y."), Category("Appearance")]
        public Rectangle WorldRect { 
            get { return _worldRect; } 
            set
            {
                // No change? It's an expensive operation so don't bother.
                if (_worldRect == value) return;
                _worldRect = value;
                Invalidate();
            }
        }

        private string _worldUnit;
        /// <summary>
        /// Unit name (i.e. m, mm, etc).
        /// </summary>
        [Description("Unit name (i.e. m, mm)."), Category("Appearance")]
        public string WorldUnit
        {
            get { return _worldUnit; }
            set
            {
                if (_worldUnit == value) return;
                _worldUnit = value;
                Invalidate();
            }
        }

        private CoordinateType _coordType;
        /// <summary>
        /// Polar coordinate system or cartesian coordinate system?
        /// </summary>
        [Description("Polar or cartesian coordinates?"), Category("Appearance")]
        public CoordinateType CoordType
        {
            get { return _coordType; }
            set
            {
                if (_coordType == value) return;
                _coordType = value;
                Invalidate();
            }
        }
        #endregion // Properties
    }

    public enum CoordinateType { Cartesian, Polar }
}