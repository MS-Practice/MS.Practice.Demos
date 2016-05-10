using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProgressBar.Commom.Enum;
using CommomCore;

namespace ProgressBar.Commom
{
    public static class AgeRangeExtensions
    {
        public static string GetText(this AgeRange range)
        {
            return range.GetAttachedData<string>(AgeRangeAttachData.Text);
        }
    }
}