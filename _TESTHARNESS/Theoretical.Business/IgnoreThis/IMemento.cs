using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Theoretical.Business
{
    public interface IMemento
    {
        Object State { get; set; }
    }
}
