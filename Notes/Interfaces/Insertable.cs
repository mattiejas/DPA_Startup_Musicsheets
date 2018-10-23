using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface Insertable<T>
    {
        void Insert(T value);
    }
}
