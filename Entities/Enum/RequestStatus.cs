public enum RequestStatus
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
    ToClosed = 17,
    Conclude = 18,
    Unpaid = 19,
    Blocked = 20,
    CCApprovedByPatient = 21,


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