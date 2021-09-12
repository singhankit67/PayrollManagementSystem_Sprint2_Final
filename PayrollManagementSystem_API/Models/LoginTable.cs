using System;
using System.Collections.Generic;

#nullable disable

namespace PayrollManagementSystem_API.Models
{
    public partial class LoginTable
    {
        public string EmployeeId { get; set; }
        public string Password { get; set; }
        public string EmployeeUserName { get; set; }
        public string SecurityAnswer { get; set; }

        public virtual EmployeeMaster Employee { get; set; }
    }
}
