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

    public interface IEmployeeService
    {
        Task<Response<List<EmployeeDto>>> GetEmployees();
         Task<Response<List<EmployeeDto>>> GetEmployeeById(int id);
        Task<Response<Employee>> AddEmployee(Employee employee);
         Task<Response<Employee>> UpdateEmployee(Employee employee);
    }


    public class EmployeeService : IEmployeeService
    {
            private Context _context;
            public EmployeeService(Context context)
            {
                _context = context;
            }

            public async Task<Response<List<EmployeeDto>>> GetEmployees()
            {
                var connection = _context.CreateConnection();
                string sql = "select e.id ,concat(e.firstname , ' ' , e.lastname) as FullName ,dm.departmentid as DepartmentId, d.name as DepartmentName FROM employee  e JOIN department  d ON d.id = e.id JOIN department_manager dm ON dm.employeeid = d.id;";
                try
                {
                    var result = await connection.QueryAsync<EmployeeDto>(sql);
                    return new Response<List<EmployeeDto>>(result.ToList());
                }
                catch (Exception ex)
                {
                    return new Response<List<EmployeeDto>>(System.Net.HttpStatusCode.InternalServerError, ex.Message);
                }
            }



            public async Task<Response<List<EmployeeDto>>> GetEmployeeById(int id)
            {
                var connection = _context.CreateConnection();
                string sql = "select e.id ,concat(e.firstname , ' ' , e.lastname) as FullName ,dm.departmentid, d.name as DepartmentName FROM employee  e JOIN department  d ON d.id = e.id JOIN department_manager dm ON dm.employeeid = d.id where e.id = @id;;";
                try
                {
                    var result = await connection.QueryAsync<EmployeeDto>(sql, new { id });
                    return new Response<List<EmployeeDto>>(result.ToList());
                }
                catch (Exception ex)
                {
                    return new Response<List<EmployeeDto>>(System.Net.HttpStatusCode.InternalServerError, ex.Message);
                }
            }


            public async Task<Response<Employee>> AddEmployee(Employee employee)
            {
                var connection = _context.CreateConnection();
                string sql = $"INSERT INTO employee (Id , BirthDate , FirstName , LastName , Gender , HireDate) VALUES (@Id,@BirthDate , @FirstName , @LastName,@Gender,@HireDate) ";
                try
                {
                    var result = await connection.ExecuteScalarAsync<Employee>(sql, new { employee.Id, employee.BirthDate, employee.FirstName, employee.LastName , employee.Gender , employee.HireDate });
                employee = result;
                    return new Response<Employee>(employee);
                }
                catch (Exception ex)
                {
                    return new Response<Employee>(System.Net.HttpStatusCode.InternalServerError, ex.Message);
                }
            }

            public async Task<Response<Employee>> UpdateEmployee(Employee employee)
            {
                using var connection = _context.CreateConnection();
                string sql = $"UPDATE  employee SET Id = @Id, BirthDate = @BirthDate,FirstName = @FirstName, LastName = @LastName,Gender = @Gender, HireDate = @HireDate  WHERE id = @Id";
                try
                {
                    var response = await connection.ExecuteAsync(sql, new { employee.Id, employee.BirthDate , employee.FirstName, employee.LastName , employee.Gender, employee.HireDate });
                    return new Response<Employee>(employee);
                }
                catch (Exception ex)
                {
                    return new Response<Employee>(System.Net.HttpStatusCode.InternalServerError, ex.Message);
                }
            }


        }
    }
