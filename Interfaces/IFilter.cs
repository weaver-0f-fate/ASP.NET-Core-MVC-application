using System;
using System.Linq.Expressions;
using Core.Models;

namespace Interfaces {
    public interface IFilter<T> where T : AbstractModel {
        Expression<Func<T, bool>> GetFilteringExpression();
    }
}
