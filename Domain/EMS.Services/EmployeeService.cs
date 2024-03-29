﻿using EMS.Contracts.Employee;
using EMS.Data.Employee;
using EMS.Data.Exceptions;
using EMS.Shared.Repositories;
using EMS.Shared.Services;
using Mapster;

namespace EMS.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IRepositoryManager repositoryManager) => _employeeRepository = repositoryManager.EmployeeRepository;
    
    public async Task<IReadOnlyList<EmployeeModel>> GetAll()
    {
        var employees = _employeeRepository.GetAll();
        return employees.Select(employee => employee.Adapt<EmployeeModel>()).ToList();
    }

    public async Task<EmployeeModel?> GetById(Guid id)
    {
        var employee = await _employeeRepository.GetById(id);
        return employee.Adapt<EmployeeModel>();
    }

    public async Task<EmployeeModel> Add(EmployeeDto employee)
    {
        var isPeselExist = await _employeeRepository.IsPeselExist(employee.Pesel);
        if (isPeselExist)
        {
            throw new PeselExistException(employee.Pesel!);
        }
        
        var newEmployee = await _employeeRepository.AddAsync(employee.Adapt<Employee>());
        await _employeeRepository.SaveChangesAsync();
        return newEmployee.Adapt<EmployeeModel>();
    }
}