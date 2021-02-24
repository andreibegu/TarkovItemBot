using System;

namespace TarkovItemBot.Services.TarkovDatabase
{
    [AttributeUsage(AttributeTargets.Field)]
    public class KindTypeAttribute : Attribute
    {
        public Type KindType;
        public KindTypeAttribute(Type kindType)
        {
            KindType = kindType;
        }
    }
}
