using Discord;
using Humanizer;
using System.Linq;
using System.Text;
using TarkovItemBot.Services;

namespace TarkovItemBot.Helpers
{
    public static class EmbedBuilderExtensions
    {
        public static void AddEffects(this EmbedBuilder builder, Effects effects)
        {
            var properties = effects.GetType().GetProperties().Where(x => x.Name != "Skill");
            if (properties.Any() || effects.Skill != null) builder.AddField("Effects and Skills", " \u200b", false);

            // Effects excluding skills as props
            foreach (var property in properties)
            {
                var value = property.GetValue(effects);
                if (value != null)
                {
                    var effect = value as Effect;

                    var action = effect.Removes ? $"*Removes* effect" : "*Adds* effect";
                    var change = effect.Value == 0 ? action : $"`{effect.Value:+0.00;-#.00}` change";
                    var info = !effect.Removes ? $"\n`{effect.Duration}` sec. duration\n`{effect.Delay}` sec. delay" :
                        $"\nUses `{effect.ResourceCosts}` resource";
                    builder.AddField(property.Name.Transform(To.TitleCase), change + $"\n `{effect.Chance * 100}`% chance{info}", true);
                }
            }

            // Skills
            if (effects.Skill != null)
            {
                foreach (var skill in effects.Skill)
                {
                    builder.AddField(skill.Name.Transform(To.TitleCase), $"`{skill.Value:+0.00;-#.00}` change" +
                        $"\n`{skill.Chance * 100}`% chance\n`{skill.Duration}` sec. duration\n`{skill.Delay}` sec. delay", true);
                }
            }
        }

        public static void AddGridModifier(this EmbedBuilder builder, GridModifier modifier)
        {
            var stringBuilder = new StringBuilder();

            if (modifier.Height != 0) stringBuilder.AppendLine($"{modifier.Height:+0;-#} height");
            if (modifier.Width != 0) stringBuilder.AppendLine($"{modifier.Width:+0;-#} width");

            var grid = stringBuilder.ToString();
            if (!string.IsNullOrWhiteSpace(grid)) builder.AddField("Grid", grid, true);
        }
    }
}
