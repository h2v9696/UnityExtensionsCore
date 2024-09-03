var JsPlugins = {
	CopyToClipboard: function (text) {
		_text = UTF8ToString(text);
    	var copyText = document.createElement("textarea");
		copyText.id = "myInput";
		copyText.type = "text";
		copyText.value = _text;
		document.body.appendChild(copyText);
    	copyText.select();
    	copyText.setSelectionRange(0, 99999);
    	navigator.clipboard.writeText(copyText.value)
      	document.body.removeChild(copyText); 
    }
	
    TweetFromUnity: function (rawMessage) {
        var message = UTF8ToString(rawMessage);
        var mobilePattern = /android|iphone|ipad|ipod/i;

        var ua = window.navigator.userAgent.toLowerCase();

        if (ua.search(mobilePattern) !== -1 || (ua.indexOf("macintosh") !== -1 && "ontouchend" in document)) {
            // Mobile
            location.href = "twitter://post?message=" + message;
        } else {
            // PC
            window.open("https://twitter.com/intent/tweet?text=" + message, "_blank");
        }
    },
    
    SuperRefreshPages: function () {
        console.log("Force clear page and clean old asset cache: " + localStorage.length);
        indexedDB.databases().then(dbs => {
            dbs.forEach(db => {
                if (db.name.startsWith('UnityCache')) {
                    indexedDB.deleteDatabase(db.name);
                }
            });
        });
        localStorage.clear();
        console.log("IndexedDB size after clear: " + localStorage.length);
        
        window.location.replace(window.location.href);
    },
}
mergeInto(LibraryManager.library, JsPlugins);