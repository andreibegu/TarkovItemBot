using Discord;
using Humanizer;
using System.Linq;

namespace TarkovItemBot.Services
{
    [Kind(ItemKind.Medical)]
    public class MedicalItem : CommonItem
    {
        public string Type { get; set; }
        public int Resources { get; set; }
        public int ResourceRate { get; set; }
        public float UseTime { get; set; }
        public Effects Effects { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Type", Type.Transform(To.TitleCase), true);
            builder.AddField("Resources", ResourceRate == 0 ? Resources : $"{Resources} / {ResourceRate}", true);
            builder.AddField("Use time", $"{UseTime} sec.", true);

            // Effects excluding skills as props
            foreach (var property in Effects.GetType().GetProperties().Where(x => x.Name != "Skill"))
            {
                var value = property.GetValue(this.Effects);
                if (value != null)
                {
                    var effect = value as Effect;

                    var change = effect.Value == 0 ? "Adds effect" : $"`{effect.Value:+0.00;-#.00}` change;";
                    var action = effect.Removes ? "Removes effect" : change;
                    var info = !effect.Removes ? $"`{effect.Duration}` sec. / `{effect.Delay}` sec. delay" :
                        $"uses `{effect.ResourceCosts}` resource";
                    builder.AddField(property.Name.Transform(To.TitleCase), action + $"\n `{effect.Chance * 100}`% chance / {info}", false);
                }
            }

            // Skills
            if(Effects.Skill != null)
            {
                foreach (var skill in Effects.Skill)
                {
                    builder.AddField(skill.Name.Transform(To.TitleCase), $"`{skill.Value:+0.00;-#.00}` change;" +
                        $"\n `{skill.Chance * 100}`% chance / `{skill.Duration}` sec. / `{skill.Delay}` sec. delay", false);
                }
            }

            return builder;
        }
    }
}
