<%@ Page Language="C#" AutoEventWireup="true" Inherits="LSPE.report_popup" Codebehind="report_popup.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>
        <rsweb:ReportViewer ID="rpvReportViewer" runat="server" Width="1044px">
        </rsweb:ReportViewer>
           
    </div>
    </form>
</body>
</html>
