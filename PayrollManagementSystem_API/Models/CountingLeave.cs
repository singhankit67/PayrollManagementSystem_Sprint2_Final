using System;
using System.Collections.Generic;

#nullable disable

namespace PayrollManagementSystem_API.Models
{
    public partial class CountingLeave
    {
        public double? NumOfLeaves { get; set; }
        public string EmployeeId { get; set; }
    }
}
