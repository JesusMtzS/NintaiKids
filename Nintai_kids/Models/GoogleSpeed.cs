using Google.Apis.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Nintai_kids.Models
{
    internal enum GoogleSpeed
    {
        [Description("Muy lento")]
        [StringValue("x-slow")]
        xslow,
        [Description("Lento")]
        [StringValue("slow")]
        slow,
        [Description("Normal o moderado")]
        [StringValue("medium")]
        medium,
        [Description("Rapido")]
        [StringValue("fast")]
        fast,
        [Description("Muy rapido")]
        [StringValue("x-fast")]
        xfast
    }
}
