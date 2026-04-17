namespace Application.Dtos.Bookings;

public sealed record BookingResult(bool Succeeded, BookingDto? Booking = null, string? ErrorMessage = null)
{
    public static BookingResult Ok(BookingDto? booking = null) => new(true, booking);
    public static BookingResult Failed(string errorMessage) => new(false, null, errorMessage);
    public static BookingResult UserAlreadyBooked() => new(false, null, "User is already booked to session");
    public static BookingResult SessionFull() => new(false, null, "Session is full");
    public static BookingResult InvalidMembership() => new(false, null, "User does not have a valid membership");
    public static BookingResult NotFound(string? message = null) => new(false, null, message ?? "Booking not found");
}
