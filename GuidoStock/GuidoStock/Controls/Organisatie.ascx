<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="organisatie.ascx.cs" Inherits="GuidoStock.Controls.Organisatie" ClassName="Organisatie"%>
<asp:HiddenField ID="hdnOrganisatie" runat="server" Value="" OnValueChanged="hdnOrganisatie_ValueChanged" />
<asp:HiddenField ID="hdnOrganisatie2" runat="server" Value="" />
<asp:HiddenField ID="hdnContactpersoon" runat="server" Value="" OnValueChanged="hdnContactpersoon_ValueChanged" />
<asp:HiddenField ID="hdnContactpersoon2" runat="server" Value="" />
<asp:HiddenField ID="hdnZaal" runat="server" Value="" OnValueChanged="hdnZaal_ValueChanged" />
<div class="col-xs-12 col-sm-8 col-md-9 col-lg-8 no-padding">
<div class="row" runat="server"> 

    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-3">
        <div class="form-group">

            <asp:Label runat="server" ID="OrganisatieNaam" AssociatedControlID="ddlOrganisatieNaam" CssClass="control-label">
                    Naam
            </asp:Label><asp:DropDownList ID="ddlOrganisatieNaam" runat="server" DataTextField="Organisatie" Style="width: 100%;" CssClass="form-control select2Item ddlOrganisatieNaam" DataValueField="Id">
            </asp:DropDownList>
        </div>
    </div>


    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-3">
        <div class="form-group">
            <asp:Label runat="server" ID="Label3" AssociatedControlID="ddlContactpersoon" CssClass="control-label">
                    Contactpersoon
            </asp:Label><asp:DropDownList ID="ddlContactpersoon" runat="server" DataTextField="ContactNaam" Style="width: 100%;" CssClass="form-control ddlContactpersoon" DataValueField="ContactTel">
            </asp:DropDownList>
        </div>
    </div>

    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-3">
        <div class="form-group">

            <asp:Label runat="server" ID="Label4" AssociatedControlID="telContact" CssClass="control-label">
                    Tel. contactpersoon

            </asp:Label><asp:TextBox runat="server" ID="telContact" CssClass="form-control" />
            <asp:RegularExpressionValidator
                ID="RegularExpressionValidator3"
                ControlToValidate="telContact"
                ValidationExpression="^\+?\d+$"
                ErrorMessage="Geen geldig telefoonnummer"
                runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group2" />
        </div>
    </div>

    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-3">
        <div class="form-group">

            <asp:Label runat="server" ID="Label6" AssociatedControlID="ddlZaal" CssClass="control-label">
                    Zaal

            </asp:Label><asp:DropDownList ID="ddlZaal" runat="server" DataTextField="Zaal" Style="width: 100%;" CssClass="form-control select2Item ddlZaal" DataValueField="Id">
            </asp:DropDownList>

        </div>
    </div>
</div>
<div class="row" runat="server">
    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-3">
        <div class="form-group">

            <asp:Label runat="server" ID="Label7" AssociatedControlID="Adres" CssClass="control-label">
                    Adres

            </asp:Label><asp:TextBox runat="server" ID="Adres" CssClass="form-control" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ToolTip="Required" ControlToValidate="Adres" ValidationGroup="group2" ErrorMessage="Adres is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />

        </div>
    </div>

    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-3">
        <div class="form-group">

            <asp:Label runat="server" ID="Label5" AssociatedControlID="Plaats" CssClass="control-label">
                    Plaats

            </asp:Label><asp:TextBox runat="server" ID="Plaats" CssClass="form-control"  />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ToolTip="Required" ControlToValidate="Plaats" ValidationGroup="group2" ErrorMessage="Plaats is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />
            <asp:RegularExpressionValidator
                ID="RegularExpressionValidator2"
                ControlToValidate="Plaats"
                ValidationExpression="^.{3,}$"
                ErrorMessage="Minimum 3 karakters"
                runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group2" />
        </div>
    </div>

    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-3">
        <div class="form-group">

            <asp:Label runat="server" ID="Label19" AssociatedControlID="Postcode" CssClass="control-label">
                    Postcode

            </asp:Label><asp:TextBox runat="server" ID="Postcode" CssClass="form-control" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ToolTip="Required" ControlToValidate="Postcode" ValidationGroup="group2" ErrorMessage="Postcode is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />

        </div>
    </div>
    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-3">
        <div class="form-group">

            <asp:Label runat="server" ID="Label20" AssociatedControlID="ddlCountries" CssClass="control-label">
                    Land
            </asp:Label><asp:DropDownList ID="ddlCountries" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control ddlNoAdd ddlCountries" DataValueField="Code">
            </asp:DropDownList>
        </div>
    </div>
</div>
<div class="row" runat="server">

    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-3">
        <div class="form-group">
            <asp:Label runat="server" ID="Label8" AssociatedControlID="Begintijd" CssClass="control-label">
                    Begintijd
            </asp:Label>
            <asp:TextBox runat="server" type="date" ID="Begintijd" Name="Datum" CssClass="form-control tijd"/>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ToolTip="Required" ControlToValidate="Begintijd" ValidationGroup="group2" ErrorMessage="Begintijd is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />

        </div>
    </div>

    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-3">
        <div class="form-group">
            <asp:Label runat="server" ID="Label9" AssociatedControlID="Eindtijd" CssClass="control-label">
                    Eindtijd
            </asp:Label>
            <asp:TextBox runat="server" type="date" ID="Eindtijd" Name="Datum" CssClass="form-control tijd"/>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ToolTip="Required" ControlToValidate="Eindtijd" ValidationGroup="group2" ErrorMessage="Eindtijd is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />

        </div>
    </div>

    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-3">
        <div class="form-group">

            <asp:Label runat="server" ID="Label10" AssociatedControlID="Opkomst" CssClass="control-label">
                    Opkomst<span> - Optioneel</span>

            </asp:Label><asp:TextBox runat="server" ID="Opkomst" CssClass="form-control" type="number" min="0"  />

        </div>
    </div>

    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-3">
        <div class="form-group">

            <asp:Label runat="server" ID="Label11" AssociatedControlID="Target" CssClass="control-label">
                    Target<span> - Optioneel</span>

            </asp:Label><asp:TextBox runat="server" ID="Target" CssClass="form-control" type="number" min="0"  />
        </div>
    </div>
</div>
</div>