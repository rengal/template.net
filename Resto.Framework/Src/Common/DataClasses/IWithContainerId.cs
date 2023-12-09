using System;

namespace Resto.Framework.Data
{
    public interface IWithContainerId
    {
        Guid ContainerId { get; }
    }
}