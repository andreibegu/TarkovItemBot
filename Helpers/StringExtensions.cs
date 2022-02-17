using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public static string RemoveAccents(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            char[] chars = text
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c)
                != UnicodeCategory.NonSpacingMark).ToArray();

            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        public static string Slugify(this string phrase)
        { 
            string value = phrase.RemoveAccents().ToLower();

            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "-", RegexOptions.Compiled);
            value = value.Trim('-', '_');
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
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
                if (bonus.Value != 0)
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
