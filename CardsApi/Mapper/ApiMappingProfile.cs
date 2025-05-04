using AutoMapper;
using CardsApi.Model.Dto;
using CardsApi.Model.Response;

namespace CardsApi.Mapper;

public class ApiMapperProfile : Profile
{
    public ApiMapperProfile()
    {
        CreateMap<ActionDto, ActionResponse>();
    }
}