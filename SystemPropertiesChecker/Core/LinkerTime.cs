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
        public string Value => Assembly.GetExecutingAssembly().GetLinkerTime().ToString("yyyy-MM-dd hh:mm:ss");
    }
}