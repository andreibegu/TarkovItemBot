using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace TarkovItemBot.Preconditions
{
    public class RequireLengthAttribute : ParameterPreconditionAttribute
    {
        private readonly int _min;
        private readonly int _max;

        public RequireLengthAttribute(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, ParameterInfo parameter, object value, IServiceProvider services)
        {
            var length = value.ToString().Length;

            if (length < _min || length > _max)
            {
                return Task.FromResult(PreconditionResult.FromError($"Parameter `{parameter.Name}` must be between `{_min}` and `{_max}` characters long!"));
            }

            return Task.FromResult(PreconditionResult.FromSuccess());
        }
    }
}
