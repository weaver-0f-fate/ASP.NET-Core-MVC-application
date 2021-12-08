using System;

namespace Core.Exceptions {
    public class ForeignEntitiesException : Exception {

        public override string Message => "Item cannot be deleted since it has foreign entities.";

    }
}