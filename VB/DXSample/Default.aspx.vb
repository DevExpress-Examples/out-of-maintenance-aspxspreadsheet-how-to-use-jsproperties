Imports DevExpress.Spreadsheet
Imports DevExpress.Web.ASPxSpreadsheet
Imports DevExpress.Web.Office
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace DXSample
    Partial Public Class [Default]
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)

        End Sub
        Private SessionKey As String = "EditedDocuemntID"
        Private MAX_LENGTH As Integer = 1000000
        Protected Property EditedDocuemntID() As String
            Get
                Return If(DirectCast(Session(SessionKey), String), String.Empty)
            End Get
            Set(ByVal value As String)
                Session(SessionKey) = value
            End Set
        End Property
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
            If Not IsPostBack AndAlso Not IsCallback Then
                If Not String.IsNullOrEmpty(EditedDocuemntID) Then
                    DocumentManager.CloseDocument(DocumentManager.FindDocument(EditedDocuemntID).DocumentId)
                    EditedDocuemntID = String.Empty
                End If
                EditedDocuemntID = Guid.NewGuid().ToString()
                Dim view As DataView = DirectCast(SqlDataSource1.Select(DataSourceSelectArguments.Empty), DataView)
                Spreadsheet.Open(EditedDocuemntID, DocumentFormat.Xlsx, Function()
                    Return CType(view.Table.Rows(0)("DocumentContent"), Byte())
                End Function)
            End If
        End Sub

        Protected Sub SqlDataSource1_Updating(ByVal sender As Object, ByVal e As SqlDataSourceCommandEventArgs)
            e.Command.Parameters("@DocumentID").Value = 1 ' First Row
            Dim docBytes() As Byte = Spreadsheet.SaveCopy(DocumentFormat.Xlsx)
            If docBytes.Length <= MAX_LENGTH Then
                e.Command.Parameters("@DocumentContent").Value = docBytes
            Else
                Throw New Exception("The file exceeds the maximum file size")
            End If
        End Sub

        Protected Sub ASPxSpreadsheet1_Saving(ByVal source As Object, ByVal e As DocumentSavingEventArgs)
            Dim sh As ASPxSpreadsheet = TryCast(source, ASPxSpreadsheet)
            Try
             '   SqlDataSource1.Update(); // uncomment to update the data source
                sh.JSProperties("cpSavingResult") = "OK"
            Catch ex As Exception
                sh.JSProperties("cpSavingResult") = "ERROR"
            End Try
            e.Handled = True
        End Sub
    End Class
End Namespace