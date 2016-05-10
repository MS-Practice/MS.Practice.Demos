using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UpdatePanelHelper
{
    [PersistChildren(false)]
    [ParseChildren(true)]
    [NonVisualControl]
    public class JavaScriptUpdater : Control
    {
        private const string BasicScripts =
            @"if (!window.UpdatePanels) window.UpdatePanels = {};
UpdatePanels._createUpdateMethod = function(tbnId){
    return funtion(){
        _doPostBack(btnId,' ');    
    }
}";
        private const string RegisterMethodTemplate =
            "\nUpdatePanels['{0}'] = UpdatePanels._createUpdateMethod('{1}');";
        private string clientButtonId = null;
        private bool initialized = false;

        private bool _Enabled = true;
        public bool Enabeld
        {
            get { return this._Enabled; }
            set
            {
                if (initialized)
                {
                    throw new InvalidOperationException("Cannot set the property after initialized.");
                }
                this._Enabled = value;
            }
        }
        private string _MethodName;
        public string MethodName
        {
            get { return _MethodName; }
            set
            {
                if (initialized)
                {
                    throw new InvalidOperationException(
                        "Cannot set the property after initialized.");
                }
                this._MethodName = value;
            }
        }

        public event EventHandler<ResolveUpdatePanelEventArgs> ResolveUpdatePanel;
        private List<UpdatePanel> _UpdatePanels = new List<UpdatePanel>();
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<UpdatePanel> UpdatePanels
        {
            get { return _UpdatePanels; }
        }
        //我们使用一个initialized变量来确保Enabled或MethodName属性只能在页面Init时进行修改。由于控件会在多个生命周期中进行操作，如果不做这样的限制，会让控制变得繁琐，容易出错。从下面的代码中会发现，我们会在响应页面的InitComplete事件时将initialized变量设为true。
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Page.InitComplete += new EventHandler(Page_InitComplete);
        }
        //        首先会注册一些基础脚本，我们会使用相同的Type和Key参数，这样保证了这段代码只会被注册一次。在注册每个代理方法的脚本时，就会使用该脚本的clientButtonId作为key，保证了每段脚本都会被注册成功。顺便一提，我们在这里直接使用了Page的ClientScriptManager来注册脚本，保证了在异步更新UpdatePanel时，不会将脚本发送到客户端去。

        //可能有朋友会出现疑惑，为什么我们需要在页面的PreRenderComplete事件中注册脚本呢？在页面的Load事件中一并注册了不可以吗？答案是，因为ScriptManager也是在这时候注册ASP.NET AJAX的基础脚本，我们现在这么做是为了保证了我们注册的脚本出现在ASP.NET AJAX的脚本之后。

        //哦，原来如此……等一下，是否发觉我们现在的脚本与ASP.NET AJAX的基础脚本无关？没错，事实上我们这里的确可以方法页面的Load事件中注册，我现在这么做似乎只是一个习惯——或者说是为ASP.NET AJAX编写组件的一个模式——响应页面的PreRenderComplete事件，用于注册所需的脚本。
        private void Page_InitComplete(object sender, EventArgs e)
        {
            this.initialized = true;
            if (this.Enabeld)
            {
                this.Page.Load += new EventHandler(Page_Load);
                //在页面呈现前发生一个事件
                this.Page.PreRenderComplete += new EventHandler(Page_PreRenderComplete);
            }
        }

        private void Page_PreRenderComplete(object sender, EventArgs e)
        {
            this.Page.ClientScript.RegisterClientScriptBlock(GetType(), "BasicScripts", JavaScriptUpdater.BasicScripts, true);
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),this.clientButtonId,String.Format(JavaScriptUpdater.RegisterMethodTemplate,this.MethodName, this.clientButtonId),true);
        }
        //我们在页面的Page_InitComplete事件中判断Enabled属性是否为true（这时Enabled属性已经不能修改了），如果Enabled为ture，则响应页面的Load事件，用于动态添加一个LinkButton。请注意，我们并不会将它的Visible属性设为False，否则它的HTML将不会出现在页面上。我们应该将它Style的display设为none，这样它既能在页面结构中出现，也不会显示出来。在添加了这个LinkButton之后，将会保留它的ClientID，并且找出当前页面的ScriptManager，调用RegisterAsyncPostBackControl方法，将这个LinkButton注册为异步刷新页面的控件。
        private void Page_Load(object sender, EventArgs e)
        {
            LinkButton button = new LinkButton();
            button.Text = "Update";
            button.ID = this.ID + "Button";
            button.Style[HtmlTextWriterStyle.Display] = "none";
            button.Click += new EventHandler(OnTrigger);
            this.Page.Form.Controls.Add(button);
            this.clientButtonId = button.UniqueID;

            ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl(button);
            throw new NotImplementedException();
        }

        private void OnTrigger(object sender, EventArgs e)
        {
            if (this.Enabeld) {
                foreach (UpdatePanel panel in this.UpdatePanels)
                {
                    System.Web.UI.UpdatePanel updatepanel = this.FindUpdatePanel(panel.UpdatePanelID);
                }
            }
            throw new NotImplementedException();
        }

        private System.Web.UI.UpdatePanel FindUpdatePanel(string id)
        {
            System.Web.UI.UpdatePanel result = null;
            if (id != null)
            {
                result = this.NamingContainer.FindControl(id) as System.Web.UI.UpdatePanel;
            }
            if (result == null) {
                ResolveUpdatePanelEventArgs e = new ResolveUpdatePanelEventArgs(id);
                this.OnResolveUpdatePanel(e);
                result = e.UpdatePanel;
            }
            return result;
        }

        private void OnResolveUpdatePanel(ResolveUpdatePanelEventArgs e)
        {
            if (this.ResolveUpdatePanel != null)
            {
                this.ResolveUpdatePanel(this, e);
            }
        }
    }
}
