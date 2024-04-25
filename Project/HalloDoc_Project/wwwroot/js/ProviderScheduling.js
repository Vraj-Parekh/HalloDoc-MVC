toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": false,
    "progressBar": true,
    "positionClass": "toast-bottom-right",
    "preventDuplicates": true,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

$(document).ready(function () {
    $('#openShiftModalBtn').click(function () {
        // Trigger the display of the modal
        $('#createShiftModal').modal('show');
    });
    var regionsFetched = false;

    if (!regionsFetched) {
        // Trigger the AJAX call for fetching regions
        $.ajax({
            url: '/Provider/FetchRegions',
            method: 'GET',
            success: function (response) {
                response.forEach(function (res) {
                    console.log(res);
                    $('.regionDropDown').append("<option value='" + res.regionId + "'>" + res.regionName + "</option>");
                });
                regionsFetched = true;
            }
        });
    }

    // Add the "Select Physician" option every time the modal is shown
    $('.physicianDropDown').empty();
    $('.physicianDropDown').append("<option value='' selected>Physician</option>");

    $('.regionDropDown').on('change', function () {
        var regionId = $(this).val();
        $.ajax({
            url: '/Admin/FetchPhysicianByRegion/' + regionId,
            method: 'GET',
            success: function (response) {
                $('.physicianDropDown').empty();
                $('.physicianDropDown').append("<option value='' selected> Physician</option>");
                response.forEach(function (res) {
                    console.log("calls phy");
                    $('.physicianDropDown').append("<option value='" + res.physicianid + "'>" + res.firstname + "</option>");
                });
            }
        });
    })
});

$(document).ready(function () {
    // Initially hide repeat days block
    $('#repeat_days_block').hide();

    // Show/hide the repeat days block based on the "Repeat" switch state
    $('#flexSwitchCheckChecked').change(function () {
        if ($(this).is(':checked')) {
            $('#repeat_days_block').show();
        } else {
            $('#repeat_days_block').hide();
        }
    });

    $(document).ready(function () {
        $('#ConfirmAssign').click(function () {

            var isValid = true;

            // Validate region selection
            if ($('#floatingSelect').val() == null) {
                $('#addShiftRegionError').text('Please select a region.');
                isValid = false;
            } else {
                $('#addShiftRegionError').text('');
            }

            // Validate start date
            if ($('#StartDate').val() == '') {
                $('#StartDate').addClass('is-invalid');
                isValid = false;
            } else {
                $('#StartDate').removeClass('is-invalid');
            }

            // Validate start time
            if ($('#StartTime').val() == '') {
                $('#startTimeError').text('Please enter a start time.');
                isValid = false;
            } else {
                $('#startTimeError').text('');
            }

            // Validate end time
            if ($('#EndTime').val() == '') {
                $('#endTimeError').text('Please enter an end time.');
                isValid = false;
            } else {
                $('#endTimeError').text('');
            }

            // Validate repeat end selection
            //if ($('#RepeatUpto').val() == null) {
            //    $('#RepeatUpto').addClass('is-invalid');
            //    isValid = false;
            //} else {
            //    $('#RepeatUpto').removeClass('is-invalid');
            //}

            if (isValid) {
                var createShiftDTO = {
                    RegionId: parseInt($('#floatingSelect').val()),
                    PhysicianId: $('.physicianDropDown').val(),
                    ShiftDate: $('#StartDate').val(),
                    StartTime: $('#StartTime').val(),
                    EndTime: $('#EndTime').val(),
                    Repeat: $('#flexSwitchCheckChecked').is(':checked'),
                    Repeat_Days: [],
                    RepeatUpto: parseInt($('#RepeatUpto').val())
                };

                $('input[name="Repeat_Days"]:checked').each(function () {
                    createShiftDTO.Repeat_Days.push(parseInt($(this).val()));
                });

                console.log(createShiftDTO);

                $.ajax({
                    url: '/Admin/CreateShift',
                    type: 'POST',
                    data: createShiftDTO,
                    //async: false,
                    dataType: 'text',
                    success: function (response) {
                        $('#createShiftModal').modal('show');
                        console.log(response);
                        toastr.success("Shift created successfully");
                        //calendar.removeAllEvents(); // Remove existing events
                        //calendar.addEventSource(events); // Add updated events
                        //calendar.refetchEvents(); // Refetch events from the event sources
                        //e.stopImmediatePropagation();
                        //getPhysicianShift(createShiftDTO.RegionId);
                    },
                    error: function (error) {
                        $('#createShiftModal').modal('hide');
                        console.log(error);
                        if (error.status === 400) {
                            toastr.error("Shift not created");
                            window.location.reload();
                        }
                        else {
                            toastr.success("Shift created successfully");
                            window.location.reload();
                        }
                        //getPhysicianShift(createShiftDTO.RegionId);
                        //toastr.error("Error Loading Reasonssdfvsdff");

                    }
                });
            } else {

            }

        });
    });
});

