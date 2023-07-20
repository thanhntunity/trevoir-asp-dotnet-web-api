using AutoMapper;
using HotelListing.DTOs.HotelDTO;
using HotelListing.IRepository;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("{id:int}", Name = "GetHotelById")]
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

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddHotelDTO addHotelDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST attempt in {nameof(Add)}");
            return BadRequest(ModelState);
        }

        try
        {
            var hotel = _mapper.Map<Hotel>(addHotelDTO);
            await _unitOfWork.HotelRepository.Insert(hotel);
            await _unitOfWork.Save();

            return CreatedAtRoute("GetHotelById", new { id = hotel.Id }, hotel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(Add)}");
            return StatusCode(500, "Internal server error.Please try again later.");
        }
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateHotelDTO updateHotelDTO)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid UPDATE attempt in {nameof(Update)}");
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _unitOfWork.HotelRepository.Get(h => h.Id == id);
            if (result is null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(Update)} method");
                return BadRequest("Submitted data is invalid");
            }
            _mapper.Map(updateHotelDTO, result);
            _unitOfWork.HotelRepository.Update(result);
            await _unitOfWork.Save();

            return NoContent();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(Update)}");
            return StatusCode(500, "Internal server error. Please try again later.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id < 1)
        {
            _logger.LogError($"Invalid DELETE attempt in the {nameof(Delete)} method");
            return BadRequest();
        }
        try
        {
            var result = await _unitOfWork.HotelRepository.Get(h => h.Id == id);
            if (result is null)
            {
                _logger.LogError($"Invalid DELETE attempt in the {nameof(Delete)} method");
                return BadRequest("Submitted data is invalid");
            }
            await _unitOfWork.HotelRepository.Delete(id);
            await _unitOfWork.Save();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid DELETE attempt in the {nameof(Delete)} method");
            return StatusCode(500, "Internal server error. Please try again later.");
        }
    }   
}
