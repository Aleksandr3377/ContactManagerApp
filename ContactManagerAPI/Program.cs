using ContactManagerAPI;
using ContactManagerBLL;
using ContactManagerDAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ContactDbContext>(options =>
    options.UseInMemoryDatabase("ContactsDb"));

builder.Services.AddAutoMapper(
_ => {},
typeof(MappingProfile).Assembly,
typeof(ApiMappingProfile).Assembly
);


builder.Services.AddDalLayer();
builder.Services.AddBllLayer();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();