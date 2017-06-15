<%@ Page Title="Scannen event" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="EventScannenLopend.aspx.cs" Inherits="GuidoStock.Scannen.EventScannenLopend" EnableEventValidation="false" %>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanelEventScannenLopend" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>

            <asp:HiddenField ID="hdnBarcode" runat="server" Value="" OnValueChanged="hdnBarcode_OnValueChanged" />
            <asp:HiddenField ID="hdnLocatie" runat="server" Value="" OnValueChanged="hdnLocatie_OnValueChanged" />
            <asp:HiddenField ID="hdnGaDoorKeuze" runat="server" Value="" OnValueChanged="hdnGaDoorKeuze_OnValueChanged"  />
            <asp:HiddenField ID="hdnArtikelBeschadigdAantal" runat="server" Value="" />
            <asp:HiddenField ID="hdnArtikelBeschadigdOmschrijving" runat="server" Value="" />


            <div>
                <div class="infoBar">


                    <asp:LinkButton runat="server" ID="GaDoor" class="btn btn-default  " OnClick="GaDoor_OnClick">
                    Ga door
                    </asp:LinkButton>
                </div>
                <asp:GridView runat="server" ID="ChecklistGridView" AutoGenerateColumns="False"
                    CssClass="tablesorter table table-hover table-bordered">
                    <Columns>
                        <asp:BoundField DataField="Stock.Artikel.Merk.Naam" HeaderText="Merk" />
                        <asp:BoundField DataField="Stock.DisplayNaam2" HeaderText="Product" />
                        <asp:BoundField DataField="AantalWeg" HeaderText="Aantal" />

                    </Columns>
                </asp:GridView>

                <div class="col-xs-4 col-sm-4 col-md-3 col-lg-3">
                    <div class="form-group">
                        <asp:Label runat="server" ID="lblBarcode" AssociatedControlID="Barcode" CssClass="control-label">
                    Barcode
                        </asp:Label><asp:TextBox runat="server" ID="Barcode" CssClass="form-control" />
                        <span id="ErrorBarcode" runat="server" class="text-danger errorMessage" style="display: none">Foutieve barcode</span>

                    </div>
                </div>

                <div class="col-xs-4 col-sm-4 col-md-3 col-lg-3">
                    <div class="form-group">
                        <asp:Label runat="server" ID="lblLocatie" AssociatedControlID="Locatie" CssClass="control-label">
                    Locatie
                        </asp:Label><asp:DropDownList ID="Locatie" runat="server" DataTextField="Code" Style="width: 100%;" Enabled="False" CssClass="form-control select2Item ddlLocatie" DataValueField="Id">
                        </asp:DropDownList>
                    </div>
                </div>
                
                <div class="col-xs-4 col-sm-4 col-md-2 col-lg-2">
                    <div class="form-group">
                        <asp:Label runat="server" ID="Label2" AssociatedControlID="Vervaldatum" CssClass="control-label">
                            Vervaldatum <span>- Optioneel</span>
                        </asp:Label><asp:TextBox ID="Vervaldatum" runat="server" Enabled="False" CssClass="form-control date" >
                        </asp:TextBox>
                    </div>
                </div>
                

                <div class="col-xs-4 col-sm-3 col-md-1 col-lg-2">

                    <div class="form-group">
                        <asp:Label runat="server" ID="lblAantal" AssociatedControlID="Aantal" CssClass="control-label">
                                                                                                  Aantal
                        </asp:Label><asp:TextBox runat="server" ID="Aantal" CssClass="form-control" ReadOnly="True" />
                        <asp:RequiredFieldValidator ID="rfvAantal" runat="server" ToolTip="Required" ControlToValidate="Aantal" ValidationGroup="group1" ErrorMessage="Aantal is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />
                        <asp:RegularExpressionValidator
                            ID="revAantal"
                            ControlToValidate="Aantal"
                            ValidationExpression="^[0-9]*$"
                            ErrorMessage="Aantal moet een nummer zijn"
                            runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                    </div>
                </div>








                <div class="col-xs-2 col-sm-1 col-md-1 col-lg-1">
                    <div class="form-group">
                        <label class="control control--checkbox">
                            Beschadigd?
                    <input runat="server" type="checkbox" id="Beschadigd" onchange="beschadigd(this)" />
                            <div class="control__indicator"></div>
                        </label>
                    </div>
                </div>


                <div class="col-xs-6 col-sm-2 col-md-2 col-lg-1">
                    <div class="form-group">
                        <asp:Label runat="server" ID="Label1" AssociatedControlID="CheckUit" CssClass="control-label">
                    &nbsp;  &nbsp; &nbsp; &nbsp; &nbsp;
                        </asp:Label>
                        <asp:LinkButton runat="server" ID="CheckUit" class="btn btn-primary btnNieuw " CssClass="btn btn-primary CheckUitDisabled CheckUitStandaard" OnClick="CheckUit_OnClick" ValidationGroup="group1" CausesValidation="true" Enabled="False">
                    Check in
                        </asp:LinkButton>
                    </div>
                </div>

            
            </div>

        </ContentTemplate>

    </asp:UpdatePanel>

    <script>

        //window.onbeforeunload = function (e) {
        //    var message = "Indien u de pagina nu verlaat, zal de checklist niet volledig ingescand zijn!",
        //        e = e || window.event;
        //    // For IE and Firefox
        //    if (e) {
        //        e.returnValue = message;
        //    }

        //    // For Safari
        //    return message;
        //};

        function pageLoad(sender, args) {
            $('.ddlLocatie').select2({
            });

            flatpickr('.date',
                {
                    altFormat: "j F, Y",
                    altInput: true,
                    locale: 'nl'
                }
            );


            $('#MainContent_Barcode').focus();

            $('#MainContent_Barcode').change(function () {
                $('#MainContent_hdnBarcode').val($('#MainContent_Barcode').val());
                __doPostBack('<%= UpdatePanelEventScannenLopend.ClientID %>', null);
            });

            $("#MainContent_Locatie").on("change",
                function () {
                    $('#MainContent_hdnLocatie').val($('#MainContent_Locatie').val());
                    __doPostBack('<%= UpdatePanelEventScannenLopend.ClientID %>', null);

                });

                var checkbox = $("#MainContent_Toggle");

                checkbox.change(function (event) {
                    var checkbox = event.target;
                    if (checkbox.checked) {
                        $('#MainContent_hdnToggle').val("Uitgaand");
                        __doPostBack('<%= UpdatePanelEventScannenLopend.ClientID %>', null);
                } else {
                    $('#MainContent_hdnToggle').val("Inkomend");
                    __doPostBack('<%= UpdatePanelEventScannenLopend.ClientID %>', null);
                }
            });

        }

        function showErrorMsg(eventId) {
            BootstrapDialog.show({
                title: 'Geen checklist.',
                message: 'Er is voor dit event geen checklist beschikbaar. Gelieve eerst een checklist aan te maken.',
                buttons: [{
                    label: 'Terug',
                    cssClass: 'cancelButton',
                    action: function (dialogRef) {
                        window.location.href = '/Scannen/Event.aspx';
                    }
                },
                    {
                        label: 'Bekijk event',
                        cssClass: 'btnNieuw',
                        hotkey: 13,
                        action: function () { window.location.href = '/event/Overzicht.aspx?id=' + eventId; }
                    }
                ],
                closable: false
            });
        }

        function showSuccesMsg(eventId) {
            BootstrapDialog.show({
                title: 'Checklist voltooid!',
                message: 'De checklist is succesvol ingescand.',
                buttons: [{
                    label: 'Terug',
                    cssClass: 'cancelButton',
                    action: function (dialogRef) {
                        window.location.href = '/Scannen/Event.aspx';
                    }
                },
                    {
                        label: 'Bekijk event',
                        cssClass: 'btnNieuw',
                        hotkey: 13,
                        action: function () { window.location.href = '/event/Overzicht.aspx?id=' + eventId; }
                    }
                ],
                closable: false
            });
        }

        function showSucces(eventId) {
            BootstrapDialog.show({
                title: 'Checklist voltooid!',
                message: 'De checklist is succesvol ingescand.',
                buttons: [{
                    label: 'Terug',
                    cssClass: 'cancelButton',
                    action: function (dialogRef) {
                        window.location.href = '/Scannen/Event.aspx';
                    }
                },
                    {
                        label: 'Bekijk event',
                        cssClass: 'btnNieuw',
                        hotkey: 13,
                        action: function () { window.location.href = '/event/Overzicht.aspx?id=' + eventId + "&type=verlopen"; }
                    }
                ],
                closable: false
            });
        }

        function showSuccesMsg2(orderid) {
            BootstrapDialog.show({
                title: 'Checklist voltooid!',
                message: 'De checklist is succesvol uitgescand.',
                buttons: [{
                    label: 'Terug',
                    cssClass: 'cancelButton',
                    action: function (dialogRef) {
                        window.location.href = '/Scannen/Event.aspx';
                    }
                },
                    {
                        label: 'Bekijk order',
                        cssClass: 'btnNieuw',
                        hotkey: 13,
                        action: function () { window.location.href = '/order/Overzicht.aspx?id=' + orderid; }
                    }
                ],
                closable: false
            });
        }

        function showSucces2(orderid) {
            BootstrapDialog.show({
                title: 'Checklist voltooid!',
                message: 'De checklist is succesvol ingescand.',
                buttons: [{
                    label: 'Terug',
                    cssClass: 'cancelButton',
                    action: function (dialogRef) {
                        window.location.href = '/Scannen/Event.aspx';
                    }
                },
                    {
                        label: 'Bekijk order',
                        cssClass: 'btnNieuw',
                        hotkey: 13,
                        action: function () { window.location.href = '/order/Overzicht.aspx?id=' + orderid + "&type=verlopen"; }
                    }
                ],
                closable: false
            });
        }

        function showWarning(eventId) {
            BootstrapDialog.show({
                title: 'Let op!',
                message: 'De checklist is momentel niet volledig ingescand. Wilt u doorgaan?',
                type: BootstrapDialog.TYPE_WARNING,
                buttons: [{
                        label: 'Annuleren',
                        cssClass: 'cancelButton',
                        action: function (dialogRef) {
                            dialogRef.close();
                        }
                    },
                    {
                        label: 'Checklist voltooien',
                        cssClass: 'btnNieuw',
                        hotkey: 13,
                        action: function (dialogRef) {
                            $('#MainContent_hdnGaDoorKeuze').val("opslaan");
                            __doPostBack('<%= UpdatePanelEventScannenLopend.ClientID %>', null);
                            dialogRef.close();
                        }
                    },
                    {
                        label: 'Scannen onderbreken',
                        cssClass: 'btnNieuw',
                        hotkey: 13,
                        action: function (dialogRef) {
                            $('#MainContent_hdnGaDoorKeuze').val("onderbreken");
                            __doPostBack('<%= UpdatePanelEventScannenLopend.ClientID %>', null);
                            dialogRef.close();
                        }
                    }
                ],
                closable: true
            });
        }



        function fillLocatie(aantal) {
            $("#MainContent_Aantal").TouchSpin({
                verticalbuttons: true,
                max: aantal
            });

        }

        function beschadigd(element) {
            if ($('#MainContent_Beschadigd').is(':checked')) {
                BootstrapDialog.show({
                    title: 'Artikel beschadigd',
                    message: '<span>Aantal: </span>' + '<input id="inputNaam" class="form-control" type="number">'
                        + '<span>Omschrijving: </span>' + '<textarea rows="10" cols="80" id="inputOmschrijving" class="form-control">',
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
                            action: function (dialogRef) {
                                $('#MainContent_hdnArtikelBeschadigdAantal').val($('#inputNaam').val());
                                $('#MainContent_hdnArtikelBeschadigdOmschrijving').val($('#inputOmschrijving').val());
                                dialogRef.close();
                            }
                        }
                    ],
                    closable: true
                });
            }
        }

    </script>

</asp:Content>
