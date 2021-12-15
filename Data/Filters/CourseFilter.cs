#nullable enable
using System;
using System.Linq.Expressions;
using Core.Models;
using LinqKit;

namespace Data.Filters {
    public class CourseFilter : AbstractFilter<Course>{

        public CourseFilter(string searchString = null, int? groupFilter = null, int? courseFilter = null) 
            : base(searchString, groupFilter, courseFilter) { }


        public override Expression<Func<Course, bool>> GetFilteringExpression() {
            Expression<Func<Course, bool>>? exp = PredicateBuilder.New<Course>(false);
            var original = exp;

            if (!string.IsNullOrEmpty(SearchString)) {
                exp = exp.Or(x => x.CourseName.Contains(SearchString)
                                  || x.CourseDescription.Contains(SearchString));
            }
            if (exp == original) {
                exp = x => true;
            }

            return exp;
        }
    }
}