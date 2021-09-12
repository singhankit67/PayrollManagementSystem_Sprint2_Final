using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace PayrollManagementSystem_Sprint2.Models
{
    public class AuthorizationResponse
    {
        public EmployeeMaster employeeObj { get; set; }
        public string _token { get; set; }

        public AuthorizationResponse(EmployeeMaster empObj,string tokenObj)
        {
            employeeObj = empObj;
            _token = tokenObj;
        }


    }
}
