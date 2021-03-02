using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TarkovItemBot.Services.TarkovDatabase;

namespace TarkovItemBot.Helpers
{
    class BonusTypeComparer : IEqualityComparer<Bonus>
    {
        public bool Equals(Bonus x, Bonus y)
        {
            return x.Type == y.Type;
        }

        public int GetHashCode([DisallowNull] Bonus obj)
        {
            return obj.Type.GetHashCode();
        }
    }
}
