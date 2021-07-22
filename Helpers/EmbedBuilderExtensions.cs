using Disqord;
using Humanizer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TarkovItemBot.Services.TarkovDatabase;

namespace TarkovItemBot.Helpers
{
    public static class EmbedExtensions
    {
        public static void AddEffects(this LocalEmbed embed, Effects effects)
        {
            var properties = effects.GetType().GetProperties().Where(x => x.Name != "Skill");
            if (properties.Any() || effects.Skill != null) embed.AddField("Effects and Skills", " \u200b", false);

            // Effects excluding skills as props
            foreach (var property in properties)
            {
                var value = property.GetValue(effects);
                if (value != null)
                {
                    var effect = value as Effect;

                    var action = effect.Removes ? $"*Removes* effect" : "*Adds* effect";
                    var change = effect.IsPercent ? $"{effect.Value * 100:+0.00;-#.00}%" : effect.Value.ToString("+0.00;-#.00");
                    var info = effect.Value == 0 ? action : $"`{change}` change";

                    if (effect.Duration != 0) info += $"\n`{effect.Duration}` sec. duration";
                    if (effect.Delay != 0) info += $"\n`{effect.Delay}` sec. delay";
                    if (effect.ResourceCosts != 0) info += $"\nUses `{effect.ResourceCosts}` resource";

                    embed.AddField(property.Name.Humanize(LetterCasing.Title), info + $"\n`{effect.Chance * 100}`% chance", true);
                }
            }

            // Skills
            if (effects.Skill != null)
            {
                foreach (var skill in effects.Skill)
                {
                    embed.AddField(skill.Name.Humanize(LetterCasing.Title), $"`{skill.Value:+0.00;-#.00}` change" +
                        $"\n`{skill.Duration}` sec. duration\n`{skill.Delay}` sec. delay\n`{skill.Chance * 100}`% chance", true);
                }
            }
        }

        public static void AddGridModifier(this LocalEmbed embed, GridModifier modifier)
        {
            var stringBuilder = new StringBuilder();

            if (modifier.Height != 0) stringBuilder.AppendLine($"{modifier.Height:+0;-#} height");
            if (modifier.Width != 0) stringBuilder.AppendLine($"{modifier.Width:+0;-#} width");

            var grid = stringBuilder.ToString();
            if (!string.IsNullOrWhiteSpace(grid)) embed.AddField("Grid", grid, true);
        }

        public static void AddArmorProperties(this LocalEmbed embed, ArmorProperties armor)
        {
            embed.AddField("Class", armor.Class, true);
            embed.AddField("Durability", armor.Durability, true);
            embed.AddField("Zones", armor.Zones.Humanize(x => x.Transform(To.TitleCase)), true);
            embed.AddField("Material", armor.Material.Name.Transform(To.TitleCase), true);
        }

        public static void AddGrids(this LocalEmbed embed, IReadOnlyCollection<ContainerGrid> grids)
        {
            embed.AddField("Grids", $"{grids.Count} {"grid".ToQuantity(grids.Count, ShowQuantityAs.None)}, " +
                $"{grids.Sum(x => x.Width * x.Height)} slots total", true);
        }
    }
}
