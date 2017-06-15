<%@ Page Title="Event Overzicht" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="Overzicht.aspx.cs" Inherits="GuidoStock.Event.Overzicht" EnableEventValidation="false" %>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="updatePanel" ChildrenAsTriggers="True" UpdateMode="Always">
        <ContentTemplate>
            <div class="row eventLijst">
                <div class="col-md-5 eventLijstContainer">
                    <div class="row optionsBar">
                         <div class="input-group stylish-input-group">
                             <span class="input-group-addon">
                        
                            <span class="glyphicon glyphicon-search"></span>
                    </span>
                        <asp:TextBox placeholder="Zoeken" runat="server" ID="txtSearchBar" CssClass="form-control" />
                         
                             </div>
                        <div class="pull-right">
                            <a href="~/Event/AddEvent.aspx" runat="server" class="btn btn-default btn-nieuw">+ Nieuw</a>
                            <button id="btnGroupDrop1" runat="server" class="btn btn-default btn-icon dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                <span class="glyphicon glyphicon-filter" aria-hidden="true"></span>
                            </button>
                            <ul class="dropdown-menu" aria-labelledby="btnGroupDown1">
                                <li class="fltheader">Filter</li>
                                <li><a ID="alles" href="#" runat="server" class="dropdown-item" OnServerClick="alles_OnServerClick"><span class="glyphicon glyphicon-calendar"></span>&nbsp;&nbsp;Alle evenementen</a></li>
                                <li><a ID="komende" runat="server" class="dropdown-item" OnServerClick="komende_OnServerClick"><span class="glyphicon glyphicon-calendar"></span>&nbsp;&nbsp;Komende evenementen</a></li>
                                <li><a ID="verlopen" runat="server" class="dropdown-item" OnServerClick="verlopen_OnServerClick"><span class="glyphicon glyphicon-calendar"></span>&nbsp;&nbsp;Verlopen evenementen</a></li>
                                <!--<li role="separator" class="divider"></li>
                                <li><a runat="server" class="dropdown-item"><span class="glyphicon glyphicon-wrench"></span>&nbsp;&nbsp;Instellingen</a></li> -->
                            </ul>
                        </div>
                    </div>
                    <div class="row">
                        <asp:GridView runat="server" OnSelectedIndexChanged="EvenementenGridView_OnSelectedIndexChanged" 
                            OnRowDataBound="EvenementenGridView_OnRowDataBound" ID="EvenementenGridView"  
                            CssClass="tablesorter table table-hover table-bordered" AutoGenerateColumns="False" DataKeyNames="Id">
                            <Columns>
                                <asp:BoundField DataField="Naam" HtmlEncode="False" HeaderText="Naam<i class='glyphicon glyphicon-sort filtericon pull-right'>"/>
                                <asp:BoundField DataField="Datum" HtmlEncode="False" HeaderText="Datum<i class='glyphicon glyphicon-sort filtericon pull-right'>" DataFormatString="{0:dd/MM/yyyy}"/>
                                <asp:BoundField DataField="CheckList" HtmlEncode="False"/>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <% if (Evenement != null) { %>
                <div class="col-md-7 col-xs-12 detailContainer">
                    <div class="detailWrapper col-md-12">
                        <div class="headerWrapper">
                                <span><%=Evenement.Naam %></span>
                                <asp:Linkbutton OnClick="checklistButton_OnClick" id="checklistButton" runat="server" type="button" class="btn btn-default btn-icon linkButton btnBlauw">
                                    <span class="glyphicon glyphicon-paste" aria-hidden="true" />
                                </asp:Linkbutton>
                                <button id="printButton" type="button" class="btn btn-default btn-icon">
                                    <span class="glyphicon glyphicon-print" aria-hidden="true" />
                                </button>

                                <asp:LinkButton OnClick="btnEditStock_OnClick" ID="btnEditStock" CssClass="btn btn-default btn-icon linkButton" runat="server" Text="">
                            
                                    <span class="glyphicon glyphicon-edit" aria-hidden="true"/>
                              
                                </asp:LinkButton>
                            <!--<div class="row navPills">
                                <ul class="nav nav-pills">
                                  <li class="active"><a href="#algemeneInfo">Algemeen</a></li>
                                  <li><a href="#organisaties">Organisatie</a></li>
                                  <li><a href="#rolverdeling">Rolverdeling</a></li>
                                  <li><a href="#contacten">Contacten</a></li>
                                </ul>
                            </div>-->
                        </div>
                        <div class="row algemeneInfoRow">
                            <div id="algemeneInfo" class="detailPane">
                                <h4>Algemene info</h4>
                                <table>
                                    <tr class="row">
                                        <td class="attributeNames">Opdrachtgever</td>
                                        <td class="attributeValues"><%=Evenement.Opdrachtgever.Naam %></td>
                                    </tr>
                                    <tr class="row">
                                        <td class="attributeNames">Tel. opdrachtgever</td>
                                        <td class="attributeValues"><%=Evenement.Opdrachtgever.Tel %></td>
                                    </tr>
                                    <tr class="row">
                                        <td class="attributeNames">Start event</td>
                                        <td class="attributeValues"><%=Evenement.Datum.ToString("dd-MM-yyyy Humm") %></td>
                                    </tr>
                                    <tr class="row">
                                        <td class="attributeNames">Coördinator</td>
                                        <td class="attributeValues"><%=Evenement.EventCoordinator.Naam %></td>
                                    </tr>
                                    <tr class="row">
                                        <td class="attributeNames">Tel. coördinator</td>
                                        <td class="attributeValues"><%=Evenement.EventCoordinator.Tel %></td>
                                    </tr>
                                    <tr class="row">
                                        <td class="attributeNames">FieldManager</td>
                                        <td class="attributeValues"><%=Evenement.Fieldmanager.Naam %></td>
                                    </tr>
                                    <tr class="row">
                                        <td class="attributeNames">Tel. FieldManager</td>
                                        <td class="attributeValues"><%=Evenement.Fieldmanager.Tel %></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <hr/>
                        <% if (Evenement.EvenementLocaties.Count > 0) { %>
                        <div id="organisaties" class="row">
                            <h4>Organisaties</h4>
                            <% foreach (var organisatie in Evenement.EvenementLocaties)
                               { %>
                            <div class="row">
                                <div class="detailPane1 col-md-6">
                                    <table>
                                        <tr class="row">
                                            <td class="attributeNames">Naam</td>
                                            <td class="attributeValues"><%= organisatie.EvenementKlant.Organisatie %></td>
                                        </tr>
                                        <tr class="row">
                                            <td class="attributeNames">Contactpersoon</td>
                                            <td class="attributeValues"><%= organisatie.EvenementKlant.ContactNaam %></td>
                                        </tr>
                                        <tr class="row">
                                            <td class="attributeNames">Tel. contactpersoon</td>
                                            <td class="attributeValues"><%= organisatie.EvenementKlant.ContactTel %></td>
                                        </tr>
                                        <tr class="row">
                                            <td class="attributeNames">Plaats</td>
                                            <td class="attributeValues"><%= organisatie.Locatie.Zaal %></td>
                                        </tr>
                                        <tr class="row">
                                            <td class="attributeNames">Tijdstip</td>
                                            <td class="attributeValues"><%= organisatie.BeginTijd.ToString("dd-MM-yyyy Humm") %></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="detailPane2 col-md-6">
                                    <table>
                                        <tr class="row">
                                            <td class="attributeNames">Stad</td>
                                            <td class="attributeValues"><%= organisatie.Locatie.Plaats %></td>
                                        </tr>
                                        <tr class="row">
                                            <td class="attributeNames">Postcode</td>
                                            <td class="attributeValues"><%= organisatie.Locatie.Postcode %></td>
                                        </tr>
                                        <tr class="row">
                                            <td class="attributeNames">Adres</td>
                                            <td class="attributeValues"><%= organisatie.Locatie.Straat %></td>
                                        </tr>
                                        <tr class="row">
                                            <td class="attributeNames">Verwachte opkomst</td>
                                            <td class="attributeValues"><%= organisatie.VerwachteOpkomst != -1 ? organisatie.VerwachteOpkomst.ToString() : "Geen" %></td>
                                        </tr>
                                        <tr class="row">
                                            <td class="attributeNames">Target</td>
                                            <td class="attributeValues"><%= organisatie.Target != -1 ? organisatie.Target.ToString() : "Geen" %></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <hr/>
                            <% } %>
                        </div>
                        <% } %>
                        <% if (Evenement.ParsedEvenementTaken.Count != 0)
                           { %>
                        <div id="rolverdeling" class="row">
                            <h4>Rolverdeling</h4>
                            <div class="detailPane">
                                <table>
                                    <thead>
                                        <tr>
                                            <th class="attributeNames">Taak</th>
                                            <th class="attributeNames">Datum</th>
                                            <th class="attributeNames">Tijdstip</th>
                                            <th class="attributeNames">Door</th>
                                        </tr>
                                    </thead>
                                    <tbody>

                                        <%
                                            var enumerator = Evenement.ParsedEvenementTaken.GetEnumerator();
                                            while (enumerator.MoveNext())
                                            {
                                                var list = enumerator.Current.Value;
                                                for (int i = 0; i < list.Count; i++)
                                                {
                                                    if (i == 0)
                                                    {
                                        %>          <tr>
                                                        <td class="unitNaam"><%= list[i].Taak.Naam %></td>
                                                        <td class="unitNaam"><%= list[i].Van.ToString("dd-MM-yyyy") %></td>
                                                        <td class="unitNaam"><%= list[i].VanUur + " - " + list[i].TotUur %></td>
                                                        <td class="unitNaam"><%= list[i].Gebruiker.Voornaam + " " + list[i].Gebruiker.Achternaam %></td>
                                                    </tr>
                                                        <%
                                                    }
                                                    else
                                                    {
                                                        %>
                                                    <tr>
                                                        <td class="unitNaam"></td>
                                                        <td class="unitNaam"></td>
                                                        <td class="unitNaam"></td>
                                                        <td class="unitNaam"><%= list[i].Gebruiker.Voornaam + " " + list[i].Gebruiker.Achternaam %></td>
                                                    </tr>
                                                    <%
                                                    }
                                                }
                                            }
                                                    %>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <hr/>
                        <% } %> 
                        <% if (Evenement.EvenementTeamLeden.Count != 0)
                           { %>
                        <div id="contacten" class="row">
                            <h4>Contacten</h4>
                            <div class="detailPane">
                                <table>
                                    <thead>
                                        <tr>
                                            <th class="attributeNames">Taak</th>
                                            <th class="attributeNames">Naam</th>
                                            <th class="attributeNames">Tel.</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    <% foreach (var teamlid in Evenement.EvenementTeamLeden)
                                       {
                                    %><tr>
                                            <td class="unitNaam"><%= teamlid.Functie %></td>
                                            <td class="unitNaam"><%= teamlid.Naam %></td>
                                            <td class="unitNaam"><%= teamlid.Tel %></td>
                                        </tr> <%
                                       } %>
                                        
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <hr/>
                        <% } %>
                        
                        <% if (Evenement.EvenementTransporten.Count != 0)
                           { %>
                        <div id="transporten" class="row">
                            <h4>Transporten</h4>
                            <div class="detailPane">
                                <table>
                                    <thead>
                                        <tr>
                                            <th class="attributeNames">Naam</th>
                                            <th class="attributeNames">Chauffeur Heen</th>
                                            <th class="attributeNames">Chauffeur Terug</th>
                                            <th class="attributeNames">Vertrek</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    <% foreach (var transport in Evenement.EvenementTransporten)
                                       {
                                    %><tr>
                                            <td class="unitNaam"><%= transport.Transport.Naam %></td>
                                            <td class="unitNaam"><%= transport.ChauffeurHeen.Naam %></td>
                                            <td class="unitNaam"><%= transport.ChauffeurTerug.Naam %></td>
                                            <td class="unitNaam"><%= transport.Vertrek %></td>
                                        </tr> <%
                                       } %>
                                        
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <hr/>
                        <% } %>
                        <% if (!string.IsNullOrEmpty(Evenement.Opmerking)) {%> 
                        <div id="opmerking" class="row">
                            <h4>Extra</h4>
                             <%= Evenement.Opmerking %>
                         </div>
                        <%} %>
                    </div>
                </div>
                <% } %>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $(".tablesorter").tablesorter({ dateFormat: "uk" });
            $("#MainContent_txtSearchBar").on('keyup', function () {
                var searchText = document.getElementById('MainContent_txtSearchBar').value;
                var targetTable = document.getElementById('MainContent_EvenementenGridView');
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


            $("#printButton").on('click', function() {
                window.print();
            });
        }
    </script>
</asp:Content>