using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.ModelsDTO;
using ServicesInterfaces;
using Task9.TaskViewModels;

namespace Task9.Controllers {
    public class GroupsController : Controller {
        private readonly IService<GroupDTO> _groupService;
        private readonly IService<CourseDTO> _courseService;

        public GroupsController(IService<GroupDTO> groupService, IService<CourseDTO> curseService) {
            _groupService = groupService;
            _courseService = curseService;
        }

        // GET: Groups
        public async Task<IActionResult> Index(int? selectedCourse, string searchString) {
            CourseDTO course = null;
            if (selectedCourse is not null) {
                course = await _courseService.GetAsync(selectedCourse);
            }

            var groups = await _groupService.GetAllItemsAsync(searchString, course?.CourseName);

            await PopulateCoursesDropDownList(course?.Id);

            var groupViewModel = new GroupViewModel {
                FilteredGroups = groups.ToList(),
                SelectedCourseId = course?.Id
            };

            return View(groupViewModel);
        }
        
        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id) {
            var groupDto = await _groupService.GetAsync(id);
            return View(groupDto);
        }

        // GET: Groups/Create
        public async Task<IActionResult> Create(string selectedCourse) {
            var group = new GroupDTO();
            if (int.TryParse(selectedCourse, out int id)) {
                group.CourseId = id;
            }
            await PopulateCoursesDropDownList(selectedCourse);
            return View(group);
        }

        // POST: Groups/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GroupDTO groupDto) {
            if (!ModelState.IsValid) {
                await PopulateCoursesDropDownList();
                return View(groupDto);
            }

            var course = await _courseService.GetAsync(groupDto.CourseId);
            await _groupService.CreateAsync(groupDto);
            return RedirectToAction("Index", new{ selectedCourse = course.Id});
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            var groupDto = await _groupService.GetAsync(id);
            await PopulateCoursesDropDownList();
            return View(groupDto);
        }

        // POST: Groups/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GroupDTO groupDto) {
            if (!ModelState.IsValid) {
                return View(groupDto);
            }
            var course = await _courseService.GetAsync(groupDto.CourseId);
            await _groupService.UpdateAsync(groupDto);
            return RedirectToAction("Index", new{ selectedCourse = course.Id});
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id, bool showMessage = false) {
            var groupDto = await _groupService.GetAsync(id);
            if (showMessage) {
                ViewBag.ErrorMessage = "Group cannot be deleted since it contains students.";
            }
            return View(groupDto);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var groupDto = await _groupService.GetAsync(id);
            await _groupService.DeleteAsync(id);
            return RedirectToAction("Index", new { selectedCourse = groupDto.CourseId});
        }

        public IActionResult ClearFilter() {
            return RedirectToAction("Index");
        }

        private async Task PopulateCoursesDropDownList(object selectedCourse = null) {
            var coursesDto = await _courseService.GetAllItemsAsync();
            ViewBag.Courses = new SelectList(coursesDto, "Id", "CourseName", selectedCourse);
        }
    }
}
