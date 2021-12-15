using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Data.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using Services.ModelsDTO;
using Services.Services;
using Task9.TaskViewModels;

namespace Task9.Controllers {
    public class GroupsController : Controller {
        private readonly GroupService _groupService;
        private readonly CourseService _courseService;

        public GroupsController(IService<Group, GroupDto> groupService, IService<Course, CourseDto> curseService) {
            _groupService = groupService as GroupService;
            _courseService = curseService as CourseService;
        }

        // GET: Groups
        public async Task<IActionResult> Index(int? selectedCourse, string searchString) {
            var filter = new GroupFilter(searchString, courseFilter: selectedCourse);
            var groups = await _groupService.GetAllItemsAsync(filter);
            await PopulateCoursesDropDownList(selectedCourse);

            var groupViewModel = new GroupViewModel {
                FilteredGroups = groups.ToList(),
                SelectedCourseId = selectedCourse,
                SearchString = searchString
            };
            return View(groupViewModel);
        }
        
        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id) {
            var groupDTO = await _groupService.GetAsync(id);
            return View(groupDTO);
        }

        // GET: Groups/Create
        public async Task<IActionResult> Create(string selectedCourse) {
            var group = new GroupDto();
            if (int.TryParse(selectedCourse, out var id)) {
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
        public async Task<IActionResult> Create(GroupDto groupDTO) {
            if (!ModelState.IsValid) {
                await PopulateCoursesDropDownList();
                return View(groupDTO);
            }
            await _groupService.CreateAsync(groupDTO);
            return RedirectToAction("Index", new{ selectedCourse = groupDTO.CourseId});
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            var groupDTO = await _groupService.GetAsync(id);
            await PopulateCoursesDropDownList();
            return View(groupDTO);
        }

        // POST: Groups/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GroupDto groupDTO) {
            if (!ModelState.IsValid) {
                return View(groupDTO);
            }
            await _groupService.UpdateAsync(groupDTO);
            return RedirectToAction("Index", new{ selectedCourse = groupDTO.CourseId});
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id, bool showMessage = false) {
            var groupDTO = await _groupService.GetAsync(id);
            if (showMessage) {
                ViewBag.ErrorMessage = "Group cannot be deleted since it contains students.";
            }
            return View(groupDTO);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var groupDTO = await _groupService.GetAsync(id);
            await _groupService.DeleteAsync(id);
            return RedirectToAction("Index", new { selectedCourse = groupDTO.CourseId});
        }

        private async Task PopulateCoursesDropDownList(object selectedCourse = null) {
            var coursesDto = await _courseService.GetAllItemsAsync(new CourseFilter());
            ViewBag.Courses = new SelectList(coursesDto, "Id", "CourseName", selectedCourse);
        }
    }
}
