using System;
using System.Collections.Generic;

#nullable disable

namespace PayrollManagementSystem_API.Models
{
    public partial class CountingMonthlyLeave
    {
        public double? NumOfLeaves { get; set; }
        public string EmployeeId { get; set; }
        public int? LeaveMonth { get; set; }
    }
}
