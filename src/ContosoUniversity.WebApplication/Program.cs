using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

// this environmet is for docker
if (builder.Configuration["URLAPI"] != null)
{
    builder.Services.AddHttpClient("client", client => { client.BaseAddress = new Uri(builder.Configuration["URLAPI"]); });
}
else
{
    var section = builder.Configuration.GetSection("Api");
    builder.Services.AddHttpClient("client", client => { client.BaseAddress = new Uri(section["Address"]); });
}

builder.Services.AddRazorPages();
if (builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"] != null)
{
    builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);
}

var app = builder.Build();

//if (env.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}

app.UseExceptionHandler("/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});