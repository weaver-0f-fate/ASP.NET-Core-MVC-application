using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business;
using Core.Models;
using Core.ModelsDTO;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Task9.TaskViewModels;

namespace Task9.Controllers {
    public class StudentsController : Controller {
        private readonly StudentPresentation _studentPresentation;

        public StudentsController(Task9Context context, IMapper mapper) {
            _studentPresentation = new StudentPresentation(context, mapper);
        }

        // GET: Students
        public async Task<IActionResult> Index(string studentGroup, string searchString) {
            var studentDTOs = await _studentPresentation.GetAllItems(searchString, studentGroup);
            var groups = await _studentPresentation.GetGroups();


            var studentViewModel = new StudentsViewModel {
                Groups = new SelectList(groups.Select(x => x.GroupName)),
                Students = studentDTOs.ToList()
            };
            return View(studentViewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id) {
            var studentDTO = await _studentPresentation.GetItem(id);
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
            await _studentPresentation.CreateItem(studentDTO);
            return RedirectToAction(nameof(Index));
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            var studentDTO = await _studentPresentation.GetItem(id);
            await PopulateGroupsDropDownList(studentDTO.GroupId);
            return View(studentDTO);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentDTO studentDTO) {
            if (id != studentDTO.Id) {
                return NotFound();
            }
            if (!ModelState.IsValid) {
                return View(studentDTO);
            }

            try {
                await _studentPresentation.UpdateItem(studentDTO);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ) {
                if (!_studentPresentation.ItemExists(id)) {
                    await PopulateGroupsDropDownList(studentDTO.GroupId);
                    return NotFound();
                }
                throw;
            }
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            var studentDTO = await _studentPresentation.GetItem(id);
            return View(studentDTO);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {

            try {
                await _studentPresentation.DeleteItem(id);
                return RedirectToAction("Index");
            }
            catch (Exception) {
                return RedirectToAction("Delete", new { id });
            }
        }

        public IActionResult ClearFilter() {
            return RedirectToAction("Index");
        }

        private async Task PopulateGroupsDropDownList(object selectedGroup = null) {
            var groups = await _studentPresentation.GetGroups();
            ViewBag.GroupId = new SelectList(groups, "Id", "GroupName", selectedGroup);
        }
    }
}
