using AutoMapper;

using Budget.Application.Dtos;
using Budget.Domain.Services;

namespace Budget.Application.Mappers;

public class MonthlyBudgetSummaryProfile : Profile
{
    public MonthlyBudgetSummaryProfile()
    {
        CreateMap<MonthlyBudgetSummaryModel, MonthlyBudgetSummaryDto>()
            .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.Month))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
            .ForMember(dest => dest.IncomeSum, opt => opt.MapFrom(src => src.IncomeSum))
            .ForMember(dest => dest.ExpenseSum, opt => opt.MapFrom(src => src.ExpenseSum))
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance))
            .ForMember(dest => dest.CategorizedExpenses, opt => opt.MapFrom(src => src.CategorizedExpenses));
    }
}