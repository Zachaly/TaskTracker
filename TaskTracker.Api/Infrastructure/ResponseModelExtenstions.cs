﻿using Microsoft.AspNetCore.Mvc;
using TaskTracker.Model.Response;

namespace TaskTracker.Api.Infrastructure
{
    public static class ResponseModelExtenstions
    {
        public static ActionResult<T> ReturnOkOrBadRequest<T>(this DataResponseModel<T> response)
        {
            if(!response.IsSuccess || response.Data is null)
            {
                return new BadRequestObjectResult(response);
            }

            return response.Data;
        }

        public static ActionResult ReturnCreatedOrBadRequest(this CreatedResponseModel response, string endpoint)
        {
            if (!response.IsSuccess)
            {
                return new BadRequestObjectResult(response);
            }

            return new CreatedResult($"{endpoint}/{response.NewEntityId}", response);
        }

        public static ActionResult ReturnNoContentOrBadRequest(this ResponseModel response)
        {
            if(!response.IsSuccess)
            {
                return new BadRequestObjectResult(response);
            }

            return new NoContentResult();
        }
    }
}
