using System;

namespace TarkovItemBot.Services
{
    [AttributeUsage(AttributeTargets.Class)]
    public class KindAttribute : Attribute
    {
        public ItemKind Kind;
        public KindAttribute(ItemKind kind)
        {
            Kind = kind;
        }
    }
}
