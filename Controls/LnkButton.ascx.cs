using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRE.Controls {
    public partial class LnkButton : System.Web.UI.UserControl {
        #region Public Properties

        public string Text {
            get { return btnButton.Text; }
            set { btnButton.Text = value; }
        }

        public bool CausesValidation {
            get { return btnButton.CausesValidation; }
            set { btnButton.CausesValidation = value; }
        }

        public string ValidationGroup {
            get { return btnButton.ValidationGroup; }
            set { btnButton.ValidationGroup = value; }
        }

        public string PostBackUrl {
            get { return btnButton.PostBackUrl; }
            set { btnButton.PostBackUrl = value; }
        }

        public string OnClientClick {
            get { return btnButton.OnClientClick; }
            set { btnButton.OnClientClick = value; }
        }

        public string CssClass {
            get { return pnlLinkButton.CssClass; }
            set { if ((value + " ").Substring(0, 6).ToLower() != "button ") {
                    value = "button " + value;
                }
                pnlLinkButton.CssClass = value;
            }
        }

        private string _confirmText;

        public string ConfirmText{
            get { return _confirmText; }
            set {
                _confirmText = value;
                OnConfirmationChange();
            }
        }

        private bool _enableConfirmation = false;

        public bool EnableConfirmation {
            get { return _enableConfirmation; }
            set {
                _enableConfirmation = value;
                OnConfirmationChange();
            }
        }


        public event EventHandler Click;

        #endregion 

        protected void Page_Load(object sender, EventArgs e) {
            this.btnButton.Click += new System.EventHandler(this.LinkButton_Click);
            OnConfirmationChange();
        }

        protected void OnConfirmationChange() {
            if (string.IsNullOrEmpty(_confirmText)) {
                _confirmText = "Weet je het zeker? Druk op [OK] om door te gaan of op [Annuleren] om hier te blijven.";
            }
            if (_enableConfirmation) {
                btnButton.OnClientClick = String.Format("return confirm('{0}');", _confirmText);
            } else {
                btnButton.OnClientClick = "";
            }
        }

        protected void LinkButton_Click(object sender, EventArgs e) {
            if (Click != null) {
                Click(this, e);
            }
        }
    }
}