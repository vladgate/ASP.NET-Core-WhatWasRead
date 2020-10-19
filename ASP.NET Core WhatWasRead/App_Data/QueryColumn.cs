using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_WhatWasRead.App_Data
{
   public class QueryColumn
   {
      public string ColumnName { get; set; }
      public ComparatorType Comparator { get; set; }
      public QueryColumn(string columnName, ComparatorType comparator)
      {
         ColumnName = columnName;
         Comparator = comparator;
      }
   }
   public enum ComparatorType : byte
   {
      Equal,
      Between
   }

}
