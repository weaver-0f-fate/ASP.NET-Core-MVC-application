using System;
using Core.Models;

namespace Services {
    internal static class PredicateBuilder{
        public static Func<T, bool> BuildPredicate<T> (T model, FilteringParameters parameters) 
            where T : AbstractModel {
            Func<T, bool> predicate = null;




            return predicate;
        }
        private static bool BuildCoursePredicate(Course model) {

            return false;
        }
        private static bool BuildGroupPredicate(Group model) {

            return false;
        }
        private static bool BuildStudentPredicate(Student model) {

            return false;
        }
    }
}
