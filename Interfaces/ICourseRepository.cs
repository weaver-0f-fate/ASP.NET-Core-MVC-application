using System;
using System.Collections.Generic;
using Core;

namespace Interfaces {
    public interface ICourseRepository : IDisposable {
        public IEnumerable<Course> GetCourseList();
        public Course GetCourse(int id);
        public void Create(Course item);
        public void Update(Course item);
        public void Delete(int id);
        public void Save();
    }
}
