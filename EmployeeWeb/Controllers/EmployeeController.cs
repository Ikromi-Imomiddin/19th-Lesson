using Domain.Response;
using Domain.Dto;
using Domain.Emtities;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace EmployeeWeb.Controllers
{
    public class EmployeeController
    {
        private EmployeeService _employeeService;
        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpGet("GetEmployees")]
        public async Task<Response<List<EmployeeDto>>> GetEmployees()
        {
            return await _employeeService.GetEmployees();
        }

        [HttpGet("GetEmployeeById")]
        public async Task<Response<List<EmployeeDto>>> GetEmployeeById(int id)
        {
            return await _employeeService.GetEmployeeById(id);
        }

        [HttpPost("AddEmployee")]
        public async Task<Response<Employee>> AddDepartment(Employee employee)
        {
            return await _employeeService.AddEmployee(employee);
        }

        [HttpPut("UpdateEmployee")]
        public async Task<Response<Employee>> UpdateEmployee(Employee employee)
        {
            return await _employeeService.UpdateEmployee(employee);
        }
    }
}
