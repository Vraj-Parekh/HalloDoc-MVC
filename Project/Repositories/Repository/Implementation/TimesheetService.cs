using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Repositories.Repository.Interface;


namespace Repositories.Repository.Implementation
{
    public class TimesheetService : ITimesheetService
    {
        private readonly HalloDocDbContext context;
        private readonly IPhysicianService physicianService;
        private readonly IAspNetUserService aspNetUserService;

        public TimesheetService(HalloDocDbContext context,
                                IPhysicianService physicianService,
                                IAspNetUserService aspNetUserService)
        {
            this.context = context;
            this.physicianService = physicianService;
            this.aspNetUserService = aspNetUserService;
        }

        public async Task<FinalizeTimesheetDTO> GetFinalizeTimesheetTable(string selectedvalue, int physicianid)
        {
            FinalizeTimesheetDTO list = await Gettimesheet(selectedvalue, physicianid);
            if (list.phytimesheet.Count == 0)
            {
                list.isfinalize = false;
            }
            if (list.issubmitted != true)
            {
                list.phytimesheet = new List<Finalizetimesheettime>();
            }
            return list;
        }

        public async Task<FinalizeTimesheetDTO> Gettimesheet(string selectedvalue, int physicianid)
        {
            FinalizeTimesheetDTO model = new FinalizeTimesheetDTO()
            {
                selectedvalue = selectedvalue,
            };
            string[] date = selectedvalue.Split('/');
            DateTime now = new DateTime(int.Parse(date[2]), int.Parse(date[1]), int.Parse(date[0]));
            if (physicianid == 0)
            {
                physicianid = physicianService.GetPhysicianIdByAspNetUserId(aspNetUserService.GetAspNetUserId());
            }
            model.physicianid = physicianid;
            if (date[0] == "01")
            {
                Timesheet timesheet = await context.Timesheets.FirstOrDefaultAsync(ts => ts.PhysicianId == physicianid && ts.StartDate == new DateTime(now.Year, now.Month, 1));
                if (timesheet != null)
                {
                    model.id = timesheet.TimesheetId;
                    model.phytimesheet = new List<Finalizetimesheettime>();
                    List<Finalizetimesheettime> times = new List<Finalizetimesheettime>();
                    List<Addreciepts> receipts = new List<Addreciepts>();
                    for (var i = 1; i <= 14; i++)
                    {
                        Finalizetimesheettime time = new Finalizetimesheettime();
                        Addreciepts receipt = new Addreciepts();
                        var detail = await context.TimesheetDetails.FirstOrDefaultAsync(ts => ts.TimesheetId == timesheet.TimesheetId && ts.Date == new DateTime(now.Year, now.Month, i));
                        //var receiptdetail = await context.TimesheetBills.FirstOrDefaultAsync(ts => ts.TimesheetDetailId == detail.TimesheetDetailId);
                        //receipt.date = DateOnly.FromDateTime(detail.Date);
                        //if (receiptdetail != null)
                        //{
                        //    receipt.Amount = receiptdetail.Amount.GetValueOrDefault();
                        //}
                        time.date = DateOnly.FromDateTime(detail.Date);
                        time.totalhours = (int)detail.TotalHours;
                        time.oncallhours = (int)detail.OnCallHours;
                        time.noofhousecalls = detail.NoOfHouseCall.GetValueOrDefault();
                        time.noofphoneconsult = detail.NoOfPhoneConsult.GetValueOrDefault();
                        time.holiday = detail.IsHoliday.GetValueOrDefault();
                        model.isfinalize = timesheet.IsFinalize.GetValueOrDefault();
                        model.issubmitted = timesheet.IsSubmitted.GetValueOrDefault();
                        model.isapproved = timesheet.IsApproved.GetValueOrDefault();
                        times.Add(time);
                        //receipts.Add(receipt);
                    }
                    model.phytimesheet = times;
                }
                else
                {
                    model.phytimesheet = new List<Finalizetimesheettime>();
                    List<Finalizetimesheettime> times = new List<Finalizetimesheettime>();
                    List<Addreciepts> receipts = new List<Addreciepts>();
                    for (var i = 1; i <= 14; i++)
                    {
                        double totalHours = 0;
                        Finalizetimesheettime time = new Finalizetimesheettime();
                        Addreciepts receipt = new Addreciepts();
                        var shifts = await context.Shiftdetails
                        .Include(sd => sd.Shift)
                        .Where(sd => sd.Shift.Physicianid == physicianid && sd.Shiftdate == new DateTime(now.Year, now.Month, i))
                            .ToListAsync();
                        time.date = DateOnly.FromDateTime(new DateTime(now.Year, now.Month, i).Date);
                        foreach (var shiftDetail in shifts)
                        {
                            TimeSpan duration = shiftDetail.Endtime - shiftDetail.Starttime;
                            totalHours += duration.TotalHours;
                        }
                        time.totalhours = (int)totalHours;
                        times.Add(time);
                        model.isfinalize = false;
                        model.issubmitted = false;
                        model.isapproved = false;
                        //receipts.Add(receipt);
                    }
                    model.phytimesheet = times;
                }
            }
            else
            {
                var lastDayOfMonth = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
                Timesheet timesheet = await context.Timesheets.FirstOrDefaultAsync(ts => ts.PhysicianId == physicianid && ts.StartDate == new DateTime(now.Year, now.Month, 15));
                if (timesheet != null)
                {
                    model.id = timesheet.TimesheetId;
                    model.phytimesheet = new List<Finalizetimesheettime>();
                    List<Finalizetimesheettime> times = new List<Finalizetimesheettime>();
                    List<Addreciepts> receipts = new List<Addreciepts>();
                    for (var i = 15; i <= lastDayOfMonth.Day; i++)
                    {
                        Finalizetimesheettime time = new Finalizetimesheettime();
                        Addreciepts receipt = new Addreciepts();
                        var detail = await context.TimesheetDetails.FirstOrDefaultAsync(ts => ts.TimesheetId == timesheet.TimesheetId && ts.Date == new DateTime(now.Year, now.Month, i));
                        //var receiptdetail = await context.TimesheetBills.FirstOrDefaultAsync(ts => ts.TimesheetDetailId == detail.TimesheetDetailId);
                        //receipt.date = DateOnly.FromDateTime(detail.Date);
                        //receipt.Amount = receiptdetail.Amount.GetValueOrDefault();
                        time.date = DateOnly.FromDateTime(detail.Date);
                        time.totalhours = (int)detail.TotalHours;
                        time.oncallhours = (int)detail.OnCallHours;
                        time.noofhousecalls = detail.NoOfHouseCall.GetValueOrDefault();
                        time.noofphoneconsult = detail.NoOfPhoneConsult.GetValueOrDefault();
                        time.holiday = detail.IsHoliday.GetValueOrDefault();
                        model.isfinalize = timesheet.IsFinalize.GetValueOrDefault();
                        model.issubmitted = timesheet.IsSubmitted.GetValueOrDefault();
                        model.isapproved = timesheet.IsApproved.GetValueOrDefault();
                        times.Add(time);
                        //receipts.Add(receipt);
                    }
                    model.phytimesheet = times;
                }
                else
                {
                    model.phytimesheet = new List<Finalizetimesheettime>();
                    List<Finalizetimesheettime> times = new List<Finalizetimesheettime>();
                    List<Addreciepts> receipts = new List<Addreciepts>();
                    for (var i = 15; i <= lastDayOfMonth.Day; i++)
                    {
                        double totalHours = 0;
                        Finalizetimesheettime time = new Finalizetimesheettime();
                        Addreciepts receipt = new Addreciepts();
                        var shifts = await context.Shiftdetails
                        .Include(sd => sd.Shift)
                        .Where(sd => sd.Shift.Physicianid == physicianid && sd.Shiftdate == new DateTime(now.Year, now.Month, i))
                            .ToListAsync();
                        time.date = DateOnly.FromDateTime(new DateTime(now.Year, now.Month, i).Date);
                        foreach (var shiftDetail in shifts)
                        {
                            TimeSpan duration = shiftDetail.Endtime - shiftDetail.Starttime;
                            totalHours += duration.TotalHours;
                        }
                        time.totalhours = (int)totalHours;
                        model.isfinalize = false;
                        model.issubmitted = false;
                        model.isapproved = false;
                        times.Add(time);
                        //receipts.Add(receipt);
                    }
                    model.phytimesheet = times;
                }
            }
            return model;
        }

