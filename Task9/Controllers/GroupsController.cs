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
        public async Task<IActionResult> Index(string selectedCourse, string searchString) {
            var groups = await _groupService.GetAllItemsAsync(searchString, selectedCourse);
            var coursesNames = await _courseService.GetNames();

            var groupViewModel = new GroupViewModel {
                CoursesInSelectList = new SelectList(coursesNames),
                FilteredGroups = groups.ToList()
            };
            return View(groupViewModel);
        }
        
        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id) {
            var groupDTO = await _groupService.GetAsync(id);
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

            await _groupService.CreateAsync(groupDTO);
            return RedirectToAction("Index", new{ groupCourse = groupDTO.CourseName});
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            var groupDTO = await _groupService.GetAsync(id);
            await PopulateCoursesDropDownList(groupDTO.CourseDTO);
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
            await _groupService.UpdateAsync(groupDTO);
            return RedirectToAction("Index");
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
            await _groupService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult ClearFilter() {
            return RedirectToAction("Index");
        }
        public IActionResult ViewStudents(string groupName) {
            return RedirectToAction("Index", "Students", new { studentGroup = groupName });
        }

        private async Task PopulateCoursesDropDownList(object selecetedCourse = null) {
            var coursesDTO = await _courseService.GetAllItemsAsync();
            ViewBag.CourseId = new SelectList(coursesDTO, "Id", "CourseName", selecetedCourse);
        }
    }
}
