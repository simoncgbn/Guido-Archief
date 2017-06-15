<%@ Page Title="Event" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddEvent.aspx.cs" EnableEventValidation="false" Inherits="GuidoStock.Event.AddEvent" %>

<%@ Import Namespace="GuidoStock.Code" %>


<%@ Register Src="../Controls/organisatie.ascx" TagName="organisatie" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/TransportControl.ascx" TagPrefix="uc1" TagName="TransportControl" %>
<%@ Register Src="~/Controls/TeamlidControl.ascx" TagPrefix="uc1" TagName="TeamlidControl" %>




<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
<ContentTemplate>
<asp:HiddenField ID="visibleStep" runat="server" Value="" />

<asp:HiddenField ID="hdnLocCheck" runat="server" Value="" />
<asp:HiddenField ID="hdnDefDate" runat="server" Value="" /> 
<asp:HiddenField ID="hdnMedewerkersLaden" runat="server" Value="" OnValueChanged="hdnMedewerkersLaden_ValueChanged" />
<asp:HiddenField ID="hdnMedewerkersOpbouw" runat="server" Value="" OnValueChanged="hdnMedewerkersLaden_ValueChanged" />
<asp:HiddenField ID="hdnMedewerkersAfbouw" runat="server" Value="" OnValueChanged="hdnMedewerkersLaden_ValueChanged" />
<asp:HiddenField ID="hdnMedewerkersActie" runat="server" Value="" OnValueChanged="hdnMedewerkersLaden_ValueChanged" />


<div class="section" id="AddEventSection"> 
<div class="row">
    <div class="article wizard col-md-6">
        <div class="connecting-line"></div>

        <div id="wizardStep1" class="wizard-step activeStep">
            <asp:LinkButton runat="server" ID="btnStep1" class="button btn-circle ">
                <i id="check1" class="glyphicon glyphicon-ok iconDisabled"></i> <span>1</span>
            </asp:LinkButton>

            <p>DETAILS</p>

        </div>
        <div id="wizardStep2" class="wizard-step ">
            <asp:LinkButton runat="server" ID="btnStep2" class="button btn-circle ">
                <i id="check2" class="glyphicon glyphicon-ok iconDisabled"></i> <span>2</span>
            </asp:LinkButton>

            <p>PLANNING</p>

        </div>

        <div id="wizardStep3" class="wizard-step last">

            <asp:LinkButton runat="server" ID="btnStep3" class="button btn-circle ">
                <i id="check3" class="glyphicon glyphicon-ok iconDisabled"></i> <span>3</span>
            </asp:LinkButton>
            <p>CONTACTEN</p>

        </div>


    </div>
