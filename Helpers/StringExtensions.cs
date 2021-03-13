using System.Collections.Generic;
using System.Linq;
using TarkovItemBot.Services.TarkovDatabase;

namespace TarkovItemBot.Helpers
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return char.ToLowerInvariant(str[0]) + str[1..];
            }
            return str;
        }

        public static string FirstCharUpper(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return char.ToUpperInvariant(str[0]) + str[1..];
            }
            return str;
        }

        public static string AddBonuses(this string str, Module module, int level,
            IEnumerable<Bonus> bonuses, bool previous = false)
        {
            foreach (var bonus in bonuses)
            {
                var total = module.Stages.Take(level).SelectMany(x => x.Bonuses)
                    .Where(x => x.Type == bonus.Type).Sum(x => x.Value);

                string value = "";
                if(bonus.Value != 0)
                {
                    var totalDisplay = $"`{total:+0.00;-#.00}` total";
                    value = !previous ? $"(`{bonus.Value:+0.00;-#.00}` curr. / {totalDisplay})"
                        : $"({totalDisplay})";
                }

                var skill = bonus.Type == BonusType.SkillGroupLevelingBoost ?
                    $", {bonus.SkillType}" : "";

                str += $"• {bonus.Description}{skill} {value}\n";
            }

            return str;
        }
    }
}
