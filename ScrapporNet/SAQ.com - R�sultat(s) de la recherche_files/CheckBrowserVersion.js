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

// This checks for a valid browser for the starter store.
// It displays a warning message if the browser is unconformant.

var detect = navigator.userAgent.toLowerCase();
var OS,browser,version,total,thestring;

if (checkIt('konqueror'))
{
	browser = "Konqueror";
	OS = "Linux";
}
else if (checkIt('safari')) browser = "Safari";
else if (checkIt('omniweb')) browser = "OmniWeb";
else if (checkIt('opera')) browser = "Opera";
else if (checkIt('webtv')) browser = "WebTV";
else if (checkIt('icab')) browser = "iCab";
else if (checkIt('firefox')) browser = "Firefox";
else if (checkIt('msie')) browser = "Internet Explorer";
else if (!checkIt('compatible'))
{
	//This block applyies only to NetScape browsers
	browser = "Netscape Navigator";
	var version_no = "";
	var vendor_name = "";
	//Mozill browser doesn't support navigator.vendor as it doesn't contain Vendor name in UserAgent string
	//For mozill we will use CVS Branch tag to determine its version, 
	//(SAME CAN BE USED FOR ALL NEW GECKO BROWSERS LIKE NETSCAPE, FIREFOX, MOZILLA, SEAMONKEY !!!)
	if(!navigator.vendor.toLowerCase())
	{
		//Its Mozilla
		vendor_name = "mozilla";
		version = getGeckoVersion();	//For mozilla CVS Branch tag and its Version is same.
			}
	else
	{
		//Its Netscape or firefox
		vendor_name = navigator.vendor.toLowerCase();
		var version_index = checkIt(vendor_name) + vendor_name.length ;
		version_no = detect.substr(version_index);
		
		//there are cases where more information is added after the version number
		var version_no_parts = version_no.split(' ');
		version = getVersionNo(version_no_parts[0]);
	}
	
	//version = detect.charAt(8);		//Old code.
	
}
else browser = "An unknown browser";

if (!version) version = detect.charAt(place + thestring.length);
	
if (!OS)
{
	if (checkIt('linux')) OS = "Linux";
	else if (checkIt('x11')) OS = "Unix";
	else if (checkIt('mac')) OS = "Mac";
	else if (checkIt('win')) OS = "Windows";
	else OS = "an unknown operating system";
}

//This function returns the position of partidular string exist in the UserAgent String.
function checkIt(string)
{
	place = detect.indexOf(string) + 1;
	thestring = string;
	return place;
}


//This function converts version no in to xxx.xxx.xxx format.
//So that its string comparision becomes easy
//e.g. : 1.23.4		-> 001.023.004
//	 1.3.50		-> 001.003.050
function getVersionNo(string)
{
	var version_parts = string.split('.');
	var tmp_version_no = "";
	for (var i = 0; i < version_parts.length; i++)
	{
		var part = version_parts[i];
		var part_value = parseInt(part);
		//Here I assume that the version will limit under 999.
		//We can easily add support for four digits (xxxx.xxxx.xxxx format).
		var temp_value = 0;
		if(part_value == 0)
			temp_value = "000";			//for four digits support replace 000 with 0000
		else if(part.length < 2)
			temp_value = "00" + part_value;		//for four digits support replace 00 with 000
		else if(part.length < 3)
			temp_value = "0" + part_value;		//for four digits support replace 0 with 00
		//else if(part.length < 4)			//remove comments from below code for four digits support.
		//	part_value = "0" + part_value;		//xxxx.xxxx.xxxx format.
			
		tmp_version_no = tmp_version_no + temp_value + ".";
	}
	return tmp_version_no.substr(0, tmp_version_no.length - 1);//remove the end '.'
}



//This function returns the gecko version of mozilla browsers.
//Gecko browsers which are built from the same branch share the same basic version of Netscape Gecko and 
//can be treated similarly when dealing with HTML, CSS, JavaScript, etc.
//Netscape Firefox Mozilla and other mozilla base browser now supports gecko.

//So, This method can also be used for those browsers.
//But Gecko version is different than origional version except Mozilla.
function getGeckoVersion()
{
	if (navigator.product != 'Gecko')
	{
		return "000.000.000";
	}
	var start_index = detect.indexOf('rv:');
	var end_index   = detect.indexOf(')', start_index);
	var gecko_version_string = detect.substring(start_index + 3, end_index);
	return getVersionNo(gecko_version_string);
}


//This method check for browser support.
//it was checking only for Netscape browers versions.
//Now it check for other browser version using the return value to indicates if it's supported.
// it returns an empty string if it supported and the string value of the browser if it's not.
function isBrowserSupported()
{
	var strBrowserNotSupported= browser;
			
	if(browser =="Netscape Navigator")
	{
		if(vendor_name == "netscape" && version < "007.000.000")	//Netscape Version 7.0.0
			return strBrowserNotSupported;
		else if(vendor_name == "firefox" && version < "001.000.004")	//Firefox Version 1.0.4
			return strBrowserNotSupported;
		else if(vendor_name == "mozilla" && version < "001.007.007")	//Mozilla Version 1.7.7
			return strBrowserNotSupported;
		//else if(vendor_name == "Some_mozilla" && version < "xxx.yyy.zzz")	//Remove this comment to add support for
		//	return false;							//some other mozilla broser.
	}
	else if(browser == "Internet Explorer")
	{
   		var place = detect.indexOf("msie") + 1;
 	    var version_no = detect.substr(place + 4 ,5);
	    var version_no_parts = version_no.split(';');
		version = getVersionNo(version_no_parts[0]);
		if(parseFloat(version) < 6.0)	//Internet Explorer 6.0.0
			return strBrowserNotSupported;
	}
	else if(browser == "Firefox")
	{
		var place = detect.indexOf(browser.toLowerCase()) + 1;
 	    var version_no = detect.substr(place + browser.length ,8);
	    var version_no_parts = version_no.split(';');
		version = getVersionNo(version_no_parts[0]);
		if(version < "001.005.000.010")	//Firefox 1.5.0.10
			return strBrowserNotSupported;	
	}
	else if(browser == "Safari")
	{	
		/* Unfortunately Safari's string never contains its official version; 
		only its internal Apple version number (ie. not 1.3.2 but 312.6). 
		Therefore the version number detect doesn't work in Safari. */
		var place = detect.indexOf(browser.toLowerCase()) + 1;
   		var version_no = detect.substr(place + browser.length ,8);
		if(parseFloat(version_no) < 312.0)	//Safari 1.3.0
		   return strBrowserNotSupported;	
	}
		
	else {
		return strBrowserNotSupported;	
	}
	//else if(browser == "Some Browser")
	//{
	//	..	
	//	Code for Checking support for browser 
	//	..	
	//}
	return "";
}
