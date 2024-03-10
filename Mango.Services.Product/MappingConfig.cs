using AutoMapper;

using Mango.Services.ProductsAPI.Models;
using Mango.Services.ProductsAPI.Models.Dtos;

namespace Mango.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Mango.Services.ProductAPI.Models.Product>();
                config.CreateMap<Mango.Services.ProductAPI.Models.Product, ProductDto>();
            });
            return mappingConfig;
        }


    }
}
