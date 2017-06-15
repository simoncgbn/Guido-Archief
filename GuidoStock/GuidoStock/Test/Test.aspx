<%@ Page Title="Stock" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Test.aspx.cs" EnableEventValidation="false" CodeFile="Test.aspx.cs" Inherits="GuidoStock.Test.Test" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<asp:UpdatePanel ID="UpdatePanelTest" runat="server" UpdateMode="Conditional">
<ContentTemplate>
    
    <asp:Label ID="Label3" runat="server" /><br />
    <asp:Button ID="Button3" runat="server"
                Text="Update Both Panels"  />
    <asp:Button ID="Button4" runat="server"
                Text="Update This Panel"  />
    
    
    
    </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server"
                     UpdateMode="Conditional">
        <ContentTemplate>
            <asp:DropDownList ID="ddlNieuwMerknaam" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Item ddlMerk" DataValueField="Id">
            </asp:DropDownList>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Button3" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Button runat="server" Text="Test"/>

    <script>
        

        function pageLoad(sender, args) {
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
                    return {
                        id: params.term,
                        text: params.term,
                        newOption: true
                    }
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
        }
        
        


    </script>
    </asp:Content>