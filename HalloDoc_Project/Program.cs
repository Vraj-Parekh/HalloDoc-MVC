using Entities.DataContext;
using HalloDoc.Utility;
using Repositories.Repository.Implementation;
using Repositories.Repository.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HalloDocDbContext>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<ISmsSender, SmsSender>();
builder.Services.AddTransient<IRequestServices, RequestServices>();
builder.Services.AddTransient<IRequestStatusLogServices, RequestStatusLogServices>();
builder.Services.AddTransient<IRequestClientServices, RequestClientServices>();
builder.Services.AddTransient<IRequestNotesServices, RequestNotesServices>();
builder.Services.AddTransient<IBlockRequestService, BlockRequestService>();
builder.Services.AddTransient<IRegionService, RegionService>();
builder.Services.AddTransient<IPhysicianService, PhysicianService>();
builder.Services.AddTransient<IRequestWiseFilesServices, RequestWiseFilesService>();
builder.Services.AddTransient<IHealthProfessionalTypeService, HealthProfessionalTypeService>();
builder.Services.AddTransient<IHealthProfessionalsService, HealthProfessionalsService>();
builder.Services.AddTransient<IOrderDetailsService, OrderDetailsService>();
builder.Services.AddTransient<IAspNetUserService, AspNetUserService>();
builder.Services.AddTransient<IEncounterFormService, EncounterFormService>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<IMenuService, MenuService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IRoleMenuService, RoleMenuService>();
builder.Services.AddTransient<IAspNetRoleService, AspNetRoleService>();
builder.Services.AddTransient<IAspNetUserRolesService, AspNetUserRolesService>();
builder.Services.AddTransient<IAdminRegionService, AdminRegionService>();
builder.Services.AddTransient<IPhysicianNotificationService, PhysicianNotificationService>();
builder.Services.AddTransient<IPhysicianRegionService, PhysicianRegionService>();
builder.Services.AddTransient<IShiftService, ShiftService>();
builder.Services.AddTransient<IShiftDetailService, ShiftDetailService>();
builder.Services.AddTransient<IShiftDetailRegionService, ShiftDetailRegionService>();
builder.Services.AddTransient<IEmailLogService, EmailLogService>();
builder.Services.AddTransient<ISmsLogService, SmsLogService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IHelperService, HelperService>();
builder.Services.AddTransient<IPhysicianLocationService, PhysicianLocationService>();
builder.Services.AddTransient<IHandleShiftService, HandleShiftService>();

builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
