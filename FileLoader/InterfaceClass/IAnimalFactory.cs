using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileLoader.InterfaceClass
{
    public interface IAnimalFactory<TAnimal>
    {
        TAnimal Create();
    }
}
