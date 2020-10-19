﻿using ASP.NET_Core_WhatWasRead.App_Data;
using ASP.NET_Core_WhatWasRead.App_Data.DBModels;
using ASP.NET_Core_WhatWasRead.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASP.NET_Core_WhatWasRead.ViewComponents
{
   public class NoResultViewComponent : ViewComponent
   {
      public IViewComponentResult Invoke()
      {
         return View("_NoResultPartialView");
      }
   }
}