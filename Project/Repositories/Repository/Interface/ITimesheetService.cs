using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface ITimesheetService
    {
        Task Approvetimesheet(int id);
        Task Finalizetimesheet(int id);
        Task<FinalizeTimesheetDTO> GetFinalizeTimesheetTable(string selectedvalue, int physicianid);
        Task<FinalizeTimesheetDTO> Gettimesheet(string selectedvalue, int physicianid);
        Task Posttimesheet(FinalizeTimesheetDTO viewmodel);
        Task timesheetadd(FinalizeTimesheetDTO viewmodel, Timesheet timesheet, bool exists);
    }
}