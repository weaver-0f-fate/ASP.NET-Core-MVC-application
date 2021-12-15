using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.ModelsDTO;
using Task9.TaskViewModels;

namespace Task9.Controllers {
    public class CoursesController : Controller {
        private readonly IService<CourseDto> _courseService;

        public CoursesController(IService<CourseDto> service) {
            _courseService = service;
        }

        // GET: Courses
        public async Task<IActionResult> Index(string searchString) {
            var parameters = new FilteringService(searchString: searchString);
            var courseDtos = await _courseService.GetAllItemsAsync(parameters);
            var coursesViewModel = new CoursesViewModel {
                FilteredCourses = courseDtos.ToList(),
                SearchString = searchString
            };
            return View(coursesViewModel);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id) {
            var courseDtos = await _courseService.GetAsync(id);
            return View(courseDtos);
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
        public async Task<IActionResult> Create(CourseDto courseDtos) {
            if (!ModelState.IsValid) {
                return View(courseDtos);
            }
            await _courseService.CreateAsync(courseDtos);
            return RedirectToAction("Index");
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            var courseDtos = await _courseService.GetAsync(id);
            return View(courseDtos);
        }

        // POST: Courses/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourseDto courseDtos) {
            if (!ModelState.IsValid) {
                return View(courseDtos);
            }
            await _courseService.UpdateAsync(courseDtos);
            return RedirectToAction("Index");
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            var courseDtos = await _courseService.GetAsync(id);
            return View(courseDtos);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            await _courseService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
