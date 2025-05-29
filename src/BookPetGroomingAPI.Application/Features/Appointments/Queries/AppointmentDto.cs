using AutoMapper;
using BookPetGroomingAPI.Application.Common.Mappings;
using BookPetGroomingAPI.Application.Features.Groomers.Queries;
using BookPetGroomingAPI.Application.Features.Pets.Queries;
using BookPetGroomingAPI.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries;

public class AppointmentDto : IMapFrom<Appointment>
{
    [SwaggerSchema(Description = "Unique identifier for the appointment.")]
    public int AppointmentId { get; set; }
    [SwaggerSchema(Description = "Unique identifier for the pet.")]
    public int PetId { get; set; }
    [SwaggerSchema(Description = "Pet details for the appointment.")]
    public PetDto Pet { get; set; } = null!;
    [SwaggerSchema(Description = "Groomer details for the appointment.")]
    public GroomerDto Groomer { get; set; } = null!;
    [SwaggerSchema(Description = "Unique identifier for the groomer.")]
    public int GroomerId { get; set; }
    [SwaggerSchema(Description = "Date of the appointment.")]
    public DateTime AppointmentDate { get; set; }
    [SwaggerSchema(Description = "Start time of the appointment.")]
    public TimeOnly StartTime { get; set; }
    [SwaggerSchema(Description = "Estimated duration of the appointment in minutes.")]
    public int EstimatedDuration { get; set; }
    [SwaggerSchema(Description = "Price of the appointment.")]
    public decimal Price { get; set; }
    [SwaggerSchema(Description = "Status of the appointment. Possible values: completed, approved, cancelled, pending.")]
    public string Status { get; set; } = string.Empty;
    [SwaggerSchema(Description = "Additional notes for the appointment.")]
    public string Notes { get; set; } = string.Empty;
    [SwaggerSchema(Description = "Date and time when the appointment was created.")]
    public DateTime CreatedAt { get; set; }
    [SwaggerSchema(Description = "Date and time when the appointment was last updated.")]
    public DateTime UpdatedAt { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Appointment, AppointmentDto>();
    }
}