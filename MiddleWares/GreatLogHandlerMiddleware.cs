using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EFWebApplicationWithAuthorization.MiddleWares
{
    public class GreatLogHandlerMiddleware
    {
        private RequestDelegate _next;
        public GreatLogHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            var fileName = @"C:\Users\Franek\Desktop\Programowanie\git\cwiczenia9_pgago-s20659\EFWebApplicationWithAuthorization\EFWebApplicationWithAuthorization\Logs\logs.txt";
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = 500;

                File.WriteAllText(fileName, ex.ToString());
                await httpContext.Response.WriteAsync("Unexpected error!");
            }
        }
    }
}
