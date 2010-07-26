function ProceedToCheckout() {
	document.forms[0].method = checkout_form_method;
	document.forms[0].action = checkout_routine_url;
	
	CleanupAspNetSvcFields();
}

function CleanupAspNetSvcFields() {
	var _elems = document.forms[0].elements;
	var length = _elems.length;
	
	for (var i = 0; i < length; i++) {
		var name = _elems[i].name;
		var type = _elems[i].type;
		var tagname = _elems[i].tagName.toLowerCase();
		
		if (StartsWith(name, "__") || name.indexOf("$") > -1) {
			// cleanup text, hidden fields or buttons
			if (tagname == "input")
				_elems[i].removeAttribute("name");
			// cleanup drop-down lists
			else if (tagname == "select")
				_elems[i].removeAttribute("name");
		}
	}
}

function StartsWith (strValue, starts) {
	if (strValue == null || starts == null)
		return false;
		
	if (strValue.length < starts.length)
		return false;
		
	var substr = strValue.substr(0, starts.length);
	
	// return comparison result
	return (starts == substr);
}

function RadioCheckedEvaluateIsValid(val, args) {
	var array = eval(val.radiogroup);
	//
	for (var i = 0; i < array.length; i++) {
		var rbutton = document.getElementById(array[i]);
		//
		if (rbutton != null && rbutton.checked) {
			//
			args.IsValid = true;
			//
			return true;
		}
	}
	//
	args.IsValid = false;
	//
	return false;
}

function ShowInfoPopup(title, text, popup_args) {
	if (popup_args == null || popup_args.length == 0)
		popup_args = "channelmode=no,directories=no,fullscreen=no,height=450px,left=50px,location=no,menubar=no,resizable=0,scrollbars=yes,status=no,titlebar=no,menubar=no,top=50px,width=450px";
	//
	var _win = window.open(null, 'info_popup', popup_args);
	_win.document.write("<html><head><title>" + title + "</title></head><body><p align='justify'>");
	_win.document.write(text);
	_win.document.write("</p></body></html>");
	_win.document.close();
}