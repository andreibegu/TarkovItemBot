using System.Collections.Generic;
using System.Linq;

namespace TarkovItemBot.Helpers
{
    public static class EnumerableHelper
    {
        public static IReadOnlyList<string> ToQueryStrings(IEnumerable<string> ids, int limit)
        {
            var queries = new List<string>();

            var count = ids.Count();
            var pages = count % limit == 0 ? count / limit : count / limit + 1;

            for(int i = 0; i < pages; i++)
            {
                var offset = limit * i;
                var end = count - offset;
                var queryIds = ids.Skip(offset).Take(end);
                queries.Add(string.Join(",", queryIds));
            }

            return queries;
        }
    }
}
