using System;
using System.Collections.Generic;
using System.Text;

namespace CRUDrequestsTask1.Models
{
    public class Employee
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal Salary { get; set; }
    }
}
