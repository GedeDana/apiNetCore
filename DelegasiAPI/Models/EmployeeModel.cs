using Mahas.Components;

namespace DelegasiAPI.Models
{
    [DbTable("EmployeeTable")]
    public class EmployeeModel
    {
        [DbKey(true)]
        [DbColumn]
        public int Id { get; set; }

        [DbColumn]
        public string EmployeeName { get; set; }

        [DbColumn]
        public DateTime DateOfBirth { get; set; }

        [DbColumn]
        public string Address { get; set; }

        [DbColumn]
        public decimal Salary { get; set; }


    }
}