</div>
<div id="step1" class="stepActive">
    <div class="article" runat="server" id="Algemeen">

        <h4>Algemeen</h4>
        <hr />
        <div class="row">
            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2 ">
                <div class="form-group">

                    <asp:Label runat="server" ID="nieuweventnaamlbl" AssociatedControlID="NieuwEventNaam" CssClass="control-label">
                        Naam

                    </asp:Label><asp:TextBox runat="server" ID="NieuwEventNaam" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvEventNaam" runat="server" ToolTip="Required" ControlToValidate="NieuwEventNaam" ValidationGroup="group1" ErrorMessage="Naam is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />
                    <asp:RegularExpressionValidator
                        ID="revEventNaam"
                        ControlToValidate="NieuwEventNaam"
                        ValidationExpression="^.{3,}$"
                        ErrorMessage="Minimum 3 karakters"
                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                </div>
            </div>

            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                <div class="form-group">
                    <asp:Label runat="server" ID="datumlbl" AssociatedControlID="Datum" CssClass="control-label">
                        Datum
                    </asp:Label>
                    <asp:TextBox runat="server" type="date" ID="Datum" Name="Datum" CssClass="form-control datum" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ToolTip="Required" ControlToValidate="Datum" ValidationGroup="group1" ErrorMessage="Datum is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />

                </div>
            </div>


            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                <div class="form-group">
                    <asp:Label runat="server" ID="opdrachtgeverlbl" AssociatedControlID="ddlOpdrachtgever" CssClass="control-label">
                        Opdrachtgever
                    </asp:Label><asp:DropDownList ID="ddlOpdrachtgever" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control ddlNoAdd ddlOpdrachtgever" DataValueField="Id">
                    </asp:DropDownList>
                </div>
            </div>

            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                <div class="form-group">
                    <asp:Label runat="server" ID="Label1" AssociatedControlID="ddlCoordinator" CssClass="control-label">
                        Coördinator
                    </asp:Label><asp:DropDownList ID="ddlCoordinator" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control ddlNoAdd ddlCoordinator" DataValueField="Id">
                    </asp:DropDownList>
                </div>
            </div>

            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                <div class="form-group">
                    <asp:Label runat="server" ID="Label13" AssociatedControlID="ddlFieldManager" CssClass="control-label">
                        Guido fieldmanager
                    </asp:Label>
                    <asp:DropDownList ID="ddlFieldManager" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control ddlNoAdd ddlFieldManager" DataValueField="Id">
                    </asp:DropDownList>

                </div>
            </div>
        </div>
    </div>

    <div class="article" id="Div1">
        <h4>Organisatie</h4>
        <hr />
        <div class="row">
            <div id="OrganisatieLijst" class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                <asp:Repeater ID="organisatieRepeater" runat="server"> 
                    <ItemTemplate>
                        <asp:LinkButton runat="server" Text='<%# ((EvenementLocatie)Container.DataItem).EvenementKlant.Organisatie %>' OnCommand="OnCommand" CommandName="load" CommandArgument='<%# ((EvenementLocatie)Container.DataItem).TempIndex %>' />
                        <br/>
                    </ItemTemplate>
                </asp:Repeater>
                <%--<% foreach (var organisatie in Organisaties)
                                   {%>
                                <asp:LinkButton runat="server" Text='<%# Eval("") %>' OnCommand="OnCommand" CommandName="load" CommandArgument='<%# Eval("organisatie") %>' />
                                <% } %>--%>
            </div>
            <uc1:organisatie ID="organisatie1" runat="server" />
        </div>
        <div class="row">
            <div class="col-md-2 col-md-offset-2">
                <asp:LinkButton runat="server" ID="KenmerkVerwijderen" class="button add_location_button locatieBtn" CssClass="hiddenRow">
                    - Organisatie verwijderen
                </asp:LinkButton>
                <asp:LinkButton runat="server" ID="kenmerkToevoegen" CausesValidation="false" ValidationGroup="group2" OnClick="kenmerkToevoegen_OnClick" class="button add_property_button">
                    + Organisatie opslaan
                </asp:LinkButton>                         
            </div>

        </div>
    </div>


    <a href="overzicht.aspx" runat="server" class="btn btn-danger cancelButton">Annuleren</a>

    <asp:LinkButton runat="server" ID="LinkButton1" class="btn btn-default btnVolgende" ValidationGroup="group1" CausesValidation="true">
        Volgende
    </asp:LinkButton>
</div>
<div id="step2" class="stepDisabled">
    <div class="article" id="Div3">
        <h4>Algemeen<span> - Optioneel</span></h4>
        <hr />
        <div class="row" runat="server">
            <asp:Table runat="server" ID="roleTable">
                <asp:TableHeaderRow ID="roleTableHeader" runat="server">
                    <asp:TableHeaderCell>Rol</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Begintijd</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Eindtijd</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Medewerkers</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow ID="roleTableRow1" runat="server">
                    <asp:TableCell>
                        <span>Laden transport</span></asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox runat="server" type="date" ID="BegintijdLaden" CssClass="form-control tijd" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox runat="server" type="date" ID="EindtijdLaden" CssClass="form-control tijd" />
                    </asp:TableCell><asp:TableCell>
                        <asp:ListBox ID="ddlMedewerkersLaden" SelectionMode="Multiple" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Multiple ddlMedewerkersLaden" multiple="multiple" DataValueField="Id"></asp:ListBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="roleTableRow2" runat="server">
                    <asp:TableCell>
                        <span>Opbouw</span></asp:TableCell><asp:TableCell>
                        <asp:TextBox runat="server" type="date" ID="BegintijdOpbouw" CssClass="form-control tijd" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox runat="server" type="date" ID="EindtijdOpbouw" CssClass="form-control tijd" />
                    </asp:TableCell><asp:TableCell>
                        <asp:ListBox ID="ddlMedewerkersOpbouw" SelectionMode="Multiple" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Multiple ddlMedewerkersOpbouw" multiple="multiple" DataValueField="Id"></asp:ListBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="roleTableRow3" runat="server">
                    <asp:TableCell>
                        <span>Actie</span></asp:TableCell><asp:TableCell>
                        <asp:TextBox runat="server" type="date" ID="BegintijdActie" CssClass="form-control tijd" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox runat="server" type="date" ID="EindtijdActie" CssClass="form-control tijd" />
                    </asp:TableCell><asp:TableCell>
                        <asp:ListBox ID="ddlMedewerkersActie" SelectionMode="Multiple" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Multiple ddlMedewerkersActie" multiple="multiple" DataValueField="Id"></asp:ListBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="roleTableRow4" runat="server">
                    <asp:TableCell>
                        <span>Afbouw</span></asp:TableCell><asp:TableCell>
                        <asp:TextBox runat="server" type="date" ID="BegintijdAfbouw" CssClass="form-control tijd" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox runat="server" type="date" ID="EindtijdAfbouw" CssClass="form-control tijd" />
                    </asp:TableCell><asp:TableCell>
                        <asp:ListBox ID="ddlMedewerkersAfbouw" SelectionMode="Multiple" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Multiple ddlMedewerkersAfbouw" multiple="multiple" DataValueField="Id"></asp:ListBox>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="article" id="Div4">

        <h4>Opmerking <span> - Optioneel</span></h4>
        <hr />
        <div class="row" runat="server">
            <div class="col-xs-12 ">
                <div class="form-group">
                    <asp:TextBox ID="Opmerking" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
        </div>
        <asp:LinkButton runat="server" ID="LinkButton5" class="btn btn-default btnVorige">
            Vorige
        </asp:LinkButton><asp:LinkButton runat="server" ID="LinkButton4" class="btn btn-default btnVolgende1" ValidationGroup="group22" CausesValidation="true">
            Volgende
        </asp:LinkButton>
    </div>
