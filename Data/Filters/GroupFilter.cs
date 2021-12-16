#nullable enable
using System;
using System.Linq.Expressions;
using Core.Models;
using LinqKit;

namespace Data.Filters {
    public class GroupFilter : AbstractFilter<Group> {
        public GroupFilter(string? searchString = null, int? groupFilter = null, int? courseFilter = null)
            : base(searchString, groupFilter, courseFilter) { }

        public override Expression<Func<Group, bool>> GetFilteringExpression() {
            Expression<Func<Group, bool>>? filteringExpression = PredicateBuilder.New<Group>(true);
            var original = filteringExpression;

            if (CourseFilter > 0) {
                filteringExpression = filteringExpression.And(x => x.CourseId == CourseFilter);
            }

            if (!string.IsNullOrEmpty(SearchString)) {
                filteringExpression = filteringExpression.And(x => x.GroupName.Contains(SearchString)
                                  || x.Course.CourseName.Contains(SearchString));
            }
            if (filteringExpression == original) {
                filteringExpression = x => true;
            }
            return filteringExpression;
        }
    }
}