<%@ Page Title="Order Overzicht" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="Overzicht.aspx.cs" Inherits="GuidoStock.Order.Overzicht" EnableEventValidation="false" %>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="updatePanel" ChildrenAsTriggers="True">
        <ContentTemplate>
            <div class="row orderLijst">
                <div class="col-md-5 eventLijstContainer">
                    <div class="row optionsBar">
                        <div class="input-group stylish-input-group">
                            <span class="input-group-addon">

                                <span class="glyphicon glyphicon-search"></span>
                            </span>
                            <asp:TextBox placeholder="Zoeken" runat="server" ID="txtSearchBar" CssClass="form-control" />

                        </div>
                        <div class="pull-right">
                            <a href="~/Order/AddOrder.aspx" runat="server" class="btn btn-default btn-nieuw">+ Nieuw</a>
                            <button id="btnGroupDrop1" runat="server" class="btn btn-default btn-icon dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                <span class="glyphicon glyphicon-filter" aria-hidden="true"></span>
                            </button>
                            <ul class="dropdown-menu" aria-labelledby="btnGroupDown1">
                                <li class="fltheader">Filter</li>
                                <li><a ID="alles" href="#" runat="server" class="dropdown-item" OnServerClick="alles_OnServerClick"><span class="glyphicon glyphicon-calendar"></span>&nbsp;&nbsp;Alle orders</a></li>
                                <li><a ID="komende" runat="server" class="dropdown-item" OnServerClick="komende_OnServerClick"><span class="glyphicon glyphicon-calendar"></span>&nbsp;&nbsp;Komende orders</a></li>
                                <li><a ID="verlopen" runat="server" class="dropdown-item" OnServerClick="verlopen_OnServerClick"><span class="glyphicon glyphicon-calendar"></span>&nbsp;&nbsp;Verlopen orders</a></li>
                                <!--<li role="separator" class="divider"></li>
                                <li><a runat="server" class="dropdown-item"><span class="glyphicon glyphicon-wrench"></span>&nbsp;&nbsp;Instellingen</a></li> -->
                            </ul>
                        </div>
                    </div>
                    <div class="row">
                        <asp:GridView runat="server" OnSelectedIndexChanged="OrdersGridView_OnSelectedIndexChanged"
                            OnRowDataBound="OrdersGridView_OnRowDataBound" ID="OrdersGridView"
                            CssClass="tablesorter table table-hover table-bordered" AutoGenerateColumns="False" DataKeyNames="Id">
                            <Columns>
                                <asp:BoundField DataField="Naam" HtmlEncode="False" HeaderText="Naam<i class='glyphicon glyphicon-sort filtericon pull-right'>" />
                                <asp:BoundField DataField="BeginTijd" HtmlEncode="False" HeaderText="Datum<i class='glyphicon glyphicon-sort filtericon pull-right'>" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="CheckList" HtmlEncode="False"/>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="col-md-7 col-xs-12 detailContainer">
                    <div class="detailWrapper col-md-12">
                        <% if (Order != null)
                        {%>
                        <div class="headerWrapper">
                            <span><%=Order.Naam %></span>
                            <asp:LinkButton OnClick="checklistButton_OnClick" ID="checklistButton" runat="server" type="button" class="btn btn-default btn-icon linkButton btnBlauw">
                                    <span class="glyphicon glyphicon-paste" aria-hidden="true" />
                            </asp:LinkButton>
                            <button id="printButton" type="button" class="btn btn-default btn-icon">
                                <span class="glyphicon glyphicon-print" aria-hidden="true" />
                            </button>

                            <asp:LinkButton ID="btnEditOrder" OnClick="btnEditOrder_OnClick" CssClass="btn btn-default btn-icon linkButton" runat="server" Text="">
                            
                                    <span class="glyphicon glyphicon-edit" aria-hidden="true"/>
                              
                            </asp:LinkButton>
                        </div>
                        <div class="row algemeneInfoRow">
                            <div id="algemeneInfo" class="detailPane">
                                <h4>Contactinformatie</h4>
                                <table>
                                    <tr class="row">
                                        <td class="attributeNames">Naam</td>
                                        <td class="attributeValues"><%=Order.ContactNaam %></td>
                                    </tr>
                                    <tr class="row">
                                        <td class="attributeNames">Datum</td>
                                        <td class="attributeValues"><%=Order.BeginTijd %></td>
                                    </tr>
                                    <tr class="row">
                                        <td class="attributeNames">Tel.</td>
                                        <td class="attributeValues"><%=Order.Tel %></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <hr />
                        <% if (Order.OrderLijnen.Count > 0) { %>
                        <div class="row">
                            <h4>Checklijst</h4>
                            <asp:GridView runat="server" ID="ChecklistGridView" AutoGenerateColumns="False"
                                CssClass="tablesorter table table-hover table-bordered">
                                <Columns>
                                    <asp:BoundField DataField="Artikel.Naam" HeaderText="Artikel" />
                                    <asp:BoundField DataField="Aantal" HeaderText="Aantal" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <%} %>
                        <%}%>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        function pageLoad(sender, args) {
            $("#printButton").on('click',
                function() {
                    window.print();
                });
        }
    </script>
</asp:Content>
