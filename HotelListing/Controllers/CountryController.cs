using AutoMapper;
using HotelListing.DTOs.CountryDTO;
using HotelListing.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CountryController> _logger;
    private readonly IMapper _mapper;

    public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
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
            var countries = await _unitOfWork.CountryRepository.GetAll();
            var countriesDTO = countries.Select(c => _mapper.Map<GetCountryDTO>(c));
            return Ok(countriesDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(GetAll)}");
            return StatusCode(500, "Internal server error. Please try again later.");
        }
    }

    [HttpGet("{id:int}", Name = "GetCountryById")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var country = await _unitOfWork.CountryRepository.Get(c => c.Id == id, new List<string> { "Hotels" });
            var countryDTO = _mapper.Map<GetCountryDTO>(country);
            return Ok(countryDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(GetById)}");
            return StatusCode(404, "Country not found. Please check your request and try again.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddCountryDTO addCountryDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST attempt in {nameof(Add)}");
            return BadRequest(ModelState);
        }

        try
        {
            var country = _mapper.Map<Country>(addCountryDTO);
            await _unitOfWork.CountryRepository.Insert(country);
            await _unitOfWork.Save();

            return CreatedAtRoute("GetCountryById", new { id = country.Id }, country);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(Add)}");

            return StatusCode(500, "Internal server error.Please try again later.");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCountryDTO updateCountryDTO)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid UPDATE attempt in {nameof(Update)}");
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _unitOfWork.CountryRepository.Get(c => c.Id == id);
            if (result is null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(Update)}");
                return BadRequest("Submitted data is invalid");
            }

            _mapper.Map(updateCountryDTO, result);
            _unitOfWork.CountryRepository.Update(result);
            await _unitOfWork.Save();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in {nameof(Update)}");
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
            var result = _unitOfWork.CountryRepository.Get(c => c.Id == id);
            if (result is null)
            {
                _logger.LogError($"Invalid DELETE attempt in the {nameof(Delete)} method");
                return BadRequest("Submitted data is invalid");
            }
            await _unitOfWork.CountryRepository.Delete(id);
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

