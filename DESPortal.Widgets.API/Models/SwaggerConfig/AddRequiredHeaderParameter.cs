using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DESPortal.Widgets.API.Models.SwaggerConfig
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            //if (operation.Parameters == null)
            //    operation.Parameters = new List<IParameter>();

            //operation.Parameters.Add(new NonBodyParameter
            //{
            //    Name = "UserId",
            //    In = "header",
            //    Type = "string",
            //    Required = true
            //});
        }
    }
}
