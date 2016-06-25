using System;
using System.Text;

namespace WinSPCheck.Internal
{
    /// <summary>
    /// </summary>
    public class GetWindowsVersionText : IWindowsVersionText
    {
        private readonly IWindowsVersionInformation _windowsVersionInformation;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public GetWindowsVersionText(IWindowsVersionInformation windowsVersionInformation)
        {
            if(windowsVersionInformation == null)
            {
                throw new ArgumentNullException(nameof(windowsVersionInformation));
            }
            _windowsVersionInformation = windowsVersionInformation;
        }

        /// <summary>
        /// </summary>
        public string Value
        {
            get
            {
                var values = _windowsVersionInformation.Values;

                var sb = new StringBuilder();
                sb.Append($"Version number: {values.CurrentVersion} | Current build: {values.CurrentBuild} (Build: ");
                sb.Append(!string.IsNullOrWhiteSpace(values.Ubr)
                    ? $"{values.CurrentBuild}.{values.Ubr}"
                    : $"{values.BuildLabExArray[0]}.{values.BuildLabExArray[1]}");
                sb.Append(")");
                sb.Append(Environment.NewLine);
                sb.Append($"BuildLab: {values.BuildLab}{Environment.NewLine}");
                sb.Append($"BuildLabEx: {values.BuildLabEx}{Environment.NewLine}");

                return sb.ToString();
            }
        }
    }
}