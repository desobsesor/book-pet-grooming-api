using AutoMapper;
using BookPetGroomingAPI.Application.Common.Mappings;
using BookPetGroomingAPI.Domain.Entities;

namespace BookPetGroomingAPI.Application.Features.Users.Queries;

public class UserDto : IMapFrom<User>
{
    public int UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public DateTime? LastLogin { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserDto>();
    }
}