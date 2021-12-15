using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Data.Filters;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.ModelsDTO;
using Services.Services;
using Task9.TaskViewModels;

namespace Task9.Controllers {
    public class CoursesController : Controller {
        private readonly CourseService _courseService;

        public CoursesController(IService<Course, CourseDto> service) {
            _courseService = service as CourseService;
        }

        // GET: Courses
        public async Task<IActionResult> Index(string searchString) {
            var filter = new CourseFilter(searchString);
            var courseDTOs = await _courseService.GetAllItemsAsync(filter);
            var coursesViewModel = new CoursesViewModel {
                FilteredCourses = courseDTOs.ToList(),
                SearchString = searchString
            };
            return View(coursesViewModel);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id) {
            var courseDTO = await _courseService.GetAsync(id);
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
        public async Task<IActionResult> Create(CourseDto courseDTO) {
            if (!ModelState.IsValid) {
                return View(courseDTO);
            }
            await _courseService.CreateAsync(courseDTO);
            return RedirectToAction("Index");
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            var courseDTO = await _courseService.GetAsync(id);
            return View(courseDTO);
        }

        // POST: Courses/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourseDto courseDTO) {
            if (!ModelState.IsValid) {
                return View(courseDTO);
            }
            await _courseService.UpdateAsync(courseDTO);
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
