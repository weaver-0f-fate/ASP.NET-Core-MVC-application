#nullable enable
using System;
using System.Linq.Expressions;
using Core.Models;
using LinqKit;

namespace Data.Filters {
    public class CourseFilter : AbstractFilter<Course>{

        public CourseFilter(string? searchString = null, int? groupFilter = null, int? courseFilter = null) 
            : base(searchString, groupFilter, courseFilter) { }


        public override Expression<Func<Course, bool>> GetFilteringExpression() {
            Expression<Func<Course, bool>>? filteringExpression = PredicateBuilder.New<Course>(true);
            var original = filteringExpression;

            if (!string.IsNullOrEmpty(SearchString)) {
                filteringExpression = filteringExpression.And(x => x.CourseName.Contains(SearchString)
                                  || x.CourseDescription.Contains(SearchString));
            }
            if (filteringExpression == original) {
                filteringExpression = x => true;
            }

            return filteringExpression;
        }
    }
}