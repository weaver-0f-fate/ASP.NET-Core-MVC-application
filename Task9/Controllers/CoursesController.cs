using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business;
using Core.Models;
using Core.ModelsDTO;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Task9.Controllers {
    public class CoursesController : Controller {
        private readonly CoursePresentation _coursePresentation;

        public CoursesController(Task9Context context, IMapper mapper) {
            _coursePresentation = new CoursePresentation(context, mapper);
        }

        // GET: Courses
        public async Task<IActionResult> Index(string searchString) {
            var courseDTOs = await _coursePresentation.GetAllItems(searchString);
            return View(courseDTOs);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id) {
            var courseDTO = await _coursePresentation.GetItem(id);
            return View(courseDTO);
        }

        // GET: Courses/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Courses/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseDTO courseDTO) {
            if (!ModelState.IsValid) {
                return View(courseDTO);
            }
            await _coursePresentation.CreateItem(courseDTO);
            return RedirectToAction(nameof(Index));
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }
            var courseDTO = await _coursePresentation.GetItem(id);
            if (courseDTO == null) {
                return NotFound();
            }
            return View(courseDTO);
        }

        // POST: Courses/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseDTO courseDTO) {
            if (id != courseDTO.Id) {
                return NotFound();
            }
            if (!ModelState.IsValid) {
                return View(courseDTO);
            }

            try {
                await _coursePresentation.UpdateItem(courseDTO);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException) {
                if (!_coursePresentation.ItemExists(id)) {
                    return NotFound();
                }
                throw;
            }
        }
        
        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id, string message = null) {
            if (id == null) {
                return NotFound();
            }
            var courseDTO = await _coursePresentation.GetItem(id);
            ViewBag.ErrorMessage = message;
            if (courseDTO is null) {
                return NotFound();
            }
            return View(courseDTO);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            try {
                await _coursePresentation.DeleteItem(id);
                return RedirectToAction("Index");
            }
            catch (Exception) {
                var message =
                    "Cascade Delete is restricted. Course cannot be deleted since there are associated groups.";
                return RedirectToAction("Delete", new { id, message });
            }
        }

        public IActionResult ViewGroups(string courseName) {
            return RedirectToAction("Index", "Groups", new { groupCourse = courseName });
        }
        public IActionResult ClearFilter() {
            return RedirectToAction("Index");
        }
    }
}
