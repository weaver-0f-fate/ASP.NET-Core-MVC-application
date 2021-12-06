using System;
using System.Threading.Tasks;
using Core;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Task9.Controllers {
    public class CoursesController : Controller {
        private readonly CourseRepository _courseRepository;

        public CoursesController(Task9Context context) {
            _courseRepository = CourseRepository.GetCourseRepository(context);
        }

        // GET: Courses
        public IActionResult Index(string searchString) {
            var courses = _courseRepository.GetCourseList(searchString);
            return View(courses);
        }

        // GET: Courses/Details/5
        public IActionResult Details(int? id) {
            if (id is null) {
                return NotFound();
            }
            var course = _courseRepository.GetCourse((int)id);
            if (course == null) {
                return NotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Courses/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,CourseName,CourseDescription")] Course course) {
            if (ModelState.IsValid) {
                _courseRepository.Create(course);
                return RedirectToAction(nameof(Details), new { id = course.Id });
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public IActionResult Edit(int? id) {
            if (id == null) {
                return NotFound();
            }
            var course = _courseRepository.GetCourse((int)id);
            if (course == null) {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,CourseName,CourseDescription")] Course course) {
            if (id != course.Id) {
                return NotFound();
            }
            if (!ModelState.IsValid) {
                return View(course);
            }

            try {
                _courseRepository.Update(course);
                return RedirectToAction(nameof(Details), new { course.Id });
            }
            catch (DbUpdateConcurrencyException) {
                if (!_courseRepository.CourseExists(course.Id)) {
                    return NotFound();
                }
                throw;
            }
        }
        
        // GET: Courses/Delete/5
        public IActionResult Delete(int? id, string message = null) {
            if (id == null) {
                return NotFound();
            }
            var course = _courseRepository.GetCourse((int)id);
            ViewBag.ErrorMessage = message;
            if (course is null) {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id) {
            try {
                _courseRepository.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception) {
                var message =
                    "Cascade Delete is restricted. Course cannot be deleted since there are associated groups.";
                return RedirectToAction("Delete", new { id, message });
            }
        }

        public IActionResult ViewGroups(string courseName) {
            return RedirectToAction("Index", "Groups", new { groupCourse = courseName });
        }
        public IActionResult ClearFilter() {
            return RedirectToAction("Index");
        }
    }
}
