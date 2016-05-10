using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommomCore.dll
{
    public static class JQueryValidationExtensions
    {
        public static JQueryValidation validate(this JQueryHelper jquery)
        {
            //MVC
            //return jquery.validate("(default)");
            //AspNetPage
            return jquery.validate("(default)", "1");
        }
        public static JQueryValidation validate(this JQueryHelper jquery, string name)
        {
            var key = typeof(JQueryValidation) + "+" + name;
            var page = jquery.Page;
            var validate = page.Items[key] as JQueryValidation;

            if (validate == null)
            {
                page.Items[key] = validate = new JQueryValidation(page);
            }
            return validate;
        }
        //AspNetPage
        public static JQueryValidation validate(this JQueryHelper jquery, string name, string type) {
            var key = typeof(JQueryValidation) + "+" + name;
            var page = jquery.AspNetPage;
            var validate = page.Items[key] as JQueryValidation;

            if (validate == null)
            {
                page.Items[key] = validate = new JQueryValidation(page);
            }
            return validate;
        }
    }
}
