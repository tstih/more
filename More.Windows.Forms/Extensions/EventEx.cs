/*
 * EventHandlerEx.cs
 * 
 * Extensions to the Event object.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 16.02.2020   tstih
 * 
 */
 using System;

namespace More.Windows.Forms
{
    public static class EventEx
    {
        /// <summary>
        /// Saves us from writing if (evnt!=null) evnt(sender,args) one thousand times.
        /// </summary>
        /// <typeparam name="T">Type of event arguments</typeparam>
        /// <param name="ev">Event</param>
        /// <param name="sender">Who sends it, mostly this</param>
        /// <param name="args">Event arguments</param>
        public static void Raise<T>(this EventHandler<T> ev, object sender, T args) where T:EventArgs
        {
            if (ev != null)
                ev(sender, args);
        }
    }
}
