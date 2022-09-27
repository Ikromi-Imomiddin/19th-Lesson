using Dapper;
using Domain.Response;
using Domain.Dto;
using Domain.Emtities;
using Services.DateContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{

    public interface IManagerService
    {
    Task<Response<List<ManagerDto>>> GetManagers();
    Task<Response<department_manager>> AddManager(department_manager manager);

    }

    public class ManagerService: IManagerService
    {
        private Context _context;
        public ManagerService(Context context)
        {
            _context = context;
        }

        public async Task<Response<List<ManagerDto>>> GetManagers()
        {
            var connection = _context.CreateConnection();
            string sql = "select dm.employeeid as ManagerId ,concat(e.firstname , ' ' , e.lastname) as ManagerFullName ,dm.departmentid , d.name as DepartmentName ,  dm.fromdate , dm.todate FROM department_manager dm JOIN employee e ON e.id = dm.employeeid join department d ON dm.departmentid = d.id ;";
            try
            {
                var result = await connection.QueryAsync<ManagerDto>(sql);
                return new Response<List<ManagerDto>>(result.ToList());
            }
            catch (Exception ex)
            {
                return new Response<List<ManagerDto>>(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<Response<department_manager>> AddManager(department_manager manager)
        {
            var connection = _context.CreateConnection();
            string sql = $"INSERT INTO department_manager (EmployeeId , DepartmentId , FromDate , ToDate , CurrentDepartment ) VALUES (@EmployeeId,@DepartmentId , @FromDate , @ToDate,@CurrentDepartment) ";
            try
            {
                var result = await connection.ExecuteScalarAsync<department_manager>(sql, new { manager.EmployeeId, manager.DepartmentId, manager.FromDate, manager.ToDate, manager.CurrentDepartment });
                manager = result;
                return new Response<department_manager>(manager);
            }
            catch (Exception ex)
            {
                return new Response<department_manager>(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
