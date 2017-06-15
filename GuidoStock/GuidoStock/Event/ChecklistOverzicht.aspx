<%@ Page Title="Checklist" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChecklistOverzicht.aspx.cs" EnableEventValidation="false" CodeFile="ChecklistOverzicht.aspx.cs" Inherits="GuidoStock.Event.ChecklistOverzicht" %>
<asp:Content ID="content" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel runat="server" ID="updatePanel" UpdateMode="Always" ChildrenAsTriggers="True">
        <ContentTemplate>
            <div class="row checklist">
                <div class="row">
                    <asp:GridView runat="server" ID="ChecklistGridView" AutoGenerateColumns="False"
                        CssClass="tablesorter table table-hover table-bordered">
                        <Columns>
                            <asp:BoundField DataField="Artikel.Naam" HeaderText="Artikel" />
                            <asp:BoundField DataField="Aantal" HeaderText="Aantal" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="row">
                    <asp:Button runat="server" id="btnVorige" CssClass="btn btn-danger noprint" Text="Vorige" OnClick="btnVorige_OnClick"/>
                    <asp:Button runat="server" CssClass="btn btn-default btnNieuw noprint" Text="Aanpassen" OnClick="OnClick" />
                    <asp:Button runat="server" id="btnAfdrukken" CssClass="btn btn-default noprint pull-right" Text="Afdrukken"/>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        $('#MainContent_btnAfdrukken').on('click', function () {
            window.print();
        });
    </script>
</asp:Content>
