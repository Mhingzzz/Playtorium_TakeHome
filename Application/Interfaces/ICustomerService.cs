using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface ICustomerService
    {
        Task<Customers> GetCustomerById(int id);
        Task<Customers> AddCustomerPoint(int id, int point);
    }
}
