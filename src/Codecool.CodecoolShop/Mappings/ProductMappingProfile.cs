using System.Collections.Generic;
using AutoMapper;
using Codecool.CodecoolShop.Models.DTO;
using Codecool.CodecoolShop.Models.Products;

namespace Codecool.CodecoolShop.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<KeyValuePair<Product, int>, ProductDto>()
                .ForMember(x => x.Quantity, cfg => cfg.MapFrom(y => y.Value))
                .ForMember(x => x.Name, cfg => cfg.MapFrom(y => y.Key.Name))
                .ForMember(x => x.PricePerUnit, cfg => cfg.MapFrom(y => y.Key.DefaultPrice));
        }
    }
}
