using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.ModelsDTO;
using ServicesInterfaces;
using Task9.TaskViewModels;

namespace Task9.Controllers {
    public class StudentsController : Controller {
        private readonly IService<StudentDto> _studentService;
        private readonly IService<GroupDto> _groupService;
        private readonly IService<CourseDto> _courseService;

        public StudentsController(IService<StudentDto> studentService, IService<GroupDto> groupService, IService<CourseDto> courseService) {
            _studentService = studentService;
            _groupService = groupService;
            _courseService = courseService;
        }

        // GET: Students
        public async Task<IActionResult> Index(int? selectedCourseId, int? selectedGroupId, string searchString) {
            var c = await _courseService.GetAllItemsAsync();
            var g = await _groupService.GetAllItemsAsync(filter: selectedCourseId);

            var courses = new SelectList(c, "Id", "CourseName", selectedCourseId);
            var groups = new SelectList(g, "Id", "GroupName", selectedGroupId);

            var studentViewModel = new StudentsViewModel(courses, groups);


            if (selectedCourseId > 0) {
                studentViewModel.SelectedCourseDto = await _courseService.GetAsync(selectedCourseId);
            }

            if (selectedGroupId > 0) {
                studentViewModel.SelectedGroupDto = await _groupService.GetAsync(selectedGroupId);
            }

            var students = await _studentService.GetAllItemsAsync(searchString, selectedGroupId, selectedCourseId);

            studentViewModel.FilteredStudents = students.ToList();


            //GroupDto group = null;
            //if (selectedGroupId > 0) {
            //    group = await _groupService.GetAsync(selectedGroupId);
            //}

            //var students = await _studentService.GetAllItemsAsync(searchString, group?.GroupName);
            //var filteredGroups = await _groupService.GetAllItemsAsync(filter: group?.CourseId.ToString());
            //var courses = await _courseService.GetAllItemsAsync();
            //await PopulateGroupsDropDownList(group?.Id);


            //var studentViewModel = new StudentsViewModel {
            //    FilteredStudents = students.ToList(),
            //    Groups = filteredGroups,
            //    Courses = courses
            //};
            return View(studentViewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id) {
            var studentDto = await _studentService.GetAsync(id);
            return View(studentDto);
        }

        // GET: Students/Create
        public async Task<IActionResult> Create(StudentsViewModel viewModel) {
            var student = new StudentDto();
            if (viewModel.SelectedGroupDto?.Id > 0) {
                student.GroupId = (int)viewModel.SelectedGroupDto?.Id;
            }
            await PopulateGroupsDropDownList(viewModel.SelectedGroupDto?.Id);
            return View(student);
        }

        // POST: Students/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentDto studentDto) {
            if (!ModelState.IsValid) {
                await PopulateGroupsDropDownList();
                return View(studentDto);
            }
            var student = await _studentService.CreateAsync(studentDto);
            return RedirectToAction("Index", new { selectedGroup = student.GroupId});
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
        public async Task<IActionResult> Edit(StudentDto studentDto) {
            if (!ModelState.IsValid) {
                return View(studentDto);
            }
            await _studentService.UpdateAsync(studentDto);
            return RedirectToAction("Index", new { selectedGroup = studentDto.GroupId });
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


        protected async Task<IActionResult> SelectCourse(object sender, EventArgs e) {
            return RedirectToAction("Index", new{ selectedCourseId = 1 } );
        }

        public JsonResult GetGroups() {
            

            return null;
        }
    }
}
