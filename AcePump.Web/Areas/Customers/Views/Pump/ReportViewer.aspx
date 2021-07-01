<%@ Page Language="VB" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="AcePump.Web.Controllers" %>
<%@ Import Namespace="AcePump.Web.Areas.Employees.Controllers" %>

<%@ Register assembly="Telerik.ReportViewer.WebForms" namespace="Telerik.ReportViewer.WebForms" tagprefix="telerik" %>
<%@ Register assembly="Telerik.Reporting" namespace="Telerik.Reporting" tagprefix="telerik" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title><%: ViewData("Title")%></title>
    
    <script runat="server">
        Public Overrides Sub VerifyRenderingInServerForm(control As Control)
            ' to avoid the server form (<form runat="server"> requirement
        End Sub

        Protected Overrides Sub OnLoad(e As EventArgs)
            MyBase.OnLoad(e)
            
            If ViewData(ReportController.ReportsInstanceKey) Is Nothing Then
                reportPanel.Visible = False
                viewer.Enabled = False

                noReportPanel.Visible = True
            Else
                Dim instanceReportSource = New Telerik.Reporting.InstanceReportSource()
                instanceReportSource.ReportDocument = ViewData(ReportController.ReportsInstanceKey)
                viewer.ReportSource = instanceReportSource
            End If
        End Sub
    </script>
</head>
<body>
    <asp:Panel ID="reportPanel" runat="server">
        <p>Browse through the data using the displayed controls, or, you can export to PDF, Excel, CSV,
            using the drop down menu.</p>
        <telerik:ReportViewer ID="viewer" Width="100%" Height="800px" runat="server"></telerik:ReportViewer>
    </asp:Panel>

    <asp:Panel ID="noReportPanel" runat="server" Visible="false">
        <p>You have not selected a report to view.</p>
    </asp:Panel>
</body>
</html>
