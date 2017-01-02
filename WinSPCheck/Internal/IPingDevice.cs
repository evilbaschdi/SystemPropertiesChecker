using System.Net.NetworkInformation;
using EvilBaschdi.Core.DotNetExtensions;

namespace WinSPCheck.Internal
{
    /// <summary>
    /// </summary>
    public interface IPingDevice : IValueFor<string, IPStatus>
    {
    }
}