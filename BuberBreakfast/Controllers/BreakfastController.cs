using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Entities;
using BuberBreakfast.ServiceErrors;
using BuberBreakfast.Services.Breakfasts;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreakfastController : ControllerBase
    {
        private readonly IBreakfastService _breakfastService;
        public BreakfastController(IBreakfastService breakfastService)
        {
            _breakfastService = breakfastService; ; 
        }


        [HttpPost()]
        public IActionResult CreateBreakfast(CreateBreakfastRequest request)
        {
            var breakfast = new Breakfast(
                Guid.NewGuid(),
                request.Name,
                request.Description,
                request.StartDateTime,
                request.EndDateTime,
                DateTime.UtcNow,
                request.Savory,
                request.Sweet
            );

            _breakfastService.CreateBreakfast(breakfast);

            BreakfastResponse response = MapBreakfastResponse(breakfast);

            return CreatedAtAction(
                nameof(GetBreakfast),
                new {id = response.Id}, 
                response
           );
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetBreakfast(Guid id)
        {
            ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);

            if (getBreakfastResult.IsError &&
                getBreakfastResult.FirstError == Errors.Breakfast.NotFound)
            {
                return NotFound();
            }

            var breakfast = getBreakfastResult.Value;

            BreakfastResponse response = MapBreakfastResponse(breakfast);

            return Ok(response);
        }

        private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
        {
            // Map the breakfast to the BreakfastResponse
            return new BreakfastResponse
                (
                    breakfast.Id,
                    breakfast.Name,
                    breakfast.Description,
                    breakfast.StartDateTime,
                    breakfast.EndDateTime,
                    breakfast.LastModifiedDateTime,
                    breakfast.Savory,
                    breakfast.Sweet
                );
        }

        [HttpGet()]
        public IActionResult GetBreakfast()
        {
            return Ok();
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
        {
            var breakfast = new Breakfast(
                id,
                request.Name,
                request.Description,    
                request.StartDateTime,
                request.EndDateTime,
                DateTime.UtcNow,
                request.Savory,
                request.Sweet
            );
                
           _breakfastService.UpsertBreakfast(breakfast);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteBreakfast(Guid id)
        {
            _breakfastService.DeleteBreakfast(id);

            return NoContent();
        }
    }
}
