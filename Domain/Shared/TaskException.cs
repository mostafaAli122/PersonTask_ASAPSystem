using System;

namespace Domain.Shared
{
   
    public class TaskException : Exception
    {
        public TaskValidationKeysEnum ValidationEum { get; set; }

        public TaskException(TaskValidationKeysEnum message)
            : base($"{((int)message)}-{message}")
        {
            this.ValidationEum = message;
        }

    }
}
