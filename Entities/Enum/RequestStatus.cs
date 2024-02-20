﻿public enum RequestStatus
{
    Unassigned = 1,
    Active = 2,
    Cancelled = 3,
    Reserving = 4,
    MDEnRoute = 5,
    MDOnSite = 6,
    FollowUp = 7,
    Closed = 8,
    Locked = 9,
    Declined = 10,
    Consult = 11,
    Clear = 12,
    CancelledByProvider = 13,
    CCUploadedByClient = 14,
    CCApprovedByAdmin = 15,
    Pending = 16,
}