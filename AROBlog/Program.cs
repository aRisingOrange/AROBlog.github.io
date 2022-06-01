using AROBlog.Data.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//��������֤
builder.Services.AddAuthentication(b =>
{
    b.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    b.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    b.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(b =>
{
    b.LoginPath = "/User/Login";
    b.Cookie.Name = "loginAccount";
    b.Cookie.Path = "/";
    b.Cookie.HttpOnly = true;
    b.ExpireTimeSpan = TimeSpan.FromHours(5);
});
//Session ���浽�ڴ�
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    //// Set a short timeout for easy testing.
    //options.IdleTimeout = TimeSpan.FromSeconds(200);
    //options.Cookie.HttpOnly = true;
    //// Make the session cookie essential
    //options.Cookie.IsEssential = true;

    options.IOTimeout = TimeSpan.FromHours(1);
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.Name = "userid";
});
//�������ݿ�
builder.Services.AddDbContext<BlogContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon")));
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
//ʹ��session
app.UseSession();

app.UseStaticFiles();

app.UseRouting();
//��¼��֤
app.UseAuthentication();
//��Ȩ
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
