/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Core.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Devices
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [SecurityHeaders]
    [Route("api/Device")]
    public class DeviceController : Controller
    {
        
    }
}