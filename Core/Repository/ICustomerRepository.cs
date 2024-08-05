﻿using Core.Entity;

namespace Core.Repository
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        IList<Customer> ObterTodosOperadores();
    }
}