</div>
<div id="step3" class="stepDisabled">

    <div class="article" id="Div5">
        <h4>Transport</h4>
        <hr />
        <asp:Repeater ID="transportRepeater" runat="server"> 
            <ItemTemplate>
                <uc1:TransportControl ID="TransportControl" runat="server" Test='<%# (EvenementTransport)Container.DataItem%>'/>
            </ItemTemplate>
        </asp:Repeater>
        <asp:LinkButton runat="server" ID="LinkButton2" class="button add_location_button locatieBtn" CssClass="hiddenRow">
            - Transport verwijderen
        </asp:LinkButton><asp:LinkButton runat="server" ID="LinkButton3" class="button add_property_button" OnClick="LinkButton3_OnClick">
            + Transport toevoegen
        </asp:LinkButton>
    </div>
    <div class="article" id="Div2">

        <h4>Extra teamleden</h4>
        <hr />
        <asp:Repeater runat="server" ID="teamlidRepeater">
            <ItemTemplate>
                <uc1:TeamlidControl runat="server" ID="TeamlidControl" TeamLid='<%# (EvenementTeamLid)Container.DataItem %>'/>
            </ItemTemplate>
        </asp:Repeater>

        <asp:LinkButton runat="server" ID="LinkButton6" class="button add_location_button locatieBtn" CssClass="hiddenRow">
            - Teamlid verwijderen
        </asp:LinkButton><asp:LinkButton runat="server" ID="LinkButton7" class="button add_property_button" OnClick="LinkButton7_OnClick">
            + Teamlid toevoegen
        </asp:LinkButton>
    </div>
    <asp:LinkButton runat="server" ID="LinkButton8" class="btn btn-default btnVorige1">
        Vorige
    </asp:LinkButton><asp:LinkButton runat="server" ID="SaveEvent" class="btn btn-default btnNieuw " ValidationGroup="group33" CausesValidation="true" OnClick="SaveEvent_Click">
        Opslaan
    </asp:LinkButton>
