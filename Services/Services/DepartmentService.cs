using Dapper;
using Domain.Response;
using Domain.Dto;
using Domain.Emtities;
using Services.DateContext; 
namespace Services.Services
{
     public interface IDepartmentService
    {
        Task<Response<List<DepartmentDto>>> GetDepartments();
        Task<Response<List<DepartmentDto>>> GetDepartmentById(int id);
        Task<Response<Department>> AddDepartment(Department department);
        
    }

    public class DepartmentService : IDepartmentService
    {
        private Context _context;
        public DepartmentService(Context context)
        {
              _context = context;
        }

        public async Task<Response<List<DepartmentDto>>> GetDepartments()
        {
            var connection = _context.CreateConnection();
            string sql = "select d.id ,  d.name , dm.EmployeeId as ManagerId ,concat(emp.firstname , ' ' , emp.lastname) as ManagerFullName FROM department d JOIN department_manager dm ON dm.departmentid = d.id JOIN employee emp ON dm.employeeid = emp.id ;";
            try
            {
                var result = await connection.QueryAsync<DepartmentDto>(sql);
                return new Response<List<DepartmentDto>>(result.ToList()); 
            }
            catch(Exception ex)
            {
                return new Response<List<DepartmentDto>>(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        public async Task<Response<List<DepartmentDto>>> GetDepartmentById(int id)
        {
            var connection = _context.CreateConnection();
            string sql = "select d.id ,  d.name , dm.EmployeeId as MenegerId ,concat(emp.firstname , ' ' , emp.lastname) as ManagerFullName FROM department d JOIN department_manager dm ON dm.departmentid = d.id JOIN employee emp ON dm.employeeid = emp.id where d.id = @id;;";
            try
            {
                var result = await connection.QueryAsync<DepartmentDto>(sql , new { id });
                return new Response<List<DepartmentDto>>(result.ToList());
            }
            catch (Exception ex)
            {
                return new Response<List<DepartmentDto>>(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        public async Task<Response<Department>> AddDepartment(Department department)
        {
            var connection = _context.CreateConnection();
            string sql = $"INSERT INTO Department (Id , Name) VALUES (@Id,@Name) ";
            try
            {
                var result = await connection.ExecuteScalarAsync<Department>(sql, new { department.Id, department.Name});
                department = result;
                return new Response<Department>(department);
            }
            catch (Exception ex)
            {
                return new Response<Department>(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<Response<Department>> UpdateDepartment(Department department)
        {
            using var connection = _context.CreateConnection();
            string sql = $"UPDATE  Department SET Id = @Id, Name = @Name  WHERE id = @Id";
            try
            {
                var response = await connection.ExecuteAsync(sql , new {department.Name , department.Id});
                return new Response<Department>(department);
            }
            catch (Exception ex)
            {
                return new Response<Department>(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }









    }
}
