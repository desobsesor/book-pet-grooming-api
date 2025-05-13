using AutoMapper;
using BookPetGroomingAPI.Application.Common.Mappings;
using BookPetGroomingAPI.Domain.Entities;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries;

public class AppointmentDto : IMapFrom<Appointment>
{
    public int AppointmentId { get; set; }
    public int PetId { get; set; }
    public int GroomerId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public int EstimatedDuration { get; set; }
    public decimal Price { get; set; }
    public required string Status { get; set; }
    public required string Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Appointment, AppointmentDto>();
    }
}