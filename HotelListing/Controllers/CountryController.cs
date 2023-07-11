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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var country = await _unitOfWork.CountryRepository.Get(c => c.Id == id, new List<string>{ "Hotels" });
            var countryDTO = _mapper.Map<GetCountryDTO>(country);
            return Ok(countryDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(GetById)}");
            return StatusCode(404, "Country not found. Please check your request and try again.");
        }
    }
}
