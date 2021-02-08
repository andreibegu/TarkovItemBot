using Discord.Commands;
using System.Linq;

namespace TarkovItemBot.Helpers
{
    public static class CommandExtensions
    {
        public static string GetUsage(this CommandInfo command)
        {
            var usage = command.Name;
            if (!command.Parameters.Any()) return usage;

            foreach (var parameter in command.Parameters)
            {
                if (parameter.IsOptional)
                    usage += $" [{parameter.Name} = {parameter.DefaultValue}]";
                else if (parameter.IsMultiple)
                    usage += $" |{parameter.Name}|";
                else if (parameter.IsRemainder)
                    usage += $" ...{parameter.Name}";
                else
                    usage += $" <{parameter.Name}>";
            }

            return usage;
        }
    }
}
