// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace WinSPCheck
{
    public class GlassEffectHelper
    {
        
        [DllImport("dwmapi.dll", PreserveSig = false)]
        static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        static extern bool DwmIsCompositionEnabled();

        public static bool EnableGlassEffect(Window window)
        {
            window.MouseLeftButtonDown += (s, e) => window.DragMove();
            return EnableGlassEffect(window, true);
        }

        public static bool EnableGlassEffect(Window window, bool enabled)
        {
            return EnableGlassEffect(window, enabled, new Thickness(-1));
        }

        public static bool EnableGlassEffect(Window window, bool enabled, Thickness margin)
        {
            if (!VersionHelper.IsAtLeastVista)
            {
                // Go and buy Windows 7 ;-)
                return false;
            }

            if (!DwmIsCompositionEnabled())
            {
                return false;
            }

            if (enabled)
            {
                IntPtr hwnd = new WindowInteropHelper(window).Handle;

                // Hintergrundfarbe von Fenster Transparent darstellen
                window.Background = Brushes.Transparent;

                // Die Farbe festlegen auf die den Glaseffekt bekommt
                HwndSource.FromHwnd(hwnd).CompositionTarget.BackgroundColor = 
                    Colors.Transparent;

                // Den Bereich für den Glaseffekt definieren
                MARGINS margins = new MARGINS(margin);
                
                // Glasseffekt aktivieren
                DwmExtendFrameIntoClientArea(hwnd, ref margins);
            }
            else
            {
                // Hintergrundfarbe des Fensters zurück auf die
                // Systemfarbe stellen
                window.Background = SystemColors.WindowBrush;
            }

            return true;
        }

    }
}
