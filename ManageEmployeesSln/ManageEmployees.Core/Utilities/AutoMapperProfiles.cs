using AutoMapper;
using ManageEmployees.Core.DTOs;
using ManageEmployees.Infraestructure.Models;

namespace ManageEmployees.Core.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeAddDto, Employee>();
            CreateMap<EmployeeUpdateDto, Employee>();
        }
    }
}
