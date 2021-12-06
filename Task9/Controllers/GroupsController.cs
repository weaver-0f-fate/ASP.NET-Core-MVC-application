using System;
using System.Linq;
using Core;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task9.TaskViewModels;

namespace Task9.Controllers {
    public class GroupsController : Controller {
        private readonly GroupRepository _groupRepository;

        public GroupsController(Task9Context context) {
            _groupRepository = GroupRepository.GetGroupRepository(context);
        }

        // GET: Groups
        public IActionResult Index(string groupCourse, string searchString) {
            var groups = _groupRepository.GetGroupList(groupCourse, searchString);
            var courses = _groupRepository.GetCoursesList();
            var groupViewModel = new GroupViewModel {
                Courses = new SelectList(courses),
                Groups = groups.ToList()
            };
            return View(groupViewModel);
        }
        
        // GET: Groups/Details/5
        public IActionResult Details(int? id) {
            if (id  is null) {
                return NotFound();
            }
            var group = _groupRepository.GetGroup((int)id);
            if (group is null) {
                return NotFound();
            }
            return View(group);
        }

        // GET: Groups/Create
        public IActionResult Create() {
            PopulateCoursesDropDownList();
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,CourseId,GroupName")] Group group) {
            if (ModelState.IsValid) {
                _groupRepository.Create(group);
                return RedirectToAction(nameof(Index));
            }
            PopulateCoursesDropDownList();
            return View(group);
        }

        // GET: Groups/Edit/5
        public IActionResult Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var group = _groupRepository.GetGroup((int)id);
            if (group is null) {
                return NotFound();
            }
            PopulateCoursesDropDownList(group.CourseId);
            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,CourseId,GroupName")] Group @group) {
            if (id != group.Id) {
                return NotFound();
            }
            if (!ModelState.IsValid) {
                return View(group);
            }


            try {
                _groupRepository.Update(group);
                return RedirectToAction(nameof(Details), new { group.Id });
            }
            catch (Exception) {
                if (!_groupRepository.GroupExists(group.Id)) {
                    PopulateCoursesDropDownList(group.CourseId);
                    return NotFound();
                }
                throw;
            }
        }

        // GET: Groups/Delete/5
        public IActionResult Delete(int? id, string message = null) {
            if (id == null) {
                return NotFound();
            }
            var group = _groupRepository.GetGroup((int)id);
            ViewBag.ErrorMessage = message;

            if (group is null) {
                return NotFound();
            }
            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id) {
            try {
                _groupRepository.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception) {
                var message =
                    "Cascade Delete is restricted. Group cannot be deleted since there are associated students.";
                return RedirectToAction("Delete", new { id, message });
            }
        }

        public IActionResult ClearFilter() {
            return RedirectToAction("Index");
        }
        public IActionResult ViewStudents(string groupName) {
            return RedirectToAction("Index", "Students", new { studentGroup = groupName });
        }

        private void PopulateCoursesDropDownList(object selecetedCourse = null) {
            var courses = _groupRepository.GetQueryableCourses();
            ViewBag.CourseId = new SelectList(courses, "Id", "CourseName", selecetedCourse);
        }
    }
}
