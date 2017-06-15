<%@ Page Title="Event overzicht" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="Event.aspx.cs" Inherits="GuidoStock.Scannen.Event" EnableEventValidation="false" %>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
<asp:UpdatePanel ID="UpdatePanelScannenEvent" runat="server" ChildrenAsTriggers="true">
<ContentTemplate>
    
    <asp:HiddenField runat="server" ID="EventSelect" OnValueChanged="EventSelect_OnValueChanged"/>
    
    
    <% if (LopendeScannen.Count == 0 && KomendeScannen.Count == 0)
       {
           %><h4>Er zijn geen evenementen of orders beschikbaar.</h4><%
       } %>

    <div class="cardsScannenEvent">

        <% if (LopendeScannen.Count != 0)
       {
        %><h3>Lopende events</h3>
        <%
           } %>
        
    <% foreach (var evnt in LopendeScannen)
       {
    %>

        <div id="<%=evnt.Id %>" class="card text-center col-lg-2 col-md-3 col-sm-4 col-xs-12" OnClick="SelectEvent(<%=evnt.Id %>, 'EventLopend','<%=evnt.IsEvent %>')">
            <% if (!evnt.IsEvent)
               {
                   %>
                <div class="triangle">
                    <span>ORDER</span>
                </div>
            <%
               } %>

            
            
            <div class="card-block">
                <h4 class="card-title">


                    <%=evnt.Naam %>
                </h4>

            </div>
            <div class="card-block">

                <p class="card-text"><% =evnt.Datum.ToString("dd MMMM yyyy")  %>
                    <br/>
                    <span class="card-text"><% =evnt.Subtext  %></span>
                </p>
                
            </div>
        </div>
    <%

            } %>
        
        </div>
    <div class="cardsScannenEvent">

        <% if (KomendeScannen.Count != 0)
           {
        %><h3>Komende events</h3>
        <%
           } %>

        <% foreach (var evnt in KomendeScannen)
            {
        %>

        <div id="<%=evnt.Id %>" class="card text-center col-lg-2 col-md-3 col-sm-4 col-xs-12" OnClick="SelectEvent(<%=evnt.Id %>, 'Event','<%=evnt.IsEvent %>')">
            
            <% if (!evnt.IsEvent)
               {
            %>
                <div class="triangle">
                    <span>ORDER</span>
                </div>
            <%
               } %>
            
             
            <% if (evnt.IsOnHold)
               {
            %>
                <div class="t-right">
                    <span>HALF</span>
                </div>
            <%
               } %>

            <div class="card-block">
                <h4 class="card-title">


                    <%=evnt.Naam %>
                </h4>

            </div>
            <div class="card-block">

                <p class="card-text"><% =evnt.Datum.ToString("dd MMMM yyyy")  %>
                    <br/>
                    <span class="card-text"><% =evnt.Subtext  %></span>
                </p>
                
            </div>
    
        
        </div>
        <%

            } %>
        </div>
</ContentTemplate>
    
    </asp:UpdatePanel>
    
    <script>
        function pageLoad(sender, args) {

            $(".cardsScannenEvent").matchHeight();
        }

        function SelectEvent(id, string, bool) {
          
            $('#MainContent_EventSelect').val(id + "¬" +string + "¬" + bool);
            __doPostBack('<%=UpdatePanelScannenEvent.ClientID%>', null);
        }


    </script>
   
    </asp:Content>