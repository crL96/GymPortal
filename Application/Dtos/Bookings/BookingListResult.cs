namespace Application.Dtos.Bookings;

public sealed record BookingListResult(bool Succeeded, List<BookingDto>? Bookings, string? ErrorMessage = null)
{
    public static BookingListResult Ok(List<BookingDto>? bookings) => new(true, bookings);
    public static BookingListResult Failed(string errorMessage) => new(false, null, errorMessage);
}