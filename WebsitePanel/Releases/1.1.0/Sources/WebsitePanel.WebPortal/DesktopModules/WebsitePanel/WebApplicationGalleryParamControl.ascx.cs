using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsitePanel.Providers.WebAppGallery;

namespace WebsitePanel.Portal
{
    public partial class WebApplicationGalleryParamControl : WebsitePanelModuleBase
    {
        #region Constants
        public const string DatabaseIdentifierRegexp = "^[a-zA-Z_]+[a-zA-Z0-9_]{0,63}$";
        #endregion

        #region Private Properties
        DeploymentParameterWellKnownTag wellKnownTags
        {
            get { return ViewState["WellKnownTags"] != null ? (DeploymentParameterWellKnownTag)ViewState["WellKnownTags"] : DeploymentParameterWellKnownTag.None; }
            set { ViewState["WellKnownTags"] = value; }
        }

        DeploymentParameterValidationKind validationKind
        {
            get { return ViewState["ValidationKind"] != null ? (DeploymentParameterValidationKind)ViewState["ValidationKind"] : DeploymentParameterValidationKind.None; }
            set { ViewState["ValidationKind"] = value; }
        }

        string validationString
        {
            get { return ViewState["ValidationString"] != null ? (string)ViewState["ValidationString"] : null; }
            set { ViewState["ValidationString"] = value; }
        }
        #endregion

        #region Public Properties
        public string Name
        {
            get { return ViewState["Name"] != null ? (string)ViewState["Name"] : null; }
            private set { ViewState["Name"] = value; }
        }

        public string FriendlyName
        {
            get { return friendlyName.Text; }
            private set { friendlyName.Text = value; }
        }

        public string Description
        {
            get { return description.Text; }
            private set { description.Text = value; }
        }

        public string DefaultValue
        {
            get { return ViewState["DefaultValue"] != null ? (string)ViewState["DefaultValue"] : null; }
            private set { ViewState["DefaultValue"] = value; }
        }

        public DeploymentParameterWellKnownTag WellKnownTags
        {
            get { return this.wellKnownTags; }
            set { wellKnownTags = value; BindControls(); }
        }

        public DeploymentParameterValidationKind ValidationKind
        {
            get { return this.validationKind; }
            set { validationKind = value; BindControls(); }
        }

        public string ValidationString
        {
            get { return this.validationString; }
            set { validationString = value; BindControls(); }
        }

        public string Value
        {
            get { return null; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindParameter(DeploymentParameter param)
        {
            // store parameter details
            this.Name = param.Name;
            this.FriendlyName = param.FriendlyName;
            this.Description = param.Description;
            this.DefaultValue = param.DefaultValue;
            this.wellKnownTags = param.WellKnownTags;
            this.validationKind = param.ValidationKind;
            this.validationString = param.ValidationString;

            // toggle controls
            BindControls();
        }

        public DeploymentParameter GetParameter()
        {
            DeploymentParameter parameter = new DeploymentParameter();
            parameter.Name = this.Name;
            parameter.FriendlyName = this.FriendlyName;
            parameter.Description = this.Description;
            parameter.Value = GetParameterValue();
            parameter.DefaultValue = this.DefaultValue;
            parameter.WellKnownTags = this.WellKnownTags;
            parameter.ValidationKind = this.ValidationKind;
            parameter.ValidationString = this.ValidationString;
            return parameter;
        }

        private string GetParameterValue()
        {
            if (PasswordControl.Visible)
                return password.Text;
            else if (TextControl.Visible)
                return textValue.Text.Trim();
            else if (BooleanControl.Visible)
                return boolValue.Checked.ToString();
            else if (EnumControl.Visible)
                return enumValue.SelectedValue;
            else
                return null;
        }

        private void BindControls()
        {
            // hide database server parameters
            DeploymentParameterWellKnownTag hiddenTags =
                DeploymentParameterWellKnownTag.IisApp |
                DeploymentParameterWellKnownTag.Hidden |
                DeploymentParameterWellKnownTag.DBServer |
                DeploymentParameterWellKnownTag.DBAdminUserName |
                DeploymentParameterWellKnownTag.DBAdminPassword;

            if ((WellKnownTags & hiddenTags) > 0)
            {
                this.Visible = false;
                return;
            }

            // disable all editor controls
            BooleanControl.Visible = false;
            EnumControl.Visible = false;
            PasswordControl.Visible = false;
            TextControl.Visible = false;

            // enable specific control
            if ((ValidationKind & DeploymentParameterValidationKind.Boolean) == DeploymentParameterValidationKind.Boolean)
            {
                // Boolean value
                BooleanControl.Visible = true;
                bool val = false;
                Boolean.TryParse(DefaultValue, out val);
                boolValue.Checked = val;
            }
            else if ((ValidationKind & DeploymentParameterValidationKind.Enumeration) == DeploymentParameterValidationKind.Enumeration)
            {
                // Enumeration value
                EnumControl.Visible = true;

                // fill dropdown
                enumValue.Items.Clear();
                string[] items = (ValidationString ?? "").Trim().Split(',');
                foreach (string item in items)
                    enumValue.Items.Add(item.Trim());

                // select default value
                enumValue.SelectedValue = DefaultValue;
            }
            else if ((WellKnownTags & DeploymentParameterWellKnownTag.Password) == DeploymentParameterWellKnownTag.Password)
            {
                // Password value
                PasswordControl.Visible = true;
                confirmPasswordControls.Visible = ((WellKnownTags & DeploymentParameterWellKnownTag.New) == DeploymentParameterWellKnownTag.New);
            }
            else
            {
                // Text value
                TextControl.Visible = true;
                textValue.Text = DefaultValue;
            }

            
            // enforce validation for database parameters if they are allowed empty by app pack developers
            bool isDatabaseParameter = (WellKnownTags & (
                DeploymentParameterWellKnownTag.DBName |
                DeploymentParameterWellKnownTag.DBUserName |
                DeploymentParameterWellKnownTag.DBUserPassword)) > 0;

            // enforce validation for database name and username
            if ((WellKnownTags & (DeploymentParameterWellKnownTag.DBName | DeploymentParameterWellKnownTag.DBUserName)) > 0
                    && String.IsNullOrEmpty(ValidationString))
            {
                validationKind |= DeploymentParameterValidationKind.RegularExpression;
                validationString = DatabaseIdentifierRegexp;
            }

            // validation common for all editors
            requireTextValue.Enabled = requirePassword.Enabled = requireConfirmPassword.Enabled = requireEnumValue.Enabled =
                ((ValidationKind & DeploymentParameterValidationKind.AllowEmpty) != DeploymentParameterValidationKind.AllowEmpty) || isDatabaseParameter;

            requireTextValue.Text = requirePassword.Text = requireEnumValue.Text =
                String.Format(GetLocalizedString("RequiredValidator.Text"), FriendlyName);

            regexpTextValue.Enabled = regexpPassword.Enabled = regexpEnumValue.Enabled =
                (ValidationKind & DeploymentParameterValidationKind.RegularExpression) == DeploymentParameterValidationKind.RegularExpression;

            regexpTextValue.ValidationExpression = regexpPassword.ValidationExpression = regexpEnumValue.ValidationExpression =
                ValidationString ?? "";

            regexpTextValue.Text = regexpPassword.Text = regexpEnumValue.Text =
                String.Format(GetLocalizedString("RegexpValidator.Text"), FriendlyName, ValidationString);


        }
    }
}