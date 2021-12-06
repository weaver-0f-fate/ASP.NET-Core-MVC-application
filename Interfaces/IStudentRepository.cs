using System;
using System.Collections.Generic;
using Core;

namespace Interfaces {
    public interface IStudentRepository : IDisposable {
        IEnumerable<Student> GetStudentList();
        Student GetStudent(int id);
        void Create(Student item);
        void Update(Student item);
        void Delete(int id);
        void Save();
    }
}