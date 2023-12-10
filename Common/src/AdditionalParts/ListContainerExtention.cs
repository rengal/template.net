using System;
using System.Collections.Generic;
using System.Linq;

namespace Resto.Data
{
    public static class ListContainerExtention
    {
        public static Container GetContainer(this List<Container> containers, Guid Id)
        {
            return containers.FirstOrDefault(cont => cont.Id == Id);
        }
    }
}