        public async Task Posttimesheet(FinalizeTimesheetDTO viewmodel)
        {
            using (IDbContextTransaction? transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    string[] date = viewmodel.selectedvalue.Split('/');
                    DateTime now = new DateTime(int.Parse(date[2]), int.Parse(date[1]), int.Parse(date[0]));
                    Timesheet timesheet;
                    if (date[0] == "01")
                    {
                        timesheet = await context.Timesheets.FirstOrDefaultAsync(ts => ts.PhysicianId == viewmodel.physicianid && ts.StartDate == new DateTime(now.Year, now.Month, 1));
                        if (timesheet == null)
                        {
                            timesheet = new Timesheet();
                            timesheet.PhysicianId = viewmodel.physicianid;
                            timesheet.CreatedDate = now;
                            timesheet.StartDate = new DateTime(now.Year, now.Month, 1);
                            timesheet.IsSubmitted = true;
                            await context.Timesheets.AddAsync(timesheet);
                            await timesheetadd(viewmodel, timesheet, false);
                        }
                        else
                        {
                            timesheet.ModifiedDate = now;
                            await timesheetadd(viewmodel, timesheet, true);
                        }
                    }
                    else
                    {
                        timesheet = await context.Timesheets.FirstOrDefaultAsync(ts => ts.PhysicianId == viewmodel.physicianid && ts.StartDate == new DateTime(now.Year, now.Month, 15));
                        if (timesheet == null)
                        {
                            timesheet = new Timesheet();
                            timesheet.PhysicianId = viewmodel.physicianid;
                            timesheet.CreatedDate = now;
                            timesheet.StartDate = new DateTime(now.Year, now.Month, 15);
                            timesheet.IsSubmitted = true;
                            await context.Timesheets.AddAsync(timesheet);
                            await timesheetadd(viewmodel, timesheet, false);
                        }
                        else
                        {
                            timesheet.ModifiedDate = now;
                            await timesheetadd(viewmodel, timesheet, true);
                        }
                    }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message.ToString());
                }
            }
        }

        public async Task timesheetadd(FinalizeTimesheetDTO viewmodel, Timesheet timesheet, bool exists)
        {
            List<TimesheetDetail> updatedDetails = new List<TimesheetDetail>();
            string[] date = viewmodel.selectedvalue.Split('/');
            DateTime now = new DateTime(int.Parse(date[2]), int.Parse(date[1]), int.Parse(date[0]));
            if (exists != true)
            {
                var Date = timesheet.StartDate;
                foreach (var v in viewmodel.phytimesheet)
                {
                    TimesheetDetail detail = new TimesheetDetail()
                    {
                        OnCallHours = v.oncallhours,
                        IsHoliday = v.holiday,
                        NoOfHouseCall = v.noofhousecalls,
                        NoOfPhoneConsult = v.noofphoneconsult,
                        TotalHours = (int)v.totalhours,
                        Timesheet = timesheet,
                        CreatedDate = DateTime.Now,
                        Date = Date,
                    };
                    Date = Date.AddDays(1);
                    updatedDetails.Add(detail);
                }
                await context.TimesheetDetails.AddRangeAsync(updatedDetails);
            }
            else if (date[0] == "01")
            {
                var Date = timesheet.StartDate;
                foreach (var v in viewmodel.phytimesheet)
                {
                    TimesheetDetail detail = await context.TimesheetDetails.FirstOrDefaultAsync(ts => ts.TimesheetId == timesheet.TimesheetId && ts.Date == new DateTime(Date.Year, Date.Month, Date.Day));
                    if (detail != null)
                    {
                        detail.OnCallHours = v.oncallhours;
                        detail.IsHoliday = v.holiday;
                        detail.NoOfHouseCall = v.noofhousecalls;
                        detail.NoOfPhoneConsult = v.noofphoneconsult;
                        detail.TotalHours = (int)v.totalhours;
                        Date = Date.AddDays(1);
                    }
                }
            }
            else
            {
                var Date = timesheet.StartDate;
                foreach (var v in viewmodel.phytimesheet)
                {
                    TimesheetDetail detail = await context.TimesheetDetails.FirstOrDefaultAsync(ts => ts.TimesheetId == timesheet.TimesheetId && ts.Date == new DateTime(Date.Year, Date.Month, Date.Day));
                    if (detail != null)
                    {
                        detail.OnCallHours = v.oncallhours;
                        detail.IsHoliday = v.holiday;
                        detail.NoOfHouseCall = v.noofhousecalls;
                        detail.NoOfPhoneConsult = v.noofphoneconsult;
                        detail.TotalHours = (int)v.totalhours;
                        Date = Date.AddDays(1);
                    }
                }
            }
        }

        public async Task Finalizetimesheet(int id)
        {
            Timesheet timesheet = await context.Timesheets.FirstOrDefaultAsync(ts => ts.TimesheetId == id);
            if (timesheet != null)
            {
                timesheet.IsSubmitted = true;
                timesheet.IsFinalize = true;
            }
            await context.SaveChangesAsync();
        }

        public async Task Approvetimesheet(int id)
        {
            Timesheet timesheet = await context.Timesheets.FirstOrDefaultAsync(ts => ts.TimesheetId == id);
            if (timesheet != null)
            {
                timesheet.IsApproved = true;
            }
            await context.SaveChangesAsync();
        }
    }
}
