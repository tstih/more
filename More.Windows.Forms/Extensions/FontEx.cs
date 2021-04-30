/*
 * FontEx.cs
 * 
 * Extensions for the font object.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * THANK YOU:
 *  https://microsoft.public.platformsdk.gdi.narkive.com/l0y9TIfy/gdi-how-to-get-actual-footprint-height-of-a-string
 * 
 * 24.04.2021   tstih
 * 
 */

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace More.Windows.Forms
{
    public static class FontEx
    {
        #region Win32 
        [StructLayout(LayoutKind.Sequential)]
        public struct TEXTMETRIC
        {
            public int tmHeight;
            public int tmAscent;
            public int tmDescent;
            public int tmInternalLeading;
            public int tmExternalLeading;
            public int tmAveCharWidth;
            public int tmMaxCharWidth;
            public int tmWeight;
            public int tmOverhang;
            public int tmDigitizedAspectX;
            public int tmDigitizedAspectY;
            public char tmFirstChar;
            public char tmLastChar;
            public char tmDefaultChar;
            public char tmBreakChar;
            public byte tmItalic;
            public byte tmUnderlined;
            public byte tmStruckOut;
            public byte tmPitchAndFamily;
            public byte tmCharSet;
        }

        [DllImport("Gdi32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("Gdi32.dll", CharSet = CharSet.Unicode)]
        private static extern bool GetTextMetrics(IntPtr hdc, out TEXTMETRIC lptm);

        [DllImport("Gdi32.dll", CharSet = CharSet.Unicode)]
        private static extern bool DeleteObject(IntPtr hdc);

        public static TEXTMETRIC GetTextMetrics(this Font font, Graphics g)
        {
            IntPtr hDC = IntPtr.Zero;
            TEXTMETRIC textMetric;
            IntPtr hFont = IntPtr.Zero;
            try
            {
                hDC = g.GetHdc();
                hFont = font.ToHfont();
                IntPtr hFontDefault = SelectObject(hDC, hFont);
                bool result = GetTextMetrics(hDC, out textMetric);
                SelectObject(hDC, hFontDefault);
            }
            finally
            {
                if (hFont != IntPtr.Zero) DeleteObject(hFont);
                if (hDC != IntPtr.Zero) g.ReleaseHdc(hDC);
            }
            return textMetric;
        }
        #endregion // Win32

    }
}
