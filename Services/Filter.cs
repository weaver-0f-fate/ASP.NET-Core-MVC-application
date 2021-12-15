using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;

namespace Services {
    public class Filter {
        private readonly string _searchString;
        private readonly int? _groupFilter;
        private readonly int? _courseFilter;

        public Filter(string searchString = null, int? groupFilter = null, int? courseFilter = null) {
            _searchString = searchString;
            _groupFilter = groupFilter;
            _courseFilter = courseFilter;
        }

        public IEnumerable<AbstractModel> GetFilteredCollection(IEnumerable<AbstractModel> itemsCollection) {
            return itemsCollection switch {
                IEnumerable<Student> students => GetFilteredStudents(students),
                IEnumerable<Course> courses => GetFilteredCourses(courses),
                IEnumerable<Group> groups => GetFilteredGroups(groups),
                _ => null
            };
        }

        private IEnumerable<Course> GetFilteredCourses(IEnumerable<Course> courses) {
            var predicates = new List<Func<Course, bool>>();

            if (!string.IsNullOrEmpty(_searchString)) {
                predicates.Add(CourseSearchStringPredicate);
            }
            return courses.Where(And(predicates.ToArray()));
        }

        private IEnumerable<Group> GetFilteredGroups(IEnumerable<Group> groups) {
            var predicates = new List<Func<Group, bool>>();


            if (_courseFilter > 0) {
                predicates.Add(GroupCourseFilterPredicate);
            }
            if (!string.IsNullOrEmpty(_searchString)) { 
                predicates.Add(GroupSearchStringPredicate);
            }
            return groups.Where(And(predicates.ToArray()));
        }

        private IEnumerable<Student> GetFilteredStudents(IEnumerable<Student> students) {
            var predicates = new List<Func<Student, bool>>();

            if (!string.IsNullOrEmpty(_searchString)) {
                predicates.Add(StudentSearchFilterPredicate);
            }

            if (_groupFilter > 0) {
                predicates.Add(StudentGroupFilterPredicate);
            }
            else {
                if (_courseFilter > 0) {
                    predicates.Add(StudentCourseFilterPredicate);
                }
            }
            return students.Where(And(predicates.ToArray()));
        }

        private static Func<T, bool> And<T>(params Func<T, bool>[] predicates) {
            return delegate (T item) {
                return predicates.All(predicate => predicate(item));
            };
        }

        #region Predicates
        private bool CourseSearchStringPredicate(Course course) =>
            course.CourseName.Contains(_searchString)
            || course.CourseDescription.Contains(_searchString);

        private bool GroupCourseFilterPredicate(Group group) =>
            group.CourseId == _courseFilter;
        private bool GroupSearchStringPredicate(Group group) =>
            group.GroupName.Contains(_searchString)
            || group.Course.CourseName.Contains(_searchString);

        private bool StudentCourseFilterPredicate(Student student) => 
            student.Group.CourseId == _courseFilter;
        private bool StudentGroupFilterPredicate(Student student) => 
            student.GroupId == _groupFilter;
        private bool StudentSearchFilterPredicate(Student student) => 
            student.FirstName.Contains(_searchString)
            || student.LastName.Contains(_searchString)
            || student.Group.GroupName.Contains(_searchString);
        #endregion
    }
}