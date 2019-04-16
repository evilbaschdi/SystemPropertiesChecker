using System;
using System.Text;

namespace SystemPropertiesChecker.Internal
{
    /// <summary>
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class WindowsVersionText : IWindowsVersionText
    {
        private readonly IWindowsVersionInformation _windowsVersionInformation;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public WindowsVersionText(IWindowsVersionInformation windowsVersionInformation)
        {
            _windowsVersionInformation = windowsVersionInformation ?? throw new ArgumentNullException(nameof(windowsVersionInformation));
        }

        /// <summary>
        /// </summary>
        public string Value
        {
            get
            {
                var values = _windowsVersionInformation.Value;

                var sb = new StringBuilder();
                sb.Append($"Version number: {values.CurrentVersion} | Current build: {values.CurrentBuild} (Build: ");
                sb.Append(!string.IsNullOrWhiteSpace(values.Ubr)
                    ? $"{values.CurrentBuild}.{values.Ubr}"
                    : $"{values.BuildLabExArray[0]}.{values.BuildLabExArray[1]}");
                sb.Append(")");
                sb.AppendLine();
                sb.AppendLine($"Install Date: {values.InstallDate}");
                sb.AppendLine($"BuildLab: {values.BuildLab}");
                sb.AppendLine($"BuildLabEx: {values.BuildLabEx}");

                return sb.ToString();
            }
        }
    }
}