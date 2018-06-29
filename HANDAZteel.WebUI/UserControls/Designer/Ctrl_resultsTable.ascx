<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Ctrl_resultsTable.ascx.cs" Inherits="HANDAZ.PEB.WebUI.UserControls.Designer.Ctrl_resultsTable" %>
<div class="x_panel">
<asp:GridView ID="grv_resultsTable" runat="server" class="table table-bordered" AutoGenerateColumns="False"
     BorderStyle="Solid" CellPadding="4" EnablePersistedSelection="True" ForeColor="#333333" GridLines="Vertical" 
    HorizontalAlign="Center" UseAccessibleHeader="False" DataKeyNames="Station" HeaderStyle-HorizontalAlign="Center">
   
      <AlternatingRowStyle BackColor="White" />
      <Columns>
          <asp:BoundField DataField="Station" />
          <asp:BoundField DataField="LoadCase"/>
          <asp:BoundField DataField="Axial"/>
          <asp:BoundField DataField="Shear2"/>
          <asp:BoundField DataField="Shear3"/>
          <asp:BoundField DataField="TortionalMoment"/>
          <asp:BoundField DataField="Moment2"/>
          <asp:BoundField DataField="Moment3"/>
      </Columns>
    <EditRowStyle BackColor="#2461BF" />
    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
    <RowStyle BackColor="#EFF3FB" />
    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    <SortedAscendingCellStyle BackColor="#F5F7FB" />
    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
    <SortedDescendingCellStyle BackColor="#E9EBEF" />
    <SortedDescendingHeaderStyle BackColor="#4870BE" />
</asp:GridView>
    </div>


