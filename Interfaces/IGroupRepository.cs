using System;
using System.Collections.Generic;
using Core;

namespace Interfaces {
    public interface IGroupRepository : IDisposable{
        IEnumerable<Group> GetGroupList();
        Group GetGroup(int id);
        void Create(Group item);
        void Update(Group item);
        void Delete(int id);
        void Save();
    }
}