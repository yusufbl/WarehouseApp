using DataServices.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<IWarehouse, WarehouseServices>();
builder.Services.AddSingleton<ICountry, CountryServices>();
builder.Services.AddSingleton<ICompany, CompanyServices>();
builder.Services.AddSingleton<ITaxOffice, TaxOfficeServices>();
builder.Services.AddSingleton<IHallway, HallwayServices>();
builder.Services.AddSingleton<IHallwayRow, HallwayRowServices>();
builder.Services.AddSingleton<IHallwayRowBin, HallwayRowBinServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
