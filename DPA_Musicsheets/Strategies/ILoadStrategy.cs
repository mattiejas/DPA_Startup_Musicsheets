using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;

namespace DPA_Musicsheets.Strategies
{
    public interface ILoadStrategy<T>
    {
        bool Load(T input);
        void Apply();

        /**
         * Apply only to the matching selection
         */
        void Apply(Func<IViewManager, bool> match);
    }
}
