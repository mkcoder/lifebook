using System.Collections.Generic;
using System.Linq;

namespace lifebook.core.projection.Interfaces
{
    public interface IEntitySet<T>
    {
        List<T> ToList();
        IQueryable<T> AsQueryable();
    }
}