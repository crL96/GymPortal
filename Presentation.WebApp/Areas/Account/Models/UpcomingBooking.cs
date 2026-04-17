using System;

namespace Presentation.WebApp.Areas.Account.Models;

public sealed record UpcomingBooking
(
    Guid Id,
    SessionDetails Session
);

public sealed record SessionDetails
(
    Guid Id,
    string Name,
    DateTime StartTime,
    DateTime EndTime
);
