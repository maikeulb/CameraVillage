using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CameraVillage.Domain.Models.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
    }
}
