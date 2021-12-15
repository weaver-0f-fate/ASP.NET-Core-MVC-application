using System;
using System.Linq.Expressions;
using Core.Models;
using LinqKit;

namespace Data.Filters {
    public class StudentFilter : AbstractFilter<Student> {
        public StudentFilter(string searchString = null, int? groupFilter = null, int? courseFilter = null)
            : base(searchString, groupFilter, courseFilter) { }

        public override Expression<Func<Student, bool>> GetFilteringExpression() {
            Expression<Func<Student, bool>>? exp = PredicateBuilder.New<Student>(false);
            var original = exp;

            if (GroupFilter > 0) {
                exp = exp.Or(x => x.GroupId == GroupFilter);
            }
            else if(CourseFilter > 0) {
                exp = exp.Or(x => x.Group.CourseId == CourseFilter);

            }
            if (!string.IsNullOrEmpty(SearchString)) {
                exp = exp.Or(x => x.FirstName.Contains(SearchString) 
                                  || x.LastName.Contains(SearchString) 
                                  || x.Group.GroupName.Contains(SearchString));
            }
           

            if (exp == original) {
                exp = x => true;
            }
            return exp;
        }
    }
}
