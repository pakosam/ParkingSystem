using ParkingSystem.Data;
using ParkingSystem.DTOs;
using ParkingSystem.Entities;
using ParkingSystem.Repositories;

namespace ParkingSystem.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<List<EmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto employeeDto, int parkingId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            if (employeeDto.BirthDate == DateOnly.MinValue)
            {
                throw new NotImplementedException("Birth date is required");
            }

            var age = today.Year - employeeDto.BirthDate.Year;
            if (employeeDto.BirthDate > today.AddYears(-age))
            {
                age--;
            }

            if (age > 70)
            {
                throw new NotImplementedException("Employee cannot be older than 70 years.");
            }

            if (age < 18)
            {
                throw new NotImplementedException("Employee cannot be younger than 18 years.");
            }

            if (employeeDto.Name.Length < 3 ||
                employeeDto.Surname.Length < 3 ||
                employeeDto.Username.Length < 4 ||
                employeeDto.Password.Length < 10)
            {
                throw new NotImplementedException("Data is not filled properly");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(employeeDto.Password);

            var employee = new Employee
            {
                Name = employeeDto.Name,
                Surname = employeeDto.Surname,
                BirthDate = employeeDto.BirthDate,
                Username = employeeDto.Username,
                Password = hashedPassword,
                ParkingId = parkingId
            };

            await _employeeRepository.CreateEmployeeAsync(employee);

            var employees = await _employeeRepository.GetAllEmployeesAsync();

            var employeesDtos = employees
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FullName = e.Name + " " + e.Surname,
                    BirthDate = e.BirthDate,
                    ParkingId = e.ParkingId ?? 0
                })
                .ToList();

            return employeesDtos;
        }

        public async Task<List<EmployeeDto>> DeleteEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetEmployeeAsync(id);

            if (employee == null)
                throw new ArgumentException($"Employee with ID {id} does not exist.");

            await _employeeRepository.DeleteEmployeeAsync(employee);

            var employees = await _employeeRepository.GetAllEmployeesAsync();

            var employeeDtos = employees
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FullName = e.Name + " " + e.Surname,
                    BirthDate = e.BirthDate,
                    ParkingId = e.ParkingId ?? 0
                })
                .ToList();

            return employeeDtos;
        }

        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();

            if (employees == null)
            {
                throw new KeyNotFoundException($"There are no existing employees.");
            }

            var employeeDtos = employees
                 .Select(e => new EmployeeDto
                 {
                     Id = e.Id,
                     FullName = e.Name + " " + e.Surname,
                     BirthDate = e.BirthDate,
                     ParkingId = e.ParkingId ?? 0
                 })
                 .ToList();

            return employeeDtos;
        }

        public async Task<EmployeeDto> GetEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetEmployeeAsync(id);

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} does not exist.");
            }

            return new EmployeeDto
            {
                Id = employee.Id,
                FullName = employee.Name + " " + employee.Surname,
                BirthDate = employee.BirthDate,
                ParkingId = employee.ParkingId ?? 0
            };
        }

        public async Task<List<EmployeeDto>> UpdateEmployeeAsync(UpdateEmployeeDto updatedEmployee, int parkingId)
        {
            var dbEmployee = await _employeeRepository.GetEmployeeAsync(updatedEmployee.Id);

            if (dbEmployee == null)
                throw new ArgumentException($"Employee with ID {updatedEmployee.Id} does not exist.");

            var today = DateOnly.FromDateTime(DateTime.Today);

            if (updatedEmployee.BirthDate == DateOnly.MinValue)
            {
                throw new NotImplementedException("Birth date is required");
            }

            var age = today.Year - updatedEmployee.BirthDate.Year;
            if (updatedEmployee.BirthDate > today.AddYears(-age))
            {
                age--;
            }

            if (age > 70)
            {
                throw new NotImplementedException("Employee cannot be older than 70 years.");
            }

            if (age < 18)
            {
                throw new NotImplementedException("Employee cannot be younger than 18 years.");
            }

            if (updatedEmployee.Name.Length < 3 ||
                updatedEmployee.Surname.Length <= 3)
            {
                throw new NotImplementedException("Data is not filled properly");
            }

            dbEmployee.Name = updatedEmployee.Name;
            dbEmployee.Surname = updatedEmployee.Surname;
            dbEmployee.BirthDate = updatedEmployee.BirthDate;
            dbEmployee.ParkingId = parkingId;

            await _employeeRepository.UpdateEmployeeAsync(dbEmployee);

            var employees = await _employeeRepository.GetAllEmployeesAsync();

            var employeeDtos = employees
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FullName = e.Name + " " + e.Surname,
                    BirthDate = e.BirthDate,
                    ParkingId = e.ParkingId ?? 0
                })
                .ToList();

            return employeeDtos;
        }
    }
}
