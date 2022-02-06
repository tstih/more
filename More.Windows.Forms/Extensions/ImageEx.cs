/*
 * ImageEx.cs
 * 
 * Extensions for the image object.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * THANK YOU:
 *  https://microsoft.public.platformsdk.gdi.narkive.com/l0y9TIfy/gdi-how-to-get-actual-footprint-height-of-a-string
 * 
 * 01.05.2021   tstih
 * 
 */
using System.Drawing;

namespace More.Windows.Forms
{
    public static class ImageEx
    {
        public static Rectangle Align(this Image image, Rectangle r, ContentAlignment align)
        {
            Size pointImageSize = image.Size;
            int xLoc = r.X + 2, yLoc;
            if ((align & (ContentAlignment.TopRight | ContentAlignment.MiddleRight | ContentAlignment.BottomRight)) != 0)
                xLoc = (r.X + r.Width - 4) - pointImageSize.Width;
            else if ((align & (ContentAlignment.TopCenter | ContentAlignment.MiddleCenter | ContentAlignment.BottomCenter)) != 0)
                xLoc = r.X + (r.Width - pointImageSize.Width) / 2;
            if ((align & (ContentAlignment.BottomLeft | ContentAlignment.BottomCenter | ContentAlignment.BottomRight)) != 0)
                yLoc = (r.Y + r.Height - 4) - pointImageSize.Height;
            else if ((align & (ContentAlignment.TopLeft | ContentAlignment.TopCenter | ContentAlignment.TopRight)) != 0)
                yLoc = r.Y + 2;
            else
                yLoc = r.Y + (r.Height - pointImageSize.Height) / 2;
            return new Rectangle(xLoc, yLoc, pointImageSize.Width, pointImageSize.Height);
        }
    }
}
