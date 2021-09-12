using System;
using System.Collections.Generic;

#nullable disable

namespace PayrollManagementSystem_API.Models
{
    public partial class HalfWorkingDaysTimesheet
    {
        public double? NumberOfHalfDays { get; set; }
        public string EmployeeId { get; set; }
    }
}
