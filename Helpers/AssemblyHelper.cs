using System.Reflection;

namespace TarkovItemBot.Helpers
{
    public static class AssemblyHelper
    {
        public static string GetInformationalVersion()
            => Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}
