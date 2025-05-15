using AutoMapper;
using BookPetGroomingAPI.Application.Common.Mappings;
using BookPetGroomingAPI.Domain.Entities;

namespace BookPetGroomingAPI.Application.Features.Sessions.Queries;

/// <summary>
/// Data Transfer Object for Session entity
/// </summary>
public class SessionDto : IMapFrom<Session>
{
    public int SessionId { get; set; }
    public int UserId { get; set; }
    public required string Token { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Session, SessionDto>();
    }
}