using System;
using System.Linq;
using AutoMapper;
using Core.Models;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Task9.TaskViewModels;
using Task9.TaskViewModels.ModelsDTO;

namespace Task9.Controllers {
    public class GroupsController : Controller {
        private readonly GroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GroupsController(Task9Context context, IMapper mapper) {
            _groupRepository = GroupRepository.GetGroupRepository(context);
            _mapper = mapper;
        }

        // GET: Groups
        public IActionResult Index(string groupCourse, string searchString) {
            var groups = _groupRepository.GetEntityList(groupCourse, searchString);
            var courses = _groupRepository.GetCoursesList();
            var groupViewModel = new GroupViewModel {
                Courses = new SelectList(courses),
                Groups = groups.Select(x => _mapper.Map<GroupDTO>(x)).ToList()
            };
            return View(groupViewModel);
        }
        
        // GET: Groups/Details/5
        public IActionResult Details(int? id) {
            if (id  is null) {
                return NotFound();
            }
            var group = _groupRepository.GetEntity((int)id);
            if (group is null) {
                return NotFound();
            }
            return View(_mapper.Map<GroupDTO>(group));
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
            return View(_mapper.Map<GroupDTO>(group));
        }

        // GET: Groups/Edit/5
        public IActionResult Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var group = _groupRepository.GetEntity((int)id);
            if (group is null) {
                return NotFound();
            }
            PopulateCoursesDropDownList(group.CourseId);
            return View(_mapper.Map<GroupDTO>(group));
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,CourseId,GroupName")] Group group) {
            if (id != group.Id) {
                return NotFound();
            }
            if (!ModelState.IsValid) {
                return View(_mapper.Map<GroupDTO>(group));
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
            var group = _groupRepository.GetEntity((int)id);
            ViewBag.ErrorMessage = message;

            if (group is null) {
                return NotFound();
            }
            return View(_mapper.Map<GroupDTO>(group));
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
