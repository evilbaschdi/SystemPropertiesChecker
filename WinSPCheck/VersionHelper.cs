// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

using System;
using System.Diagnostics;

namespace WinSPCheck
{
    /// <summary>
    /// Helpmethods for OS Version checks
    /// </summary>
    public class VersionHelper
    {
        /// <summary>
        /// OS is at least Windows Vista
        /// </summary>
        public static bool IsAtLeastVista
        {
            get 
            {
                if (Environment.OSVersion.Version.Major < 6)
                {
                    Debug.WriteLine("How about trying this on Vista?");
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// OS is Windows 7 or higher
        /// </summary>
        public static bool IsWindows7orHigher
        {
            get
            {
                if (Environment.OSVersion.Version.Major == 6 &&
                    Environment.OSVersion.Version.Minor >= 1)
                {
                    return true;
                }
                else if (Environment.OSVersion.Version.Major > 6)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
