<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TeamlidControl.ascx.cs" Inherits="GuidoStock.Controls.TeamlidControl" %>
<div class="row">

    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2 ">
        <div class="form-group">

            <asp:Label runat="server" ID="Label16" AssociatedControlID="TeamlidNaam" CssClass="control-label">
                        Naam

            </asp:Label><asp:TextBox runat="server" ID="TeamlidNaam" CssClass="form-control" />

        </div>
    </div>
     
    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2 ">
        <div class="form-group">

            <asp:Label runat="server" ID="Label17" AssociatedControlID="TeamlidFunctie" CssClass="control-label">
                        Functie

            </asp:Label><asp:TextBox runat="server" ID="TeamlidFunctie" CssClass="form-control" />

        </div>
    </div>

    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2 ">
        <div class="form-group">

            <asp:Label runat="server" ID="Label18" AssociatedControlID="TeamlidTel" CssClass="control-label">
                        Tel.

            </asp:Label><asp:TextBox runat="server" ID="TeamlidTel" CssClass="form-control" />

        </div>
    </div>

</div>
