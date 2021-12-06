using System;
using System.Linq;
using AutoMapper;
using Core.Models;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Task9.TaskViewModels;
using Task9.TaskViewModels.ModelsDTO;

namespace Task9.Controllers {
    public class StudentsController : Controller {
        private readonly StudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentsController(Task9Context context, IMapper mapper) {
            _studentRepository = StudentRepository.GetStudentData(context);
            _mapper = mapper;
        }

        // GET: Students
        public IActionResult Index(string studentGroup, string searchString) {
            var students = _studentRepository.GetEntityList(studentGroup, searchString);
            var groups = _studentRepository.GetGroupsList();

            var studentDTOs = students.Select(x => _mapper.Map<StudentDTO>(x));
            foreach (var studentDTO in studentDTOs) {
                var student = students.FirstOrDefault(x => x.Id == studentDTO.Id);
                studentDTO.GroupName = student.Group.GroupName;
            }

            var studentViewModel = new StudentsViewModel {
                Groups = new SelectList(groups),
                Students = studentDTOs.ToList()
            };
            return View(studentViewModel);
        }

        // GET: Students/Details/5
        public IActionResult Details(int? id) {
            if (id is null) {
                return NotFound();
            }
            var student = _studentRepository.GetEntity((int)id);
            if (student is null) {
                return NotFound();
            }
            var studentDTO = _mapper.Map<StudentDTO>(student);
            studentDTO.GroupName = student.Group.GroupName;
            studentDTO.CourseName = student.Group.Course.CourseName;
            return View(studentDTO);
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
        public IActionResult Create([Bind("Id,GroupId,FirstName,LastName")] Student student) {
            if (ModelState.IsValid) {
                _studentRepository.Create(student);
                return RedirectToAction(nameof(Index));
            }
            PopulateGroupsDropDownList();
            var studentDTO = _mapper.Map<StudentDTO>(student);
            studentDTO.GroupName = student.Group.GroupName;
            studentDTO.CourseName = student.Group.Course.CourseName;
            return View(studentDTO);
        }

        // GET: Students/Edit/5
        public IActionResult Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var student = _studentRepository.GetEntity((int)id);
            if (student is null) {
                return NotFound();
            }
            PopulateGroupsDropDownList(student.GroupId);
            var studentDTO = _mapper.Map<StudentDTO>(student);
            studentDTO.GroupName = student.Group.GroupName;
            studentDTO.CourseName = student.Group.Course.CourseName;
            return View(studentDTO);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,GroupId,FirstName,LastName")] Student student) {
            if (id != student.Id) {
                return NotFound();
            }
            if (!ModelState.IsValid) {
                var studentDTO = _mapper.Map<StudentDTO>(student);
                studentDTO.GroupName = student.Group.GroupName;
                studentDTO.CourseName = student.Group.Course.CourseName;
                return View(studentDTO);
            }

            try {
                _studentRepository.Update(student);
                return RedirectToAction(nameof(Details), new { student.Id });
            }
            catch (Exception ) {
                if (!_studentRepository.StudentExists(student.Id)) {
                    PopulateGroupsDropDownList(student.GroupId);
                    return NotFound();
                }
                throw;
            }
        }

        // GET: Students/Delete/5
        public IActionResult Delete(int? id) {
            if (id == null) {
                return NotFound();
            }
            var student = _studentRepository.GetEntity((int)id);

            if (student is null) {
                return NotFound();
            }
            var studentDTO = _mapper.Map<StudentDTO>(student);
            studentDTO.GroupName = student.Group.GroupName;
            studentDTO.CourseName = student.Group.Course.CourseName;
            return View(studentDTO);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id) {

            try {
                _studentRepository.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception) {
                return RedirectToAction("Delete", new { id });
            }
        }

        public IActionResult ClearFilter() {
            return RedirectToAction("Index");
        }

        private void PopulateGroupsDropDownList(object selectedGroup = null) {
            var groups = _studentRepository.GetQueryableGroups();
            ViewBag.GroupId = new SelectList(groups, "Id", "GroupName", selectedGroup);
        }
    }
}
