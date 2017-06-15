<%@ Page Title="Scannen levering" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="NoEvent.aspx.cs" Inherits="GuidoStock.Scannen.NoEvent" EnableEventValidation="false" %>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
<asp:UpdatePanel ID="UpdatePanelEventScannenLevering" runat="server" ChildrenAsTriggers="true">
<ContentTemplate>
    <asp:HiddenField ID="hdnBarcode" runat="server" Value="" OnValueChanged="hdnBarcode_OnValueChanged" />
    <asp:HiddenField ID="hdnLocatie" runat="server" Value="" OnValueChanged="hdnLocatie_OnValueChanged" />

   
    
   
        
    <div class="col-xs-4 col-sm-3 col-md-3 col-lg-3">
        <div class="form-group">
            <asp:Label runat="server" ID="Label1" AssociatedControlID="Barcode" CssClass="control-label">
                Barcode
            </asp:Label><asp:TextBox runat="server" ID="Barcode" CssClass="form-control"/>
                        <span id="ErrorBarcode" runat="server" class="text-danger errorMessage" Style="display:none">Foutieve barcode</span>
        </div>
    </div>
                
    <div class="col-xs-4 col-sm-3 col-md-3 col-lg-3">
        <div class="form-group">
            <asp:Label runat="server" ID="Label2" AssociatedControlID="Locatie" CssClass="control-label">
                Locatie
            </asp:Label><asp:DropDownList ID="Locatie" runat="server"  DataTextField="LocatieVervaldatum" Style="width: 100%;" Enabled="False" CssClass="form-control select2Item ddlLocatie" DataValueField="Id">
            </asp:DropDownList>
        </div>
    </div>
    
    <% if (Type == "inkomend")
       {%>
        <div class="col-xs-4 col-sm-3 col-md-2 col-lg-2">
            <div class="form-group">
                <asp:Label runat="server" ID="Label8" AssociatedControlID="Vervaldatum" CssClass="control-label">
                    Vervaldatum
                </asp:Label>
                <asp:TextBox runat="server" type="date" ID="Vervaldatum" Name="Datum" CssClass="form-control tijd"/>
            </div>
        </div>
      <% } %>
   

                
    <div class="col-xs-4 col-sm-3 col-md-2 col-lg-2">
        <div class="form-group">
            <asp:Label runat="server" ID="lblAantal" AssociatedControlID="Aantal" CssClass="control-label">
                Aantal
            </asp:Label><asp:TextBox runat="server" ID="Aantal" CssClass="form-control" ReadOnly="True"/>
            <asp:RequiredFieldValidator ID="rfvAantal" runat="server" ToolTip="Required" ControlToValidate="Aantal" ValidationGroup="group1" ErrorMessage="Aantal is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />
            <asp:RegularExpressionValidator
                ID="revAantal"
                ControlToValidate="Aantal"
                ValidationExpression="^[0-9]*$"
                ErrorMessage="Aantal moet een nummer zijn"
                runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
        </div>
    </div>
                
    <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2">
        <div class="form-group">
            <asp:Label runat="server" ID="Label3" AssociatedControlID="CheckUit" CssClass="control-label">
                &nbsp;  &nbsp; &nbsp; &nbsp; &nbsp;
            </asp:Label>
            <asp:LinkButton runat="server" ID="CheckUit" class="btn btn-primary btnNieuw "  CssClass="btn btn-primary CheckUitDisabled CheckUitStandaard" OnClick="CheckUit_OnClick" ValidationGroup="group1" CausesValidation="true" Enabled="False">
                <%=IsInkomend ? "Check in" : "Check uit" %>
            </asp:LinkButton>
        </div>
    </div>

    </ContentTemplate>
    
    </asp:UpdatePanel>
    
    <script>
        function pageLoad(sender, args) {
            flatpickr('.tijd',
                {
                    altFormat: "j F, Y",
                    altInput: true,
                    locale: 'nl'

                });
            $('.ddlLocatie').select2({
            });

            $('#MainContent_Barcode').focus();

            $('#MainContent_Barcode').change(function() {
                $('#MainContent_hdnBarcode').val($('#MainContent_Barcode').val());
                __doPostBack('<%= UpdatePanelEventScannenLevering.ClientID %>', null);
            });

            <%--$('#MainContent_Barcode').keyup(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault();
                    $('#MainContent_hdnBarcode').val($('#MainContent_Barcode').val());
                    __doPostBack('<%= UpdatePanelEventScannenLevering.ClientID %>', null);

                }
            });--%>

            $("#MainContent_Aantal").TouchSpin({
                verticalbuttons: true
            });

            $("#MainContent_Locatie").on("change",
                function () {
                    $('#MainContent_hdnLocatie').val($('#MainContent_Locatie').val());
                    __doPostBack('<%= UpdatePanelEventScannenLevering.ClientID %>', null);

                });
        }

        function fillLocatie(aantal) {
            $("#MainContent_Aantal").TouchSpin({
                verticalbuttons: true,
                max: aantal
            });

        }

    </script>
    
    </asp:Content>