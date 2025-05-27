using AutoMapper;
using BookPetGroomingAPI.Application.Common.Mappings;
using BookPetGroomingAPI.Domain.Entities;

namespace BookPetGroomingAPI.Application.Features.Users.Queries;

public class UserLightDto : IMapFrom<User>
{
    public int UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserLightDto>();
    }
}