</div>
</div>
</div>
</ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
    function pageLoad(sender, args) {
        if ($('#MainContent_visibleStep').val() === "1") {
            show1();
        } else if ($('#MainContent_visibleStep').val() === "2") {
            show2();
        } else if ($('#MainContent_visibleStep').val() === "3") {
            show3();
        }

        //$('.organisatieId').on('click', function () {
        //   $('#MainContent_hdnLinkId').val(this.innerHTML);
        //  __doPostBack('', null);
        //});

        function show1() {
            $('#step1').addClass("stepActive");
            $('#step1').removeClass("stepDisabled");

            $('#step2').removeClass("stepActive");
            $('#step2').addClass("stepDisabled");

            $('#step3').removeClass("stepActive");
            $('#step3').addClass("stepDisabled");

            $('#wizardStep1').removeClass("activeStep");

            $('#wizardStep2').addClass("activeStep");

            $('#wizardStep3').addClass("activeStep");

            $('#check1').addClass('iconDisabled');
            $('#wizardStep1 > a > span').removeClass('iconDisabled');

            $('#check2').addClass('iconDisabled');
            $('#wizardStep2 > a > span').removeClass('iconDisabled');

            $('#check3').addClass('iconDisabled');
            $('#wizardStep3 > a > span').removeClass('iconDisabled');
        }

        function show2() {
            $('#step1').removeClass("stepActive");
            $('#step1').addClass("stepDisabled");

            $('#step2').addClass("stepActive");
            $('#step2').removeClass("stepDisabled");

            $('#step3').removeClass("stepActive");
            $('#step3').addClass("stepDisabled");

            $('#wizardStep1').removeClass("activeStep");
            $('#wizardStep2').addClass("activeStep");
            $('#wizardStep3').removeClass("activeStep");

            $('#check1').removeClass('iconDisabled');
            $('#wizardStep1 > a > span').addClass('iconDisabled');

            $('#check2').addClass('iconDisabled');
            $('#wizardStep2 > a > span').removeClass('iconDisabled');

            $('#check3').addClass('iconDisabled');
            $('#wizardStep3 > a > span').removeClass('iconDisabled');
        }

        function show3() {
            $('#step1').removeClass("stepActive");
            $('#step1').addClass("stepDisabled");

            $('#step2').removeClass("stepActive");
            $('#step2').addClass("stepDisabled");

            $('#step3').addClass("stepActive");
            $('#step3').removeClass("stepDisabled");

            $('#wizardStep1').removeClass("activeStep");
            $('#wizardStep2').removeClass("activeStep");
            $('#wizardStep3').addClass("activeStep");

            $('#check1').removeClass('iconDisabled');
            $('#wizardStep1 > a > span').addClass('iconDisabled');

            $('#check2').removeClass('iconDisabled');
            $('#wizardStep2 > a > span').addClass('iconDisabled');

            $('#check3').addClass('iconDisabled');
            $('#wizardStep3 > a > span').removeClass('iconDisabled');
        }

        flatpickr('.datum', {
            altFormat: "j F, Y",
            altInput: true,
            locale: 'nl',
            onChange: function (selectedDates, dateStr, instance) {
                $('#MainContent_hdnDefDate').val(selectedDates);
                console.log('triggered');
            }
        });

        if ($('MainContent_hdnDefDate').val() != "") {
            flatpickr('.tijd', {
                altFormat: "j F, Y - H:i",
                altInput: true,
                locale: 'nl',
                enableTime: true,
                time_24hr: true,
                defaultDate: $('MainContent_hdnDefDate').val()
            });
        } else {
            flatpickr('.tijd', {
                altFormat: "j F, Y - H:i",
                altInput: true,
                locale: 'nl',
                enableTime: true,
                time_24hr: true

            });
        }

        $('.select2Item').select2({
            tags: true,
            language: {
                "noResults": function () {
                    return "Geen resultaten";
                }
            },
            escapeMarkup: function (markup) {
                return markup;
            },
            createTag: function (params) {
                var pId = (parseInt(params.term)) ? "Guido" : params.term;
                return {
                    id: pId,
                    text: params.term,
                    newOption: true
                };
            },
            templateResult: function (data) {
                var $result = $("<span></span>");

                $result.text(data.text);

                if (data.newOption) {
                    $result.append(" <em>(nieuw)</em>");
                }
                return $result;
            }
        });

        $("[id*='ddlContactpersoon']").select2({
            tags: true,
            language: {
                "noResults": function () {
                    return "Geen resultaten";
                }
            },
            escapeMarkup: function (markup) {
                return markup;
            },
            createTag: function (params) {
                return {
                    id: "NieuweContactpersoon",
                    text: params.term,
                    newOption: true
                };
            },
            templateResult: function (data) {
                var $result = $("<span></span>");

                $result.text(data.text);

                if (data.newOption) {
                    $result.append(" <em>(nieuw)</em>");
                }
                return $result;
            }
        });

        $('.select2Multiple').select2({
            language: {
                "noResults": function () {
                    return "Geen resultaten";
                }
            },
            escapeMarkup: function (markup) {
                return markup;
            }
        });

        $('.ddlNoAdd').select2({
        });

        var $el = $('.select2Multiple');
        $el.on('select2:unselecting', function (e) {
            $el.data('unselecting', true);
        }).on('select2:open', function (e) {
            if ($el.data('unselecting')) {
                $el.removeData('unselecting');
                $el.select2('close');
            }
        });


        if ($('#MainContent_visibleStep').val() === "1") {
            show1();
        } else if ($('#MainContent_visibleStep').val() === "2") {
            show2();
        } else if ($('#MainContent_visibleStep').val() === "3") {
            show3();
        }

        $('.btnVolgende').on('click', function () {

            if ($("#OrganisatieLijst > a").length == 0) {
            if ($('#MainContent_NieuwEventNaam').val().length >= 3) {
                if ($('#MainContent_Datum').val().length >= 4) {
                    BootstrapDialog.show({
                        title: 'Geen organisatie!', message: 'Gelieve een organisatie toe te voegen.', buttons: [{
                            label: 'Sluiten', action: function (dialogRef) {
                                dialogRef.close();
                            }
                        }]
                    });
                }
            }
    }
    else
    {       
            $('#MainContent_visibleStep').val('2');
            $('#MainContent_changeValGroup').val("2");
               }  

        });
        $('.btnVolgende1').on('click', function () {
            $('#MainContent_visibleStep').val('3');

            $('#MainContent_changeValGroup').val("3");

        });
        $('.btnVorige').on('click', function () {
            $('#MainContent_visibleStep').val('1');

            $('#MainContent_changeValGroup').val("1");

        });
        $('.btnVorige1').on('click', function () {
            $('#MainContent_visibleStep').val('2');

            $('#MainContent_changeValGroup').val("2");

        });
        $("[id*='ddlOrganisatieNaam']").on("change", function () {
            $("#MainContent_organisatie1_hdnOrganisatie").val(this.value);
            $("#MainContent_organisatie1_hdnOrganisatie2").val($("[id$='MainContent_organisatie1_ddlOrganisatieNaam'] option:selected").text());
            __doPostBack('<%= UpdatePanel2.ClientID %>', null);
        });
        $("[id*='ddlContactpersoon']").on("change", function () {
            $("#MainContent_organisatie1_hdnContactpersoon").val(this.value);
            $("#MainContent_organisatie1_hdnContactpersoon2").val($("[id$='MainContent_organisatie1_ddlContactpersoon'] option:selected").text());
            __doPostBack('<%= UpdatePanel2.ClientID %>', null);
        });

        $("[id*='ddlZaal']").on("change", function () {
            $("#MainContent_organisatie1_hdnZaal").val(this.value);
            __doPostBack('<%= UpdatePanel2.ClientID %>', null);

        });

        $(".ddlTransportNaam").on("change", function () {
            var temp = this.id.split('_')[3];
            $("[id$='MainContent_transportRepeater_TransportControl_" + temp + "_hdnTransport_" + temp + "']").val($("[id$='MainContent_transportRepeater_TransportControl_" + temp + "_ddlTransportNaam_" + temp + "'] option:selected").val());
            __doPostBack('<%= UpdatePanel2.ClientID %>', null);
        });

        $(".ddlMedewerkersLaden").on("change", function () {
            var gebruikers = $(".ddlMedewerkersLaden").val();

            $("#MainContent_hdnMedewerkersLaden").val(gebruikers);
            __doPostBack('<%= UpdatePanel2.ClientID %>', null);
        });

        $(".ddlMedewerkersOpbouw").on("change", function () {
            var gebruikers = $(".ddlMedewerkersOpbouw").val();

            $("#MainContent_hdnMedewerkersOpbouw").val(gebruikers);
            __doPostBack('<%= UpdatePanel2.ClientID %>', null);
        });

        $(".ddlMedewerkersAfbouw").on("change", function () {
            var gebruikers = $(".ddlMedewerkersAfbouw").val();

            $("#MainContent_hdnMedewerkersAfbouw").val(gebruikers);
            __doPostBack('<%= UpdatePanel2.ClientID %>', null);
        });

        $(".ddlMedewerkersActie").on("change", function () {
            var gebruikers = $(".ddlMedewerkersActie").val();

            $("#MainContent_hdnMedewerkersActie").val(gebruikers);
            __doPostBack('<%= UpdatePanel2.ClientID %>', null);
        });

    }
</script>
</asp:Content>
