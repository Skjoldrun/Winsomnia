using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Winsomnia.Utility
{
    /// <summary>
    /// Reads application settings from appsettings.json.
    /// Missing keys fall back to the original App.config defaults.
    /// </summary>
    public sealed class AppSettings
    {
        public const string DefaultFilePath = "appsettings.json";

        private readonly IReadOnlyDictionary<string, string> _values;

        public AppSettings(string path = DefaultFilePath)
        {
            if (!Path.IsPathRooted(path))
                path = Path.Combine(AppContext.BaseDirectory, path);

            _values = Load(path);
        }

        private static IReadOnlyDictionary<string, string> Load(string path)
        {
            var values = new Dictionary<string, string>();

            if (File.Exists(path))
            {
                using var document = JsonDocument.Parse(File.ReadAllText(path));

                foreach (var property in document.RootElement.EnumerateObject())
                {
                    values[property.Name] = ToStringValue(property.Value);
                }
            }

            return values;
        }

        private static string ToStringValue(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString() ?? string.Empty,
                JsonValueKind.Number => element.GetRawText(),
                JsonValueKind.True or JsonValueKind.False => element.GetBoolean().ToString().ToLowerInvariant(),
                _ => element.GetRawText(),
            };
        }

        private string? this[string key] =>
            _values.TryGetValue(key, out var value) ? value : null;

        /// <summary>
        /// Interval in minutes for the virtual input timer.
        /// </summary>
        public int VirtualInputTimer => int.TryParse(this["VirtualInputTimer"], out var value) ? value : 4;

        /// <summary>
        /// Whether the app activates automatically on startup.
        /// </summary>
        public bool ActivateOnStart => bool.TryParse(this["ActivateOnStart"], out var value) ? value : true;

        /// <summary>
        /// Whether virtual mouse movement is activated.
        /// </summary>
        public bool VirtualMouseMoveActivated => bool.TryParse(this["VirtualMouseMoveActivated"], out var value) ? value : false;

        /// <summary>
        /// Whether virtual key press is activated.
        /// </summary>
        public bool VirtualKeyPressActivated => bool.TryParse(this["VirtualKeyPressActivated"], out var value) ? value : true;

        /// <summary>
        /// Whether system state idle prevention is activated.
        /// </summary>
        public bool SystemStateIdlePreventionActivated => bool.TryParse(this["SystemStateIdlePreventionActivated"], out var value) ? value : false;
    }
}
