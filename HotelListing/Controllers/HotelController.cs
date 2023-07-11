using AutoMapper;
using HotelListing.DTOs.HotelDTO;
using HotelListing.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HotelController> _logger;
    private readonly IMapper _mapper;

    public HotelController(IUnitOfWork unitOfWork, ILogger<HotelController> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var hotels = await _unitOfWork.HotelRepository.GetAll();
            var hotelsDTO = hotels.Select(h => _mapper.Map<GetHotelDTO>(h));
            return Ok(hotelsDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Some error happened in {nameof(GetAll)}");
            return StatusCode(500, "Please try again");
        }
        
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var hotel = await _unitOfWork.HotelRepository.Get(h => h.Id == id, new List<string> { "Country" });
            var hotelDTO = _mapper.Map<GetHotelDTO>(hotel);
            return Ok(hotelDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Some error happend in the {nameof(GetById)}");
            return StatusCode(404, "Hotel not found");
        }
    }

}
