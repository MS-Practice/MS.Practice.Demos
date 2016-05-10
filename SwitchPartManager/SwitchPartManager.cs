using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace SwitchPartManager
{
    public class SwitchPartManager:Control
    {
        private const string HiddenElementName = "__PartType__";

        private bool initialized = false;
        private string partTypeToSave = null;
        public static SwitchPartManager GetCurrent(Page page) {
            return page.Items[typeof(SwitchPartManager)] as SwitchPartManager;
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.Page.Items.Contains(typeof(SwitchPartManager))) {
                throw new InvalidOperationException("One SwitchPartManager per page.");
            }

            this.Page.Items[typeof(SwitchPartManager)] = this;
            this.Page.InitComplete += new EventHandler(Page_InitComplete);
            this.Page.PreRenderComplete += new EventHandler(Page_PreRenderComplete);
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(partTypeToSave)) {
                ScriptManager.RegisterHiddenField(this.Page, SwitchPartManager.HiddenElementName, this.partTypeToSave);
            }
        }

        void Page_InitComplete(object sender, EventArgs e)
        {
            this.initialized = true;
            string partType = this.Page.Request.Params[SwitchPartManager.HiddenElementName];
            if (partType != null) {
                this.SwichTo(partType);
            }
        }
        public void SwichTo(string partType)
        {
            Control container = this.PlaceHoderUpdatePanel.ContentTemplateContainer;
            container.Controls.Clear();

            Control control = this.Page.LoadControl(partType + ".ascx");
            container.ID = "JustToPreserveUniqueName";
            container.Controls.Add(control);
            this.partTypeToSave = partType;
        }

        public UpdatePanel PlaceHoderUpdatePanel { get; set; }
    }
}
