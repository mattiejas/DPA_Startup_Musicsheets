using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface IView<in T>
    {
        void Load(T data);
    }
}
