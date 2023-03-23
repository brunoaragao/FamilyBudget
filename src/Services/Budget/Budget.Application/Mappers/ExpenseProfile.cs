using AutoMapper;

using Budget.Application.Dtos;
using Budget.Domain.AggregateModels.ExpenseAggregates;

namespace Budget.Application.Mappers;

public class ExpenseProfile : Profile
{
    public ExpenseProfile()
    {
        CreateMap<Expense, ExpenseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => (ExpenseCategoryDto)src.CategoryId));
    }
}