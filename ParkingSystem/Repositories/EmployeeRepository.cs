﻿using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.DTOs;
using ParkingSystem.Entities;

namespace ParkingSystem.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _dataContext;

        public EmployeeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task CreateEmployeeAsync(Employee employee)
        {
            _dataContext.Employees.Add(employee);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(Employee employee)
        {
            _dataContext.Employees.Remove(employee);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            var employees = await _dataContext.Employees.ToListAsync();
            return employees;
        }

        public async Task<Employee> GetEmployeeAsync(int id)
        {
            var employee = await _dataContext.Employees
                .FirstOrDefaultAsync(e => e.Id == id);
            return employee;
        }

        public async Task UpdateEmployeeAsync(Employee updatedEmployee)
        {
            _dataContext.Employees.Update(updatedEmployee);
            await _dataContext.SaveChangesAsync();
        }
        public async Task<List<int>> GetEmployeesIdByParkingIdAsync(int parkingId)
        {
            var employeesIdByParkingId = await _dataContext.Employees 
                .Where(e => e.ParkingId == parkingId)
                .Select(e => e.Id)
                .ToListAsync();

            return employeesIdByParkingId;
        }

        public async Task<List<Employee>> GetEmployeesByIdsAsync(List<int> employeeIds)
        {
            return await _dataContext.Employees
                .Where(e => employeeIds.Contains(e.Id))
                .ToListAsync();
        }

        public async Task UpdateEmployeesParkingIdAsync(List<int> employeeIds)
        {
            var employees = await _dataContext.Employees
                .Where(e => employeeIds.Contains(e.Id))
                .ToListAsync();

            foreach (var employee in employees) 
            {
                employee.ParkingId = null; 
            }

            await _dataContext.SaveChangesAsync();
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _dataContext.Employees
                .AnyAsync(e => e.Username == username);
        }

        public async Task<Employee> GetEmployeeByUsernameAsync(string username)
        {
            return await _dataContext.Employees
                .FirstOrDefaultAsync(e => e.Username == username);
        }
    }
}