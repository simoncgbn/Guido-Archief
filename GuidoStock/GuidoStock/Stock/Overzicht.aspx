<%@ Page Title="Stock" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Overzicht.aspx.cs" Inherits="GuidoStock.Stock.Overzicht" EnableEventValidation="false" %>

<%@ Import Namespace="GuidoStock.App_Code" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="updatePanel" ChildrenAsTriggers="True">
        <ContentTemplate>
            <div class="row stockLijst">
                <div runat="server" ID="messageBox" class="alert alert-success alert-dismissible" role="alert" style="display: none">
                  <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                  Artikel successvol verwijderd.
                </div>
                <div style="display: none" class="col-md-12 stockLijstContainer">
                    <div class="row optionsBar">
                         <div class="input-group stylish-input-group">
                             <span class="input-group-addon">
                        
                            <span class="glyphicon glyphicon-search"></span>
                        
                    </span>
                        <asp:TextBox placeholder="Zoeken" runat="server" ID="txtSearchBar" CssClass="form-control" />
                         
                             </div>
                        <div class="BolLegenda mobileHidden">
                            <div class="bolDetail">
                            <div class="circleLegend circle circle_orange"></div>
                            <span class="legendDetail"> Product vervalt in minder dan 3 maanden</span>
                            </div>
                            <div class="bolDetail">
                            <div class="circleLegend circle circle_red"></div>
                            <span class="legendDetail"> Product vervalt in minder dan een maand</span>
                                </div>
                        </div>
                        

                        <div class="pull-right">
                            <a href="addArtikel.aspx" runat="server" class="btn btn-default btn-nieuw">+ Nieuw</a>
                            
                            <div class="btn-group">
                            <button id="Button1" runat="server" class="btn btn-default btn-icon dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                <span class="glyphicon glyphicon-filter" aria-hidden="true"></span>
                            </button>
                            <ul class="dropdown-menu" aria-labelledby="btnGroupDown2">
                                <li class="fltheader">Filter</li>
                                <li><a ID="A1" href="#" runat="server" OnServerClick="FilterAlles" class="dropdown-item"><span class="glyphicon glyphicon-list-alt"></span>&nbsp;&nbsp;Alle artikelen</a></li>
                                <li><a ID="A2" runat="server"  OnServerClick="FilterStock" class="dropdown-item"><span class="glyphicon glyphicon-list-alt"></span>&nbsp;&nbsp;Artikelen op voorraad</a></li>
                                <li><a ID="A3" runat="server"  OnServerClick="FilterNoStock" class="dropdown-item"><span class="glyphicon glyphicon-list-alt"></span>&nbsp;&nbsp;Artikelen niet op voorraad</a></li>

                            </ul>
                            </div>
                            
                            <div class="btn-group">
                            <button id="btnGroupDrop1" runat="server" class="btn btn-default btn-icon dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                <span class="glyphicon glyphicon-option-horizontal" aria-hidden="true"></span>
                            </button>
                            <ul class="dropdown-menu" aria-labelledby="btnGroupDown1">
                                <li class="fltheader">Stocklijst</li>
                                <li><a ID="exportExcel" href="#" runat="server" OnServerClick="ExportToExcel" class="dropdown-item"><span class="glyphicon glyphicon-export"></span>&nbsp;&nbsp;Exporteer naar xlsx</a></li>
                                <li><a ID="exportCSV" runat="server"  OnServerClick="ExportToCSV" class="dropdown-item"><span class="glyphicon glyphicon-export"></span>&nbsp;&nbsp;Exporteer naar csv</a></li>
                                <!--<li role="separator" class="divider"></li>
                                <li><a runat="server" class="dropdown-item"><span class="glyphicon glyphicon-wrench"></span>&nbsp;&nbsp;Instellingen</a></li> -->
                            </ul>
                                </div>
                        </div>
                    </div>
                    <div class="row">
                            <asp:GridView ID="StockGridView" CssClass="tablesorter table table-hover table-bordered"
                            AutoGenerateColumns="false" OnRowDataBound="StockGridView_OnRowDataBound" OnSelectedIndexChanged="StockGridView_OnSelectedIndexChanged"
                            runat="server" DataKeyNames="Id">
                            <Columns>
                                <asp:BoundField DataField="Merk.Naam" HtmlEncode="False" HeaderText="Merk<i class='glyphicon glyphicon-sort filtericon pull-right'>" />
                                <asp:BoundField DataField="Naam" HtmlEncode="False" HeaderText="Artikel<i class='glyphicon glyphicon-sort filtericon pull-right'>" />
                                <asp:BoundField DataField="Categorie.Naam" HtmlEncode="False" HeaderText="Categorie<i class='glyphicon glyphicon-sort filtericon pull-right'>">
                                    <HeaderStyle CssClass="mobileTest"></HeaderStyle>
                                    <ItemStyle CssClass="mobileTest"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ArtikelLocatie" HtmlEncode="False" HeaderText="Locatie<i class='glyphicon glyphicon-sort filtericon pull-right'>">
                                    <HeaderStyle CssClass="mobileTest"></HeaderStyle>
                                    <ItemStyle CssClass="mobileTest"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="VervalDatumString" HtmlEncode="False" HeaderText="Vervaldatum<i class='glyphicon glyphicon-sort filtericon pull-right'>">
                                    <HeaderStyle CssClass="mobileTest"></HeaderStyle>
                                    <ItemStyle CssClass="mobileTest"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Aantal" HtmlEncode="False" HeaderText="Voorraad<i class='glyphicon glyphicon-sort filtericon pull-right'>">
                                    <HeaderStyle CssClass="mobileTest"></HeaderStyle>
                                    <ItemStyle CssClass="mobileTest"></ItemStyle>
                                </asp:BoundField>
                                <%--Remove this--%>
                                <asp:BoundField  DataField="Barcode" HtmlEncode="False" HeaderText="Barcode<i class='glyphicon glyphicon-sort filtericon pull-right'>"/>
                            </Columns>

                        </asp:GridView>
                    </div>
                </div>
                <div class="col-md-7 col-xs-12 detailContainer" style="display: none">
                    <div class="detailContainer">
                        <div class="detailWrapper col-md-12">
                            <div class="headerWrapper">
                                <span><%=Artikel.Naam %></span><a id="closeButtonLink" href="#"><i class="pe-7s-close"></i></a>
                                <button runat="server" id="btnRemove" type="button" class="btn btn-default btn-icon">
                                    <span class="glyphicon glyphicon-trash" aria-hidden="true" />
                                </button>
                                <button id="printButton" type="button" class="btn btn-default btn-icon">
                                    <span class="glyphicon glyphicon-print" aria-hidden="true" />
                                </button>

                                 <asp:LinkButton ID="btnPrintBarcode" CssClass="btn btn-default btn-icon linkButton" runat="server" Text="" >
                            
                                    <span class="glyphicon glyphicon-barcode" aria-hidden="true"/>
                              
                                </asp:LinkButton>


                                <asp:LinkButton ID="btnEditStock" CssClass="btn btn-default btn-icon linkButton" runat="server" Text="" OnClick="btnEditStock_Click">
                            
                                    <span class="glyphicon glyphicon-edit" aria-hidden="true"/>
                              
                                </asp:LinkButton>

                            </div>
                            <div class="row">
                                <div class="detailPane1 col-md-6">
                                    <h4>Algemene info</h4>
                                    <table>
                                        <tr class="row">
                                            <td class="attributeNames">Merk</td> <td class="attributeValues"><%=Artikel.Merk != null ? Artikel.Merk.Naam : "Geen" %></td>
                                        </tr>
                                        <tr class="row">
                                            <td class="attributeNames">Categorie</td><td class="attributeValues"><%=Artikel.Categorie != null ? Artikel.Categorie.Naam : "Geen" %></td>
                                        </tr>
                                        <tr class="row">
                                            <td class="attributeNames">Barcode</td><td class="attributeValues"><%=Artikel.Barcode %></td>
                                        </tr>
                                        <tr class="row">
                                            <td class="attributeNames">Gewicht</td><td class="attributeValues"><%=Artikel.Gewicht == -1 ? "Geen" : Artikel.Gewicht.ToString("0.00 kg")%></td>
                                        </tr>
                                        <tr class="row">
                                            <td class="attributeNames">Naturaprijs</td><td class="attributeValues"><%=Artikel.Naturaprijs == -1 ? "Geen" : Artikel.Naturaprijs.ToString("C") %></td>
                                        </tr>
                                        <tr class="row">
                                            <td class="attributeNames">Verhuurprijs</td><td class="attributeValues"><%=Artikel.Verhuurprijs == -1 ? "Geen" : Artikel.Verhuurprijs.ToString("C") %></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="detailPane2 col-md-6">
                                    <% if (Artikel.ArtikelMetaTags.Count != 0)
                                       {
                                            %><h4>Kenmerken</h4><%
                                       } %>
                                    <table>
                                        <% foreach (var metatag in Artikel.ArtikelMetaTags)
                                           {
                                        %><tr>
                                            <td class="attributeNames"><%= metatag.Naam %></td>
                                            <td class="attributeValues"><%= metatag.Waarde %></td>
                                        </tr> <% } %>
                                    </table>
                                </div>
                            </div>
                            <%
                                if (Artikel.Stocks.Count != 0)
                                {
                                    %>
                                <div class="row locatiePane col-md-6">
                                    <h4>Locatie</h4>
                                    <table>
                                        <thead>
                                        <tr>
                                            <th>Vervaldatum</th>
                                            <th>Naam</th>
                                            <th>Aantal</th>
                                        </tr>
                                        </thead>
                                        <tbody>
                                        <% foreach (Stock stock in Artikel.Stocks)
                                           {
                                        %>
                                        <tr>
                                            <td class="locatieVervaldatum"><%=stock.VervalDatumString == "" ? "Geen" : stock.VervalDatumString %></td>
                                            <td class="locatieNaam"><%=stock.ArtikelLocatie.Code %></td>
                                            <td class="locatieAantal"><%=stock.Aantal %></td>
                                        </tr>
                                        <%
                                            } %>
                                        </tbody>
                                    </table>

                                </div>
                                        
                            <%
                                }
                                 %>
                            
                            <% if (Artikel.Units.Count != 0)
                               {
                                    %>
                            <div class="row unitPane col-md-7">
                                <h4>Eenheden</h4>
                                <table>
                                    <thead>
                                        <tr>
                                            <th class="unitNaam">Naam</th>
                                            <th class="unitAantal">Aantal per verpakking</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <% foreach (var unit in Artikel.Units)
                                            {
                                        %>
                                        <tr>
                                            <td><%=unit.NaamEnkelvoud %></td>
                                            <td><%=unit.Aantal %></td>
                                            <td> <a href="http://www.guido.be/barcode.aspx?code=<%=unit.Barcode%>" target="_blank"><img src="http://www.guido.be/barcode.aspx?code=<%=unit.Barcode%>"/></a> </td>
                                        </tr>
                                        <%
                                            } %>
                                    </tbody>
                                </table>

                            </div>
                            <%
                               } %>
                            
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="shouldBeDetail" runat="server" Value="False" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="exportExcel"/>
            <asp:PostBackTrigger ControlID="exportCSV"/>
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function pageLoad(sender, args) {
            $(document).ready(function () {
                $(".tablesorter").tablesorter({ dateFormat: "uk" });

                $('#MainContent_StockGridView td:nth-child(5)').each(function () {
                    var datum = $(this).find(":text").prevObject[0].innerHTML;

                    var from = datum.split("/");
                    var d = new Date(from[2], from[1] - 1, from[0]);

                    var d1 = new Date();
                    d1.setDate(d1.getDate() + 31);

                    var d2 = new Date();
                    d2.setDate(d2.getDate() + 92);
                    if (d <= d1) {
                        $(this).html('<div class="circle circle_red"></div> &nbsp; ' + datum);
                    } else if (d <= d2) {
                        $(this).html('<div class="circle circle_orange"></div> &nbsp; ' + datum);
                    } else {
                        $(this).html('<div class="circle" ></div> &nbsp; ' + datum);
                    }

                });

                $("#MainContent_txtSearchBar").on('keyup', function () {
                    var searchText = document.getElementById('MainContent_txtSearchBar').value;
                    var targetTable = document.getElementById('MainContent_StockGridView');
                    var targetTableColCount = 0;

                    //Loop through table rows
                    for (var rowIndex = 0; rowIndex < targetTable.rows.length; rowIndex++) {
                        var rowData = '';

                        //Get column count from header row
                        if (rowIndex == 0) {
                            targetTableColCount = targetTable.rows.item(rowIndex).cells.length;
                            continue; //do not execute further code for header row.
                        }

                        //Process data rows. (rowIndex >= 1)
                        for (var colIndex = 0; colIndex < targetTableColCount; colIndex++) {
                            rowData += targetTable.rows.item(rowIndex).cells.item(colIndex).textContent;
                        }

                        //If search term is not found in row data
                        //then hide the row, else show
                        if (rowData.toUpperCase().indexOf(searchText.toUpperCase()) == -1)
                            targetTable.rows.item(rowIndex).style.display = 'none';
                        else
                            targetTable.rows.item(rowIndex).style.display = 'table-row';
                    }

                });

                // Javascript voor de resize van Master/Detail
                function makeMaster() {
                    var stockLijst = $('.stockLijstContainer');
                    var detailField = $('.detailContainer');
                    stockLijst.removeClass('col-md-5');
                    stockLijst.addClass('col-md-12');
                    stockLijst.show();
                    detailField.hide();
                    detailField.addClass('mobileHidden');
                    stockLijst.removeClass('mobileHidden');
                    $(".BolLegenda").show();
                };

                function makeDetail() {
                    var stockLijst = $('.stockLijstContainer');
                    var detailField = $('.detailContainer');
                    stockLijst.removeClass('col-md-12');
                    stockLijst.addClass('col-md-5');
                    stockLijst.show();
                    detailField.show();
                    stockLijst.addClass('mobileHidden');
                    detailField.removeClass('mobileHidden');
                    $(".BolLegenda").hide();
                };

                if ($('#MainContent_shouldBeDetail').val() === "True") {
                    makeDetail();
                } else {
                    makeMaster();
                }

                // Javascript om StockLijst aan te passen naar Master/Detail
                function alterTable() {
                    $('#MainContent_StockGridView').find('tr').each(function () {
                        var i = 0;
                        $(this).find('td, th').each(function () {
                            if (i++ > 1) {
                                if ($('#MainContent_shouldBeDetail').val() === "True") {
                                    $(this).hide();
                                } else {
                                    $(this).show();
                                }
                            }
                        });
                    });
                }

                $('#closeButtonLink').click(function () {
                    $('#MainContent_shouldBeDetail').val('False');
                    makeMaster();
                    alterTable();
                });

                $('#MainContent_btnRemove').click(function () {
                    BootstrapDialog.show({
                        title: 'Artikel verwijderen',
                        message: 'Bent u zeker dat u dit artikel wilt verwijderen?',
                        cssClass: 'bootstrapDialog',
                        buttons: [{
                            id: 'btn-cancel',
                            icon: 'glyphicon glyphicon-remove',
                            label: 'Annuleren',
                            cssClass: 'btn-primary',
                            autospin: false,
                            action: function(dialogRef) {
                                dialogRef.close();
                            }
                        },{
                            id: 'btn-ok',
                            icon: 'glyphicon glyphicon-trash',
                            label: 'Verwijderen',
                            cssClass: 'btn-danger',
                            autospin: false,
                            action: function (dialogRef) {
                                var pageId = '<%=  Page.ClientID %>';
                                __doPostBack(pageId, "verwijder");
                                dialogRef.close();
                            }
                        }]
                    });
                });

                alterTable();
            });

            $("#printButton").on('click', function() {
                window.print();
            });

            $("#MainContent_btnPrintBarcode").on('click', BarcodePrint);

        }

        function BarcodePrint() {
            var url = 'http://www.guido.be/barcode.aspx?code=';
            var waterfles = "<%= Artikel.Barcode %>";
            url += waterfles;
            window.open(url, '_blank');
        }
    </script>

</asp:Content>
