using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CrudUsingADO.Models
{
    public class tblDepartment
    {
        public int ID { get; set; }
        [RegularExpression("[a-z A-Z]*", ErrorMessage="Enter Only Characters")]
        public String DepartmentName { get; set; }
        [RegularExpression("[a-z A-Z 0-9_\\-.]+[@]+[a-z]+[\\.]+[a-z]{2,3}", ErrorMessage ="Enter a valid Email Address")]
        public String Location { get; set; }
        public bool IsActive { get; set; }
    }
}