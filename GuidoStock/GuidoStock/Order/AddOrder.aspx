<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddOrder.aspx.cs" Inherits="GuidoStock.Order.AddOrder" EnableEventValidation="false" %>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="updatePanel" UpdateMode="Always" ChildrenAsTriggers="True">
        <ContentTemplate>
            <asp:HiddenField runat="server" Id="hdnToggle" OnValueChanged="hdnToggle_OnValueChanged"/>
            <div class="article">
                <h4>Algemeen</h4>
                <hr />
                <div class="row">
                    <div class="col-md-12">
                        <div id="checkboxControl" class="form-group">
                            <div class="col-md-2">
                                <asp:Label runat="server" ID="lblIsVerhuur" AssociatedControlID="chkIsVerhuur" CssClass="control-label">Verhuur
                                </asp:Label>
                                <input id="chkIsVerhuur" runat="server" type="checkbox" class="tgl tgl-light" />
                                <asp:Label runat="server" ID="Label73" AssociatedControlID="chkIsVerhuur" CssClass="tgl-btn"> </asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div id="infoControls" class="form-group">
                            <div class="col-md-2">
                                <asp:Label AssociatedControlID="txtNaam" runat="server">Titel</asp:Label>
                                <asp:TextBox ID="txtNaam" runat="server" class="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvArtikelNaam" runat="server" ToolTip="Required" ControlToValidate="txtNaam" ValidationGroup="group1" ErrorMessage="Naam is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />
                                <asp:RegularExpressionValidator
                                    ID="revArtikelNaam"
                                    ControlToValidate="txtNaam"
                                    ValidationExpression="^.{3,}$"
                                    ErrorMessage="Minimum 3 karakters"
                                    runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                            </div>
                            <div class="col-md-2">
                                <asp:Label AssociatedControlID="txtBeginTijd" runat="server">Begintijdstip</asp:Label>
                                <asp:TextBox ID="txtBeginTijd" runat="server" class="form-control datum"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ToolTip="Required" ControlToValidate="txtBeginTijd" ValidationGroup="group1" ErrorMessage="Begintijd is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />
                                
                            </div>
                            <div class="col-md-2 eindtijd">
                                <asp:Label AssociatedControlID="txtEindTijd" runat="server">Eindtijdstip</asp:Label>
                                <asp:TextBox ID="txtEindTijd" runat="server" class="form-control datum"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ToolTip="Required" ControlToValidate="txtEindTijd" ValidationGroup="group1" ErrorMessage="Eindtijd is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />

                            </div>
                            <div class="col-md-2">
                                <asp:Label AssociatedControlID="txtNaamContactpersoon" runat="server">Naam Contactpersoon</asp:Label>
                                <asp:TextBox ID="txtNaamContactpersoon" runat="server" class="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ToolTip="Required" ControlToValidate="txtNaamContactpersoon" ValidationGroup="group1" ErrorMessage="Contactpersoon is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />
                                <asp:RegularExpressionValidator
                                    ID="RegularExpressionValidator2"
                                    ControlToValidate="txtNaamContactpersoon"
                                    ValidationExpression="^.{3,}$"
                                    ErrorMessage="Minimum 3 karakters"
                                    runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                               
                            </div>
                            <div class="col-md-2">
                                <asp:Label AssociatedControlID="txtTel" runat="server">Tel. Contactpersoon</asp:Label>
                                <asp:TextBox ID="txtTel" runat="server" class="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ToolTip="Required" ControlToValidate="txtTel" ValidationGroup="group1" ErrorMessage="Telefoonnummer is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />
                                <asp:RegularExpressionValidator
                                    ID="RegularExpressionValidator1"
                                    ControlToValidate="txtTel"
                                    ValidationExpression="^\+?\d+$"
                                    ErrorMessage="Vul een geldig nummer in"
                                    runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row orderButtons">
                <a href="overzicht.aspx" runat="server" class="btn btn-danger cancelButton">Annuleren</a>

                <asp:LinkButton runat="server" ID="LinkButton1" class="btn btn-default btnNieuw" OnClick="LinkButton1_OnClick" ValidationGroup="group1" CausesValidation="true">
										Opslaan
                </asp:LinkButton>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        function pageLoad(sender, args) {
            flatpickr('.datum', {
                altFormat: "j F, Y",
                altInput: true,
                locale: 'nl'
                //onChange: function (selectedDates, dateStr, instance) {
                //    $('#MainContent_hdnDefDate').val(selectedDates);
                //}
            });

            var value = $('#MainContent_chkIsVerhuur')[0].checked;
            if (value) {
                $('.eindtijd').show();
            } else {
                $('.eindtijd').hide();
            }

            $('#MainContent_chkIsVerhuur').on("change", function () {
                $('.eindtijd').toggle();
                var value2 = $('#MainContent_chkIsVerhuur')[0].checked;
                $("#MainContent_hdnToggle").val(value2);
                __doPostBack('<%= updatePanel.ClientID %>', null);
            });

            
        }
    </script>
</asp:Content>
