using System.Windows;

namespace WinSPCheck.Extension
{
    /// <summary>
    ///     Extentionclass for Controls
    /// </summary>
    public static class ControlHelpers
    {
        /// <summary>
        ///     returns Visibility by bool.
        /// </summary>
        /// <param name="isVisible"></param>
        /// <returns></returns>
        public static Visibility ToVisibility(this bool isVisible)
        {
            return isVisible ? Visibility.Visible : Visibility.Hidden;
        }
    }
}