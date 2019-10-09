using System.Collections.Generic;

namespace lifebook.core.projection.Interfaces
{
    public interface IEntitySet<T>
    {
        List<T> ToList();
    }
}