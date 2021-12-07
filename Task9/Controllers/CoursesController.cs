using System;
using System.Linq;
using AutoMapper;
using Core.Models;
using Core.ModelsDTO;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Task9.Controllers {
    public class CoursesController : Controller {
        private readonly CourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public CoursesController(Task9Context context, IMapper mapper) {
            _courseRepository = CourseRepository.GetCourseRepository(context);
            _mapper = mapper;
        }

        // GET: Courses
        public IActionResult Index(string searchString) {
            var courses = _courseRepository.GetEntityList(searchString);
            var coursesDTOs = courses.Select(x => _mapper.Map<CourseDTO>(x));
            
            return View(coursesDTOs);
        }

        // GET: Courses/Details/5
        public IActionResult Details(int? id) {
            if (id is null) {
                return NotFound();
            }
            var course = _courseRepository.GetEntity((int)id);
            if (course == null) {
                return NotFound();
            }

            var courseDTO = _mapper.Map<CourseDTO>(course);
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
        public IActionResult Create([Bind("Id,CourseName,CourseDescription")] Course course) {
            if (ModelState.IsValid) {
                
                _courseRepository.Create(course);
                return RedirectToAction(nameof(Index), new { id = course.Id });
            }
            var courseDTO = _mapper.Map<CourseDTO>(course);
            return View(courseDTO);
        }

        // GET: Courses/Edit/5
        public IActionResult Edit(int? id) {
            if (id == null) {
                return NotFound();
            }
            var course = _courseRepository.GetEntity((int)id);
            if (course == null) {
                return NotFound();
            }
            var courseDTO = _mapper.Map<CourseDTO>(course);
            return View(courseDTO);
        }

        // POST: Courses/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,CourseName,CourseDescription")] Course course) {
            if (id != course.Id) {
                return NotFound();
            }
            if (!ModelState.IsValid) {
                var courseDTO = _mapper.Map<CourseDTO>(course);
                return View(courseDTO);
            }

            try {
                _courseRepository.Update(course);
                return RedirectToAction(nameof(Index), new { course.Id });
            }
            catch (DbUpdateConcurrencyException) {
                if (!_courseRepository.CourseExists(course.Id)) {
                    return NotFound();
                }
                throw;
            }
        }
        
        // GET: Courses/Delete/5
        public IActionResult Delete(int? id, string message = null) {
            if (id == null) {
                return NotFound();
            }
            var course = _courseRepository.GetEntity((int)id);
            ViewBag.ErrorMessage = message;
            if (course is null) {
                return NotFound();
            }
            var courseDTO = _mapper.Map<CourseDTO>(course);
            return View(courseDTO);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id) {
            try {
                _courseRepository.Delete(id);
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
