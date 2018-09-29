using Common.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Common.Utils
{
    public static class DurationUtils
    {
        public static Durations GetClosestDuration(double duration)
        {
            IList<int> durations = Enum.GetValues(typeof(Durations)).OfType<Durations>().Select(d => (int) d).ToList();
            return (Durations) durations.Aggregate((x, y) => Math.Abs(x - duration) < Math.Abs(y - duration) ? x : y);
        }
    }
}
