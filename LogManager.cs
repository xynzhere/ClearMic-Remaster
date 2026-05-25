// this code was written by kingofnetflix, creator of seralyth mod menu https://github.com/Seralyth/Seralyth-Menu

using System;

namespace Seralyth.Managers
{
    public enum Level
    {
        Info,
        Warning,
        Error,
        Debug
    }

    public static class LogManager
    {
        private static Action<Level, string> _sink;

        /// <summary>Call once from the loader entrypoint (BepInEx or MelonLoader).</summary>
        public static void SetLogger(Action<Level, string> sink) => _sink = sink;

        private static void Write(Level level, object log)
        {
            string msg = log?.ToString() ?? string.Empty;

            // Fallback
            if (_sink is null)
            {
                UnityEngine.Debug.Log($"[{level}] {msg}");
                return;
            }

            _sink(level, msg);
        }

        public static void Log(object log) => Write(Level.Info, log);

        public static void Log(object log, object[] args) =>
            Write(Level.Info, string.Format(log?.ToString() ?? "", args));

        public static void LogError(object log) => Write(Level.Error, log);

        public static void LogError(object log, object[] args) =>
            Write(Level.Error, string.Format(log?.ToString() ?? "", args));

        public static void LogWarning(object log) => Write(Level.Warning, log);

        public static void LogWarning(object log, object[] args) =>
            Write(Level.Warning, string.Format(log?.ToString() ?? "", args));

        public static void LogDebug(object log) => Write(Level.Debug, log);

        public static void LogDebug(object log, object[] args) =>
            Write(Level.Debug, string.Format(log?.ToString() ?? "", args));
    }
}