$(document).ready(function () {
    var calendarEl = document.getElementById('calendar');
    var RegionId = $('#stateDropdown').val();
    var calendar;
    console.log(RegionId)
    getPhysicianShift(RegionId);

    function getPhysicianShift(RegionId) {
        $.ajax({
            type: "POST",
            url: "/Admin/FetchPhysician",
            data: { regionId: RegionId },
            dataType: "json",
            success: function (response) {
                console.log(response);
                const resources = response.map(physician => ({
                    id: physician.physicianid,
                    title: physician.firstname

                }));
                $.ajax({
                    type: "GET",
                    url: "/Admin/FetchShiftDetails",
                    dataType: "json",
                    async: false,
                    success: function (response) {
                        console.log(response);
                        const events = response.map(event => ({
                            id: event.shiftid,
                            resourceId: event.shift.physicianid,
                            title: `${formatTime(event.starttime)} - ${formatTime(event.endtime)} / ${event.shift.physician.firstname}`,
                            start: event.shiftdate.substring(0, 11) + event.starttime,
                            end: event.shiftdate.substring(0, 11) + event.endtime,
                            eventBackgroundColor: event.status == 2 ? '#32d97d' : '#e39de8',
                            color: event.status == 2 ? '#32d97d' : '#e39de8',
                            ShiftDetailId: event.shiftdetailid,
                            region: event.regionid,
                            regionName: event.shift.physician.city,
                            status: event.status
                        }));

                        //console.log("adsfal;ksjdf;lsakj" + events);

                        var ResjsonString = JSON.stringify(events);

                        initializeCalendar(resources, events);//resources-->physicians, events-->shiftdetails

                    },
                    error: function (xhr) {
                        console.log(xhr.status);
                    }
                })
                function formatTime(time) {
                    const [hours, minutes] = time.split(':').map(Number);
                    const suffix = hours >= 12 ? 'PM' : 'AM';
                    const adjustedHours = hours % 12 || 12;
                    return `${adjustedHours}:${minutes < 10 ? '0' : ''}${minutes} ${suffix}`;
                }
            },
            error: function (xhr) {
                console.log(xhr.status);
            }
        })
    }

    function initializeCalendar(resources, events) {
        console.log(resources);
        console.log(events);
        $('#stateDropdown').on('change', function () {
            const newRegion = $(this).val();
            getPhysicianShift(newRegion);
        });
        console.log(resources)
        calendar = new FullCalendar.Calendar(calendarEl, {
            height: 'auto',
            schedulerLicenseKey: 'GPL-My-Project-Is-Open-Source',
            themeSystem: 'bootstrap5',
            headerToolbar: false,
            initialView: 'dayGridMonth',
            views: {
                resourceTimelineDay: {
                    buttonText: 'Day',
                    slotDuration: '01:00:00',
                    slotLabelContent: function (arg) {
                        return (arg.date.getHours() % 12 || 12) + (arg.date.getHours() < 12 ? 'AM' : 'PM');
                    },
                },
                resourceTimelineWeek: {
                    buttonText: 'Week',
                    slotDuration: { days: 1 },
                    slotLabelInterval: { days: 1 },
                    slotMinTime: '00:00:00',
                    slotMaxTime: '23:59:59',
                    slotLabelFormat: {
                        omitWeekday: false,
                        weekday: 'short',
                        day: '2-digit',
                        omitCommas: true,
                        meridiem: 'short'
                    },

                },
                dayGridMonth: {
                    buttonText: 'Month'
                }
            },
            datesSet: function (info) {

                var title = info.view.title;
                $("#date-title").html(title);
            },
            resources: resources,

            events: events,
            eventClick: function (info) {
                // Open the modal when an event is clicked
                $('#eventModal').modal('show');
                $('#eventModal').on('click', '#editbtn', function () {
                    // Hide the edit button
                    $('#editbtn').addClass('d-none');
                    // Show the save button
                    $('#savebtn').removeClass('d-none');
                    $('#StartDateView').prop('disabled', false);
                    $('#StartTimeView').prop('disabled', false);
                    $('#EndTimeView').prop('disabled', false);
                });

                $('#eventModal').on('click', '#returnshift', function (e) {
                    // Get the ShiftDetailId from the event
                    var shiftDetailId = $('#shiftDetailId').val(); // Assuming you have an input field with id 'shiftDetailId' in your modal
                    // Call AJAX to return
                    $.ajax({
                        type: 'POST',
                        url: '/Admin/ReturnShift',
                        data: { shiftDetailId: shiftDetailId },
                        async: false,
                        success: function (response) {

                            $('#eventModal').modal('hide');
                            console.log(response);
                            const events = response.map(event => ({
                                id: event.shiftid,
                                resourceId: event.shift.physicianid,
                                title: `${formatTime(event.starttime)} - ${formatTime(event.endtime)} / ${event.shift.physician.firstname}`,
                                start: event.shiftdate.substring(0, 11) + event.starttime,
                                end: event.shiftdate.substring(0, 11) + event.endtime,
                                eventBackgroundColor: event.status == 2 ? '#32d97d' : '#e39de8',
                                color: event.status == 2 ? '#32d97d' : '#e39de8',
                                ShiftDetailId: event.shiftdetailid,
                                region: event.regionid,
                                regionName: event.shift.physician.city,
                                status: event.status
                            }));
                            calendar.removeAllEvents(); // Remove existing events
                            calendar.addEventSource(events); // Add updated events
                            calendar.refetchEvents(); // Refetch events from the event sources
                            e.stopImmediatePropagation();
                        },
                        error: function (xhr, status, error) {
                            toastr.error("Something Went Wrong");
                        }
                    });
                    function formatTime(time) {
                        const [hours, minutes] = time.split(':').map(Number);
                        const suffix = hours >= 12 ? 'PM' : 'AM';
                        const adjustedHours = hours % 12 || 12;
                        return `${adjustedHours}:${minutes < 10 ? '0' : ''}${minutes} ${suffix}`;
                    }
                });

                $('#eventModal').on('click', '#deletebtn', function () {
                    // Get the ShiftId from the event
                    var shiftDetailId = $('#shiftDetailId').val();
                    $.ajax({
                        type: 'POST',
                        url: '/Admin/DeleteShift',
                        data: { shiftDetailId: shiftDetailId },
                        async: false,
                        success: function (response) {
                            $('#eventModal').modal('hide');

                            const events = response.map(event => ({
                                id: event.shiftid,
                                resourceId: event.shift.physicianid,
                                title: `${formatTime(event.starttime)} - ${formatTime(event.endtime)} / ${event.shift.physician.firstname}`,
                                start: event.shiftdate.substring(0, 11) + event.starttime,
                                end: event.shiftdate.substring(0, 11) + event.endtime,
                                eventBackgroundColor: event.status == 2 ? '#32d97d' : '#e39de8',
                                color: event.status == 2 ? '#32d97d' : '#e39de8',
                                ShiftDetailId: event.shiftdetailid,
                                region: event.regionid,
                                regionName: event.shift.physician.city,
                                status: event.status
                            }));

                            toastr.success("Shift deleted successfully");
                            calendar.removeAllEvents(); // Remove existing events
                            calendar.addEventSource(events); // Add updated events
                            calendar.refetchEvents(); // Refetch events from the event sources
                        },
                        error: function (xhr, status, error) {

                            toastr.success("error in Shift delete");
                        }
                    });
                    function formatTime(time) {
                        const [hours, minutes] = time.split(':').map(Number);
                        const suffix = hours >= 12 ? 'PM' : 'AM';
                        const adjustedHours = hours % 12 || 12;
                        return `${adjustedHours}:${minutes < 10 ? '0' : ''}${minutes} ${suffix}`;
                    }
                });

                $('#eventModal').on('click', '#savebtn', function () {
                    // Get data to be saved
                    var shiftDetailId = $('#shiftDetailId').val(); // Assuming you have an input field with id 'shiftDetailId' in your modal
                    var startDate = $('#StartDateView').val();
                    var startTime = $('#StartTimeView').val();
                    var endTime = $('#EndTimeView').val();
                    console.log(shiftDetailId);
                    // Call AJAX to save
                    $.ajax({
                        url: '/Admin/SaveShift',
                        type: 'POST',
                        data: {
                            ShiftDetailId: shiftDetailId,
                            Startdate: startDate,
                            Starttime: startTime,
                            Endtime: endTime
                        },
                        success: function (response) {
                            $('#eventModal').modal('hide');

                            const events = response.map(event => ({
                                id: event.shiftid,
                                resourceId: event.shift.physicianid,
                                title: `${formatTime(event.starttime)} - ${formatTime(event.endtime)} / ${event.shift.physician.firstname}`,
                                start: event.shiftdate.substring(0, 11) + event.starttime,
                                end: event.shiftdate.substring(0, 11) + event.endtime,
                                eventBackgroundColor: event.status == 2 ? '#32d97d' : '#e39de8',
                                color: event.status == 2 ? '#32d97d' : '#e39de8',
                                ShiftDetailId: event.shiftdetailid,
                                region: event.regionid,
                                regionName: event.shift.physician.city,
                                status: event.status
                            }));
                            calendar.removeAllEvents(); // Remove existing events
                            calendar.addEventSource(events); // Add updated events
                            calendar.refetchEvents(); // Refetch events from the event sources
                            toastr.success('Shift edited');
                            // Hide the save button and show the edit button
                            $('#savebtn').addClass('d-none');
                            $('#editbtn').removeClass('d-none');
                        },
                        error: function (xhr, status, error) {
                            toastr.error("Shift can't be edited");
                            $('#eventModal').modal('hide');
                        }
                    });
                    function formatTime(time) {
                        const [hours, minutes] = time.split(':').map(Number);
                        const suffix = hours >= 12 ? 'PM' : 'AM';
                        const adjustedHours = hours % 12 || 12;
                        return `${adjustedHours}:${minutes < 10 ? '0' : ''}${minutes} ${suffix}`;
                    }
                });
                // Get the ShiftDetailId from the event's extendedProps
                var shiftDetailId = info.event.extendedProps.ShiftDetailId;
                var region = info.event.extendedProps.region;
                var regionName = info.event.extendedProps.regionName;
                console.log(regionName)
                // Populate modal content with event details
                var event = info.event;
                var modalBody = document.querySelector('#eventModal .modal-body');
                var eventDetails = `
                                                    <form asp-action="viewshift">
                                                    <input class="d-none" id="shiftDetailId" value="${shiftDetailId}">
                                                       <div class="form-floating mb-3 mt-3">
                                                <select class="form-control" aria-label="Default select example" id="selectregion" disabled>
                                                        <option value="">${regionName}</option>
                                                </select>
                                                 <label class="form-label" for="selectregion">Region</label>
                                            </div>
                                                   
                                                                    <div class="col-md-12 form-floating mb-3">
                                                    <input id="StartDateView" class="form-control rounded vcs" name="Startdate" type="date" placeholder="Suchtext" autocomplete="off" value="${formatDate(event.start)}" disabled>
                                            <label for="StartDate">Shifted Date</label>
                                            <div class="d-flex gap-2 mt-3">
                                            <div class="col-md-6 form-floating mb-3">
                                                    <input id="StartTimeView" asp-for="Starttime" disabled class="form-control rounded vcs" name="Starttime" type="time" placeholder="Suchtext" autocomplete="off" value="${formatTime(event.start)}" >
                                            <label for="StartTime">Start</label>
                                        </div>
                                        <div class="col-md-6 form-floating mb-3">
                                                    <input id="EndTimeView" asp-for="Endtime" disabled	 class="form-control rounded vcs" name="Endtime" type="time" placeholder="Suchtext" autocomplete="off" value="${formatTime(event.end)}" >
                                            <label for="EndTime">End</label>
                                        </div>
                                        </div>
                                            <div class="d-flex justify-content-end gap-2">
                                        <button class="btn btn-info text-white" id="editbtn" type="button">Edit</button>
                                            <button class="btn btn-success text-white d-none" id="savebtn"  type="button">Save</button>
                                </div>
                                        </form>
                                                        `;
                modalBody.innerHTML = eventDetails;
            },
            eventBackgroundColor: function (event) {
                return event.eventBackgroundColor;
            },
            //resourceLabelDidMount: function (resourceObj) {
            //	const img = document.createElement('img');
            //	const imgUrl = resourceObj.resource.extendedProps.imageUrl || `/signatures/${resourceObj.el.dataset.photo}`;
            //	img.src = imgUrl;
            //	console.log(imgUrl);
            //	img.style.maxHeight = '40px';
            //	resourceObj.el.querySelector('.fc-datagrid-cell-main').appendChild(img);
            //},
            dateClick: function (info) {
                var shiftedDateISO = info.dateStr;
                var shiftedDate = shiftedDateISO.split('T')[0];
                var startTime = new Date(info.date).toISOString().substr(11, 5);
                console.log("clicked");
                $('#StartDate').val(shiftedDate);
                $('#StartTime').val(startTime);

                // Open the create shift modal
                $('#createShiftModal').modal('show');
            },

        });
        console.log(calendar.events);
        calendar.on('datesSet', function (info) {
            var date = info.view.currentStart;
            var formattedDate = formatDate(date);
            $("#date-title").val(formattedDate);
        });
        function showDate() {
            var view = calendar.view;
            var date = view.title;
            var formattedDate = new Date(date).toLocaleDateString(undefined, {
                weekday: 'long',
                month: 'short',
                day: 'numeric',
                year: 'numeric'
            });
            document.getElementById('header_date').innerHTML = formattedDate;
        }


        $("#next-button").click(function () {
            calendar.next();
            showDate();
        });

        $("#prev-button").click(function () {
            calendar.prev();
            showDate();
        });

        $("#day-button").click(function () {
            calendar.changeView('resourceTimelineDay');
        });

        $("#week-button").click(function () {
            calendar.changeView('resourceTimelineWeek');
        });

        $("#month-button").click(function () {
            calendar.changeView('dayGridMonth');
        });

        calendar.render();
    }

    // Function to format date to YYYY-MM-DD
    function formatDate(dateString) {
        const date = new Date(dateString);
        const year = date.getFullYear();
        let month = (date.getMonth() + 1).toString().padStart(2, '0');
        let day = date.getDate().toString().padStart(2, '0');
        return `${year}-${month}-${day}`;
    }

    // Function to format time to HH:mm
    function formatTime(dateString) {
        const date = new Date(dateString);
        let hours = date.getHours().toString().padStart(2, '0');
        let minutes = date.getMinutes().toString().padStart(2, '0');
        return `${hours}:${minutes}`;
    }
});


