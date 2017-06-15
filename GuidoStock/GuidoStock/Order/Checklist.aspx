<%@ Page Title="Checklist" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="Checklist.aspx.cs" Inherits="GuidoStock.Order.Checklist" EnableEventValidation="false" %>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="updatePanel" UpdateMode="Always" ChildrenAsTriggers="True">
        <ContentTemplate>
            <div id="error" class="alert alert-danger alert-dismissible" role="alert" style="display: none">
            </div>
            <div class="row">
                <div id="Algemeen" class="article">
            </div>
            <div class="row checklijst">
                <div id="Checklist" class="article">
                    <h4>Checklist</h4>
                    <hr />
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
                                CssClass="tablesorter table table-hover table-bordered" DataKeyNames="Id" OnRowCommand="StocklijstGridView_OnRowCommand">
                                <Columns>
                                    <asp:BoundField DataField="Merk.Naam" HeaderText="Merk" />
                                    <asp:BoundField DataField="Naam" HeaderText="Artikel" />
                                    <asp:BoundField DataField="AvailableAantal" HeaderText="Aantal" />
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemStyle CssClass="center-icon" />
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
                        <!-- Show current state of checklist -->
                        <div class="row">
                            <asp:GridView runat="server" ID="ChecklistGridView" AutoGenerateColumns="False"
                                CssClass="tablesorter table table-hover table-bordered" OnRowDataBound="ChecklistGridView_OnRowDataBound" OnRowCommand="ChecklistGridView_OnRowCommand">
                                <Columns>
                                    <asp:BoundField DataField="Artikel.Merk.Naam" HeaderText="Merk" />
                                    <asp:BoundField DataField="Artikel.Naam" HeaderText="Artikel" />
                                    <asp:TemplateField HeaderText="Aantal">
                                        <ItemTemplate>
                                            <asp:TextBox CssClass="aantalInput form-control" OnTextChanged="txtAantal_OnTextChanged" CommandArgument='<%# Eval("Artikel.Id") %>' runat="server" ID="txtAantal" Text='<%# Bind("Aantal") %>'></asp:TextBox>
                                            <asp:HiddenField runat="server" Value='<%# Eval("Artikel.Id") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemStyle CssClass="center-icon" />
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
            </div>
            <asp:HiddenField runat="server" ID="EvenementId" />
            <div class="row controlButtons">
                <asp:Button runat="server" CssClass="btn btn-danger" Text="Annuleren" OnClick="OnClick" />
                <asp:Button runat="server" CssClass="btn btn-default btnNieuw" Text="Opslaan" OnClick="BtnNieuw_OnClick" />
            </div>
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

        function showSuccesModal(eventId) {
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
                        label: 'Bekijk order',
                        cssClass: 'btnNieuw',
                        hotkey: 13,
                        action: function () { window.location.href = '/Order/Overzicht.aspx?id=' + eventId; }
                    }
                ],
                closable: true
            });
        }
    </script>
</asp:Content>
