using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface IBuilder<T>
    {
        T Build();
    }
}
