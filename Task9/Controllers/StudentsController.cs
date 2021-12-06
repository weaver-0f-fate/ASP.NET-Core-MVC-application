using System.Threading.Tasks;
using DataAccessLayer.Data;
using DataAccessLayer.DomainObjects;
using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Task9.TaskViewModels;

namespace Task9.Controllers {
    public class StudentsController : Controller {
        private readonly StudentRepository _studentData;

        public StudentsController(Task9Context context) {
            _studentData = StudentRepository.GetStudentData(context);
        }

        // GET: Students
        public async Task<IActionResult> Index(string studentGroup, string searchString) {
            var students = await _studentData.GetStudents(studentGroup, searchString);
            var groups = await _studentData.GetGroupsList();

            var studentViewModel = new StudentsViewModel {
                Groups = new SelectList(groups),
                Students = students
            };
            return View(studentViewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id is null) {
                return NotFound();
            }
            var student = await _studentData.GetStudentById(id);
            if (student is null) {
                return NotFound();
            }
            return View(student);
        }

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
                await 
                    _studentData.CreateStudent(student);
                return RedirectToAction(nameof(Index));
            }
            PopulateGroupsDropDownList();
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var student = await _studentData.GetStudentById(id);
            if (student is null) {
                return NotFound();
            }
            PopulateGroupsDropDownList(student.GroupId);
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
            if (!ModelState.IsValid) {
                return View(student);
            }

            if (await _studentData.UpdateStudent(student)) {
                return RedirectToAction(nameof(Details), new { student.Id });
            }

            PopulateGroupsDropDownList(student.GroupId);
            return NotFound();
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }
            var student = await _studentData.GetStudentById(id);

            if (student is null) {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            if (await _studentData.DeleteStudent(id)) {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Delete", new { id });
        }

        public IActionResult ClearFilter() {
            return RedirectToAction("Index");
        }

        private void PopulateGroupsDropDownList(object selectedGroup = null) {
            var groups = _studentData.GetQueryableGroups();
            ViewBag.GroupId = new SelectList(groups, "Id", "GroupName", selectedGroup);
        }
    }
}
