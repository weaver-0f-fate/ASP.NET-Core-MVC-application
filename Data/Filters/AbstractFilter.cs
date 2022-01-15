using System;
using System.Linq.Expressions;
using Core.Models;
using Interfaces;

namespace Data.Filters {
    public abstract class AbstractFilter<T> : IFilter<T> where T : AbstractModel{
        protected readonly string SearchString;
        protected readonly int? GroupFilter;
        protected readonly int? CourseFilter;

        protected AbstractFilter(string searchString = null, int? groupFilter = null, int? courseFilter = null) {
            SearchString = searchString;
            GroupFilter = groupFilter;
            CourseFilter = courseFilter;
        }

        public abstract Expression<Func<T, bool>> GetFilteringExpression();
    }
}