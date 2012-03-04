// SAQ.js

// Permet de masquer/afficher un avertissement 
function toggle(obj, contenuId)
{
    var contenu = document.getElementById(contenuId);
    if (obj.value == "0")
    {
        contenu.className = "visible";
    }
    else
    {
        contenu.className = "invisible";
    }
}

// Popup
function popup(url, target, width, height, left, top, features) {
	var theWindow = window.open(url, target, 'width='+width+',height='+height+',left='+left+',top='+top+','+features);
	theWindow.focus();
	return theWindow;
}


// Affiche l'infobulle pour les pastilles de gout.
function afficheInfobulle(id,statut) {
   if(statut == 1){
	   document.getElementById('infobulle_pastille_recherche'+id).style.display='inline';
   } else if(statut==0){
	   document.getElementById('infobulle_pastille_recherche'+id).style.display='none';
   }
}