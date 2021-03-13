using Qmmands;
using System.Linq;

namespace TarkovItemBot.Helpers
{
    public static class CommandExtensions
    {
        public static string GetUsage(this Command command)
        {
            var usage = command.Name;
            if (!command.Parameters.Any()) return usage;

            foreach (var parameter in command.Parameters)
                usage += $" {parameter.GetUsage()}";

            return usage;
        }

        public static string GetUsage(this Parameter parameter)
        {
            if (parameter.IsOptional)
                return $"[{parameter.Name} = {parameter.DefaultValue}]";
            else if (parameter.IsMultiple)
                return $"|{parameter.Name}|";
            else if (parameter.IsRemainder)
                return $"...{parameter.Name}";
            else
                return $"<{parameter.Name}>";
        }
    }
}
