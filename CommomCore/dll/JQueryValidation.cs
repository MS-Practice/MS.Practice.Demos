using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace CommomCore.dll
{
    public class JQueryValidation
    {
        /// <summary>
        /// 规则描述
        /// </summary>
        private Dictionary<string, Dictionary<string, object>> m_rules =
        new Dictionary<string, Dictionary<string, object>>();
        /// <summary>
        /// 错误信息
        /// </summary>
        private Dictionary<string, Dictionary<string, string>> m_messages =
            new Dictionary<string, Dictionary<string, string>>();

        private void AddRuleAndMessage(string name, string rule, object value, string message)
        {
            if (!this.m_rules.ContainsKey(name))
                this.m_rules[name] = new Dictionary<string, object>();
            this.m_rules[name][rule] = value;

            if (!String.IsNullOrEmpty(message))
            {
                if (!this.m_messages.ContainsKey(name))
                    this.m_messages[name] = new Dictionary<string, string>();
                this.m_messages[name][rule] = message;
            }
        }
        public void Required(string name, string message)
        {
            this.AddRuleAndMessage(name, "required", true, message);
        }

        public void Email(string name, string message)
        {
            this.AddRuleAndMessage(name, "email", true, message);
        }

        public void Number(string name, string message)
        {
            this.AddRuleAndMessage(name, "number", true, message);
        }

        public void Range(string name, int min, int max, string message)
        {
            this.AddRuleAndMessage(name, "range", new int[] { min, max }, message);
        }

        public JQueryValidation(ViewPage page)
        {
            this.Page = page;
        }

        //AspNetPage
        public JQueryValidation(Page page)
        {
            this.AspNetPage = page;
        }

        public string ToScripts(string form)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string strrules = serializer.Serialize(m_rules);
            string strmessage = serializer.Serialize(m_messages);
            StringBuilder builder = new StringBuilder();
            builder.Append("$(");
            serializer.Serialize(form, builder);
            builder.Append(").validate(");
            serializer.Serialize(
                new
                {
                    rule = this.m_rules,
                    message = this.m_messages,
                    onkeyup = false
                }, builder
                );
            builder.Append(");");
            return builder.ToString();
        }
        public ViewPage Page { get; private set; }

        public System.Web.UI.Page AspNetPage { get; private set; }

        public ValidationElement Element(string name)
        {
            return new ValidationElement(name, this);
        }
        public class ValidationElement
        {
            internal ValidationElement(string name, JQueryValidation validation)
            {
                this.Name = name;
                this.Validation = validation;
            }

            //在内部验证 调用JQueryValidation中的方法
            public ValidationElement Required(string message)
            {
                this.Validation.Required(this.Name, message);
                return this;
            }
            public ValidationElement Email(string message)
            {
                this.Validation.Required(this.Name, message);
                return this;
            }

            public string Name { get; private set; }

            public JQueryValidation Validation { get; private set; }
        }
    }
}
