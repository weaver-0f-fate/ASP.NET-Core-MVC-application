using System.Threading.Tasks;
using DataAccessLayer.Data;
using DataAccessLayer.DomainObjects;
using DomainLayer.Models.TaskModels;
using Microsoft.AspNetCore.Mvc;

namespace Task9.Controllers {
    public class CoursesController : Controller {
        private readonly CourseData _courseData;

        public CoursesController(Task9Context context) {
            _courseData = CourseData.GetCourseData(context);
        }

        // GET: Courses
        public async Task<IActionResult> Index(string searchString) {
            return View(await _courseData.GetCourses(searchString));
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id is null) {
                return NotFound();
            }
            var course = await _courseData.GetCourseById(id);
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
        public async Task<IActionResult> Create([Bind("Id,CourseName,CourseDescription")] Course course) {
            if (ModelState.IsValid) {
                await _courseData.CreateCourse(course);
                return RedirectToAction(nameof(Details), new { id = course.Id });
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }
            var course = await _courseData.GetCourseById(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseName,CourseDescription")] Course course) {
            if (id != course.Id) {
                return NotFound();
            }
            if (!ModelState.IsValid) {
                return View(course);
            }

            if(await _courseData.UpdateCourse(course)) {
                return RedirectToAction(nameof(Details), new { course.Id });
            }

            return NotFound();
        }
        
        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id, string message = null) {
            if (id == null) {
                return NotFound();
            }
            var course = await _courseData.GetCourseById(id);
            ViewBag.ErrorMessage = message;
            if (course is null) {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            if (await _courseData.DeleteCourse(id)) {
                return RedirectToAction("Index");
            }
            var message =
                "Cascade Delete is restricted. Course cannot be deleted since there are associated groups.";
            return RedirectToAction("Delete", new { id, message });
        }

        public IActionResult ViewGroups(string courseName) {
            return RedirectToAction("Index", "Groups", new { groupCourse = courseName });
        }
        public IActionResult ClearFilter() {
            return RedirectToAction("Index");
        }
    }
}
