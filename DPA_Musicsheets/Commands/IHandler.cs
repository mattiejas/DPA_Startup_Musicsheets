using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);
        Request Handle(Request request);
    }
}
