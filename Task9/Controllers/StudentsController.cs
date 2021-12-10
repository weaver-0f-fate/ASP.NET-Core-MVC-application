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
        public async Task<IActionResult> Index(int? selectedGroup, string searchString) {
            GroupDTO group = null;
            if (selectedGroup is not null) {
                group = await _groupService.GetAsync(selectedGroup);
            }

            var students = await _studentService.GetAllItemsAsync(searchString, group?.GroupName);
            await PopulateGroupsDropDownList(group?.Id);


            var studentViewModel = new StudentsViewModel {
                FilteredStudents = students.ToList(),
                SelectedGroupId = group?.Id
            };
            return View(studentViewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id) {
            var studentDto = await _studentService.GetAsync(id);
            return View(studentDto);
        }

        // GET: Students/Create
        public async Task<IActionResult> Create(string selectedGroup) {
            var student = new StudentDTO();
            if (int.TryParse(selectedGroup, out int id)) {
                student.GroupId = id;
            }
            await PopulateGroupsDropDownList(selectedGroup);
            return View(student);
        }

        // POST: Students/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentDTO studentDto) {
            if (!ModelState.IsValid) {
                await PopulateGroupsDropDownList();
                return View(studentDto);
            }
            var group = await _groupService.GetAsync(studentDto.GroupId);
            await _studentService.CreateAsync(studentDto);
            return RedirectToAction("Index", new { selectedGroup = group.Id});
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            var studentDto = await _studentService.GetAsync(id);
            await PopulateGroupsDropDownList();
            return View(studentDto);
        }

        // POST: Students/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StudentDTO studentDto) {
            if (!ModelState.IsValid) {
                return View(studentDto);
            }

            var group = await _groupService.GetAsync(studentDto.GroupId);
            await _studentService.UpdateAsync(studentDto);
            return RedirectToAction("Index", new { selectedGroup = group.Id });
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            var studentDto = await _studentService.GetAsync(id);
            return View(studentDto);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var studentDto = await _studentService.GetAsync(id);
            await _studentService.DeleteAsync(id);
            return RedirectToAction("Index", new {selectedGroup = studentDto.GroupId});
        }

        public IActionResult ClearFilter() {
            return RedirectToAction("Index");
        }

        private async Task PopulateGroupsDropDownList(object selectedGroup = null) {
            var groups = await _groupService.GetAllItemsAsync();
            ViewBag.Groups = new SelectList(groups, "Id", "GroupName", selectedGroup);
        }
    }
}
