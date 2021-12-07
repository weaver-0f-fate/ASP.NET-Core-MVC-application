using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business;
using Core.ModelsDTO;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Task9.TaskViewModels;

namespace Task9.Controllers {
    public class GroupsController : Controller {
        private readonly GroupPresentation _groupPresentation;

        public GroupsController(Task9Context context, IMapper mapper) {
            _groupPresentation = new GroupPresentation(context, mapper);
        }

        // GET: Groups
        public async Task<IActionResult> Index(string groupCourse, string searchString) {
            var courses = await _groupPresentation.GetCourses();
            var groupViewModel = new GroupViewModel {
                Courses = new SelectList(courses.Select(x => x.CourseName)),
                Groups = await _groupPresentation.GetAllItems(searchString, groupCourse)
            };
            return View(groupViewModel);
        }
        
        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id) {
            var groupDTO = await _groupPresentation.GetItem(id);
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

            await _groupPresentation.CreateItem(groupDTO);
            return RedirectToAction(nameof(Index));
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }
            var groupDTO = await _groupPresentation.GetItem(id);
            await PopulateCoursesDropDownList(groupDTO.CourseId);
            return View(groupDTO);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GroupDTO groupDTO) {
            if (id != groupDTO.Id) {
                return NotFound();
            }
            if (!ModelState.IsValid) {
                return View(groupDTO);
            }
            try {
                await _groupPresentation.UpdateItem(groupDTO);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception) {
                if (!_groupPresentation.ItemExists(id)) {
                    await PopulateCoursesDropDownList(groupDTO.CourseId);
                    return NotFound();
                }
                throw;
            }
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id, string message = null) {
            if (id == null) {
                return NotFound();
            }
            var groupDTO = await _groupPresentation.GetItem(id);
            ViewBag.ErrorMessage = message;

            if (groupDTO is null) {
                return NotFound();
            }
            return View(groupDTO);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            try {
                await _groupPresentation.DeleteItem(id);
                return RedirectToAction("Index");
            }
            catch (Exception) {
                var message =
                    "Group cannot be deleted since it contains students.";
                return RedirectToAction("Delete", new { id, message });
            }
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
