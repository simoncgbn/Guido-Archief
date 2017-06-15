<%@ Page Title="Dashboard" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="~/Dashboard/Overzicht.aspx.cs" Inherits="GuidoStock.Dashboard.Overzicht" EnableEventValidation="false" %>

<%@ Import Namespace="Microsoft.AspNet.Identity.Owin" %>
<%@ Import Namespace="GuidoStock" %>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="UpdatePanel">
    <ContentTemplate>
    <asp:HiddenField ID="hdnVerwijder" runat="server" Value="" OnValueChanged="hdnVerwijder_OnValueChanged"  />
    <div class="row">
        <div class="col-md-9">
            <!-- Wijzigingen -->
            <% if (Wijzigingen != null)
               {
                   %>
            <div class="wijzingWrapper">
                
                <% if (NieuwsteEvent != null)
                   {
                        %>
                    <div>
                        <h4 class="dashboardTitel">Nieuwste event</h4>
                        <a href="../Event/Overzicht.aspx?id=<%=NieuwsteEvent.Id %>&type=alles">
                            <div class="dashboardCard">

                                <h2><%=NieuwsteEvent.Naam %></h2>
                                <span>
                                    <span><i class="glyphicon glyphicon-calendar"></i><%=NieuwsteEvent.Datum.ToString("dd MMMM yyyy") %></span>
                                    <span><i class="glyphicon glyphicon-map-marker"></i><%=NieuwsteEvent.EvenementLocaties[0].Locatie.Plaats %></span>
                                </span>
                                <span>Toegevoegd door <%=NieuwEventUser %></span>
                            </div>
                        </a>

                    </div>
                <%
                       
                   } %>

                <% if (WijzigdeEvent != null)
                   {
                       %>
                    <div>
                        <h4 class="dashboardTitel">Laatst gewijzigde event</h4>
                        <a href="../Event/Overzicht.aspx?id=<%=WijzigdeEvent.Id %>&type=alles">

                            <div class="dashboardCard">

                                <h2><%=WijzigdeEvent.Naam %></h2>
                                <span>
                                    <span><i class="glyphicon glyphicon-calendar"></i><%=WijzigdeEvent.Datum.ToString("dd MMMM yyyy") %></span>
                                    <span><i class="glyphicon glyphicon-map-marker"></i><%=WijzigdeEvent.EvenementLocaties[0].Locatie.Plaats %></span>
                                </span>
                                <span>Toegevoegd door <%=WEventUser %></span>
                            </div>
                        </a>

                    </div>
                <%
                   } %>
                
                <% if (NieuwsteArtikel != null)
                   {
                       %>
                    <div>
                        <h4 class="dashboardTitel">Nieuwste artikel</h4>
                        <a href="../Stock/Overzicht.aspx?id=<%=NieuwsteArtikel.Id %>&type=alles">

                            <div class="dashboardCard">

                                <h2><%=NieuwsteArtikel.Naam %></h2>
                                <span>
                                    <span><i class="glyphicon glyphicon-calendar"></i><%=WijzigingNieuwArtikel.Time.ToString("dd MMMM yyyy") %></span>
                                    <span><i class="glyphicon glyphicon-map-marker"></i><%=NieuwsteArtikel.Categorie.Naam %></span>
                                </span>
                                <span>Toegevoegd door <%=NieuwArtikelUser %></span>
                            </div>
                        </a>

                    </div>  
                <%
                   } %>
                
                <% if (WijzigdeArtikel != null)
                   {
                       %>
                    <div>
                        <h4 class="dashboardTitel">Laatst gewijzigde artikel</h4>
                        <a href="../Stock/Overzicht.aspx?id=<%=WijzigdeArtikel.Id %>&type=alles">
                            <div class="dashboardCard">
                                <h2><%=WijzigdeArtikel.Naam %></h2>
                                <span>
                                    <span><i class="glyphicon glyphicon-calendar"></i><%=WijzigingArtikel.Time.ToString("dd MMMM yyyy") %></span>
                                    <span><i class="glyphicon glyphicon-map-marker"></i><%=WijzigdeArtikel.Categorie.Naam %></span>
                                </span>
                                <span>Toegevoegd door <%=WArtikelUser %></span>
                            </div>
                        </a>

                    </div>
                            <%
                   } %>
               


            </div>
            
            <%
               } %>
            


            <div class="dashboardMid">

                <!-- Vervallen -->
                <% if (Stocks.Count != 0)
                   {
                       %>
                    <div class="vervalWrapper">
                        <h4 class="dashboardTitel">Artikelen die vervallen</h4>
                        <article class="Verval">
                            <%foreach (var stk in Stocks)
                              {
                                  var vvdClass = "";
                                  var d = stk.Vervaldatum;
                                  var d1 = DateTime.Today;
                                  d1 = d1.AddDays(31);

                                  var d2 = DateTime.Today;
                                  d2 = d2.AddDays(92);

                                  if (d <= d1)
                                  {
                                      vvdClass = "red";
                                  }
                                  else if (d <= d2)
                                  {
                                      vvdClass = "orange";
                                  }
                                  else
                                  {
                                      vvdClass = "";
                                  }%>
                                <a href="../Stock/Overzicht.aspx?id=<%=stk.Artikel.Id%>&type=alles">
                                    <div class="vervalContainer">
                                        <h4><%=stk.Artikel.Naam%></h4>
                                        <span class="gebeurtenisInfo">
                                            <span class="VervalDatum">
                                                <i class="glyphicon glyphicon-calendar"></i><span class="<%=vvdClass%>"><%=stk.Vervaldatum.ToString("dd/MM/yyyy")%></span>
                                            </span>
                                            <span class="vervalLocatie">
                                                <i class="glyphicon glyphicon-map-marker"></i><%=stk.ArtikelLocatie.Code%>
                                            </span>
                                            <span class="vervalAantal">
                                                <i class="glyphicon glyphicon-ban-circle"></i><%=stk.Aantal%>
                                            </span>
                                        </span>
                                    </div>
                                </a>
                            <%}%>
                        </article>
                    </div>
                <%
                   } %>
                

                <!-- Gebeurtenissen -->
                <% if (Wijzigingen != null)
                   {
                                %>
                   <div class="gebeurtenisWrapper">
                   <h4 class="dashboardTitel">Gebeurtenissen</h4>
                   <article class="Verval">
                        
                       <%
                                  foreach (var w in Wijzigingen)
                                  {
                                      var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                                      var user = manager.FindByIdAsync(w.UserId).Result.Name;
                                      var page = "";
                                      var msg = "";

                                      switch (w.Type)
                                      {
                                          case "NArtikel":
                                              page = "stock";
                                              msg = "Nieuw artikel toegevoegd.";
                                              break;
                                          case "WArtikel":
                                              page = "stock";
                                              msg = "Artikel gewijzigd.";
                                              break;
                                          case "PArtikel":
                                              page = "stock";
                                              msg = "Artikel ingecheckt.";
                                              break;
                                          case "MArtikel":
                                              page = "stock";
                                              msg = "Artikel uitgecheckt.";
                                              break;
                                          case "NEvent":
                                              page = "event";
                                              msg = "Nieuw event toegevoegd.";
                                              break;
                                          case "WEvent":
                                              page = "event";
                                              msg = "Event gewijzigd.";
                                              break;
                                          case "NOrder":
                                              page = "order";
                                              msg = "Nieuw order toegevoegd.";
                                              break;
                                          case "WOrder":
                                              page = "order";
                                              msg = "Order gewijzigd.";
                                              break;
                                          case "NChecklist":
                                              page = "event";
                                              msg = "Checklist uitgescand.";
                                              break;
                                          case "WChecklist":
                                              page = "event";
                                              msg = "Checklist ingescand.";
                                              break;
                                      }
                %><a href="../<%= page %>/Overzicht.aspx?id=<%= w.Id %>&type=alles">
                            <div class="vervalContainer">
                                <h5><%= user %></h5>
                                <span class="gebeurtenisTijdstip">
                                    <% var date = DateTime.Now;
                                       var diff = date.Subtract(w.Time).TotalSeconds;
                                       if (diff < 60)
                                       {
                                    %><%= Math.Floor(diff) %> seconden geleden<%
                                       }
                                       else if (diff < 120)
                                       {
                                                                              %><%= Math.Floor(diff / 60) %> minuut geleden<%
                                       }
                                       else if (diff < 3600)
                                       {
                                                                                                                           %><%= Math.Floor(diff / 60) %> minuten geleden<%
                                       }
                                       else if (diff < 86400)
                                       {
                                                                                                                                                                         %><%= Math.Floor(diff / 3600) %> uur geleden<%
                                       }
                                       else if (diff < 172800)
                                       {
                                                                                                                                                                                                                     %><%= Math.Floor(diff / 86400) %> dag geleden<%
                                       }
                                       else
                                       {
                                                                                                                                                                                                                                                                  %><%= Math.Floor(diff / 86400) %> dagen geleden<%
                                       } %>
                                </span>
                                <span class="gebeurtenisInfo"><%= msg %>
                                </span>
                            </div>
                        </a>
                           
                          
                            
                   
              
                <% }
                   } %>
                   </article>
            </div>
                </div>
            <%
                if (VermisteArtikelens != null)
                {
                    if (VermisteArtikelens.Count != 0)
                    {
                        
                   
                    %>
            <div>
                <h4 class="dashboardTitel">Niet teruggkeerde artikelen</h4>
                <asp:GridView runat="server" ID="VermisteArtikelen" AutoGenerateColumns="False"
                              CssClass="tablesorter table table-hover table-bordered">
                    <Columns>
                        <asp:BoundField DataField="Artikel.Merk.Naam" HeaderText="Merk"/>
                        <asp:BoundField DataField="Artikel.Naam" HeaderText="Artikel"/>
                        <asp:BoundField DataField="Aantal" HeaderText="Aantal"/>
                        <asp:BoundField DataField="CombNaam" HeaderText="Checklist"/>
                        <asp:BoundField DataField="CombDatum" HeaderText="Datum"  DataFormatString="{0:dd MMMM yyyy}"/>
                        <asp:BoundField DataField="UserName" HeaderText="Verantwoordelijke"/>
                        <asp:TemplateField ShowHeader="False">
                            <ItemStyle CssClass="center-icon"/>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="btnAdd" Text="Delete" OnCommand="btnAdd_OnCommand"  CommandArgument='<%# Eval("Id") %>' CommandName="Delete2">
                                    <span class="glyphicon glyphicon-trash"></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div> 
            <% }
                } %>
           

        </div>
        <div class="col-md-3 height100">
            <!-- Timeline  -->
            <% if (EventsEnOrders.Count != 0)
               {
                   %>
                <div class="timelineWrapper">
                    <h4 class="dashboardTitel">Timeline</h4>
                    <article class="Timeline">
                        <div id="timelineParent">
                            <div id="timeline">
                                <%foreach (var i in EventsEnOrders)
                                  {
                                      var evnt = "";
                                      if (i.IsEvent) evnt = "Event";
                                      else evnt = "Order";%>
                                    <div class="timeline-item">
                                        <div class="timeline-icon">
                                            <%if (i.IsEvent)
                                              {%>
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            <%}
                                              else
                                              {%>
                                                <i class="glyphicon glyphicon-shopping-cart"></i>
                                            <%}%>
                                        </div>
                                        <div class="timeline-content">

                                            <a href="../<%=evnt%>/Overzicht.aspx?id=<%=i.Id%>&type=komende">
                                                <h2><%=i.Naam %></h2>
                                            </a>
                                            <p>
                                                <%=i.Subtext %>
                                            </p>
                                        </div>
                                    </div>
                                <%}%>
                            </div>

                        </div>
                    </article>
                </div>    
            <%
               } %>
           
        </div>
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        function pageLoad(sender, args) {
            $(".tablesorter").tablesorter({ dateFormat: "uk" });
        }

        function showWarning() {
            BootstrapDialog.show({
                title: 'Let op!',
                message: 'Bent u zeker dat u dit artikel uit de lijst wilt verwijderen?',
                buttons: [{
                        label: 'Annuleren',
                        cssClass: 'cancelButton',
                        action: function (dialogRef) {
                            dialogRef.close();
                        }
                    },
                    {
                        label: 'Verwijder',
                        cssClass: 'btnNieuw',
                        hotkey: 13,
                        action: function (dialogRef) {
                            $('#MainContent_hdnVerwijder').val("verwijder");
                            __doPostBack('<%= UpdatePanel.ClientID %>', null);
                            dialogRef.close();
                        }
                    }
                ],
                closable: false
            });
        }

        function showError() {
            BootstrapDialog.show({
                title: 'Niet voldoende rechten!',
                message: 'U heeft niet voldoende rechten om een artikel te verwijderen.',
                buttons: [{
                        label: 'Sluiten',
                        cssClass: 'cancelButton',
                        action: function (dialogRef) {
                            dialogRef.close();
                        }
                    }
                ],
                closable: false
            });
        }

        function showConfirm() {
            BootstrapDialog.show({
                title: 'Artikel verwijderd.',
                message: 'Het artikel is succesvol verwijderd.',
                buttons: [{
                        label: 'Sluiten',
                        cssClass: 'cancelButton',
                        action: function (dialogRef) {
                            window.location.reload(false);
                        }
                    }
                ],
                closable: false
            });
        }



    </script>




</asp:Content>
