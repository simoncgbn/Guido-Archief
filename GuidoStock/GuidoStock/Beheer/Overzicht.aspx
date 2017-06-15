<%@ Page Title="Beheer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Overzicht.aspx.cs" EnableEventValidation="false" CodeFile="Overzicht.aspx.cs" Inherits="GuidoStock.Beheer.Overzicht" %>

<asp:Content runat="server" ID="content" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel runat="server" ID="updatePanel" UpdateMode="Always" ChildrenAsTriggers="True">
        <ContentTemplate>
            <div class="row beheer">
                <ul class="nav nav-tabs">

                    <li id="listGebruikers"><a data-toggle="tab" href="#gebruikers">Gebruikers</a></li>
                    <li id="listRechten"><a data-toggle="tab" href="#rechten">Rollen</a></li>
                    <li id="listStocks"><a data-toggle="tab" href="#stocks">Stocks</a></li>
                    <li id="listTransporten"><a data-toggle="tab" href="#transporten">Transporten</a></li>
                </ul>

                <div class="tab-content">
                    <div id="gebruikers" class="tab-pane fade">
                        <div class="row gebruikersLijst">
                            <asp:GridView runat="server" ID="GebruikersGridView" AutoGenerateColumns="False"
                                CssClass="tablesorter table table-hover table-bordered">
                                <Columns>
                                    <asp:BoundField DataField="User.Name" HeaderText="Naam"/>
                                    <asp:BoundField DataField="User.Email" HeaderText="Email"/>
                                    <asp:BoundField  DataField="Rol" HeaderText="Rol"/>
                                   <%-- <asp:TemplateField HeaderText = "Rol">
                                        <ItemTemplate>
                                            <asp:Label ID="Rol" runat="server" Text='<%# Eval("Rol") %>' Visible = "false" />
                                            <asp:DropDownList ID="ddlRol" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemStyle CssClass="center-icon"/>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="btnAdd" Text="Delete" OnCommand="btnAdd_OnCommand" CommandArgument='<%# Eval("User.Email") %>' CommandName="Delete2">
                                            <span class="glyphicon glyphicon-trash"></span>
                                        </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div id="changeRechten" class="row">
                            <h4>Rol wijzigen</h4>
                            <hr/>
                            <div class="form-group col-md-12 ddlRMR">
                                <div class="col-md-3">
                                    <asp:DropDownList DataTextField="UserName" DataValueField="UserId" id="ddlGebruiker" runat="server" CssClass="form-control ddlNoAdd"/>                                    
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList DataTextField="Name" DataValueField="Id" ID="ddlRecht" runat="server" CssClass="form-control ddlNoAdd"/>
                                </div>
                                <asp:LinkButton runat="server" id="btnChangeRole" OnClick="btnChangeRole_OnClick" CssClass="btn btn-default btnNieuw ">Opslaan</asp:LinkButton>
                            </div>
                        </div>
                        <div id="addRechten" class="row">
                            <h4>Rol toevoegen</h4>
                            <hr/>
                            <div class="form-group col-md-12 ddlRMR">
                                <div class="col-md-3">
                                    <asp:TextBox id="txtRolNaam" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:LinkButton runat="server" id="btnAddRole" OnClick="btnAddRole_OnClick" CssClass="btn btn-default btnNieuw ">Opslaan</asp:LinkButton>
                            </div>
                        </div>
                        <div id="removeRechten" class="row">
                            <h4>Rol verwijderen</h4>
                            <hr/>
                            <div class="form-group col-md-12 ddlRMR">
                                <div class="col-md-3">
                                    <asp:DropDownList DataTextField="Name" DataValueField="Id" ID="ddlRemoveRecht" runat="server" CssClass="form-control ddlNoAdd"/>
                                </div>
                                <asp:LinkButton runat="server" id="btnRemoveRole" OnClick="btnRemoveRole_OnClick" CssClass="btn btn-danger">Verwijder</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div id="rechten" class="tab-pane fade">
                        <div class="row rechtenLijst">
                            <asp:GridView runat="server" ID="RechtenGridView" AutoGenerateColumns="False"
                                CssClass="tablesorter table table-hover table-bordered">
                                <Columns>
                                    <asp:BoundField DataField="Rol" HeaderText="Naam"/>
                                    <asp:BoundField DataField="Module" HeaderText="Onderdeel"/>
                                    <asp:BoundField DataField="Rechten" HeaderText="Rechten"/>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div id="addControls" class="row">
                            <div class="form-group col-md-12 ddlRMR">
                                <div class="col-md-3">
                                    <asp:DropDownList DataTextField="Name" DataValueField="Id" id="ddlRol" runat="server" CssClass="form-control ddlNoAdd"/>                                    
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList DataTextField="Naam" DataValueField="Id" ID="ddlModule" runat="server" CssClass="form-control ddlNoAdd"/>
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlRechten" runat="server" CssClass="form-control ddlNoAdd"/>
                                </div>
                                <asp:LinkButton runat="server" OnClick="OnClick" CssClass="btn btn-default btnNieuw ">Opslaan</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                     <div id="stocks"  class="tab-pane fade">
                        <div class="row stockslijst">
                            <asp:GridView ID="StockGridView" runat="server" AutoGenerateColumns="False"
                                CssClass="tablesorter table table-hover table-bordered">
                                <Columns>
                                    <asp:BoundField DataField="Code" HeaderText="Naam"/>
                                    <asp:BoundField DataField="AantalStocks" HeaderText="Aantal stocks"/>
                                    <asp:BoundField DataField="AantalArtikelen" HeaderText="Aantal artikelen"/>
                                    <asp:TemplateField ShowHeader="True" HeaderText="Barcode">
                                        <ItemStyle CssClass="center-icon" Width="10%"/>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="btnBarcode" CommandArgument='<%# Eval("BarcodeImage") %>' CommandName="Barcode" OnClick="btnBarcode_OnClick">
                                                <asp:Image ID="Barcode" runat="server" ImageUrl='<%# Eval("BarcodeImage") %>' style="border-width: 0px;" />
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemStyle CssClass="center-icon"/>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="btnEdit" Text="EditRow" CommandArgument='<%# Eval("Code") %>' CommandName="EditRow" OnClick="btnEdit_OnClick">
                                                <span class="glyphicon glyphicon-edit"></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <h4>Nieuwe locatie</h4>
                            <hr />
                             <div class="col-xs-5 col-sm-4 col-md-3 col-lg-2">
        <div class="form-group">
            <asp:Label runat="server" ID="lblNaam" AssociatedControlID="txtCode" CssClass="control-label">
                Naam
            </asp:Label><asp:TextBox runat="server" ID="txtCode" CssClass="form-control" />
            <asp:RequiredFieldValidator ID="rfvAantal" runat="server" ToolTip="Required" ControlToValidate="txtCode" ValidationGroup="group1" ErrorMessage="Naam is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />
        </div>
    </div>
                
    <div class="col-xs-2 col-sm-2 col-md-2 col-lg-1">
        <div class="form-group">
            <asp:Label runat="server" ID="Label3" AssociatedControlID="VoegToe" CssClass="control-label">
                &nbsp;  &nbsp; &nbsp; &nbsp; &nbsp;
            </asp:Label>
            <asp:LinkButton runat="server" ID="VoegToe" class="btn btn-primary btnNieuw "  CssClass="btn btn-primary CheckUitStandaard" OnClick="VoegToe_Click" ValidationGroup="group1" CausesValidation="true" >
                Voeg toe
            </asp:LinkButton>
        </div>
    </div>
                        </div>
                    </div>
                     <div id="transporten" class="tab-pane fade">
                        <div class="row transportlijst">
                            <asp:GridView runat="server" ID="TransportengridView" AutoGenerateColumns="False"
                                CssClass="tablesorter table table-hover table-bordered">
                                <Columns>
                                    <asp:BoundField DataField="Naam" HeaderText="Naam"/>
                                    <asp:BoundField DataField="MaxGewicht" HeaderText="Maximum gewicht (kg)"/>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemStyle CssClass="center-icon"/>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="btnEditTransport" Text="EditRow" CommandArgument='<%# Eval("Naam") %>' CommandName="EditRow" OnClick="btnEditTransport_OnClick">
                                                <span class="glyphicon glyphicon-edit"></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemStyle CssClass="center-icon"/>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="btnDeleteTransport" Text="Delete" CommandArgument='<%# Eval("Naam") %>' CommandName="DeleteRow" OnClick="btnDeleteTransport_OnClick">
                                                <span class="glyphicon glyphicon-trash"></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                             <h4>Nieuw transport</h4>
                            <hr />
                             <div class="col-xs-5 col-sm-4 col-md-3 col-lg-2">
        <div class="form-group">
            <asp:Label runat="server" ID="Label1" AssociatedControlID="txtTransport" CssClass="control-label">
                Naam
            </asp:Label><asp:TextBox runat="server" ID="txtTransport" CssClass="form-control" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ToolTip="Required" ControlToValidate="txtTransport" ValidationGroup="group2" ErrorMessage="Naam is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />
        </div>
    </div>
                              <div class="col-xs-5 col-sm-4 col-md-3 col-lg-2">
        <div class="form-group">
            <asp:Label runat="server" ID="Label4" AssociatedControlID="txtGewicht" CssClass="control-label">
                Max. gewicht (kg)
            </asp:Label><asp:TextBox runat="server" ID="txtGewicht" CssClass="form-control" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ToolTip="Required" ControlToValidate="txtGewicht" ValidationGroup="group2" ErrorMessage="Gewicht is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />
            <asp:RegularExpressionValidator
                    ID="revAantal"
                    ControlToValidate="txtGewicht"
                    ValidationExpression="^[0-9]*$"
                    ErrorMessage="Gewicht moet een nummer zijn"
                    runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group2" />
        </div>
    </div>
                
    <div class="col-xs-2 col-sm-2 col-md-2 col-lg-1">
        <div class="form-group">
            <asp:Label runat="server" ID="Label2" AssociatedControlID="VoegToe" CssClass="control-label">
                &nbsp;  &nbsp; &nbsp; &nbsp; &nbsp;
            </asp:Label>
            <asp:LinkButton runat="server" ID="LinkButton1" class="btn btn-primary btnNieuw "  CssClass="btn btn-primary CheckUitStandaard" OnClick="VoegTransportToe_Click" ValidationGroup="group2" CausesValidation="true" >
                Voeg toe
            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>

        function pageLoad(sender, args) {
            $('.ddlNoAdd').select2({
            });
        }

        function setActive(type) {
            if (type == "gebruikers") {
                $("#listGebruikers").addClass("active");
                $("#gebruikers").addClass("in active");
            }
            else if (type == "stock") {
                $("#listStocks").addClass("active");
                $("#stocks").addClass("in active");
            }
            else if (type == "transport") {
                $("#listTransporten").addClass("active");
                $("#transporten").addClass("in active");
            }
            else if (type == "rechten") {
                $("#listRechten").addClass("active");
                $("#rechten").addClass("in active");
            }
        }

        function editLocatie(code) {
            BootstrapDialog.show({
                title: 'Bewerk locatie '+code,
                message: '<span>Naam: </span>'  + '<input id="inputNieuwNaam" type="text" class="form-control" placeholder="'+code+'" >',
                buttons: [{
                    label: 'Annuleren',
                    cssClass: 'cancelButton',
                    action: function (dialogRef) {
                        dialogRef.close();
                    }
                },
                    {
                        label: 'Opslaan',
                        cssClass: 'btnNieuw',
                        hotkey: 13,
                        action: function () { window.location.href = '/Beheer/Overzicht.aspx?id=stock&oud='+code+"&nieuw="+ $("#inputNieuwNaam").val(); }
                    }
                ]
            });
        }

        function editTransport(transport, gewicht) {
            BootstrapDialog.show({
                title: 'Bewerk transport ' + transport,
                message: '<span>Naam: </span>' + '<input id="inputNieuwNaam" type="text" class="form-control" placeholder="' + transport + '">'
                + '<span>Maximum gewicht (kg): </span>' + '<input id="inputGewicht" type="text" class="form-control" placeholder="' + gewicht + '">'
                ,
                buttons: [{
                    label: 'Annuleren',
                    cssClass: 'cancelButton',
                    action: function (dialogRef) {
                        dialogRef.close();
                    }
                },
                    {
                        label: 'Opslaan',
                        cssClass: 'btnNieuw',
                        hotkey: 13,
                        action: function () { window.location.href = '/Beheer/Overzicht.aspx?id=transport&oud=' + transport + "&nieuw=" + $("#inputNieuwNaam").val() + '&gewicht=' + $("#inputGewicht").val(); }
                    }
                ]
            });
        }

        function error(type) {

            if (type == "stock") {
                $("#listStocks").addClass("active");
                $("#stocks").addClass("in active");
            } else if (type == 'transportnaam') {
                $("#listTransporten").addClass("active");
                $("#transporten").addClass("in active");
            }

            BootstrapDialog.show({
                title: 'Naam is al in gebruik',
                message: 'Deze ' + type + ' is al in gebruik. Gelieve een andere naam te kiezen of de huidige naam aan te passen.',
                buttons: [
                    {
                        label: 'Terug',
                        cssClass: 'btnNieuw',
                        hotkey: 13,
                        action: function (dialogRef) {
                            dialogRef.close();
                        }
                    }
                ]
            });

        }

        function deleteTransport(naam, id) {
            BootstrapDialog.show({
                title: 'Verwijder transport ' + naam,
                message: 'Bent u zeker dat u dit transport wilt verwijderen?'
                ,
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
                        action: function () { window.location.href = '/Beheer/Overzicht.aspx?id=transport&oud=' + naam + "&v=true"; }
                    }
                ]
            });
        }

        function deleteGebruiker(naam, id) {
            BootstrapDialog.show({
                title: 'Verwijder gebruiker ' + naam,
                message: 'Bent u zeker dat u deze gebruiker wilt verwijderen? Opgepast: deze gebruikers worden aangemaakt' +
                    ' wanneer een persoon met een AD account aanmeldt. Als deze persoon niet verwijderd is uit de AD zal deze nog ' +
                    'steeds kunnen aanmelden, weliswaar zonder rechten.'
                ,
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
                        action: function () { window.location.href = '/Beheer/Overzicht.aspx?deleteGebruiker=' + id; }
                    }
                ]
            });
        }

        function navigateTo(url) {
            $("#listStocks").addClass("active");
            $("#stocks").addClass("in active");
            window.open(url, '_blank');
        }
    </script>
</asp:Content>
