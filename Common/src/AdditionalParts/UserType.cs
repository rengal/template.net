using System;

namespace Resto.Data
{
    [Flags]
    public enum UserType
    {
        NONE = 0,
        EMPLOYEE = 1,
        CLIENT = 2,
        SUPPLIER = 4,
        ALL = EMPLOYEE | CLIENT | SUPPLIER,
    }
}