using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Buildings
        CreateMap<Building, BuildingResponse>()
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Nombre : null));

        CreateMap<CreateBuildingRequest, Building>();
        CreateMap<UpdateBuildingRequest, Building>();

        // Companies
        CreateMap<Company, CompanyResponse>();
        CreateMap<CreateCompanyRequest, Company>();
        CreateMap<UpdateCompanyRequest, Company>();

        // Employees
        CreateMap<Employee, EmployeeResponse>()
            .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Building != null ? src.Building.Nombre : null));

        CreateMap<CreateEmployeeRequest, Employee>();
        CreateMap<UpdateEmployeeRequest, Employee>();

        // Quotations
        CreateMap<Quotation, QuotationResponse>()
            .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Building != null ? src.Building.Nombre : null));

        CreateMap<CreateQuotationRequest, Quotation>();
        CreateMap<UpdateQuotationRequest, Quotation>();

        // QuotationItems
        CreateMap<QuotationItem, QuotationItemResponse>();
        CreateMap<CreateQuotationItemRequest, QuotationItem>();
        CreateMap<UpdateQuotationItemRequest, QuotationItem>();

        // Users
        CreateMap<User, UserResponse>();
        CreateMap<CreateUserRequest, User>();
        CreateMap<UpdateUserRequest, User>();
    }
}
