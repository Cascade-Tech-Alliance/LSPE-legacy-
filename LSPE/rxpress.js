function validateNumeric(oSrc, args){
	var valid;
	var substrings = args.Value.split('.');
	if ((substrings.length - 1) > 1) {
		valid = false;
	}
	else {
		valid = (verifyCharsInBag(args.Value, '-,.0123456789'));
	}
	args.IsValid = valid;
}

function verifyCharsInBag (s, bag)
{
	for (var i = 0; i < s.length; i++)
	{
		var c = s.charAt(i);
		if (bag.indexOf(c) == -1) {return false;};
	}
	return true;
}

function confirm_delete()
{
	if (confirm("Are you sure you want to delete this record?")==true)
		return true;
	else
		return false;
}

var Nav4 = ((navigator.appName == 'Netscape') && (parseInt(navigator.appVersion) >= 4))
var dialogWin = new Object();
function openDialog(url, width, height, args, name) {
   if (!dialogWin.win || (dialogWin.win && dialogWin.win.closed)) 
   {
		dialogWin.args = args
		dialogWin.url = url
		dialogWin.width = width
		dialogWin.height = height
		dialogWin.name = name      
		if (Nav4) 
		{
				dialogWin.left = window.screenX + ((window.outerWidth - dialogWin.width) / 2)
				dialogWin.top = window.screenY + ((window.outerHeight - dialogWin.height) / 2)
				var attr = "screenX=" + dialogWin.left + ",screenY=" + dialogWin.top + ",scrollbars=yes,resizable=yes,width=" + dialogWin.width + ",height=" + dialogWin.height
		} 
		else 
		{
				dialogWin.left = (screen.width - dialogWin.width) / 2
				dialogWin.top = (screen.height - dialogWin.height) / 2
				var attr = "left=" + dialogWin.left + ",top=" + dialogWin.top + ",scrollbars=yes,resizable=yes,width=" + dialogWin.width + ",height=" + dialogWin.height
		}
		dialogWin.win=window.open(dialogWin.url, dialogWin.name, attr)
		dialogWin.win.focus()
   } 
   else
   {
      dialogWin.win.focus()
   }
}
