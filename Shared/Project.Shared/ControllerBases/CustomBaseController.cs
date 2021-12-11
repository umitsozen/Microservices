using Microsoft.AspNetCore.Mvc;
using Project.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Shared.ControllerBases
{
    public class CustomBaseController: ControllerBase
    {
        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
