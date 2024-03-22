public enum RequestStatus
{
    //new
    Unassigned = 1,

    //pending
    Pending = 16,

    //active
    Active = 2,
    MDEnRoute = 5,
    MDOnSite = 6,

    //conclude
    Conclude = 18,

    //to close
    Cancelled = 3,
    Closed = 8,
    CCApprovedByPatient = 21,

    //unpaid
    Unpaid = 19,


    Reserving = 4,
    FollowUp = 7,
    Locked = 9,
    Declined = 10,
    Consult = 11,
    Clear = 12,
    CancelledByProvider = 13,
    CCUploadedByClient = 14,
    CCApprovedByAdmin = 15,
    ToClosed = 17,
    Blocked = 20,


    //Request Status		 Dashboard Status

    //1. Unassigned			New
    //-------
    //2. Accepted			Pending
    //-------
    //4. MDEnRoute			Active
    //5. MDONSite			Active
    //-------
    //6. Conclude			Conclude
    //-------
    //3. Cancelled			To-close
    //7. CancelledByPatient	To-close
    //8. Closed				To-close
    //-------
    //9. Unpaid				Unpaid
    //-------
    //10. Clear				Will not show in dashboard

}