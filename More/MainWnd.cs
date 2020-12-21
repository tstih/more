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
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.More;

namespace More
{
    public partial class MainWnd : Form
    {
        public MainWnd()
        {
            InitializeComponent();
        }
        
        private void MainWnd_Load(object sender, System.EventArgs e)
        {
            Image sprite = Image.FromFile("pacman.png");
            _spriteGrid.SourceImage = sprite;
        }

        private void _spriteGrid_ZoomIn(object sender, ZoomInArgs e)
        {
            int size = _spriteGrid.CellWidth;
            size = size + 2;
            if (size<32)
                _spriteGrid.CellWidth = _spriteGrid.CellHeight = size;
        }

        private void _spriteGrid_ZoomOut(object sender, ZoomOutArgs e)
        {
            int size = _spriteGrid.CellWidth;
            if (size > 2)
                size = size - 2;
            else
                size = 1;
            
            _spriteGrid.CellWidth = _spriteGrid.CellHeight = size;
        }

        private bool _mouseDown;
        private int dr, dc;

        private void _spriteGrid_CellMouseDown(object sender, CellMouseButtonArgs e)
        {
            dr = e.Row;
            dc = e.Column;
            _mouseDown = true;
        }

        private void _spriteGrid_CellMouseMove(object sender, CellMousePosArgs e)
        {
            if (_mouseDown)
            {

            }
        }

        private void _spriteGrid_CellMouseUp(object sender, CellMouseButtonArgs e)
        {
            if (_mouseDown)
            {
                // Action!
                _mouseDown = false;
            }
        }
    }
}