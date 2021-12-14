using System.Collections.Generic;
using System.Linq;
using Core.Models;

namespace Services {
    public class FilteringService {
        private readonly string _searchString;
        private readonly int? _groupFilter;
        private readonly int? _courseFilter;

        public FilteringService(string searchString = null, int? groupFilter = null, int? courseFilter = null) {
            _searchString = searchString;
            _groupFilter = groupFilter;
            _courseFilter = courseFilter;
        }

        public IEnumerable<AbstractModel> GetFilteredCollection(IEnumerable<AbstractModel> models) {
            return models switch {
                IEnumerable<Student> students => GetFilteredStudents(students),
                IEnumerable<Course> courses => GetFilteredCourses(courses),
                IEnumerable<Group> groups => GetFilteredGroups(groups),
                _ => null
            };
        }

        private IEnumerable<Student> GetFilteredStudents(IEnumerable<Student> students) {
            if (_courseFilter > 0) {
                students = students.Where(x => x.Group.CourseId == _courseFilter);
            }
            if (_groupFilter > 0) {
                students = students.Where(x => x.GroupId == _groupFilter);
            }
            if (!string.IsNullOrEmpty(_searchString)) {
                students = students.Where(x =>
                    x.FirstName.Contains(_searchString)
                    || x.LastName.Contains(_searchString)
                    || x.Group.GroupName.Contains(_searchString));
            }
            return students;
        }

        private IEnumerable<Course> GetFilteredCourses(IEnumerable<Course> courses) {
            if (!string.IsNullOrEmpty(_searchString)) {
                courses = courses.Where(x => 
                    x.CourseName.Contains(_searchString) 
                 || x.CourseDescription.Contains(_searchString));
            }
            return courses;
        }

        private IEnumerable<Group> GetFilteredGroups(IEnumerable<Group> groups) {
            if (_courseFilter > 0) {
                groups = groups.Where(x => x.Course.Id == _courseFilter);
            }
            if (!string.IsNullOrEmpty(_searchString)) {
                groups = groups.Where(x 
                    => x.GroupName.Contains(_searchString) 
                    || x.Course.CourseName.Contains(_searchString));
            }
            return groups;
        }
    }
}