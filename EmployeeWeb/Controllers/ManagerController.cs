using Domain.Response;
using Domain.Dto;
using Domain.Emtities;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace EmployeeWeb.Controllers
{
    public class ManagerController
    {
        private ManagerService _managerService;
        public ManagerController(ManagerService managerService)
        {
            _managerService = managerService;
        }
        [HttpGet("GetManagers")]
        public async Task<Response<List<ManagerDto>>> GetManagers()
        {
            return await _managerService.GetManagers();
        }
        [HttpPost("AddManager")]
        public async Task<Response<department_manager>> AddManager(department_manager department)
        {
            return await _managerService.AddManager(department);
        }
    }
}
