using AutoMapper;

using Budget.Application.Dtos;
using Budget.Domain.Services;

namespace Budget.Application.Mappers;

public class CategorizedExpenseProfile : Profile
{
    public CategorizedExpenseProfile()
    {
        CreateMap<CategorizedExpenseModel, CategorizedExpenseDto>()
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.AmountSum, opt => opt.MapFrom(src => src.AmountSum));
    }
}