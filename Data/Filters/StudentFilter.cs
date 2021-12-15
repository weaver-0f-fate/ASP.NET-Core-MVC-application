#nullable enable
using System;
using System.Linq.Expressions;
using Core.Models;
using LinqKit;

namespace Data.Filters {
    public class StudentFilter : AbstractFilter<Student> {
        public StudentFilter(string? searchString = null, int? groupFilter = null, int? courseFilter = null)
            : base(searchString, groupFilter, courseFilter) { }

        public override Expression<Func<Student, bool>> GetFilteringExpression() {
            Expression<Func<Student, bool>>? filteringExpression = PredicateBuilder.New<Student>(false);
            var original = filteringExpression;

            if (GroupFilter > 0) {
                filteringExpression = filteringExpression.Or(x => x.GroupId == GroupFilter);
            }
            else if(CourseFilter > 0) {
                filteringExpression = filteringExpression.Or(x => x.Group.CourseId == CourseFilter);

            }
            if (!string.IsNullOrEmpty(SearchString)) {
                filteringExpression = filteringExpression.Or(x => x.FirstName.Contains(SearchString) 
                                  || x.LastName.Contains(SearchString) 
                                  || x.Group.GroupName.Contains(SearchString));
            }
           

            if (filteringExpression == original) {
                filteringExpression = x => true;
            }
            return filteringExpression;
        }
    }
}
