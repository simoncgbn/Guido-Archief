<%@ Page Title="Checklist" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Checklist.aspx.cs" EnableEventValidation="false" CodeFile="Checklist.aspx.cs" Inherits="GuidoStock.Event.Checklist" %>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="updatePanel" UpdateMode="Always" ChildrenAsTriggers="True" >
        <ContentTemplate>
            <div class="row checklijst">
                <div id="error" class="alert alert-danger alert-dismissible" role="alert" style="display: none">
                </div>
  
                <div class="col-md-5 stockLijst">
                    <div class="row">
                        <div class="optionsBar">
                            <div class="input-group stylish-input-group maxOverride">
                             <span class="input-group-addon">
                        
                            <span class="glyphicon glyphicon-search"></span>
                        
                    </span>
                        <asp:TextBox placeholder="Zoeken" runat="server" ID="txtSearchBar" CssClass="form-control" />
                         
                             </div>
                        </div>
                        <asp:GridView runat="server" ID="StocklijstGridView" AutoGenerateColumns="False"
                            CssClass="tablesorter table table-hover table-bordered" DataKeyNames="Id" OnRowCommand="StocklijstGridView_OnRowCommand" >
                            <Columns>
                                <asp:BoundField DataField="Merk.Naam" HeaderText="Merk"/>
                                <asp:BoundField DataField="Naam" HeaderText="Artikel"/>
                                <asp:BoundField DataField="AvailableAantal" HeaderText="Aantal"/>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemStyle CssClass="center-icon"/>     
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="btnAdd" Text="Add" CommandArgument='<%# Eval("Id") %>' CommandName="Add">
                                            <span class="glyphicon glyphicon-plus"></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="col-md-7 checklijst">
                    <%--Stopcontacten + gewicht--%>
                    
                    <div class="checklistInfoWrapper">
                        
                        <div class="gewicht">
                            <div class="totaalGewicht">
                                <p class="checklistInfoGroot">
                                    <%=TotaalGewicht %> kg
                                </p> 
                                <p class="checklistInforKlein">
                                     Totaal gewicht
                                </p>
                            </div>
                            <div class="totaalGewicht">
                                <p class="checklistInfoGroot">
                                    <%=MaxGewicht %> kg
                                </p>
                                <p class="checklistInforKlein">
                                    Max. gewicht
                                </p>
                            </div>
                            <span>Het verpakkingsgewicht wordt niet meegerekend!</span>
                        </div>
                        <div class="stopcontact">
                            <p class="checklistInfoGroot">
                                    <%=Stopcontacten %>
                            </p>
                            <p class="checklistInforKlein">
                                    Min. aantal stopcontacten
                            </p>
                        </div>

                    </div>
                    

                    <!-- Show current state of checklist -->
                    <div class="row">
                        <asp:GridView runat="server" ID="ChecklistGridView" AutoGenerateColumns="False"
                            CssClass="tablesorter table table-hover table-bordered" OnRowDataBound="ChecklistGridView_OnRowDataBound" OnRowCommand="ChecklistGridView_OnRowCommand">
                            <Columns>
                                <asp:BoundField DataField="Artikel.Merk.Naam" HeaderText="Merk"/>
                                <asp:BoundField DataField="Artikel.Naam" HeaderText="Artikel"/>
                                <asp:TemplateField HeaderText="Aantal">
                                    <ItemTemplate>
                                        <asp:TextBox CssClass="aantalInput form-control" onchange="javascript: Changed( this );" data-cmdname='<%# Eval("Artikel.Id") %>' CommandArgument='<%# Eval("Artikel.Id") %>' runat="server"  ID="txtAantal" Text='<%# Bind("Aantal") %>' UseSubmitBehavior="False"></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="hdnAantalId" Value='<%# Eval("Artikel.Id") %>'/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemStyle CssClass="center-icon"/>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="btnRemove" CommandArgument='<%# Eval("Artikel.Id") %>' CommandName="Remove">
                                            <span class="glyphicon glyphicon-trash"></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <asp:HiddenField runat="server" ID="EvenementId"/>
            <div class="row controlButtons">
                <asp:Button runat="server" CssClass="btn btn-danger" Text="Annuleren" OnClick="Annuleren_OnClick" UseSubmitBehavior="False"/>
                <asp:Button runat="server" CssClass="btn btn-default btnNieuw" Text="Opslaan" OnClick="OnClick" UseSubmitBehavior="False"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" id="UpdatePanelHdnAantal">
        <ContentTemplate>
        <asp:HiddenField ID="hdnAantal" runat="server" OnValueChanged="hdnAantal_OnValueChanged"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        function pageLoad(sender, args) {
            $(".tablesorter").tablesorter({ dateFormat: "uk" });


            $("#MainContent_txtSearchBar").on('keyup', function () {
                var searchText = document.getElementById('MainContent_txtSearchBar').value;
                var targetTable = document.getElementById('MainContent_StocklijstGridView');
                var targetTableColCount = 0;

                //Loop through table rows
                for (var rowIndex = 0; rowIndex < targetTable.rows.length; rowIndex++) {
                    var rowData = '';

                    //Get column count from header row
                    if (rowIndex == 0) {
                        targetTableColCount = targetTable.rows.item(rowIndex).cells.length;
                        continue; //do not execute further code for header row.
                    }

                    //Process data rows. (rowIndex >= 1)
                    for (var colIndex = 0; colIndex < targetTableColCount; colIndex++) {
                        rowData += targetTable.rows.item(rowIndex).cells.item(colIndex).textContent;
                    }

                    //If search term is not found in row data
                    //then hide the row, else show
                    if (rowData.toUpperCase().indexOf(searchText.toUpperCase()) == -1)
                        targetTable.rows.item(rowIndex).style.display = 'none';
                    else
                        targetTable.rows.item(rowIndex).style.display = 'table-row';
                }

            });
        }
        function openModal(name, aantal) {
            $('#error').html('<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
                "<strong>Opgepast!</strong> " + name + " was door iemand anders aan een checklijst toegevoegd. Er zijn nog " + aantal + " exemplaren beschikbaar.");
            $('#error').show();
        }

        function Changed(textControl) {
            var aantal = (textControl.value);
            var id = (textControl.getAttribute("data-cmdname"));
            $("#MainContent_hdnAantal").val(aantal + "¬" + id);
            __doPostBack('<%= UpdatePanelHdnAantal.ClientID %>', null);
        }

        function showSuccesModal(eventId, type) {
            BootstrapDialog.show({
                title: 'Geslaagd!',
                message: 'De checklist is succesvol opgeslagen.',
                buttons: [{
                        label: 'Bewerken',
                        cssClass: 'cancelButton',
                        action: function (dialogRef) {
                            dialogRef.close();
                        }
                    },
                    {
                        label: 'Bekijk event',
                        cssClass: 'btnNieuw',
                        hotkey: 13,
                        action: function(){ window.location.href = '/event/Overzicht.aspx?id='+ eventId + "&type=" +type ;}
                    }
                ],
                closable: true,
            });
        }
        
        function weightWarning() {
            $(".gewicht").addClass('weightWarning');
        }

        function dontWeightWarning() {
            $(".gewicht").removeClass('weightWarning');
        }


    </script>
</asp:Content>
