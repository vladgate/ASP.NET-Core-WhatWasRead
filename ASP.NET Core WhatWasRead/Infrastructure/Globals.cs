using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_WhatWasRead.Infrastructure
{
   public static class Globals
   {
      public const int ITEMS_PER_BOOKROW = 2;
      public const int ITEMS_PER_PAGE = 4;
      public const string CONNECTION_NAME_CONFIG = "ConnectionStrings:WhatWasReadLocalSqlServer";
   }
}
