using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.ModelsDTO;
using Services.Presentations;
using Task9.TaskViewModels;

namespace Task9.Controllers {
    public class StudentsController : Controller {
        private readonly StudentService _studentPresentation;
        private readonly GroupService _groupService;

        public StudentsController(Task9Context context, IMapper mapper) {
            _studentPresentation = new StudentService(context, mapper);
            _groupService = new GroupService(context, mapper);
        }

        // GET: Students
        public async Task<IActionResult> Index(string studentGroup, string searchString) {
            var studentDTOs = await _studentPresentation.GetAllItemsAsync(searchString, studentGroup);
            var groupsNames = await _groupService.GetGroupsNames();


            var studentViewModel = new StudentsViewModel {
                GroupsInSelectList = new SelectList(groupsNames),
                FilteredStudents = studentDTOs.ToList()
            };
            return View(studentViewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id) {
            var studentDTO = await _studentPresentation.GetAsync(id);
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
            await _studentPresentation.CreateAsync(studentDTO);
            return RedirectToAction("Index");
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            var studentDTO = await _studentPresentation.GetAsync(id);
            await PopulateGroupsDropDownList(studentDTO.GroupId);
            return View(studentDTO);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentDTO studentDTO) {
            if (!ModelState.IsValid) {
                return View(studentDTO);
            }
            await _studentPresentation.UpdateAsync(studentDTO);
            return RedirectToAction("Index");
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            var studentDTO = await _studentPresentation.GetAsync(id);
            return View(studentDTO);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            await _studentPresentation.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult ClearFilter() {
            return RedirectToAction("Index");
        }

        private async Task PopulateGroupsDropDownList(object selectedGroup = null) {
            var groups = await _groupService.GetGroups();
            ViewBag.GroupId = new SelectList(groups, "Id", "GroupName", selectedGroup);
        }
    }
}
