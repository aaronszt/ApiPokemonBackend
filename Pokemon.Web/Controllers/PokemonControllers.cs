using Pokemon.Domain.QueryModel.Commands.Pokemons;
using Pokemon.Domain.QueryModel.Queries.Pokemons;
using Pokemon.Domain.QueryModel.Dtos;
using Microsoft.AspNetCore.Mvc;
using Pokemon.Domain.Entities;
using Pokemon.Web.Models;
using MediatR;

namespace Pokemon.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PokemonsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PokemonsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PokemonsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PokemonsDto>>> GetPokemons()
        {
            var result = await _mediator.Send(new PokemonsListQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PokemonsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PokemonsDto>> GetPokemon(int id)
        {
            var result = await _mediator.Send(new PokemonListQueryById(id));
            return Ok(result);
        }

        [HttpGet("name/{name}")]
        [ProducesResponseType(typeof(PokemonsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PokemonsDto>> GetByName(string name)
        {
            var result = await _mediator.Send(new GetPokemonByNameQuery(name));
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreatePokemon>> CreatePokemon([FromBody] CreatePokemon command)
        {
            await _mediator.Send(new CreatePokemonCommand(command));
            return NoContent();
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeletePokemon(int id)
        {
            await _mediator.Send(new DeletePokemonCommand(id));
            return NoContent();
        }

    }
}