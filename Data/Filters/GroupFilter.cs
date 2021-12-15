using System;
using System.Linq.Expressions;
using Core.Models;
using LinqKit;

namespace Data.Filters {
    public class GroupFilter : AbstractFilter<Group> {
        public GroupFilter(string searchString = null, int? groupFilter = null, int? courseFilter = null)
            : base(searchString, groupFilter, courseFilter) { }

        public override Expression<Func<Group, bool>> GetFilteringExpression() {
            Expression<Func<Group, bool>>? exp = PredicateBuilder.New<Group>(false);
            var original = exp;

            if (CourseFilter > 0) {
                exp = exp.Or(x => x.CourseId == CourseFilter);
            }

            if (!string.IsNullOrEmpty(SearchString)) {
                exp = exp.Or(x => x.GroupName.Contains(SearchString)
                                  || x.Course.CourseName.Contains(SearchString));
            }
            if (exp == original) {
                exp = x => true;
            }
            return exp;
        }
    }
}