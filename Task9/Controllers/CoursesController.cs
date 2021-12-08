using System.Threading.Tasks;
using AutoMapper;
using Data;
using Microsoft.AspNetCore.Mvc;
using Services.ModelsDTO;
using Services.Presentations;

namespace Task9.Controllers {
    public class CoursesController : Controller {
        private readonly CourseService _coursePresentation;

        public CoursesController(Task9Context context, IMapper mapper) {
            _coursePresentation = new CourseService(context, mapper);
        }

        // GET: Courses
        public async Task<IActionResult> Index(string searchString) {
            var courseDTOs = await _coursePresentation.GetAllItemsAsync(searchString);
            return View(courseDTOs);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id) {
            var courseDTO = await _coursePresentation.GetAsync(id);
            return View(courseDTO);
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
        public async Task<IActionResult> Create(CourseDTO courseDTO) {
            if (!ModelState.IsValid) {
                return View(courseDTO);
            }
            await _coursePresentation.CreateAsync(courseDTO);
            return RedirectToAction("Index");
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            var courseDTO = await _coursePresentation.GetAsync(id);
            return View(courseDTO);
        }

        // POST: Courses/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseDTO courseDTO) {
            if (!ModelState.IsValid) {
                return View(courseDTO);
            }
            await _coursePresentation.UpdateAsync(courseDTO);
            return RedirectToAction("Index");
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            var courseDTO = await _coursePresentation.GetAsync(id);
            return View(courseDTO);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            await _coursePresentation.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult ViewGroups(string courseName) {
            return RedirectToAction("Index", "Groups", new { groupCourse = courseName });
        }
        public IActionResult ClearFilter() {
            return RedirectToAction("Index");
        }
    }
}
