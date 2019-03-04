using System.Globalization;
using System.Reflection;
using EvilBaschdi.Core.Extensions;

namespace SystemPropertiesChecker.Core
{
    /// <summary>
    /// </summary>
    public class LinkerTime : ILinkerTime
    {
        /// <summary>
        /// </summary>
        public string Value
        {
            get {

                var version = typeof(MainWindow).Assembly.GetName().Version;
                return version.ToString();
            }
        }
    }
}