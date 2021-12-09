using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.ModelsDTO;
using ServicesInterfaces;
using Task9.TaskViewModels;

namespace Task9.Controllers {
    public class StudentsController : Controller {
        private readonly IService<StudentDTO> _studentService;
        private readonly IService<GroupDTO> _groupService;

        public StudentsController(IService<StudentDTO> studentService, IService<GroupDTO> groupService) {
            _studentService = studentService;
            _groupService = groupService;
        }

        // GET: Students
        public async Task<IActionResult> Index(string selectedGroup, string searchString) {
            var studentDTOs = await _studentService.GetAllItemsAsync(searchString, selectedGroup);
            var groupsNames = await _groupService.GetNames();


            var studentViewModel = new StudentsViewModel {
                GroupsInSelectList = new SelectList(groupsNames),
                FilteredStudents = studentDTOs.ToList()
            };
            return View(studentViewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id) {
            var studentDTO = await _studentService.GetAsync(id);
            return View(studentDTO);
        }

        // GET: Students/Create
        public async Task<IActionResult> Create() {
            await PopulateGroupsDropDownList();
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentDTO studentDTO) {
            if (!ModelState.IsValid) {
                await PopulateGroupsDropDownList();
                return View(studentDTO);
            }
            var group = await _groupService.GetAsync(studentDTO.GroupId);
            await _studentService.CreateAsync(studentDTO);
            return RedirectToAction("Index", new { selectedGroup = group.GroupName});
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            var studentDTO = await _studentService.GetAsync(id);
            await PopulateGroupsDropDownList(studentDTO.GroupId);
            return View(studentDTO);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StudentDTO studentDTO) {
            if (!ModelState.IsValid) {
                return View(studentDTO);
            }

            var group = await _groupService.GetAsync(studentDTO.GroupId);
            await _studentService.UpdateAsync(studentDTO);
            return RedirectToAction("Index", new { selectedGroup = group.GroupName });
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            var studentDTO = await _studentService.GetAsync(id);
            return View(studentDTO);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            await _studentService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult ClearFilter() {
            return RedirectToAction("Index");
        }

        private async Task PopulateGroupsDropDownList(object selectedGroup = null) {
            var groups = await _groupService.GetAllItemsAsync();
            ViewBag.GroupId = new SelectList(groups, "Id", "GroupName", selectedGroup);
        }
    }
}
