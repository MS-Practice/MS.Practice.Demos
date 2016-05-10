using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommomCore;

namespace ProgressBar.Commom.Enum
{
    public enum AgeRange
    {
        [AttachData(AgeRangeAttachData.Text,"18岁及以下")]
        LessThan18,
        [AttachData(AgeRangeAttachData.Text,"18岁至29岁")]
        From19To29,
        [AttachData(AgeRangeAttachData.Text,"30岁及以上")]
        Above29
    }
    public enum AgeRangeAttachData
    {
        Text
    }
}