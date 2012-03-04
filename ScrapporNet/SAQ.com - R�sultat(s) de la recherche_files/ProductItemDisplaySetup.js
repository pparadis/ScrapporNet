//
//-------------------------------------------------------------------
// Licensed Materials - Property of IBM
//
// WebSphere Commerce
//
// (c) Copyright IBM Corp. 2006
//
// US Government Users Restricted Rights - Use, duplication or
// disclosure restricted by GSA ADP Schedule Contract with IBM Corp.
//-------------------------------------------------------------------
//

//
// ***
// * This javascript function is used by the 'Add to Shopcart' button.  Since the HTML form is shared by both 'Add to Shopcart' and 'Add to Wish List' button,
// * appropriate values are set using this javascript before the form is submitted.
// * The variable 'busy' is used to avoid submitting the same forms multiple times when users click the button more than once.
// ***
//
var busy = false;
function Add2ShopCartDepuisRecherche(form, catEntryId, catEntryQuantity, storeId, catalogId, pageSize, beginIndex)
{ 

       if (!busy) {
              busy = true;
              form.action="OrderItemAdd";
              form.catEntryId.value = catEntryId;
              form.quantity.value = catEntryQuantity;

			  //URL si on veut retourner à la page du panier mise à jour après l'ajout d'un produit
			  //à partir de la recherche
              //Florin Marinoiu: ajout du parametre "calculationUsageId=-1" pour forcer le calcul des ajustements
              //form.URL.value = 'OrderCalculate?URL=OrderItemDisplay&storeId='+storeId+'&catalogId='+catalogId+'&updatePrices=1';
              form.URL.value = 'OrderCalculate?URL=OrderItemDisplay&storeId='+storeId+'&catalogId='+catalogId+'&updatePrices=1&calculationUsageId=-1';
              //(End): Florin Marinoiu: ajout du parametre "calculationUsageId=-1" pour forcer le calcul des ajustements
              
              //URL si on veut rester à la recherche après l'ajout du produit dans le panier
              //non fonctionnel pour l'instant!
              //form.URL.value = 'https://localhost/webapp/wcs/stores/servlet/CatalogSearchResultView?storeId='+storeId+'&catalogId='+catalogId+'&searchTerm=e&resultCatEntryType=2&pageSize='+pageSize+'&sType=SimpleSearch&searchTermScope=&beginIndex='+beginIndex;

              //alert(form.URL.value);
              form.submit();
       }
}
function Add2ShopCart(form, catEntryId, catEntryQuantity)
{

       if (!busy) {
              busy = true;
              form.action="OrderItemAdd";
              form.catEntryId.value = catEntryId;
              form.quantity.value = catEntryQuantity;
              form.URL.value='OrderCalculate?item_quantity*=&URL=OrderItemDisplay';
              form.submit();
       }
}
// This javascript function is used by the 'Add to Wish List' button to set appropriate values before the form is submitted
function Add2WishList(form, catEntryId)
{
       if (!busy) {
              busy = true;
              form.action="InterestItemAdd";
              form.catEntryId.value = catEntryId;
              form.URL.value='InterestItemDisplay';
              form.submit();
       }
}
