using System.Threading.Tasks;
using AutoMapper;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.ModelsDTO;
using Services.Presentations;
using Task9.TaskViewModels;

namespace Task9.Controllers {
    public class GroupsController : Controller {
        private readonly GroupPresentation _groupPresentation;

        public GroupsController(Task9Context context, IMapper mapper) {
            _groupPresentation = new GroupPresentation(context, mapper);
        }

        // GET: Groups
        public async Task<IActionResult> Index(string groupCourse, string searchString) {
            var groups = await _groupPresentation.GetAllItemsAsync(searchString, groupCourse);
            var coursesNames = await _groupPresentation.GetCoursesNames();

            var groupViewModel = new GroupViewModel {
                Courses = new SelectList(coursesNames),
                Groups = groups
            };
            return View(groupViewModel);
        }
        
        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id) {
            var groupDTO = await _groupPresentation.GetItemAsync(id);
            return View(groupDTO);
        }

        // GET: Groups/Create
        public async Task<IActionResult> Create() {
            await PopulateCoursesDropDownList();
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GroupDTO groupDTO) {
            if (!ModelState.IsValid) {
                await PopulateCoursesDropDownList();
                return View(groupDTO);
            }

            await _groupPresentation.CreateItemAsync(groupDTO);
            return RedirectToAction(nameof(Index));
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            var groupDTO = await _groupPresentation.GetItemAsync(id);
            await PopulateCoursesDropDownList(groupDTO.CourseId);
            return View(groupDTO);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GroupDTO groupDTO) {
            if (!ModelState.IsValid) {
                return View(groupDTO);
            }
            await _groupPresentation.UpdateItemAsync(groupDTO);
            return RedirectToAction(nameof(Index));
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id, bool showMessage = false) {
            var groupDTO = await _groupPresentation.GetItemAsync(id);
            if (showMessage) {
                ViewBag.ErrorMessage = "Group cannot be deleted since it contains students.";
            }
            return View(groupDTO);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            await _groupPresentation.DeleteItemAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult ClearFilter() {
            return RedirectToAction("Index");
        }
        public IActionResult ViewStudents(string groupName) {
            return RedirectToAction("Index", "Students", new { studentGroup = groupName });
        }

        private async Task PopulateCoursesDropDownList(object selecetedCourse = null) {
            var courses = await _groupPresentation.GetCourses();
            ViewBag.CourseId = new SelectList(courses, "Id", "CourseName", selecetedCourse);
        }
    }
}
