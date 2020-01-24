using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Services;

namespace WebApplication2.Domain
{
    public interface IFormRepository
    {
        List<Form> GetAll();
        void Add(Form form);
    }
}
