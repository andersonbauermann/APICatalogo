using APICatalogo.Models;
using AutoMapper;

namespace APICatalogo.DTOs.Mappings;

public class ProductDtoMappingProfile : Profile
{
    public ProductDtoMappingProfile()
    {
        CreateMap<Product, ProductDTO>().ReverseMap();
        CreateMap<Product, ProductDTOUpdateRequest>().ReverseMap();
        CreateMap<Product, ProductDTOUpdateResponse>().ReverseMap();
    }
}