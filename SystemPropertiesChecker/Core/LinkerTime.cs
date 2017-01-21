using System.Globalization;
using System.Reflection;
using EvilBaschdi.Core.Application;

namespace SystemPropertiesChecker.Core
{
    /// <summary>
    /// </summary>
    public class LinkerTime : ILinkerTime
    {
        /// <summary>
        /// </summary>
        public string Value => Assembly.GetExecutingAssembly().GetLinkerTime().ToString(CultureInfo.InvariantCulture);
    }
}