<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ArchiveList.aspx.cs" Inherits="ArchiveProject.ArchiveList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnLoadResult" />

        </Triggers>
        <ContentTemplate>

        
    <table>
        <tr>
            <th>
                <asp:Label runat="server" Text="URL"></asp:Label>
            </th>
            <td>
                <asp:TextBox runat="server" ID="txtURL" PlaceHolder="Example:http://www.cnn.com/"  Text="http://www.cnn.com/"></asp:TextBox>
                 <asp:RequiredFieldValidator runat="server" ID="rfURL" ControlToValidate="txtURL" ErrorMessage="*" ForeColor="Red" Display="Dynamic" ValidationGroup="URL"></asp:RequiredFieldValidator>
            </td>
            <th>
                <asp:Label runat="server" Text="Start Date"></asp:Label>
            </th>
            <td>
              <asp:TextBox runat="server" ID="txtStartDate"  PlaceHolder="MM/DD/YYYY-12/31/2015"  Text="01/01/2016"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtStartDate"  ForeColor="Red"  ErrorMessage="*" Display="Dynamic" ValidationGroup="URL"></asp:RequiredFieldValidator>

            </td>
           <th>
                <asp:Label runat="server" Text="End Date"></asp:Label>
            </th>
            <td>
              <asp:TextBox runat="server" ID="txtEndDate" PlaceHolder="MM/DD/YYYY-12/31/2017" Text="01/01/2016"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtEndDate"  ForeColor="Red"  ErrorMessage="*" Display="Dynamic" ValidationGroup="URL"></asp:RequiredFieldValidator>
                <asp:Button runat="server" ID="btnLoadResult" Text="Load" OnClick="btnLoadResult_Click" ValidationGroup="URL" />

            </td>
        </tr>

    </table>
    <br />
            <center>
    <asp:UpdateProgress runat="server" id="PageUpdateProgress">
            <ProgressTemplate>
               <img src="Images/ajax-loader.gif" />
            </ProgressTemplate>
        </asp:UpdateProgress>
                </center>
    <asp:GridView CellPadding="4" runat="server" ForeColor="#333333" GridLines="None" ID="grdList" Width="100%" >
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>

               <asp:GridView CellPadding="4" runat="server" ForeColor="#333333" GridLines="None" ID="imgGrid" Width="100%" >
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>

              <asp:GridView CellPadding="4" runat="server" ForeColor="#333333" GridLines="None" ID="videoGrid" Width="100%" >
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>

             <asp:GridView CellPadding="4" runat="server" ForeColor="#333333" GridLines="None" ID="adGrid" Width="100%" >
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
