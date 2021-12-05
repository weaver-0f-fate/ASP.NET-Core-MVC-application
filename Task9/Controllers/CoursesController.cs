using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task9.Data;
using Task9.Models.TaskModels;

namespace Task9.Controllers
{
    public class CoursesController : Controller
    {
        private readonly Task9Context _context;

        public CoursesController(Task9Context context)
        {
            _context = context;
        }

        #region Index
        // GET: Courses
        public async Task<IActionResult> Index(string searchString) {
            var courses = from c in _context.Course select c;

            if (!string.IsNullOrEmpty(searchString)) {
                courses = courses.Where(s => s.CourseName!.Contains(searchString));
            }
            return View(await courses.ToListAsync());
        }
        #endregion

        #region Details
        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var course = await _context.Course
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null) {
                return NotFound();
            }

            return View(course);
        }
        #endregion

        #region Create
        // GET: Courses/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseName,CourseDescription")] Course course) {
            if (ModelState.IsValid) {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }
        #endregion

        #region Edit
        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null) {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseName,CourseDescription")] Course course) {
            if (id != course.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!CourseExists(course.Id)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }
        #endregion

        #region Delete
        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null) {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var course = await _context.Course.FindAsync(id);
            var associatedGroups = _context.Group.Where(s => s.CourseId == course.Id);

            if (associatedGroups.Any()) {
                //TODO message cannot delete since there are associated groups;
            }
            else {
                _context.Course.Remove(course);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Redirection
        public IActionResult ViewGroups(string courseName) {
            return RedirectToAction("Index", "Groups", new { groupCourse = courseName });
        }
        #endregion

        #region Implementation
        private bool CourseExists(int id) {
            return _context.Course.Any(e => e.Id == id);
        }
        #endregion
    }
}
