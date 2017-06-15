<%@ Page Title="Scannen" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="Overzicht.aspx.cs" Inherits="GuidoStock.Scannen.Overzicht" EnableEventValidation="false" %>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="text-center scanTitle">Wat wilt u scannen?</h3>
    <div class="row text-center cardsScannenOverzicht">
        <div class="card text-center">
        <div class="card-block">
            <h4 class="card-title">Inkomende levering</h4>
           <span class="pe-7s-plane mirror"></span>
        </div>
        <div class="card-block">
            <p class="card-text">Selecteer deze optie wanneer u producten wilt inchecken voor een inkomende levering.</p>
            <asp:LinkButton ID="btnScanInkomend" CssClass="" runat="server" Text="" OnClick="btnScanEvent_OnClick">                    
                <span>SELECTEER</span>
            </asp:LinkButton>
        </div>
    </div>
    
    <div class="card text-center">
        <div class="card-block">
            <h4 class="card-title">Checklist</h4>
            <span class="pe-7s-date"></span>
        </div>
        <div class="card-block">
            <p class="card-text">Selecteer deze optie wanneer u producten wilt in- of uitchecken aan de hand van een checklist.</p>
            <asp:LinkButton ID="btnScanEvent" CssClass="" runat="server" Text="" OnClick="btnScanEvent_OnClick">                    
                <span>SELECTEER</span>
            </asp:LinkButton>
        </div>
    </div>
    
    <div class="card text-center">
        <div class="card-block">
            <h4 class="card-title">Uitgaande levering</h4>
            <span class="pe-7s-plane"></span>
        </div>
        <div class="card-block">
            <p class="card-text">Selecteer deze optie wanneer u producten wilt uitchecken voor een uitgaande levering.</p>
            <asp:LinkButton ID="btnScanUitgaand" CssClass="" runat="server" Text="" OnClick="btnScanEvent_OnClick">                    
                <span>SELECTEER</span>
            </asp:LinkButton>
        </div>
    </div>
    </div>
</asp:Content>
