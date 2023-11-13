using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Shared.Messages
{
    public class CourseNameChangedEvent
    {
        public string CourseId { get; set; }
        public string UpdateName { get; set; }
    }
}
