using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_commerce_backend.Models;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EcommerceContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeesController(EcommerceContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(bool? isDeleted = null)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }

            IQueryable<Employee> employeesQuery = _context.Employees;

            if (isDeleted.HasValue)
            {
                // Filter by IsDeleted if the parameter is provided
                employeesQuery = employeesQuery.Where(c => c.IsDeleted == isDeleted.Value);
            }
            var employees = await employeesQuery.ToListAsync();
            return employees;
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
          if (_context.Employees == null)
          {
              return NotFound();
          }
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, [FromForm] IFormFile? employeeImageFile, [FromForm] Employee employee)
        {
            employee.Id = id;
            // Lấy thông tin khách hàng cũ từ database
            var existingEmployee = await _context.Employees.FindAsync(id);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            // Kiểm tra xem có hình ảnh cũ không
            if (!string.IsNullOrEmpty(employee.EmployeeImage))
            {
                // Xóa hình ảnh cũ
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, employee.EmployeeImage);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Kiểm tra xem có dữ liệu mới để cập nhật hình ảnh không
            if (employeeImageFile != null && employeeImageFile.Length > 0)
            {
                // Lưu trữ hình ảnh mới
                var fileName = $"{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(employeeImageFile.FileName)}";
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "employee_images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await employeeImageFile.CopyToAsync(stream);
                }

                // Cập nhật thông tin của khách hàng
                employee.EmployeeImage = "employee_images/" + fileName;
            }

            // Cập nhật thông tin khách hàng
            //_context.Entry(existingEmployee).CurrentValues.SetValues(employee);
            foreach (var property in typeof(Employee).GetProperties())
            {
                var newValue = property.GetValue(employee);
                if (newValue != null)
                {
                    property.SetValue(existingEmployee, newValue);
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(employee);
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee([FromForm] IFormFile? employeeImageFile, [FromForm] Employee employee)
        {

            if (employeeImageFile != null && employeeImageFile.Length > 0)
            {
                var fileName = $"{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(employeeImageFile.FileName)}";
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "employee_images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await employeeImageFile.CopyToAsync(stream);
                }

                employee.EmployeeImage = "employee_images/" + fileName;
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            //_context.Employees.Remove(employee);
            employee.IsDeleted = true;
            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        // PUT: api/Employees/Restore/5
        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> RestoreEmployee(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            // Restore the employee by setting IsDeleted to false
            employee.IsDeleted = false;

            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        private bool EmployeeExists(int id)
        {
            return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
