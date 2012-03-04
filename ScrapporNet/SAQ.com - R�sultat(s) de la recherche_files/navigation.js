// Menu en popup
<!--
window.onload=affiche;
function affiche(id) {

var d = document.getElementById(id);
	for (var i = 1; i<=10; i++) {
		if (document.getElementById('smenu'+i)) {document.getElementById('smenu'+i).style.display='none';}
	}
	if (d) {d.style.display='block';}

	//Place le focus sur le InputBox Couriel.
	var f = document.getElementById('WC_RememberMeLogonForm_FormInput_logonId_In_Logon_1');
	if (f!=null) f.focus();	
}
function cacher(id) {
var d = document.getElementById(id);
	for (var i = 1; i<=10; i++) {
		if (document.getElementById('smenu'+i)) {document.getElementById('smenu'+i).style.display='none';}
	}
	if (d) {d.style.display='none';}
}

/* Courtesy of Maxime Lafontaine */
function changeTitre(idTitreMenu, texte){

            eltitre = document.getElementById(idTitreMenu);
            eltitre = eltitre.getElementsByTagName("a");
            eltitre[0].innerHTML = texte ;
            return false;

}
function changeValeurChamp(valeur, champ, form){

			champForm = form.elements[champ];				
			champForm.value = valeur;			
            return false;
}

function changerSousMenu(form, idMenu, idTitreMenu, texte, valeur, champ){
    changeTitre(idTitreMenu, texte);
    changeValeurChamp(valeur, champ, form);
    cacher(idMenu);
    return false;
}

/* (CMS) Fonction pour changer le URL 
selon une valeur sélectionné dans 
une liste déroulante */
function changeLocation(menuObj) {

    var i = menuObj.selectedIndex;
    if(i >= 0) {
      window.location = encodeURI(menuObj.options[i].value);
    }

  }

function gup( name ){
	var regexS = "[\\?&]"+name+"=([^&#]*)";
	var regex = new RegExp( regexS );
	var tmpURL = window.location.href;
	var results = regex.exec( tmpURL );
	if( results == null ) {
		return "";
	} else {
		return encodeURI(results[1]);
	}	
}

/* validation de la recherche */
function validateGreaterAndLower(form,toSubmit) {
	if(form.searchTerm.value.indexOf('>') != -1 || 
	form.searchTerm.value.indexOf('<') != -1 ) {
		var reg1=new RegExp(">", "gi");
		var reg2=new RegExp("<", "gi");
		form.searchTerm.value = form.searchTerm.value.replace(reg1,"");
        form.searchTerm.value =	form.searchTerm.value.replace(reg2,"");
	}
	if(toSubmit){
		form.submit();
    }
}

//-->