/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Registration
{
    /// <summary>
    /// 
    /// </summary>
    [Route("Registration/RequestAccess")]
    public class RequestAccessController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("OidcCallback")]
        public JsonResult Callback()
        {
            return Json(new {});
        }
    }
}