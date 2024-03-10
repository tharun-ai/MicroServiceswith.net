
using AutoMapper;
using Mango.Services.ProductAPI.Models.Dto;
using Mango.Services.ProductsAPI.Data;
using Mango.Services.ProductsAPI.Models;
using Mango.Services.ProductsAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;
        public ProductApiController(AppDbContext db,IMapper mapper) {
          _db=db;
            _mapper = mapper;
            _response =new ResponseDto();
           
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Product> objList=_db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
               
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Product objList= _db.Products.First(u=>u.ProductId==id);
                Console.WriteLine(objList);
                _response.Result = _mapper.Map<ProductDto>(objList);
            }
            catch (Exception ex) { 
              _response.IsSuccess= false;
                _response.Message=ex.Message;
            }
            return _response;
        }

        

        [HttpPost]
        [Authorize(Roles ="ADMIN")]
        public ResponseDto Post([FromBody]ProductDto ProductDto) 
        {
            try
            {
                Product obj=_mapper.Map<Product>(ProductDto);
                _db.Products.Add(obj);
                _db.SaveChanges();
                _response.Result=_mapper.Map<ProductDto>(obj);
            }
            catch(Exception ex)
            {
                _response.IsSuccess= false;
                _response.Message=ex.Message;
            }
            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto put([FromBody] ProductDto ProductDto)
        {
            try
            {
                Product obj = _mapper.Map<Product>(ProductDto);
                _db.Products.Update(obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {   
                Product obj = _db.Products.First(u=>u.ProductId == id);
                _db.Products.Remove(obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
