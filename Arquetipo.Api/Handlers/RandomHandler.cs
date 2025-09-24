using Arquetipo.Api.Models.Response;
using Arquetipo.Api.Models.Response.Random;

namespace Arquetipo.Api.Handlers;

public class RandomHandler : IRandomHandler
{
    private readonly ILogger<RandomHandler> _logger;
    public RandomHandler(ILogger<RandomHandler> logger)
    {
        _logger = logger;
    }

    public async Task<ResponseDataExample> GeDataRandomDammy()
    {
        _logger.LogInformation("Ejecucion Handler Random GeDataRandomDammy");

        var response = new ResponseDataExample{ Data = new ResponseRandom() };
        var dataRandom = new ResponseRandom{
                                DatosPersonales = new PersonaRandom(),
                                SegurosAsociados = new List<SeguroRandom>()
                            };

        dataRandom.DatosPersonales = RandomHandlerMapper.GetPersonaRandom();
        dataRandom.SegurosAsociados = RandomHandlerMapper.GetSegurosRandomList();
        response.Data = dataRandom;

        await Task.Delay( TimeSpan.FromMicroseconds(10) );

        return response;
    }

}