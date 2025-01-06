<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataSummary.ascx.cs" Inherits="Assessment.UserControls.DataSummary" %>
<tr id="trLanguage">
    <td id="tdFirstName" class="left-align" runat="server" enableviewstate="false"></td>
    <td id="tdLastName" class="center-align" runat="server" enableviewstate="false"></td>
    <td id="tdEmailId" class="center-align" runat="server" enableviewstate="false"></td>
    <td id="tdRole" class="center-align" runat="server" enableviewstate="false">
        <%--//trail--%>
        
      <asp:DropDownList ID="ddlRole" CssClass="form-control" runat="server" AutoPostBack="true" onchange="UpdateRole(this)">
    <asp:ListItem Value="Intern">Intern</asp:ListItem>
    <asp:ListItem Value="Admin">Admin</asp:ListItem>
    <asp:ListItem Value="Trainee">Trainee</asp:ListItem>
</asp:DropDownList>

    </td>
     <td class="text-xs-center" id="tdAction" >
        <i id="Edit" class="fa fa-edit i-action cursor-pointer linkcolor scroll-edit" runat="server" enableviewstate="false">
        </i><i id="Delete" class="fa fa-trash-o i-action cursor-pointer red"
            runat="server" enableviewstate="false"></i>
    </td>

</tr>
