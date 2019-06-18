<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DXSample.Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxSpreadsheet.v18.1, Version=18.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpreadsheet" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="styles.css">
    <script>

        function onEndCallback(s, e) {
            if (s.cpSavingResult == "OK") {
                PopupControl.ShowWindow(PopupControl.GetWindowByName("OK"));
                s.cpSavingResult = null;
            }
            else if (s.cpSavingResult == "ERROR")
                PopupControl.ShowWindow(PopupControl.GetWindowByName("ERROR"));
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxPopupControl runat="server" ID="PopupControl" ClientInstanceName="PopupControl" ShowHeader="false"  Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                <Windows>
                    <dx:PopupWindow Name="OK">
                        <ContentStyle CssClass="success">
                        </ContentStyle>
                        <ContentCollection>
                            <dx:PopupControlContentControl>
                                <div class="centerText">
                                    <h4 class="header">The file was saved successfully
                                    </h4>
                                    <dx:ASPxButton runat="server" Text="OK" AutoPostBack="false">
                                        <ClientSideEvents Click="function(s,e) { PopupControl.HideWindow(PopupControl.GetWindowByName('OK')); }" />
                                    </dx:ASPxButton>
                                </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:PopupWindow>
                    <dx:PopupWindow Name="ERROR">
                        <ContentStyle CssClass="error">
                        </ContentStyle>
                        <ContentCollection>
                            <dx:PopupControlContentControl>
                                <div class="centerText">
                                    <h4 class="header">The file exceeds the maximum file size </h4>
                                    <dx:ASPxButton runat="server" Text="OK" AutoPostBack="false">
                                        <ClientSideEvents Click="function(s,e) { PopupControl.HideWindow(PopupControl.GetWindowByName('ERROR')); }" />
                                    </dx:ASPxButton>
                                </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:PopupWindow>
                </Windows>
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                OnUpdating="SqlDataSource1_Updating" SelectCommand="SELECT * FROM [Docs]"
                UpdateCommand="UPDATE [Docs] SET [DocumentContent] = @DocumentContent WHERE [DocumentID] = @DocumentID">
                <UpdateParameters>
                    <asp:Parameter Name="DocumentContent" DbType="Binary" />
                    <asp:Parameter Name="DocumentID" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>
            <dx:ASPxSpreadsheet ID="Spreadsheet" ClientInstanceName="Spreadsheet" runat="server" ShowConfirmOnLosingChanges="false"
                WorkDirectory="~/App_Data/WorkDirectory" OnSaving="ASPxSpreadsheet1_Saving">
                <ClientSideEvents EndCallback="onEndCallback" />   
            </dx:ASPxSpreadsheet>
        </div>
    </form>
</body>
</html>
