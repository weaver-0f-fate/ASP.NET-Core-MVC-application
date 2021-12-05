using System.Threading.Tasks;
using DataAccessLayer.Data;
using DataAccessLayer.DomainObjects;
using DomainLayer.Models.TaskModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Task9.TaskViewModels;

namespace Task9.Controllers {
    public class GroupsController : Controller {
        private readonly GroupData _groupData;
        private readonly Task9Context _context;

        public GroupsController(Task9Context context) {
            _context = context;
            _groupData = GroupData.GetGroupData(context);
        }

        // GET: Groups
        public async Task<IActionResult> Index(string groupCourse, string searchString) {
            var groups = await _groupData.GetGroups(groupCourse, searchString);
            var courses = await _groupData.GetCoursesList();
            var groupViewModel = new GroupViewModel {
                Courses = new SelectList(courses),
                Groups = groups
            };
            return View(groupViewModel);
        }
        
        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id  is null) {
                return NotFound();
            }
            var group = await _groupData.GetGroupById(id);
            if (group is null) {
                return NotFound();
            }
            return View(group);
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
        public async Task<IActionResult> Create([Bind("Id,CourseId,GroupName")] Group group) {
            if (ModelState.IsValid) {
                await _groupData.CreateGroup(group);
                return RedirectToAction(nameof(Index));
            }
            PopulateCoursesDropDownList();
            return View(group);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var group = await _groupData.GetGroupById(id);
            if (group is null) {
                return NotFound();
            }
            PopulateCoursesDropDownList(group.CourseId);
            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,GroupName")] Group @group) {
            if (id != group.Id) {
                return NotFound();
            }
            if (!ModelState.IsValid) {
                return View(group);
            }

            if (await _groupData.UpdateGroup(group)) {
                return RedirectToAction(nameof(Details), new { group.Id });
            }

            PopulateCoursesDropDownList(group.CourseId);
            return NotFound();
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id, string message = null) {
            if (id == null) {
                return NotFound();
            }
            var group = await _groupData.GetGroupById(id);
            ViewBag.ErrorMessage = message;

            if (group is null) {
                return NotFound();
            }
            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            if (await _groupData.DeleteGroup(id)) {
                return RedirectToAction("Index");
            }
            var message =
                "Cascade Delete is restricted. Course cannot be deleted since there are associated groups.";
            return RedirectToAction("Delete", new { id, message });
        }

        public IActionResult ClearFilter() {
            return RedirectToAction("Index");
        }
        public IActionResult ViewStudents(string groupName) {
            return RedirectToAction("Index", "Students", new { studentGroup = groupName });
        }

        private void PopulateCoursesDropDownList(object selecetedCourse = null) {
            var courses = _groupData.GetQueryableCourses();
            ViewBag.CourseId = new SelectList(courses, "Id", "CourseName", selecetedCourse);
        }
    }
}
