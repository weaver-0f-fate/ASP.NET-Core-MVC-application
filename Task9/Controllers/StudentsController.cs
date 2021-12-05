using System.Linq;
using System.Threading.Tasks;
using DomainLayer.Models.TaskModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task9.TaskViewModels;

namespace Task9.Controllers
{
    public class StudentsController : Controller
    {
        private readonly DataAccessLayer.Data.Task9Context _context;

        public StudentsController(DataAccessLayer.Data.Task9Context context)
        {
            _context = context;
        }

        #region Index
        // GET: Students
        public async Task<IActionResult> Index(string studentGroup, string searchString) {
            var groupQuery = from g in _context.Student
                orderby g.GroupId
                select g.Group.GroupName;

            var students = GetStudentsWithGroups();

            if (!string.IsNullOrEmpty(searchString)) {
                students = students.Where(s => s.FirstName!.Contains(searchString) 
                                               || s.LastName!.Contains(searchString) 
                                               || s.Group.GroupName!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(studentGroup)) {
                students = students.Where(x => x.Group.GroupName == studentGroup);
            }

            var studentViewModel = new StudentsViewModel() {
                Groups = new SelectList(await groupQuery.Distinct().ToListAsync()),
                Students = await students.ToListAsync()
            };

            return View(studentViewModel);
        }
        #endregion

        #region Details
        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var student = await _context.Student
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null) {
                return NotFound();
            }

            var group = _context.Group.FirstOrDefault(x => x.Id == student.GroupId);
            group.Course = _context.Course.FirstOrDefault(x => x.Id == group.CourseId);
            student.Group = group;

            return View(student);
        }
        #endregion

        #region Create
        // GET: Students/Create
        public IActionResult Create() {
            PopulateGroupsDropDownList();
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GroupId,FirstName,LastName")] Student student) {
            if (ModelState.IsValid) {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateGroupsDropDownList();
            return View(student);
        }
        #endregion

        #region Edit
        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null) {
                return NotFound();
            }
            PopulateGroupsDropDownList();
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GroupId,FirstName,LastName")] Student student) {
            if (id != student.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!StudentExists(student.Id)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateGroupsDropDownList();
            return View(student);
        }
        #endregion

        #region Delete
        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var student = await _context.Student
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null) {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Actions
        public IActionResult ClearFilter() {
            return RedirectToAction("Index");
        }
        #endregion

        #region Implementation
        private bool StudentExists(int id) {
            return _context.Student.Any(e => e.Id == id);
        }

        private IQueryable<Student> GetStudentsWithGroups() {
            var students = from s in _context.Student select s;
            var groups = from g in _context.Group select g;
            foreach (var student in students) {
                student.Group = groups.FirstOrDefault(g => g.Id == student.GroupId);
            }
            return students;
        }

        private void PopulateGroupsDropDownList(object selectedGroup = null) {
            var groupsQuery = from x in _context.Group
                orderby x.GroupName
                select x;
            ViewBag.GroupId = new SelectList(groupsQuery.AsNoTracking(), "Id", "GroupName", selectedGroup);
        }
        #endregion
    }
}
