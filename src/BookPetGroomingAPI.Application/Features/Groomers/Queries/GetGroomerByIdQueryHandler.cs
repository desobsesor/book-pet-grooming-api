using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Groomers.Queries
{
    /// <summary>
    /// Handler for processing the GetGroomerByIdQuery and returning the groomer details
    /// </summary>
    public class GetGroomerByIdQueryHandler(IGroomerRepository groomerRepository, IMapper mapper) : IRequestHandler<GetGroomerByIdQuery, GroomerDto>
    {
        public async Task<GroomerDto> Handle(GetGroomerByIdQuery request, CancellationToken cancellationToken)
        {
            var groomer = await groomerRepository.GetByIdAsync(request.GroomerId);
            return mapper.Map<GroomerDto>(groomer);
        }
    }
}