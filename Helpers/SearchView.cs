using Disqord;
using Disqord.Extensions.Interactivity.Menus;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using TarkovItemBot.Services.TarkovDatabase;
using TarkovItemBot.Services.TarkovDatabaseSearch;

namespace TarkovItemBot.Helpers
{
    public class SearchView : ViewBase
    {
        public SearchView(IReadOnlyCollection<SearchItem> items, TarkovDatabaseClient client)
            : base(new LocalMessage()
                  .WithContent("Multiple results found! Please pick one item from the list:"))
        {
            var selectionComponent = new SelectionViewComponent(async args =>
            {
                this.Menu.Stop();
                this.ClearComponents();

                var response = args.Interaction.SelectedValues[0].Split("/");
                var itemId = response[0];
                var kind = Enum.Parse<ItemKind>(response[1]);

                var item = await client.GetItemAsync(itemId, kind);
                this.TemplateMessage = new LocalMessage().WithEmbeds(item.ToEmbed());
                await (this.Menu as DefaultMenu).ApplyChangesAsync();
            });

            foreach (var item in items)
            {
                selectionComponent.Row = 0;
                selectionComponent.Options.Add(
                    new LocalSelectionComponentOption($"{item.ShortName} ({item.Kind.Humanize()})".Truncate(25),
                        $"{item.Id}/{item.Kind}")
                    .WithDescription(item.Description.Truncate(50)));
            }

            this.AddComponent(selectionComponent);
        }
    }
}
