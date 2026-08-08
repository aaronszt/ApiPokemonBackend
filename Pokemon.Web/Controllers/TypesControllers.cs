using Pokemon.Domain.QueryModel.Queries.Types;
using Pokemon.Domain.QueryModel.Dtos;
using Microsoft.AspNetCore.Mvc;
using Pokemon.Web.Models;
using MediatR;

namespace Pokemon.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("api/types")]
    public class TypesControllers : ControllerBase
    {
        private readonly IMediator _mediator;

        public TypesControllers(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PokemonTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PokemonTypeDto>>> GetTypes()
        {
            var result = await _mediator.Send(new TypesListQuery());
            return Ok(result);
        }
    }
}