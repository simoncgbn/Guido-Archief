using GuidoStock.App_Code;
using GuidoStock.Code;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GuidoStock.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GuidoStock.Stock
{
    public partial class AddArtikel : System.Web.UI.Page
    {
        private int countLocations
        {
            get { return (int) ViewState["countLocations"]; }
            set { ViewState["countLocations"] = value; }
        }
        private int countUnits
        {
            get { return (int) ViewState["countUnits"]; }
            set { ViewState["countUnits"] = value; }
        }
        private int countKenmerken
        {
            get { return (int) ViewState["countKenmerken"]; }
            set { ViewState["countKenmerken"] = value; }
        }
        private bool exist
        {
            get { return (bool) ViewState["exist"]; }
            set { ViewState["exist"] = value; }
        }
        private int cntrLocs 
        {
            get { return (int) ViewState["cntrLocs"]; }
            set { ViewState["cntrLocs"] = value; }
        }
        private List<string> vvdList
        {
            get { return (List<string>) ViewState["vvdList"]; }
            set { ViewState["vvdList"] = value; }
        }
        private List<int> countList
        {
            get { return (List<int>) ViewState["countList"]; }
            set { ViewState["countList"] = value; }
        }
        private string updateArtikelId
        {
            get { return (string) ViewState["updateArtikelId"]; }
            set { ViewState["updateArtikelId"] = value; }
        }
        private string Type
        {
            get { return (string)ViewState["Type"]; }
            set { ViewState["Type"] = value; }
        }
        private Artikel selectedArtikel
        {
            get { return (Artikel) ViewState["selectedArtikel"]; }
            set { ViewState["selectedArtikel"] = value; }
        }

        private DBClass db = new DBClass();
        private String value;
      

        //Locatie
        List<string> defPlaatsArr = new List<string>();
        List<string> defPlaatsNaamArr = new List<string>();
        List<string> defVervalArr = new List<string>();
        List<string> defAantalArr = new List<string>();
        List<DropDownList> dropDownLists;
        public List<TextBox> vervalDatums;
        List<TextBox> aantallen;
        List<HtmlGenericControl> fieldsArray;
        List<HiddenField> hdnLocArray;

        //Eenheid
        List<string> defEenheidNaamArr = new List<string>();
        List<string> defEenheidAantalArr = new List<string>();
        List<string> defEenheidMultiArr = new List<string>();
        List<string> defEenheidBarcodeArr = new List<string>();
        List<TextBox> eenheidNaam;
        List<TextBox> eenheidAantallen;
        List<DropDownList> eenheidMultipliers;
        List<TextBox> eenheidBarcodes;
        List<HtmlGenericControl> fieldsEenheidArray;

        //Kenmerk
        List<string> defKenmerkArr = new List<string>();
        List<string> defKenmerkNaamArr = new List<string>();
        List<string> defKenmerkWaardeArr = new List<string>();
        List<DropDownList> dropDownListsKenmerk;
        List<TextBox> kenmerkWaardes;
        List<HtmlGenericControl> fieldsKenmerkArray;
        List<HiddenField> hdnKenmerkArray;





        protected void Page_Load(object sender, EventArgs e)
        {
           
            // Eenheid
            eenheidNaam = new List<TextBox>() { NieuwEenheidnaam, NieuwEenheidnaam1, NieuwEenheidnaam2, NieuwEenheidnaam3, NieuwEenheidnaam4, NieuwEenheidnaam5, NieuwEenheidnaam6, NieuwEenheidnaam7, NieuwEenheidnaam8, NieuwEenheidnaam9 };
            eenheidAantallen = new List<TextBox>() { EenheidAantal, EenheidAantal1, EenheidAantal2, EenheidAantal3, EenheidAantal4, EenheidAantal5, EenheidAantal6, EenheidAantal7, EenheidAantal8, EenheidAantal9 };
            eenheidMultipliers = new List<DropDownList>() { ddlEenheidMultiplier0, ddlEenheidMultiplier1, ddlEenheidMultiplier2, ddlEenheidMultiplier3, ddlEenheidMultiplier4, ddlEenheidMultiplier5, ddlEenheidMultiplier6, ddlEenheidMultiplier7, ddlEenheidMultiplier8, ddlEenheidMultiplier9};
            eenheidBarcodes = new List<TextBox>() { EenheidBarcode0, EenheidBarcode1, EenheidBarcode2, EenheidBarcode3, EenheidBarcode4, EenheidBarcode5, EenheidBarcode6, EenheidBarcode7, EenheidBarcode8, EenheidBarcode9 };
            fieldsEenheidArray = new List<HtmlGenericControl>() { fieldEenheid1, fieldEenheid2, fieldEenheid3, fieldEenheid4, fieldEenheid5, fieldEenheid6 ,fieldEenheid7, fieldEenheid8, fieldEenheid9, fieldEenheid10 };

            //Kenmerk
            dropDownListsKenmerk = new List<DropDownList>() { ddlKenmerkNaam, ddlKenmerkNaam1, ddlKenmerkNaam2, ddlKenmerkNaam3, ddlKenmerkNaam4, ddlKenmerkNaam5, ddlKenmerkNaam6, ddlKenmerkNaam7, ddlKenmerkNaam8, ddlKenmerkNaam9 };
            kenmerkWaardes = new List<TextBox>() { KenmerkWaarde, KenmerkWaarde1, KenmerkWaarde2, KenmerkWaarde3, KenmerkWaarde4, KenmerkWaarde5, KenmerkWaarde6, KenmerkWaarde7, KenmerkWaarde8, KenmerkWaarde9 };
            fieldsKenmerkArray = new List<HtmlGenericControl>() { fieldKenmerk, fieldKenmerk1, fieldKenmerk2, fieldKenmerk3, fieldKenmerk4, fieldKenmerk5, fieldKenmerk6, fieldKenmerk7, fieldKenmerk8, fieldKenmerk9 };
            hdnKenmerkArray = new List<HiddenField>() { HiddenFieldKenmerk1, HiddenFieldKenmerk2, HiddenFieldKenmerk3, HiddenFieldKenmerk4, HiddenFieldKenmerk5, HiddenFieldKenmerk6, HiddenFieldKenmerk7, HiddenFieldKenmerk8, HiddenFieldKenmerk9, HiddenFieldKenmerk10 };

           

            if (!IsPostBack)
            {
                exist = false;

                countLocations = 0;
                countUnits = 0;
                countKenmerken = 0;
                cntrLocs = 0;
                vvdList = new List<string>();
                countList = new List<int>();

                updateArtikelId = Request.QueryString["id"];
                Type = Request.QueryString["type"];

                ddlNieuwMerknaam.DataSource = db.GetMerken();
                ddlNieuwMerknaam.DataBind();

                ddlNieuwCategorienaam.DataSource = db.GetCategorien();
                ddlNieuwCategorienaam.DataBind();

                // Medewerkers invullen
                Verantwoordelijke.DataSource = db.GetGebruikers();
                Verantwoordelijke.DataBind();


                List<UnitMultiplierModel> unitMultipliers = new List<UnitMultiplierModel>();
                UnitMultiplierModel umm = new UnitMultiplierModel(0,"Individueel artikel");
                unitMultipliers.Add(umm);
                ddlEenheidMultiplier0.DataSource = unitMultipliers;
                ddlEenheidMultiplier0.DataBind();

                var metaTags = db.GetMetaTags();
                var defMetaTags = new List<ArtikelMetaTag>();
                foreach (var tag in metaTags)
                {
                    bool exist = false;
                    foreach (var meta in defMetaTags.Where(meta => tag.Naam == meta.Naam))
                    {
                        exist = true;
                    }
                    if (!exist)
                    {
                        defMetaTags.Add(tag);
                    }
                }
                ddlKenmerkNaam.DataSource = defMetaTags;
                ddlKenmerkNaam.DataBind();

                if (!string.IsNullOrEmpty(updateArtikelId))
                {
                    int n;
                    bool parsed = int.TryParse(updateArtikelId, out n);



                    if (parsed)
                    {
                        List<Artikel> artikelen = db.GetArtikelen();

                        foreach (var art in artikelen.Where(art => art.Id == int.Parse(updateArtikelId)))
                        {
                            fillDetails();
                        }
                    }
                }
               
                

            }
        }

       
        

        private void fillDetails()
        {

            exist = true;

            selectedArtikel = db.GetArtikel(int.Parse(updateArtikelId));
            selectedArtikel.Units = selectedArtikel.Units.Where(x => x.ChildUnitId != 0).ToList();

            cntrLocs = 0;
            int cntrunits = 0;
            int cntrknmrk = 0;

            vvdList.Clear();
            countList.Clear();

        // Algemeen

        NieuwArtikelnaam.Text = selectedArtikel.Naam;
            ddlNieuwMerknaam.Items.FindByValue(selectedArtikel.Merk.Id.ToString()).Selected = true;
            ddlNieuwCategorienaam.Items.FindByValue(selectedArtikel.Categorie.Id.ToString()).Selected = true;
            
            if(selectedArtikel.Barcode != "")
            {
                Barcodenaam.Text = selectedArtikel.Barcode;
            }
            if(selectedArtikel.Naturaprijs != -1)
            {
                Naturaprijs.Text = selectedArtikel.Naturaprijs.ToString();
            }

            if (selectedArtikel.Verhuurprijs != -1)
            {
                Verhuurprijs.Text = selectedArtikel.Verhuurprijs.ToString();
            }

            if (selectedArtikel.Gewicht != -1)
            {
                Gewicht.Text = selectedArtikel.Gewicht.ToString();
            }

            Verbruiksgoed.Checked = selectedArtikel.IsHerbruikbaar;


            // eenheid
            var childList = new List<Dictionary<int, int>>();
            var idList = new List<Dictionary<int, int>>();
            foreach (var unit in selectedArtikel.Units)
            {
                if (unit.ChildUnitId == 0) continue;
                if (cntrunits > 0)
                {
                    fieldsEenheidArray[cntrunits].Style.Add("display", "block");
                    EenheidVerwijderen.Style.Add("display", "block");
                }

                eenheidNaam[cntrunits].Text = unit.NaamEnkelvoud;
                eenheidBarcodes[cntrunits].Text = unit.Barcode;

                // Deel aantal door childunit aantal
                Dictionary<int, int> d = new Dictionary<int, int>();
                d.Add(unit.Id, unit.Aantal);
                Dictionary<int, int> c = new Dictionary<int, int>();
                c.Add(unit.Id, unit.Id);
                var isChild = false;
                var parent = 0;

                foreach (var u in childList)
                {
                    if (u.ContainsKey(unit.ChildUnitId))
                    {
                        unit.Aantal /= u[unit.ChildUnitId];
                        isChild = true;
                        foreach (var a in idList)
                        {
                            if (a.ContainsKey(unit.ChildUnitId))
                            {
                                parent = a[unit.ChildUnitId];
                            }
                        }
                        break;
                    }
                }
                childList.Add(d);
                idList.Add(c);
                eenheidAantallen[cntrunits].Text = unit.Aantal.ToString();
                // Selecteer childunit in dropdown
                var datasc = new List<UnitMultiplierModel>();
                datasc.Add(new UnitMultiplierModel(0, "Individueel artikel"));

                for (var i = 0; i < cntrunits + 1; i++)
                {
                    if (i == cntrunits) continue;
                    datasc.Add(new UnitMultiplierModel(i + 1, eenheidNaam[i].Text));
                }

                var index = "";
                eenheidMultipliers[cntrunits].DataSource = datasc;
                eenheidMultipliers[cntrunits].DataBind();

                if (isChild)
                {
                    var uns = db.GetUnitsForArtikel(selectedArtikel.Id);
                    foreach (var uni in uns)
                    {
                        if (uni.Id == parent)
                        {
                            index = uni.NaamEnkelvoud;
                            break;
                        }
                    }
                    eenheidMultipliers[cntrunits].Items.FindByText(index).Selected = true;
                }
                else
                {
                    eenheidMultipliers[cntrunits].SelectedIndex = 0;
                }
                cntrunits++;
            }

            //kenmerk

            foreach(var kenmerk in selectedArtikel.ArtikelMetaTags)
            {
                if (cntrknmrk > 0)
                {
                    fieldsKenmerkArray[cntrknmrk].Style.Add("display", "block");
                    KenmerkVerwijderen.Style.Add("display", "block");

                }

                dropDownListsKenmerk[cntrknmrk].DataSource = db.GetMetaTags();
                dropDownListsKenmerk[cntrknmrk].DataBind();

                dropDownListsKenmerk[cntrknmrk].Items.FindByValue(kenmerk.Id.ToString()).Selected = true;
                kenmerkWaardes[cntrknmrk].Text = kenmerk.Waarde.ToString();

                cntrknmrk++;
            }

            countLocations = cntrLocs -1;
            countUnits = cntrunits -1;
            countKenmerken = cntrknmrk -1;

            if (countUnits == -1) countUnits = 0;
            if (countKenmerken == -1) countKenmerken = 0;

            // Verantwoordelijke
            var v = db.GetArtikelVerantwoordelijkeById(selectedArtikel.Id);
            if (!v.Any()) return;
            var lst = Verantwoordelijke;
            foreach (var g in v)
            {
                lst.Items.FindByValue(g.UserId.ToString()).Selected = true;
            }

        }

        protected void generateBarcode(object sender, EventArgs e)
        {
            var id = (sender as Control).ClientID;
            string idNumber = Regex.Match(id, @"\d+").Value;

            if(idNumber == "0")
            {
                if (NieuwArtikelnaam.Text.Length >= 3)
                {
                    Barcodenaam.Text = NieuwArtikelnaam.Text;
                }
                else
               { 
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertUser", "BootstrapDialog.alert({title:'Foutieve artikelnaam', message: 'Gelieve eerst een artikelnaam op te geven. Een artikelnaam moet tenminste 3 karakters bevatten.',cssClass: 'bootstrapDialog'});", true);
                }
            }
            else
            {
                if(eenheidNaam[int.Parse(idNumber) - 1].Text.Length >= 3 && NieuwArtikelnaam.Text.Length >= 3)
                {
                    eenheidBarcodes[int.Parse(idNumber) - 1].Text = NieuwArtikelnaam.Text + "_" + eenheidNaam[int.Parse(idNumber) - 1].Text;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertUser", "BootstrapDialog.alert({title:'Foutieve artikelnaam', message: 'Gelieve eerst een artikelnaam en eenheidnaam op te geven. Een naam moet tenminste 3 karakters bevatten.',cssClass: 'bootstrapDialog'});", true);
                }

            }
        }

      
        protected void saveArticle()
        {
            var DuplicateBarcode = false;
            
                foreach (var c in db.GetArtikelen())
                {
                    if (exist)
                    {
                        if (c.Id == selectedArtikel.Id) continue;
                    }
                    if (c.Naam == NieuwArtikelnaam.Text)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertUser",
                            "BootstrapDialog.show({title: 'Opslaan mislukt!', message: 'Gelieve een unieke artikelnaam in te vullen.', buttons: [{ label: 'Sluiten', action: function(dialogRef){ dialogRef.close();}}],  type: BootstrapDialog.TYPE_DANGER}); ", true);
                        DuplicateBarcode = true;
                        break;
                    }
                }
            
           
            if (!DuplicateBarcode)
            {
                for (var q = 0; q < defEenheidAantalArr.Count; q++)
                {
                    if (defEenheidBarcodeArr[q] == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertUser",
                            "BootstrapDialog.show({title: 'Opslaan mislukt!', message: 'Gelieve geen enkele barcode leeg te laten.', buttons: [{ label: 'Sluiten', action: function(dialogRef){ dialogRef.close();}}],  type: BootstrapDialog.TYPE_DANGER}); ", true);
                        DuplicateBarcode = true;
                        break;
                    }

                    if (Barcodenaam.Text == defEenheidBarcodeArr[q])
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertUser",
                            "BootstrapDialog.show({title: 'Opslaan mislukt!', message: 'Gelieve niet twee dezelfde barcodes in te vullen.', buttons: [{ label: 'Sluiten', action: function(dialogRef){ dialogRef.close();}}],  type: BootstrapDialog.TYPE_DANGER}); ", true);
                        DuplicateBarcode = true;
                        break;
                    }

                    var artikelen = db.GetArtikelen();
                    foreach (var a in artikelen)
                    {
                        if (a.Barcode == defEenheidBarcodeArr[q])
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertUser",
                                "BootstrapDialog.show({title: 'Opslaan mislukt!', message: 'Een1 ingevulde barcode is al in gebruik.', buttons: [{ label: 'Sluiten', action: function(dialogRef){ dialogRef.close();}}],  type: BootstrapDialog.TYPE_DANGER}); ", true);
                            DuplicateBarcode = true;
                            break;
                        }

                        if (exist)
                        {
                            if (a.Id == selectedArtikel.Id) continue;
                        }
                        if (a.Units != null)
                        {
                            foreach (var u in a.Units)
                            {
                                if (u.Barcode == defEenheidBarcodeArr[q])
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertUser",
                                        "BootstrapDialog.show({title: 'Opslaan mislukt!', message: 'Een2 ingevulde barcode is al in gebruik.', buttons: [{ label: 'Sluiten', action: function(dialogRef){ dialogRef.close();}}],  type: BootstrapDialog.TYPE_DANGER}); ", true);
                                    DuplicateBarcode = true;
                                    break;
                                }
                            }
                        }
                        
                       
                        if (a.Barcode == Barcodenaam.Text)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertUser",
                                "BootstrapDialog.show({title: 'Opslaan mislukt!', message: 'Een3 ingevulde barcode is al in gebruik.', buttons: [{ label: 'Sluiten', action: function(dialogRef){ dialogRef.close();}}],  type: BootstrapDialog.TYPE_DANGER}); ", true);
                            DuplicateBarcode = true;
                            break;
                        }
                    }

                }
            }

            if (!DuplicateBarcode)
            {
                if (defEenheidBarcodeArr.Count != defEenheidBarcodeArr.Distinct().Count())
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertUser",
                        "BootstrapDialog.show({title: 'Opslaan mislukt!', message: 'Gelieve niet twee dezelfde barcodes in te vullen.', buttons: [{ label: 'Sluiten', action: function(dialogRef){ dialogRef.close();}}],  type: BootstrapDialog.TYPE_DANGER}); ", true);
                    DuplicateBarcode = true;
                }
            }

           

            if (!DuplicateBarcode)
            {
                if (Page.IsValid)
                {

                    int categorieId, merkId, artikelId, basicUnitId;
                    string naturaprijs, verhuurprijs, gewicht, stopcontact;
                    float unitPrijs;

                    List<int> artkLocId = new List<int>();
                    List<string> vvdata = new List<string>();

                    int catId = int.Parse(ddlNieuwCategorienaam.SelectedItem.Value);
                    int mrkId = int.Parse(ddlNieuwMerknaam.SelectedItem.Value);



                    if (catId == int.MaxValue)
                    {
                        categorieId = db.AddCategorie(ddlNieuwCategorienaam.SelectedItem.Text);
                    }
                    else
                    {
                        categorieId = catId;
                    }

                    if (mrkId == int.MaxValue)
                    {
                        merkId = db.AddMerk(ddlNieuwMerknaam.SelectedItem.Text);
                    }
                    else if (string.IsNullOrEmpty(mrkId.ToString()))
                    {
                        merkId = -1;
                    }
                    else
                    {
                        merkId = mrkId;
                    }


                    naturaprijs = string.IsNullOrEmpty(Naturaprijs.Text) ? "-1" : Naturaprijs.Text.Replace('.', ',');
                    verhuurprijs = string.IsNullOrEmpty(Verhuurprijs.Text) ? "-1" : Verhuurprijs.Text.Replace('.', ',');
                    gewicht = string.IsNullOrEmpty(Gewicht.Text) ? "-1" : Gewicht.Text;
                    stopcontact = string.IsNullOrEmpty(Stopcontact.Text) ? "-1" : Stopcontact.Text.Replace('.', ',');


                    if (!exist)
                    {
                        artikelId = db.AddArtikel(NieuwArtikelnaam.Text, Barcodenaam.Text,
                            float.Parse(gewicht, CultureInfo.InvariantCulture),
                            float.Parse(naturaprijs, CultureInfo.InvariantCulture),
                            float.Parse(verhuurprijs, CultureInfo.InvariantCulture), Verbruiksgoed.Checked, merkId,
                            categorieId, int.Parse(stopcontact));

                        basicUnitId = db.AddUnit(NieuwArtikelnaam.Text, 1, 0, Barcodenaam.Text, artikelId,0);

                        List<Code.Unit> unitsList = new List<Code.Unit>();
                        for (int i = 0; i < defEenheidNaamArr.Count; i++)
                        {
                            if (naturaprijs != "-1")
                            {
                                unitPrijs = float.Parse(naturaprijs) * int.Parse(defEenheidAantalArr[i]);
                            }
                            else
                            {
                                unitPrijs = -1;
                            }

                            if (defEenheidMultiArr[i] != "Individueel artikel")
                            {
                                for (var j = 0; j < defEenheidMultiArr.Count; j++)
                                {
                                    if (defEenheidMultiArr[i] == defEenheidNaamArr[j])
                                    {
                                        defEenheidAantalArr[i] =
                                            (int.Parse(defEenheidAantalArr[i]) * int.Parse(defEenheidAantalArr[j]))
                                            .ToString();
                                        break;
                                    }
                                }
                            }

                            Code.Unit u = new Code.Unit(defEenheidNaamArr[i], int.Parse(defEenheidAantalArr[i]), defEenheidBarcodeArr[i], artikelId, defEenheidMultiArr[i]);
                            unitsList.Add(u);

                        }


                        unitsList = unitsList.OrderBy(x => x.Aantal).ToList();
                        var idList = new List<Dictionary<string, int>>();

                        foreach (var u in unitsList)
                        {
                            int ehdId;
                            if (u.UnitChildString == "Individueel artikel")
                            {
                                ehdId = db.AddUnit(u.NaamEnkelvoud, u.Aantal, 0, u.Barcode, u.ArtikelId, basicUnitId);
                                Dictionary<string,int> d = new Dictionary<string, int>();
                                d.Add(u.NaamEnkelvoud,ehdId);
                                idList.Add(d);
                            }
                            else
                            {
                                foreach (var id in idList)
                                {
                                    if (id.ContainsKey(u.UnitChildString))
                                    {
                                        ehdId = db.AddUnit(u.NaamEnkelvoud, u.Aantal, 0, u.Barcode, u.ArtikelId, id[u.UnitChildString]);
                                        Dictionary<string, int> d = new Dictionary<string, int>();
                                        d.Add(u.NaamEnkelvoud, ehdId);
                                        idList.Add(d);
                                        break;
                                    }
                                }
                            }

                        }

                        for (int i = 0; i < defKenmerkWaardeArr.Count; i++)
                        {
                            int knmkId;
                            knmkId = db.AddArtikelMetaTag(defKenmerkNaamArr[i], defKenmerkWaardeArr[i], artikelId);
                        }
                    }
                    else
                    {
                        artikelId = int.Parse(updateArtikelId);

                        db.UpdateArtikel(int.Parse(updateArtikelId), NieuwArtikelnaam.Text, Barcodenaam.Text,
                            float.Parse(gewicht), float.Parse(naturaprijs), float.Parse(verhuurprijs),
                            Verbruiksgoed.Checked, merkId, categorieId, int.Parse(stopcontact));
                        // Get unitId => where artikelid = artikelId and ChildUnit = 0;
                        var uList = db.GetUnitsForArtikel(artikelId);
                        var unitId = uList.Where(a => a.ChildUnitId == 0)
                            .Select(c => c.Id).FirstOrDefault();
                        db.UpdateUnit(unitId, NieuwArtikelnaam.Text,1,0, Barcodenaam.Text,artikelId,0);

                        List<Code.Unit> unitsList = new List<Code.Unit>();

                        if (defEenheidAantalArr.Count > selectedArtikel.Units.Count)
                        {
                            for (int i = 0; i < defEenheidAantalArr.Count; i++)
                            {
                                if (naturaprijs != "-1")
                                {
                                    unitPrijs = float.Parse(naturaprijs) * int.Parse(defEenheidAantalArr[i]);
                                }
                                else
                                {
                                    unitPrijs = -1;
                                }



                                if (defEenheidMultiArr[i] != "Individueel artikel")
                                {
                                    for (var j = 0; j < defEenheidMultiArr.Count; j++)
                                    {
                                        if (defEenheidMultiArr[i] == defEenheidNaamArr[j])
                                        {
                                            defEenheidAantalArr[i] =
                                                (int.Parse(defEenheidAantalArr[i]) * int.Parse(defEenheidAantalArr[j]))
                                                .ToString();
                                            break;
                                        }
                                    }
                                }

                                Code.Unit u = new Code.Unit(defEenheidNaamArr[i], int.Parse(defEenheidAantalArr[i]), defEenheidBarcodeArr[i], artikelId, defEenheidMultiArr[i]);
                                unitsList.Add(u);

                               
                            }





                            unitsList = unitsList.OrderBy(x => x.Aantal).ToList();
                            var idList = new List<Dictionary<string, int>>();
                            int cnt = 0;
                            foreach (var u in unitsList)
                            {
                                int ehdId;

                                if (selectedArtikel.Units.ElementAtOrDefault(cnt) != null)
                                {

                                    if (u.UnitChildString == "Individueel artikel")
                                    {
                                        db.UpdateUnit(selectedArtikel.Units[cnt].Id, u.NaamEnkelvoud, u.Aantal, 0,
                                            u.Barcode, u.ArtikelId, unitId);
                                        Dictionary<string, int> d = new Dictionary<string, int>();
                                        d.Add(u.NaamEnkelvoud, selectedArtikel.Units[cnt].Id);
                                        idList.Add(d);
                                    }
                                    else
                                    {
                                        foreach (var id in idList)
                                        {
                                            if (id.ContainsKey(u.UnitChildString))
                                            {
                                                db.UpdateUnit(selectedArtikel.Units[cnt].Id, u.NaamEnkelvoud, u.Aantal,
                                                    0, u.Barcode, u.ArtikelId, id[u.UnitChildString]);
                                                Dictionary<string, int> d = new Dictionary<string, int>();
                                                d.Add(u.NaamEnkelvoud, selectedArtikel.Units[cnt].Id);
                                                idList.Add(d);
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (u.UnitChildString == "Individueel artikel")
                                    {
                                        ehdId = db.AddUnit(u.NaamEnkelvoud, u.Aantal, 0, u.Barcode, u.ArtikelId, unitId);
                                        Dictionary<string, int> d = new Dictionary<string, int>();
                                        d.Add(u.NaamEnkelvoud, ehdId);
                                        idList.Add(d);
                                    }
                                    else
                                    {
                                        foreach (var id in idList)
                                        {
                                            if (id.ContainsKey(u.UnitChildString))
                                            {
                                                ehdId = db.AddUnit(u.NaamEnkelvoud, u.Aantal, 0, u.Barcode, u.ArtikelId, id[u.UnitChildString]);
                                                Dictionary<string, int> d = new Dictionary<string, int>();
                                                d.Add(u.NaamEnkelvoud, ehdId);
                                                idList.Add(d);
                                                break;
                                            }
                                        }
                                    }
                                }
                                cnt++;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < selectedArtikel.Units.Count; i++)
                            {
                                if (naturaprijs != "-1")
                                {
                                    unitPrijs = float.Parse(naturaprijs) * int.Parse(defEenheidAantalArr[i]);
                                }
                                else
                                {
                                    unitPrijs = -1;
                                }
                                if (defEenheidAantalArr.ElementAtOrDefault(i) != null)
                                {

                                    if (defEenheidMultiArr[i] != "Individueel artikel")
                                     {
                                         for (var j = 0; j < defEenheidMultiArr.Count; j++)
                                         {
                                             if (defEenheidMultiArr[i] == defEenheidNaamArr[j])
                                             {
                                                 defEenheidAantalArr[i] =
                                                     (int.Parse(defEenheidAantalArr[i]) * int.Parse(defEenheidAantalArr[j]))
                                                     .ToString();
                                                 break;
                                             }
                                         }
                                     }

                                
                                    Code.Unit u = new Code.Unit(defEenheidNaamArr[i], int.Parse(defEenheidAantalArr[i]),
                                        defEenheidBarcodeArr[i], artikelId, defEenheidMultiArr[i]);
                                    unitsList.Add(u);
                                }
                                else
                                {
                                    db.DeleteUnit(selectedArtikel.Units[i].Id);

                                }

                            }


                            unitsList = unitsList.OrderBy(x => x.Aantal).ToList();
                            var idList = new List<Dictionary<string, int>>();
                            int cnt = 0;
                            foreach (var u in unitsList)
                            {
                                int ehdId;

                                

                                    if (u.UnitChildString == "Individueel artikel")
                                    {
                                        db.UpdateUnit(selectedArtikel.Units[cnt].Id, u.NaamEnkelvoud, u.Aantal, 0,
                                            u.Barcode, u.ArtikelId, unitId);
                                        Dictionary<string, int> d = new Dictionary<string, int>();
                                        d.Add(u.NaamEnkelvoud, selectedArtikel.Units[cnt].Id);
                                        idList.Add(d);
                                    }
                                    else
                                    {
                                        foreach (var id in idList)
                                        {
                                            if (id.ContainsKey(u.UnitChildString))
                                            {
                                                db.UpdateUnit(selectedArtikel.Units[cnt].Id, u.NaamEnkelvoud, u.Aantal,
                                                    0, u.Barcode, u.ArtikelId, id[u.UnitChildString]);
                                                Dictionary<string, int> d = new Dictionary<string, int>();
                                                d.Add(u.NaamEnkelvoud, selectedArtikel.Units[cnt].Id);
                                                idList.Add(d);
                                                break;
                                            }
                                        }
                                    }
                                
                                cnt++;
                            }


                        }

                        if (defKenmerkWaardeArr.Count > selectedArtikel.ArtikelMetaTags.Count)
                        {
                            for (int i = 0; i < defKenmerkWaardeArr.Count; i++)
                            {
                                if (selectedArtikel.ArtikelMetaTags.ElementAtOrDefault(i) != null)
                                {
                                    db.UpdateArtikelMetaTag(selectedArtikel.ArtikelMetaTags[i].Id, defKenmerkNaamArr[i],
                                        defKenmerkWaardeArr[i], artikelId);
                                }
                                else
                                {

                                    int knmkId;
                                    knmkId = db.AddArtikelMetaTag(defKenmerkNaamArr[i], defKenmerkWaardeArr[i],
                                        artikelId);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < selectedArtikel.ArtikelMetaTags.Count; i++)
                            {
                                if (defKenmerkWaardeArr.ElementAtOrDefault(i) != null)
                                {
                                    db.UpdateArtikelMetaTag(selectedArtikel.ArtikelMetaTags[i].Id, defKenmerkNaamArr[i],
                                        defKenmerkWaardeArr[i], artikelId);
                                }
                                else
                                {
                                    db.DeleteArtikelMetaTag(selectedArtikel.ArtikelMetaTags[i].Id);
                                }
                            }
                        }


                    }


                    var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var currentUser = manager.FindByName(HttpContext.Current.User.Identity.Name);
                    db.AddWijziging(artikelId, DateTime.Now, currentUser.Id, exist ? "WArtikel" : "NArtikel");


                    // Verantwoordelijke
                    var oldV = db.GetArtikelVerantwoordelijkeById(artikelId);
                    if (oldV.Count != 0)
                    {
                        foreach (var v in oldV)
                        {
                            // true -> Item bestaat nog steeds | False -> Item moet verwijderd worden
                            var VerwijderdOud = Verantwoordelijke.GetSelectedIndices()
                                .Any(x => Verantwoordelijke.Items[x].Value == v.UserId.ToString());

                            if (!VerwijderdOud)
                            {
                                // remove
                                db.DeleteArtikelVerantwoordelijke(v);
                            }
                        }
                    }
                    foreach (var av in Verantwoordelijke.GetSelectedIndices())
                    {
                        db.AddArtikelVerantwoordelijke(
                            new ArtikelVerantwoordelijke(artikelId,
                                int.Parse(Verantwoordelijke.Items[av].Value)));
                    }

                    countLocations = 0;
                    countUnits = 0;
                    countKenmerken = 0;
                    if (!exist)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertUser",
                            "BootstrapDialog.show({title: 'Geslaagd!', message: 'Het artikel is toegevoegd.', buttons: [{ label: 'Sluiten', action: function(){ window.location.href = '/stock/Overzicht.aspx?id=" +
                            artikelId + "&type=alles';}}]}); ", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertUser",
                            "BootstrapDialog.show({title: 'Geslaagd!', message: 'Het artikel is gewijzigd.', buttons: [{ label: 'Sluiten', action: function(){ window.location.href = '/stock/Overzicht.aspx?id=" +
                            artikelId + "&type=alles';}}]}); ", true);
                    }
                    

                }
            }
        }

        protected void saveArticle2(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (!String.IsNullOrEmpty(NieuwEenheidnaam.Text))
                {
                List<string> eenheidNaamArray = new List<string>();
                List<string> eenheidAantalArray = new List<string>();
                List<string> eenheidMultiplierArray = new List<string>();
                List<string> eenheidBarcodeArray = new List<string>();

                for(int i = 0; i <= countUnits; i++)
                {
                    eenheidNaamArray.Add(eenheidNaam[i].Text);
                    eenheidAantalArray.Add(eenheidAantallen[i].Text);
                    eenheidMultiplierArray.Add(eenheidMultipliers[i].SelectedItem.Text);
                    eenheidBarcodeArray.Add(eenheidBarcodes[i].Text);
                }

                for (int i = 0; i < eenheidNaamArray.Count; i++)
                {
                    if (eenheidNaamArray[i] != "" || eenheidNaamArray[i] != null || !string.IsNullOrEmpty(eenheidNaamArray[i]))
                    {
                        defEenheidNaamArr.Add(eenheidNaamArray[i]);
                        defEenheidAantalArr.Add(eenheidAantalArray[i]);
                        defEenheidMultiArr.Add(eenheidMultiplierArray[i]);
                        defEenheidBarcodeArr.Add(eenheidBarcodeArray[i]);
                    }

                }
                }

                if (!String.IsNullOrEmpty(KenmerkWaarde.Text))
                {

                    List<string> kenmerkArr = new List<string>();
                    List<string> kenmerkNaamArr = new List<string>();
                    List<string> kenmerkWaardeArr = new List<string>();

                    for (int i = 0; i <= countKenmerken; i++)
                    {
                        kenmerkArr.Add(dropDownListsKenmerk[i].SelectedItem.Value);
                        kenmerkNaamArr.Add(dropDownListsKenmerk[i].SelectedItem.Text);
                        kenmerkWaardeArr.Add(kenmerkWaardes[i].Text);
                    }

                    for (int i = 0; i < kenmerkWaardeArr.Count; i++)
                    {
                        if (kenmerkWaardeArr[i] != "" || kenmerkWaardeArr[i] != null || !string.IsNullOrEmpty(kenmerkWaardeArr[i]))
                        {
                            defKenmerkArr.Add(kenmerkArr[i]);
                            defKenmerkNaamArr.Add(kenmerkNaamArr[i]);
                            defKenmerkWaardeArr.Add(kenmerkWaardeArr[i]);
                        }

                    }

                }

                saveArticle();
            }
        }

        protected void AddOption(string option)
        {
            Debug.WriteLine(option);
        }

        protected void ddlNieuwCategorienaam_SelectedIndexChanged(object sender, EventArgs e)
        {
            value = ddlNieuwCategorienaam.SelectedItem.Value;
            Debug.WriteLine(value);
        }

        protected void hdnDDL1_ValueChanged(object sender, EventArgs e)
        {
            string nieuwCat = hdnDDL1.Value.ToString();

            if (!Regex.IsMatch(nieuwCat, @"^\d+$"))
            {
                List<Categorie> categorieList = new List<Categorie>();

                Categorie cat = new Categorie();
                cat.Id = int.MaxValue;
                cat.Naam = nieuwCat;
                cat.Omschrijving = nieuwCat;

                categorieList.Add(cat);

                foreach (var categorie in db.GetCategorien())
                {
                    categorieList.Add(categorie);
                }

                ddlNieuwCategorienaam.DataSource = categorieList;
                ddlNieuwCategorienaam.DataBind();
            }
        }

        protected void hdnDDL2_ValueChanged(object sender, EventArgs e)
        {
            string nieuwMerk = hdnDDL2.Value.ToString();

            if (!Regex.IsMatch(nieuwMerk, @"^\d+$"))
            {
                List<Merk> merkList = new List<Merk>();

                Merk merk = new Merk();
                merk.Id = int.MaxValue;
                merk.Naam = nieuwMerk;
                merkList.Add(merk);

                foreach (var mrk in db.GetMerken())
                {
                    merkList.Add(mrk);
                }

                ddlNieuwMerknaam.DataSource = merkList;
                ddlNieuwMerknaam.DataBind();
            }
        }
        protected void hdnDDL4_ValueChanged(object sender, EventArgs e)
        {
            string nieuwKenmerk = hdnDDL4.Value.ToString();

            if (!Regex.IsMatch(nieuwKenmerk, @"^\d+$"))
            {
                List<ArtikelMetaTag> tagList = new List<ArtikelMetaTag>();

                ArtikelMetaTag artktag = new ArtikelMetaTag();
                artktag.Id = int.MaxValue;
                artktag.Naam = nieuwKenmerk;
                tagList.Add(artktag);

                foreach (var tag in db.GetMetaTags())
                {
                    tagList.Add(tag);
                }

                ddlKenmerkNaam.DataSource = tagList;
                ddlKenmerkNaam.DataBind();
            }
        }

        protected void eenheidToevoegen(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(eenheidNaam[countUnits].Text))
            {
                if (countUnits != 9)
                {
                    countUnits++;
                    for(var i = 0; i< countUnits + 1; i++)
                    {
                        var datasc = new List<UnitMultiplierModel>();
                        datasc.Add(new UnitMultiplierModel(0,"Individueel artikel"));
                        for (var j = 0; j < countUnits; j++)
                        {
                            if (j == i) continue;
                            datasc.Add(new UnitMultiplierModel(j + 1, eenheidNaam[j].Text));
                        }
                        var index = eenheidMultipliers[i].SelectedIndex;
                        eenheidMultipliers[i].DataSource = datasc;
                        eenheidMultipliers[i].DataBind();
                        eenheidMultipliers[i].SelectedIndex = index;
                    }
                    EenheidVerwijderen.Style.Add("display", "block");
                    fieldsEenheidArray[countUnits].Style.Add("display", "block");

                    if (countUnits == 9)
                    {
                        EenheidToevoegen.Style.Add("display", "none");
                    }
                }
            }
        }

        protected void EenheidVerwijderen_Click(object sender, EventArgs e)
        {
            fieldsEenheidArray[countUnits].Style.Add("display", "none");
            countUnits--;

            for (var i = 0; i < countUnits + 1; i++)
            {
                var datasc = new List<UnitMultiplierModel>();
                datasc.Add(new UnitMultiplierModel(0, "Individueel artikel"));
                for (var j = 0; j < countUnits + 1 ; j++)
                {
                    if (j == i) continue;
                    datasc.Add(new UnitMultiplierModel(j + 1 , eenheidNaam[j].Text));
                }
                var index = eenheidMultipliers[i].SelectedIndex;
                eenheidMultipliers[i].DataSource = datasc;
                eenheidMultipliers[i].DataBind();
                eenheidMultipliers[i].SelectedIndex = index;
            }

            if (countUnits == 0)
            {
                EenheidVerwijderen.Style.Add("display", "none");
            }
            else if (countUnits == 8)
            {
                EenheidToevoegen.Style.Add("display", "block");
            }
            else
            {
                EenheidVerwijderen.Style.Add("display", "block");

            }

        }

        protected void KenmerkToevoegen_Click(object sender, EventArgs e)
        {

            if (countKenmerken != 9)
            {

                countKenmerken++;
                KenmerkVerwijderen.Style.Add("display", "block");
                fieldsKenmerkArray[countKenmerken].Style.Add("display", "block");
                dropDownListsKenmerk[countKenmerken].DataSource = db.GetMetaTags();
                dropDownListsKenmerk[countKenmerken].DataBind();

                if (countKenmerken == 9)
                {
                    kenmerkToevoegen.Style.Add("display", "none");
                }

            }

        }

        protected void KenmerkVerwijderen_Click(object sender, EventArgs e)
        {
            fieldsKenmerkArray[countKenmerken].Style.Add("display", "none");
            countKenmerken--;

            if (countKenmerken == 0)
            {
                KenmerkVerwijderen.Style.Add("display", "none");
            }
            else if (countKenmerken == 8)
            {
                kenmerkToevoegen.Style.Add("display", "block");
            }
            else
            {
                KenmerkVerwijderen.Style.Add("display", "block");

            }

        }

      
        protected void kenmerkValueChanged(object sender, EventArgs e)
        {
            var id = (sender as Control).ClientID;
            string idNumber = Regex.Match(id, @"\d+").Value;

            string nieuwKenmerk;

            nieuwKenmerk = hdnKenmerkArray[Convert.ToInt32(idNumber) - 1].Value;

            if (!Regex.IsMatch(nieuwKenmerk, @"^\d+$"))
            {
                List<ArtikelMetaTag> kenmerkList = new List<ArtikelMetaTag>();

                ArtikelMetaTag kenmerk = new ArtikelMetaTag();
                kenmerk.Id = int.MaxValue;
                kenmerk.Naam = nieuwKenmerk;
                kenmerkList.Add(kenmerk);

                foreach (var kenmerkje in db.GetMetaTags())
                {
                    kenmerkList.Add(kenmerkje);
                }
                dropDownListsKenmerk[Convert.ToInt32(idNumber) - 1].DataSource = kenmerkList;
                dropDownListsKenmerk[Convert.ToInt32(idNumber) - 1].DataBind();
            }
        }


        protected void hdnMedewerkers_OnValueChanged(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;
            var lst = Verantwoordelijke;
            var gebruikers = ((HiddenField)sender).Value;
            if (string.IsNullOrEmpty(gebruikers)) return;
            var gebruikersList = gebruikers.Split(',');
            foreach (var b in gebruikersList)
            {
                lst.Items.FindByValue(b).Selected = true;
            }
        }
    }
}