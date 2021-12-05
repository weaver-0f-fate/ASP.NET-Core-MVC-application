using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task9.Data;
using Task9.Models.TaskModels;
using Task9.Models.TaskViewModels;

namespace Task9.Controllers
{
    public class GroupsController : Controller
    {
        private readonly Task9Context _context;

        public GroupsController(Task9Context context)
        {
            _context = context;
        }

        #region Index
        // GET: Groups
        public async Task<IActionResult> Index(string groupCourse, string searchString) {

            var courseQuery = from m in _context.Group
                orderby m.CourseId
                select m.Course.CourseName;

            var groups = GetGroupsWithCourse();


            if (!string.IsNullOrEmpty(searchString)) {
                groups = groups.Where(s => s.GroupName!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(groupCourse)) {
                groups = groups.Where(x => x.Course.CourseName == groupCourse);
            }

            var groupViewModel = new GroupViewModel {
                Courses = new SelectList(await courseQuery.Distinct().ToListAsync()),
                Groups = await groups.ToListAsync()
            };

            return View(groupViewModel);
        }
        #endregion

        #region Details
        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var @group = await _context.Group
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null) {
                return NotFound();
            }

            group.Course = _context.Course.FirstOrDefault(s => s.Id == group.CourseId);

            return View(@group);
        }
        #endregion

        #region Create
        // GET: Groups/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseId,Name")] Group @group) {
            if (ModelState.IsValid) {
                _context.Add(@group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@group);
        }
        #endregion

        #region Edit
        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var @group = await _context.Group.FindAsync(id);
            if (@group == null) {
                return NotFound();
            }
            return View(@group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,Name")] Group @group) {
            if (id != @group.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(@group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!GroupExists(@group.Id)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@group);
        }
        #endregion

        #region Delete
        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var @group = await _context.Group
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null) {
                return NotFound();
            }

            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var @group = await _context.Group.FindAsync(id);

            var associatedStudents = _context.Student.Where(s => s.GroupId == group.Id);

            if (associatedStudents.Any()) {
                //TODO message cannot delete since there are associated students
            }
            else {
                _context.Group.Remove(@group);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Redirection
        public IActionResult ViewStudents(string groupName) {
            return RedirectToAction("Index", "Students", new { studentGroup = groupName });
        }
        #endregion

        #region Implementation
        private bool GroupExists(int id) {
            return _context.Group.Any(e => e.Id == id);
        }

        private IQueryable<Group> GetGroupsWithCourse() {
            var groups = from g in _context.Group select g;
            var courses = from c in _context.Course select c;
            foreach (var @group in groups) {
                group.Course = courses.FirstOrDefault(s => s.Id == group.CourseId);
            }
            return groups;
        }
        #endregion






    }
}
