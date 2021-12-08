using System;

namespace Core.Exceptions {
    public class NoEntityException : Exception {
        public override string Message => "Entity doesn't exist.";
    }
}
