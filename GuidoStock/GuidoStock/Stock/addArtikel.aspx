<%@ Page Title="Stock" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddArtikel.aspx.cs" EnableEventValidation="false" CodeFile="AddArtikel.aspx.cs" Inherits="GuidoStock.Stock.AddArtikel" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
        <asp:HiddenField ID="hdnMedewerkers" runat="server" Value="" OnValueChanged="hdnMedewerkers_OnValueChanged" />

            <div class="section" id="AddArtikelSection" runat="server">

                <div class="article" runat="server">

                    <h4>
                        <asp:Localize runat="server" meta:resourcekey="Algemeen" /></h4>
                    <hr />
                    <div class="row" runat="server">
                        <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2 ">
                            <div class="form-group">
                                <asp:Label runat="server" ID="nieuwartikelnaamlbl" AssociatedControlID="NieuwArtikelnaam" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>

                                </asp:Label><asp:TextBox runat="server" ID="NieuwArtikelnaam" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvArtikelNaam" runat="server" ToolTip="Required" ControlToValidate="NieuwArtikelnaam" ValidationGroup="group1" ErrorMessage="Naam is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />
                                <asp:RegularExpressionValidator
                                    ID="revArtikelNaam"
                                    ControlToValidate="NieuwArtikelnaam"
                                    ValidationExpression="^.{3,}$"
                                    ErrorMessage="Minimum 3 karakters"
                                    runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                <asp:RegularExpressionValidator
                                    ID="RegularExpressionValidator46"
                                    ControlToValidate="NieuwArtikelnaam"
                                    ValidationExpression="^[^&quot;]*$"
                                    ErrorMessage="Geen speciale karakters"
                                    runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                            </div>

                        </div> 

                        <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                            <div class="form-group">
                                <asp:Label runat="server" ID="nieuwmerknaamlbl" AssociatedControlID="ddlNieuwMerknaam" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Merknaam"/>
                                </asp:Label><asp:DropDownList ID="ddlNieuwMerknaam" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Item ddlMerk" DataValueField="Id">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                            <div class="form-group">
                                <asp:Label runat="server" ID="nieuwcategorienaamlbl" AssociatedControlID="ddlNieuwCategorienaam" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Categorienaam"/>
                                </asp:Label><asp:DropDownList ID="ddlNieuwCategorienaam" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Item ddlCategorie" DataValueField="Id">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <asp:HiddenField runat="server" ID="hdnDDL1" Value="test" OnValueChanged="hdnDDL1_ValueChanged" />
                        <asp:HiddenField runat="server" ID="hdnDDL2" Value="test" OnValueChanged="hdnDDL2_ValueChanged" />
                        <asp:HiddenField runat="server" ID="hdnDDL4" Value="test" OnValueChanged="hdnDDL4_ValueChanged" />

                        <div class="col-xs-12 col-sm-5 col-md-3">
                            <div class="col-xs-11 col-md-10">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="barcodenaamlbl" AssociatedControlID="Barcodenaam" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Barcodenaam"/>
                                    </asp:Label><asp:TextBox runat="server" ID="Barcodenaam" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ToolTip="Required" ControlToValidate="Barcodenaam" ValidationGroup="group1" ErrorMessage="Barcode is verplicht" CssClass="text-danger errorMessage" Display="Dynamic" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator1"
                                        ControlToValidate="Barcodenaam"
                                        ValidationExpression="^.{3,}$"
                                        ErrorMessage="Minimum 3 karakters"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />



                                </div>
                            </div>
                            <div class="col-xs-1 col-xs-1 col-md-2 vertAlign">
                                <asp:LinkButton runat="server" ID="barcodeButton0" class="button" OnClick="generateBarcode">
										<span class="glyphicon glyphicon-cog"></span>
                                </asp:LinkButton>
                            </div>
                        </div>


                    </div>
                    <div class="row">
                        <div class="col-xs-12 col-sm-3 col-lg-2">
                            <div class="form-group">
                                <asp:Label runat="server" ID="naturaprijslbl" AssociatedControlID="Naturaprijs" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Naturaprijs"/>
                                     <span> - Optioneel</span>
                                </asp:Label><asp:TextBox runat="server" ID="Naturaprijs" CssClass="form-control" type="number" min="0" />
                                <asp:RegularExpressionValidator
                                    ID="RegularExpressionValidator2"
                                    ControlToValidate="Naturaprijs"
                                    ValidationExpression="^\d*\.?\d+$"
                                    ErrorMessage="Vul een positief nummer in"
                                    runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                            </div>
                        </div>


                        <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                            <div class="form-group">
                                <asp:Label runat="server" ID="verhuurprijslbl" AssociatedControlID="Verhuurprijs" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Verhuurprijs"/>
                                     <span> - Optioneel</span>
                                </asp:Label><asp:TextBox runat="server" ID="Verhuurprijs" CssClass="form-control" type="number" min="0" />
                                <asp:RegularExpressionValidator
                                    ID="RegularExpressionValidator3"
                                    ControlToValidate="Verhuurprijs"
                                    ValidationExpression="^\d*\.?\d+$"
                                    ErrorMessage="Vul een positief nummer in"
                                    runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                            </div>
                        </div>


                        <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                            <div class="form-group">
                                <asp:Label runat="server" ID="gewichtlbl" AssociatedControlID="Gewicht" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Gewicht"/>
                                    in kg <span> - Optioneel</span>
                                </asp:Label><asp:TextBox runat="server" ID="Gewicht" CssClass="form-control" type="number" min="0" />
                                <asp:RegularExpressionValidator
                                    ID="RegularExpressionValidator4"
                                    ControlToValidate="Gewicht"
                                    ValidationExpression="^\d*\.?\d+$"
                                    ErrorMessage="Vul een positief nummer in"
                                    runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                            </div>
                        </div>
                        
                        <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                            <div class="form-group">
                                <asp:Label runat="server" ID="Label74" AssociatedControlID="Stopcontact" CssClass="control-label">Stopcontact
                                     <span> - Optioneel</span>
                                </asp:Label><asp:TextBox runat="server" ID="Stopcontact" CssClass="form-control" type="number" min="0" />
                                <asp:RegularExpressionValidator
                                    ID="RegularExpressionValidator45"
                                    ControlToValidate="Stopcontact"
                                    ValidationExpression="^\d*\.?\d+$"
                                    ErrorMessage="Vul een positief nummer in"
                                    runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                            </div>
                        </div>

                        <div class="col-xs-12 col-sm-2 col-md-3 col-lg-2">
                            <div class="form-group">
                                <asp:Label runat="server" ID="verbruiksgoedlbl" AssociatedControlID="Verbruiksgoed" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Verbruiksgoed"/>
                                </asp:Label>
                                <input id="Verbruiksgoed" runat="server" type="checkbox" class="tgl tgl-light" />
                                <asp:Label runat="server" ID="Label73" AssociatedControlID="Verbruiksgoed" CssClass="tgl-btn"> </asp:Label>

                            </div>
                        </div>
                    </div>

                </div>
              
                <asp:HiddenField runat="server" ID="hdnvvd2" Value="" />


                

                <div class="article">

                    <h4>
                        <asp:Localize runat="server" meta:resourcekey="Eenheid" />
                        <span>- Optioneel</span></h4>
                    <hr />
                    <div class="input_fields_wrap">

                        <div class="row" runat="server" id="fieldEenheid1">

                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="eenheidnaamlbl" AssociatedControlID="NieuwEenheidnaam" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>

                                    </asp:Label><asp:TextBox runat="server" ID="NieuwEenheidnaam" CssClass="form-control" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator35"
                                        ControlToValidate="NieuwEenheidnaam"
                                        ValidationExpression="^.{3,}$"
                                        ErrorMessage="Minimum 3 karakters"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="eenheidaantallbl" AssociatedControlID="EenheidAantal" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="AantalPer"/>

                                    </asp:Label><asp:TextBox runat="server" ID="EenheidAantal" CssClass="form-control" type="number" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator25"
                                        ControlToValidate="EenheidAantal"
                                        ValidationExpression="^\d*\.?\d+$"
                                        ErrorMessage="Vul een positief nummer in"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>
                            
                            
                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label75" AssociatedControlID="ddlEenheidMultiplier0" CssClass="control-label">
                                        Veelvoud
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlEenheidMultiplier0" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2ItemSingle"  DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-5 col-md-3">
                                <div class="col-xs-11 col-md-10">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="eenheidbarcodelbl" AssociatedControlID="EenheidBarcode0" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Barcodenaam"/>
                                        </asp:Label><asp:TextBox runat="server" ID="EenheidBarcode0" CssClass="form-control" />
                                        <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator5"
                                            ControlToValidate="EenheidBarcode0"
                                            ValidationExpression="^.{3,}$"
                                            ErrorMessage="Minimum 3 karakters"
                                            runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />



                                    </div>
                                </div>
                                <div class="col-xs-1 col-xs-1 col-md-2 vertAlign">
                                    <asp:LinkButton runat="server" ID="barcodeButton1" class="button" OnClick="generateBarcode">
										<span class="glyphicon glyphicon-cog"></span>
                                    </asp:LinkButton>
                                </div>
                            </div>

                        </div>

                        <div class="row hiddenRow" runat="server" id="fieldEenheid2">

                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label28" AssociatedControlID="NieuwEenheidnaam1" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>

                                    </asp:Label><asp:TextBox runat="server" ID="NieuwEenheidnaam1" CssClass="form-control" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator36"
                                        ControlToValidate="NieuwEenheidnaam1"
                                        ValidationExpression="^.{3,}$"
                                        ErrorMessage="Minimum 3 karakters"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label29" AssociatedControlID="EenheidAantal1" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="AantalPer"/>

                                    </asp:Label><asp:TextBox runat="server" ID="EenheidAantal1" CssClass="form-control" type="number" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator26"
                                        ControlToValidate="EenheidAantal1"
                                        ValidationExpression="^\d*\.?\d+$"
                                        ErrorMessage="Vul een positief nummer in"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>
                            
                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label76" AssociatedControlID="ddlEenheidMultiplier1" CssClass="control-label">
                                        Veelvoud
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlEenheidMultiplier1" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2ItemSingle"  DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-xs-12 col-sm-5 col-md-3">
                                <div class="col-xs-11 col-md-10">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label30" AssociatedControlID="EenheidBarcode1" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Barcodenaam"/>
                                    <span> - Optioneel</span>
                                        </asp:Label><asp:TextBox runat="server" ID="EenheidBarcode1" CssClass="form-control" />

                                        <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator6"
                                            ControlToValidate="EenheidBarcode1"
                                            ValidationExpression="^.{3,}$"
                                            ErrorMessage="Minimum 3 karakters"
                                            runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />



                                    </div>
                                </div>
                                <div class="col-xs-1 col-md-2 vertAlign">
                                    <asp:LinkButton runat="server" ID="barcodeButton2" class="button" OnClick="generateBarcode">
										<span class="glyphicon glyphicon-cog"></span>
                                    </asp:LinkButton>
                                </div>
                            </div>

                        </div>

                        <div class="row hiddenRow" runat="server" id="fieldEenheid3">

                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label31" AssociatedControlID="NieuwEenheidnaam2" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>

                                    </asp:Label><asp:TextBox runat="server" ID="NieuwEenheidnaam2" CssClass="form-control" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator37"
                                        ControlToValidate="NieuwEenheidnaam2"
                                        ValidationExpression="^.{3,}$"
                                        ErrorMessage="Minimum 3 karakters"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label32" AssociatedControlID="EenheidAantal2" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="AantalPer"/>

                                    </asp:Label><asp:TextBox runat="server" ID="EenheidAantal2" CssClass="form-control" type="number" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator27"
                                        ControlToValidate="EenheidAantal2"
                                        ValidationExpression="^\d*\.?\d+$"
                                        ErrorMessage="Vul een positief nummer in"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>
                            
                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label77" AssociatedControlID="ddlEenheidMultiplier2" CssClass="control-label">
                                        Veelvoud
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlEenheidMultiplier2" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2ItemSingle"  DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-xs-12 col-sm-5 col-md-3">
                                <div class="col-xs-11 col-md-10">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label33" AssociatedControlID="EenheidBarcode2" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Barcodenaam"/>
                                    <span> - Optioneel</span>
                                        </asp:Label><asp:TextBox runat="server" ID="EenheidBarcode2" CssClass="form-control" />

                                        <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator7"
                                            ControlToValidate="EenheidBarcode2"
                                            ValidationExpression="^.{3,}$"
                                            ErrorMessage="Minimum 3 karakters"
                                            runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />

                                    </div>
                                </div>
                                <div class="col-xs-1 col-xs-1 col-md-2 vertAlign">
                                    <asp:LinkButton runat="server" ID="barcodeButton3" class="button" OnClick="generateBarcode">
										<span class="glyphicon glyphicon-cog"></span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>

                        <div class="row hiddenRow" runat="server" id="fieldEenheid4">

                            <div class="col-xs-12 col-sm-4 col-md-4 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label34" AssociatedControlID="NieuwEenheidnaam3" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>

                                    </asp:Label><asp:TextBox runat="server" ID="NieuwEenheidnaam3" CssClass="form-control" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator38"
                                        ControlToValidate="NieuwEenheidnaam3"
                                        ValidationExpression="^.{3,}$"
                                        ErrorMessage="Minimum 3 karakters"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label35" AssociatedControlID="EenheidAantal3" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="AantalPer"/>

                                    </asp:Label><asp:TextBox runat="server" ID="EenheidAantal3" CssClass="form-control" type="number" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator28"
                                        ControlToValidate="EenheidAantal3"
                                        ValidationExpression="^\d*\.?\d+$"
                                        ErrorMessage="Vul een positief nummer in"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>
                            
                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label78" AssociatedControlID="ddlEenheidMultiplier3" CssClass="control-label">
                                        Veelvoud
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlEenheidMultiplier3" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2ItemSingle"  DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-xs-12 col-sm-5 col-md-3">
                                <div class="col-xs-11 col-md-10">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label36" AssociatedControlID="EenheidBarcode3" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Barcodenaam"/>
                                    <span> - Optioneel</span>
                                        </asp:Label><asp:TextBox runat="server" ID="EenheidBarcode3" CssClass="form-control" />

                                        <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator8"
                                            ControlToValidate="EenheidBarcode3"
                                            ValidationExpression="^.{3,}$"
                                            ErrorMessage="Minimum 3 karakters"
                                            runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />

                                    </div>
                                </div>
                                <div class="col-xs-1 col-md-2 vertAlign">
                                    <asp:LinkButton runat="server" ID="barcodeButton4" class="button" OnClick="generateBarcode">
										<span class="glyphicon glyphicon-cog"></span>
                                    </asp:LinkButton>
                                </div>
                            </div>

                        </div>

                        <div class="row hiddenRow" runat="server" id="fieldEenheid5">

                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label37" AssociatedControlID="NieuwEenheidnaam4" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>

                                    </asp:Label><asp:TextBox runat="server" ID="NieuwEenheidnaam4" CssClass="form-control" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator39"
                                        ControlToValidate="NieuwEenheidnaam4"
                                        ValidationExpression="^.{3,}$"
                                        ErrorMessage="Minimum 3 karakters"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label38" AssociatedControlID="EenheidAantal4" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="AantalPer"/>

                                    </asp:Label><asp:TextBox runat="server" ID="EenheidAantal4" CssClass="form-control" type="number" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator29"
                                        ControlToValidate="EenheidAantal4"
                                        ValidationExpression="^\d*\.?\d+$"
                                        ErrorMessage="Vul een positief nummer in"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>
                            
                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label79" AssociatedControlID="ddlEenheidMultiplier4" CssClass="control-label">
                                        Veelvoud
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlEenheidMultiplier4" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2ItemSingle"  DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-5 col-md-3">
                                <div class="col-xs-11 col-md-10">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label39" AssociatedControlID="EenheidBarcode4" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Barcodenaam"/>
                                    <span> - Optioneel</span>
                                        </asp:Label><asp:TextBox runat="server" ID="EenheidBarcode4" CssClass="form-control" />

                                        <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator9"
                                            ControlToValidate="EenheidBarcode4"
                                            ValidationExpression="^.{3,}$"
                                            ErrorMessage="Minimum 3 karakters"
                                            runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />

                                    </div>
                                </div>
                                <div class="col-xs-1 col-md-2 vertAlign">
                                    <asp:LinkButton runat="server" ID="barcodeButton5" class="button" OnClick="generateBarcode">
										<span class="glyphicon glyphicon-cog"></span>
                                    </asp:LinkButton>
                                </div>
                            </div>

                        </div>

                        <div class="row hiddenRow" runat="server" id="fieldEenheid6">

                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label40" AssociatedControlID="NieuwEenheidnaam5" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>

                                    </asp:Label><asp:TextBox runat="server" ID="NieuwEenheidnaam5" CssClass="form-control" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator40"
                                        ControlToValidate="NieuwEenheidnaam5"
                                        ValidationExpression="^.{3,}$"
                                        ErrorMessage="Minimum 3 karakters"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label41" AssociatedControlID="EenheidAantal5" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="AantalPer"/>

                                    </asp:Label><asp:TextBox runat="server" ID="EenheidAantal5" CssClass="form-control" type="number" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator30"
                                        ControlToValidate="EenheidAantal5"
                                        ValidationExpression="^\d*\.?\d+$"
                                        ErrorMessage="Vul een positief nummer in"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>

                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label80" AssociatedControlID="ddlEenheidMultiplier5" CssClass="control-label">
                                        Veelvoud
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlEenheidMultiplier5" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2ItemSingle"  DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-xs-12 col-sm-5 col-md-3">
                                <div class="col-xs-11 col-md-10">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label42" AssociatedControlID="EenheidBarcode5" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Barcodenaam"/>
                                    <span> - Optioneel</span>
                                        </asp:Label><asp:TextBox runat="server" ID="EenheidBarcode5" CssClass="form-control" />

                                        <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator10"
                                            ControlToValidate="EenheidBarcode5"
                                            ValidationExpression="^.{3,}$"
                                            ErrorMessage="Minimum 3 karakters"
                                            runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />

                                    </div>
                                </div>
                                <div class="col-xs-1 col-md-2 vertAlign">
                                    <asp:LinkButton runat="server" ID="barcodeButton6" class="button" OnClick="generateBarcode">
										<span class="glyphicon glyphicon-cog"></span>
                                    </asp:LinkButton>
                                </div>
                            </div>

                        </div>

                        <div class="row hiddenRow" runat="server" id="fieldEenheid7">

                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label43" AssociatedControlID="NieuwEenheidnaam6" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>

                                    </asp:Label><asp:TextBox runat="server" ID="NieuwEenheidnaam6" CssClass="form-control" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator41"
                                        ControlToValidate="NieuwEenheidnaam6"
                                        ValidationExpression="^.{3,}$"
                                        ErrorMessage="Minimum 3 karakters"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label44" AssociatedControlID="EenheidAantal6" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="AantalPer"/>

                                    </asp:Label><asp:TextBox runat="server" ID="EenheidAantal6" CssClass="form-control" type="number" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator31"
                                        ControlToValidate="EenheidAantal6"
                                        ValidationExpression="^\d*\.?\d+$"
                                        ErrorMessage="Vul een positief nummer in"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>
                            
                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label81" AssociatedControlID="ddlEenheidMultiplier6" CssClass="control-label">
                                        Veelvoud
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlEenheidMultiplier6" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2ItemSingle"  DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-xs-12 col-sm-5 col-md-3">
                                <div class="col-xs-11 col-md-10">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label45" AssociatedControlID="EenheidBarcode6" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Barcodenaam"/>
                                    <span> - Optioneel</span>
                                        </asp:Label><asp:TextBox runat="server" ID="EenheidBarcode6" CssClass="form-control" />

                                        <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator11"
                                            ControlToValidate="EenheidBarcode6"
                                            ValidationExpression="^.{3,}$"
                                            ErrorMessage="Minimum 3 karakters"
                                            runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />

                                    </div>
                                </div>
                                <div class="col-xs-1 col-md-2 vertAlign">
                                    <asp:LinkButton runat="server" ID="barcodeButton7" class="button" OnClick="generateBarcode">
										<span class="glyphicon glyphicon-cog"></span>
                                    </asp:LinkButton>
                                </div>
                            </div>

                        </div>

                        <div class="row hiddenRow" runat="server" id="fieldEenheid8">

                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label46" AssociatedControlID="NieuwEenheidnaam7" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>

                                    </asp:Label><asp:TextBox runat="server" ID="NieuwEenheidnaam7" CssClass="form-control" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator42"
                                        ControlToValidate="NieuwEenheidnaam7"
                                        ValidationExpression="^.{3,}$"
                                        ErrorMessage="Minimum 3 karakters"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>

                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label47" AssociatedControlID="EenheidAantal7" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="AantalPer"/>

                                    </asp:Label><asp:TextBox runat="server" ID="EenheidAantal7" CssClass="form-control" type="number" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator32"
                                        ControlToValidate="EenheidAantal7"
                                        ValidationExpression="^\d*\.?\d+$"
                                        ErrorMessage="Vul een positief nummer in"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>
                            
                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label82" AssociatedControlID="ddlEenheidMultiplier7" CssClass="control-label">
                                        Veelvoud
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlEenheidMultiplier7" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2ItemSingle"  DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-xs-12 col-sm-5 col-md-3">
                                <div class="col-xs-11 col-md-10">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label48" AssociatedControlID="EenheidBarcode7" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Barcodenaam"/>
                                    <span> - Optioneel</span>
                                        </asp:Label><asp:TextBox runat="server" ID="EenheidBarcode7" CssClass="form-control" />

                                        <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator12"
                                            ControlToValidate="EenheidBarcode7"
                                            ValidationExpression="^.{3,}$"
                                            ErrorMessage="Minimum 3 karakters"
                                            runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />

                                    </div>
                                </div>
                                <div class="col-xs-1 col-md-2 vertAlign">
                                    <asp:LinkButton runat="server" ID="barcodeButton8" class="button" OnClick="generateBarcode">
										<span class="glyphicon glyphicon-cog"></span>
                                    </asp:LinkButton>
                                </div>
                            </div>

                        </div>

                        <div class="row hiddenRow" runat="server" id="fieldEenheid9">

                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label49" AssociatedControlID="NieuwEenheidnaam8" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>

                                    </asp:Label><asp:TextBox runat="server" ID="NieuwEenheidnaam8" CssClass="form-control" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator43"
                                        ControlToValidate="NieuwEenheidnaam8"
                                        ValidationExpression="^.{3,}$"
                                        ErrorMessage="Minimum 3 karakters"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label50" AssociatedControlID="EenheidAantal8" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="AantalPer"/>

                                    </asp:Label><asp:TextBox runat="server" ID="EenheidAantal8" CssClass="form-control" type="number" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator33"
                                        ControlToValidate="EenheidAantal8"
                                        ValidationExpression="^\d*\.?\d+$"
                                        ErrorMessage="Vul een positief nummer in"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>

                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label83" AssociatedControlID="ddlEenheidMultiplier8" CssClass="control-label">
                                        Veelvoud
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlEenheidMultiplier8" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2ItemSingle"  DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-xs-12 col-sm-5 col-md-3">
                                <div class="col-xs-11 col-md-10">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label51" AssociatedControlID="EenheidBarcode8" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Barcodenaam"/>
                                    <span> - Optioneel</span>
                                        </asp:Label><asp:TextBox runat="server" ID="EenheidBarcode8" CssClass="form-control" />

                                        <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator13"
                                            ControlToValidate="EenheidBarcode8"
                                            ValidationExpression="^.{3,}$"
                                            ErrorMessage="Minimum 3 karakters"
                                            runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />

                                    </div>
                                </div>
                                <div class="col-xs-1 col-md-2 vertAlign">
                                    <asp:LinkButton runat="server" ID="barcodeButton9" class="button" OnClick="generateBarcode">
										<span class="glyphicon glyphicon-cog"></span>
                                    </asp:LinkButton>
                                </div>
                            </div>

                        </div>

                        <div class="row hiddenRow" runat="server" id="fieldEenheid10">

                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label52" AssociatedControlID="NieuwEenheidnaam9" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>

                                    </asp:Label><asp:TextBox runat="server" ID="NieuwEenheidnaam9" CssClass="form-control" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator44"
                                        ControlToValidate="NieuwEenheidnaam9"
                                        ValidationExpression="^.{3,}$"
                                        ErrorMessage="Minimum 3 karakters"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label53" AssociatedControlID="EenheidAantal9" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="AantalPer"/>

                                    </asp:Label><asp:TextBox runat="server" ID="EenheidAantal9" CssClass="form-control" type="number" />
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator34"
                                        ControlToValidate="EenheidAantal9"
                                        ValidationExpression="^\d*\.?\d+$"
                                        ErrorMessage="Vul een positief nummer in"
                                        runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />
                                </div>
                            </div>
                            
                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label84" AssociatedControlID="ddlEenheidMultiplier9" CssClass="control-label">
                                        Veelvoud
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlEenheidMultiplier9" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2ItemSingle"  DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-5 col-md-3">
                                <div class="col-xs-11 col-md-10">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label54" AssociatedControlID="EenheidBarcode9" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Barcodenaam"/>
                                    <span> - Optioneel</span>
                                        </asp:Label><asp:TextBox runat="server" ID="EenheidBarcode9" CssClass="form-control" />

                                        <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator14"
                                            ControlToValidate="EenheidBarcode9"
                                            ValidationExpression="^.{3,}$"
                                            ErrorMessage="Minimum 3 karakters"
                                            runat="server" CssClass="text-danger errorMessage" Display="Dynamic" ValidationGroup="group1" />

                                    </div>
                                </div>
                                <div class="col-xs-1 col-md-2 vertAlign">
                                    <asp:LinkButton runat="server" ID="barcodeButton10" class="button" OnClick="generateBarcode">
										<span class="glyphicon glyphicon-cog"></span>
                                    </asp:LinkButton>
                                </div>
                            </div>

                        </div>

                        <asp:LinkButton runat="server" ID="EenheidVerwijderen" class="button add_location_button locatieBtn" OnClick="EenheidVerwijderen_Click" CssClass="hiddenRow">
										- Eenheid verwijderen
                        </asp:LinkButton>


                        <asp:LinkButton runat="server" ID="EenheidToevoegen" class="button add_unit_button" OnClick="eenheidToevoegen">
										+ Eenheid toevoegen
                        </asp:LinkButton>
                    </div>
                </div>

                <asp:HiddenField runat="server" ID="HiddenFieldKenmerk1" Value="" OnValueChanged="kenmerkValueChanged" />
                <asp:HiddenField runat="server" ID="HiddenFieldKenmerk2" Value="" OnValueChanged="kenmerkValueChanged" />
                <asp:HiddenField runat="server" ID="HiddenFieldKenmerk3" Value="" OnValueChanged="kenmerkValueChanged" />
                <asp:HiddenField runat="server" ID="HiddenFieldKenmerk4" Value="" OnValueChanged="kenmerkValueChanged" />
                <asp:HiddenField runat="server" ID="HiddenFieldKenmerk5" Value="" OnValueChanged="kenmerkValueChanged" />
                <asp:HiddenField runat="server" ID="HiddenFieldKenmerk6" Value="" OnValueChanged="kenmerkValueChanged" />
                <asp:HiddenField runat="server" ID="HiddenFieldKenmerk7" Value="" OnValueChanged="kenmerkValueChanged" />
                <asp:HiddenField runat="server" ID="HiddenFieldKenmerk8" Value="" OnValueChanged="kenmerkValueChanged" />
                <asp:HiddenField runat="server" ID="HiddenFieldKenmerk9" Value="" OnValueChanged="kenmerkValueChanged" />
                <asp:HiddenField runat="server" ID="HiddenFieldKenmerk10" Value="" OnValueChanged="kenmerkValueChanged" />


                <div class="article">

                    <h4>
                        <asp:Localize runat="server" meta:resourcekey="Kenmerken" />
                        <span>- Optioneel</span></h4>
                    <hr />
                    <div class="input_fields_wrap">

                        <div class="row" runat="server" id="fieldKenmerk">

                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="kenmerknaamlbl" AssociatedControlID="ddlKenmerkNaam" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>
                                    </asp:Label><asp:DropDownList ID="ddlKenmerkNaam" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Item ddlKenmerk1" onchange="javascript:kenmerkChanged(1);" DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="kenmerwaardelbl" AssociatedControlID="KenmerkWaarde" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Waarde"/>

                                    </asp:Label><asp:TextBox runat="server" ID="KenmerkWaarde" CssClass="form-control" />
                                </div>
                            </div>

                        </div>

                        <div class="row hiddenRow" runat="server" id="fieldKenmerk1">

                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label55" AssociatedControlID="ddlKenmerkNaam1" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>
                                    </asp:Label><asp:DropDownList ID="ddlKenmerkNaam1" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Item ddlKenmerk2" onchange="javascript:kenmerkChanged(2);" DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label56" AssociatedControlID="KenmerkWaarde1" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Waarde"/>

                                    </asp:Label><asp:TextBox runat="server" ID="KenmerkWaarde1" CssClass="form-control" />
                                </div>
                            </div>

                        </div>

                        <div class="row hiddenRow" runat="server" id="fieldKenmerk2">

                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label57" AssociatedControlID="ddlKenmerkNaam2" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>
                                    </asp:Label><asp:DropDownList ID="ddlKenmerkNaam2" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Item ddlKenmerk3" onchange="javascript:kenmerkChanged(3);" DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label58" AssociatedControlID="KenmerkWaarde2" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Waarde"/>

                                    </asp:Label><asp:TextBox runat="server" ID="KenmerkWaarde2" CssClass="form-control" />
                                </div>
                            </div>

                        </div>

                        <div class="row hiddenRow" runat="server" id="fieldKenmerk3">

                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label59" AssociatedControlID="ddlKenmerkNaam3" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>
                                    </asp:Label><asp:DropDownList ID="ddlKenmerkNaam3" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Item ddlKenmerk4" onchange="javascript:kenmerkChanged(4);" DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label60" AssociatedControlID="KenmerkWaarde3" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Waarde"/>

                                    </asp:Label><asp:TextBox runat="server" ID="KenmerkWaarde3" CssClass="form-control" />
                                </div>
                            </div>

                        </div>

                        <div class="row hiddenRow" runat="server" id="fieldKenmerk4">

                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label61" AssociatedControlID="ddlKenmerkNaam4" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>
                                    </asp:Label><asp:DropDownList ID="ddlKenmerkNaam4" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Item ddlKenmerk5" onchange="javascript:kenmerkChanged(5);" DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label62" AssociatedControlID="KenmerkWaarde4" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Waarde"/>

                                    </asp:Label><asp:TextBox runat="server" ID="KenmerkWaarde4" CssClass="form-control" />
                                </div>
                            </div>

                        </div>

                        <div class="row hiddenRow" runat="server" id="fieldKenmerk5">

                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label63" AssociatedControlID="ddlKenmerkNaam5" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>
                                    </asp:Label><asp:DropDownList ID="ddlKenmerkNaam5" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Item ddlKenmerk6" onchange="javascript:kenmerkChanged(6);" DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label64" AssociatedControlID="KenmerkWaarde5" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Waarde"/>

                                    </asp:Label><asp:TextBox runat="server" ID="KenmerkWaarde5" CssClass="form-control" />
                                </div>
                            </div>

                        </div>

                        <div class="row hiddenRow" runat="server" id="fieldKenmerk6">

                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label65" AssociatedControlID="ddlKenmerkNaam6" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>
                                    </asp:Label><asp:DropDownList ID="ddlKenmerkNaam6" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Item ddlKenmerk7" onchange="javascript:kenmerkChanged(7);" DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label66" AssociatedControlID="KenmerkWaarde6" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Waarde"/>

                                    </asp:Label><asp:TextBox runat="server" ID="KenmerkWaarde6" CssClass="form-control" />
                                </div>
                            </div>

                        </div>

                        <div class="row hiddenRow" runat="server" id="fieldKenmerk7">

                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label67" AssociatedControlID="ddlKenmerkNaam7" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>
                                    </asp:Label><asp:DropDownList ID="ddlKenmerkNaam7" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Item ddlKenmerk8" onchange="javascript:kenmerkChanged(8);" DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label68" AssociatedControlID="KenmerkWaarde7" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Waarde"/>

                                    </asp:Label><asp:TextBox runat="server" ID="KenmerkWaarde7" CssClass="form-control" />
                                </div>
                            </div>

                        </div>

                        <div class="row hiddenRow" runat="server" id="fieldKenmerk8">

                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label69" AssociatedControlID="ddlKenmerkNaam8" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>
                                    </asp:Label><asp:DropDownList ID="ddlKenmerkNaam8" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Item ddlKenmerk9" onchange="javascript:kenmerkChanged(9);" DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label70" AssociatedControlID="KenmerkWaarde8" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Waarde"/>

                                    </asp:Label><asp:TextBox runat="server" ID="KenmerkWaarde8" CssClass="form-control" />
                                </div>
                            </div>

                        </div>

                        <div class="row hiddenRow" runat="server" id="fieldKenmerk9">

                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label71" AssociatedControlID="ddlKenmerkNaam9" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Artikelnaam"/>
                                    </asp:Label><asp:DropDownList ID="ddlKenmerkNaam9" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Item ddlKenmerk10" onchange="javascript:kenmerkChanged(10);" DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>


                            <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="Label72" AssociatedControlID="KenmerkWaarde9" CssClass="control-label"><asp:Localize runat=server meta:resourcekey="Waarde"/>

                                    </asp:Label><asp:TextBox runat="server" ID="KenmerkWaarde9" CssClass="form-control" />
                                </div>
                            </div>

                        </div>

                        <asp:LinkButton runat="server" ID="KenmerkVerwijderen" class="button add_location_button locatieBtn" OnClick="KenmerkVerwijderen_Click" CssClass="hiddenRow">
										- Kenmerk verwijderen
                        </asp:LinkButton>

                        <asp:LinkButton runat="server" ID="kenmerkToevoegen" class="button add_property_button" OnClick="KenmerkToevoegen_Click">
										+ Kenmerk toevoegen
                        </asp:LinkButton>
                    </div>

                </div>
                
            <div class="article" runat="server">

            <h4>
                Verantwoordelijke<span> - Optioneel</span></h4>
            <hr />
            <div class="row" runat="server">
                <div class="col-xs-12 col-sm-4 col-md-4 col-lg-4 ">
                    <div class="form-group">
                        <asp:Label runat="server" ID="Label1" AssociatedControlID="Verantwoordelijke" CssClass="control-label">Verantwoordelijke
                        </asp:Label><asp:ListBox ID="Verantwoordelijke" SelectionMode="Multiple" runat="server" DataTextField="Naam" Style="width: 100%;" CssClass="form-control select2Multiple" multiple="multiple" DataValueField="Id"></asp:ListBox>
                    </div>

                </div> 
                </div>
                </div>

                <br />

                <a href="overzicht.aspx" runat="server" class="btn btn-danger cancelButton">Annuleren</a>

                <asp:LinkButton runat="server" ID="LinkButton1" class="btn btn-default btnNieuw " OnClick="saveArticle2" ValidationGroup="group1" CausesValidation="true">
										Opslaan
                </asp:LinkButton>
            </div>
        </ContentTemplate>

    </asp:UpdatePanel>

    <script type="text/javascript">

        var vvdList = ["a", "a", "a", "a", "a", "a", "a", "a", "a", "a"];
        var vvdList2 = ["a", "a", "a", "a", "a", "a", "a", "a", "a", "a"];


        function pageLoad(sender, args) {


            $('.select2ItemSingle').select2({
            });



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

            $('.select2Multiple').select2({
                language: {
                    "noResults": function () {
                        return "Geen resultaten";
                    }
                },
                escapeMarkup: function (markup) {
                    return markup;
                }
            });

            var $el = $('.select2Multiple');
            $el.on('select2:unselecting', function (e) {
                $el.data('unselecting', true);
            }).on('select2:open', function (e) {
                if ($el.data('unselecting')) {
                    $el.removeData('unselecting');
                    $el.select2('close');
                }
            });
            
            $(".select2Multiple").on("change", function () {
                var gebruikers = $(".select2Multiple").val();

                $("#MainContent_hdnMedewerkers").val(gebruikers);
                __doPostBack('<%= UpdatePanel1.ClientID %>', null);
            });


            $(".ddlCategorie").on("change", function () {
                var naam = $(".ddlCategorie").val();
                $("#MainContent_hdnDDL1").val(naam);
                document.getElementById("MainContent_hdnDDL1").value = naam;
                __doPostBack('<%=UpdatePanel1.ClientID%>', null);
            })

            $(".ddlMerk").on("change", function () {
                var naam = $(".ddlMerk").val();
                $("#MainContent_hdnDDL2").val(naam);
                document.getElementById("MainContent_hdnDDL2").value = naam;
                __doPostBack('<%=UpdatePanel1.ClientID%>', null);
            })
            $(".ddlKenmerk").on("change", function () {
                var naam = $(".ddlKenmerk").val();
                $("#MainContent_hdnDDL4").val(naam);
                document.getElementById("MainContent_hdnDDL4").value = naam;
                __doPostBack('<%=UpdatePanel1.ClientID%>', null);
            })

            //$(".col-md-2").matchHeight();
            $('.confirmButton').on('click', function () {
                //$.fn.matchHeight()._update();
                //Voorlopige oplossing
                //$(window).trigger('resize');
            })

        




        }

  

        function kenmerkChanged(id) {
            var naam = $(".ddlKenmerk" + id).val();
            $("#MainContent_HiddenFieldKenmerk" + id).val(naam);
            document.getElementById("MainContent_HiddenFieldKenmerk" + id).value = naam;
            __doPostBack('<%=UpdatePanel1.ClientID%>', null);
        }

     



    </script>



</asp:Content>



