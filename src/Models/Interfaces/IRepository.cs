﻿using RolleiShop.Models.Entities;
using System.Collections.Generic;

namespace RolleiShop.Models.Interfaces
{
    public interface IRepository<T> where T : Entity
    {
        T GetById(int id);
        T GetSingleBySpec(ISpecification<T> spec);
        IEnumerable<T> ListAll();
        IEnumerable<T> List(ISpecification<T> spec);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}