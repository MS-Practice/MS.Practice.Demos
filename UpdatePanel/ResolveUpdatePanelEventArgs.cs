using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace UpdatePanelHelper
{
    public class ResolveUpdatePanelEventArgs : EventArgs
    {
        private string _ID = null;
        public string ID
        {
            get { return _ID; }
        }
        private System.Web.UI.UpdatePanel _UpdatePanel = null;
        public System.Web.UI.UpdatePanel UpdatePanel
        {
            get { return _UpdatePanel; }
            set { _UpdatePanel = value; }
        }
        public ResolveUpdatePanelEventArgs(string id) {
            this._ID = id;
        }
    }
}
