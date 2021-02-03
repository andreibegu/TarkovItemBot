using System;

namespace TarkovItemBot.Services
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
