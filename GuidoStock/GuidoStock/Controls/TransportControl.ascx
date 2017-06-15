<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransportControl.ascx.cs" Inherits="GuidoStock.Controls.TransportControl" %>
<div class="row" runat="server">

    <asp:HiddenField ID="hdnTransport" runat="server" Value="" OnValueChanged="hdnTransport_OnValueChanged"/> 
    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
        <div class="form-group">
            <asp:Label runat="server" ID="Label12" AssociatedControlID="ddlTransportNaam" CssClass="control-label">
                        Naam
            </asp:Label><asp:DropDownList ID="ddlTransportNaam" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Item ddlTransportNaam ddlNoAdd" DataValueField="Id">
            </asp:DropDownList>
        </div>
    </div>

    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
        <div class="form-group">
            <asp:Label runat="server" ID="Label14" AssociatedControlID="ddlChauffeurHeen" CssClass="control-label">
                        Chauffeur heen
            </asp:Label><asp:DropDownList ID="ddlChauffeurHeen" runat="server" OnSelectedIndexChanged="ddlChauffeurHeen_OnSelectedIndexChanged" DataTextField="Naam" Style="width: 100%;" CssClass="form-control ddlNoAdd ddlChauffeurHeen" DataValueField="Id">
            </asp:DropDownList>
        </div>
    </div>

    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
        <div class="form-group">
            <asp:Label runat="server" ID="Label15" AssociatedControlID="ddlChauffeurTerug" CssClass="control-label">
                        Chauffeur terug
            </asp:Label><asp:DropDownList ID="ddlChauffeurTerug" OnSelectedIndexChanged="ddlChauffeurTerug_OnSelectedIndexChanged" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control ddlNoAdd ddlChauffeurTerug" DataValueField="Id">
            </asp:DropDownList>
        </div>
    </div>

    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
        <div class="form-group">
            <asp:Label runat="server" ID="Label21" AssociatedControlID="VertrekTransport" CssClass="control-label">
                        Vertrek
            </asp:Label><asp:TextBox runat="server" type="date" ID="VertrekTransport" CssClass="form-control tijd" />

        </div>
    </div>

</div>
