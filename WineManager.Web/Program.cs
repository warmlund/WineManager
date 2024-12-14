#region Configure the web server host and services

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
var app = builder.Build();

#endregion

#region Configure the HTTP pipeline an routes

if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapRazorPages();
app.MapGet("/hello", () => $"Environment is {app.Environment.EnvironmentName}");

#endregion

//Start the web server, host the website, wait for requests
app.Run(); //Thread-blocking call
WriteLine("This executes after the webserver has stopped");
