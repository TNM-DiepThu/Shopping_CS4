using Shopping_Application.IServices;
using Shopping_Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IProductServices, ProductServices>();//
//builder.Services.AddSingleton<IProductServices, ProductServices>();//
//builder.Services.AddScoped<IProductServices, ProductServices>();//
/*
 * Singleton: Services sẽ chỉ được tạo 1 lần trong suốt lifetime của ứng dụng.
 * Phù hợp cho các service có tính toàn cục và không thay đổi.
 * Scope: Mỗi 1 request sẽ khỏi tạo lại service 1 lần. Dùng cho các service có
 * tính chất đặc thù nào đó.
 * Transient: Được khỏi tạo mỗi khi có yêu cầu, Mỗi request sẽ được nhận 1 
 * services khác nhau. Và được sử dụng với services có nhiều yêu cầu http
 */
// Khai báo sử dụng Session với thời gian timeout là 30 giây
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(30);
});
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
app.UseStatusCodePagesWithReExecute("/Home/Index");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
