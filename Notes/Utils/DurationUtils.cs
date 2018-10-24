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
            IList<int> durations = Enum.GetValues(typeof(Durations)).OfType<Durations>().Select(d => (int)d).ToList();
            return (Durations)durations.Aggregate((x, y) => Math.Abs(x - duration) < Math.Abs(y - duration) ? x : y);
        }

        /*
         * Als maatsoort 4/4 is, dan heeft een achtste noot een duur van 0,5 tellen.
         * Als een achtste noot één dot heeft, dan duurt de noot 0,5 + (0,5/2) = 0,75 tellen.
         * Als een achtste noot twee dots heeft, dan duurt de noot 0,75 + (0,25/2) = 0,875 tellen:
         */
        public static double GetProgressDuration(double duration, int dots)
        {
            return GetProgressDurationHelper(duration, duration, dots);
        }

        private static double GetProgressDurationHelper(double duration, double alterDuration, int dots)
        {
            if (dots == 0) return duration;
            return GetProgressDurationHelper(duration + (alterDuration / 2), (alterDuration / 2), --dots);
        }
    }
}
