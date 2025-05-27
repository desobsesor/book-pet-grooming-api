using AutoMapper;
using BookPetGroomingAPI.Application.Common.Mappings;
using BookPetGroomingAPI.Application.Features.Appointments.Queries;
using BookPetGroomingAPI.Domain.Entities;

namespace BookPetGroomingAPI.Application.Features.Notifications.Queries;

public class NotificationDto : IMapFrom<Notification>
{
    public int NotificationId { get; set; }
    public int? AppointmentId { get; set; }
    public string RecipientType { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public AppointmentDto Appointment { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Notification, NotificationDto>();
    }
}