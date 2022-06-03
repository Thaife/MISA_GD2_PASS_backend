using MISA.WEB02.GD2.Core.Interfaces.Infrastructure;
using MISA.WEB02.GD2.Core.Interfaces.Service;
using MISA.WEB02.GD2.Core.Service;
using MISA.WEB02.GD2.Infrastructure;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IAccountingRepository, AccountingRepository>();
builder.Services.AddScoped<IAccountingService, AccountingService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddScoped<IVendorGroupRepository, VendorGroupRepository>();
builder.Services.AddScoped<IVendorGroupAssistantRepository, VendorGroupAssistantRepository>();
//builder.Services.AddScoped<IVendorGroupAssistantService, VendorGroupAssistantService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();


// Thêm Newtonsoft JSON để sửa lỗi trả về của API
builder.Services.AddMvc().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});


var app = builder.Build();


//add cors
//tránh bị chặn
app.UseCors(opt => opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
