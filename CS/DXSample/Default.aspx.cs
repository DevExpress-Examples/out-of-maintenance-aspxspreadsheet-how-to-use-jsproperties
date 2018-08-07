using DevExpress.Spreadsheet;
using DevExpress.Web.ASPxSpreadsheet;
using DevExpress.Web.Office;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXSample {
    public partial class Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }
        string SessionKey = "EditedDocuemntID";
        int MAX_LENGTH = 1000000;
        protected string EditedDocuemntID {
            get { return (string)Session[SessionKey] ?? string.Empty; }
            set { Session[SessionKey] = value; }
        }
        protected void Page_Init(object sender, EventArgs e) {
            if (!IsPostBack && !IsCallback) {
                if (!string.IsNullOrEmpty(EditedDocuemntID)) {
                    DocumentManager.CloseDocument(DocumentManager.FindDocument(EditedDocuemntID).DocumentId);
                    EditedDocuemntID = string.Empty;
                }
                EditedDocuemntID = Guid.NewGuid().ToString();
                DataView view = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                Spreadsheet.Open(
                    EditedDocuemntID,
                    DocumentFormat.Xlsx,
                    () => { return (byte[])view.Table.Rows[0]["DocumentContent"]; }
                );
            }
        }

        protected void SqlDataSource1_Updating(object sender, SqlDataSourceCommandEventArgs e) {
            e.Command.Parameters["@DocumentID"].Value = 1; // First Row
            byte[] docBytes = Spreadsheet.SaveCopy(DocumentFormat.Xlsx);
            if (docBytes.Length <= MAX_LENGTH)
                e.Command.Parameters["@DocumentContent"].Value = docBytes;
            else
                throw new Exception("The file exceeds the maximum file size");
        }

        protected void ASPxSpreadsheet1_Saving(object source, DocumentSavingEventArgs e) {
            ASPxSpreadsheet sh = source as ASPxSpreadsheet;
            try {
             //   SqlDataSource1.Update(); // uncomment to update the data source
                sh.JSProperties["cpSavingResult"] = "OK";
            }
            catch (Exception ex) {
                sh.JSProperties["cpSavingResult"] = "ERROR";
            }
            e.Handled = true;
        }
    }
}