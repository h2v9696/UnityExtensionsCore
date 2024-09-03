mergeInto(LibraryManager.library, {
    SignInAnonymously: function (objectName, callback, fallback) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);
        try {
            firebase.auth().signInAnonymously().then(function (result) {
                console.log("Success: signed in anonymously");
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: signed up for " + JSON.stringify(result));
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    CreateUserWithEmailAndPassword: function (email, password, objectName, callback, fallback) {
        var parsedEmail = UTF8ToString(email);
        var parsedPassword = UTF8ToString(password);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);
        try {
            firebase.auth().createUserWithEmailAndPassword(parsedEmail, parsedPassword).then(function (result) {
                console.log("Success: signed up for " + JSON.stringify(result));
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(result.user));
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    SignInWithEmailAndPassword: function (email, password, objectName, callback, fallback) {
        var parsedEmail = UTF8ToString(email);
        var parsedPassword = UTF8ToString(password);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);
        try {
            firebase.auth().signInWithEmailAndPassword(parsedEmail, parsedPassword).then(function (result) {
                console.log("Success: signed in for " + JSON.stringify(result));
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(result.user));
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    SignInWithGoogle: function (objectName, callback, fallback) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);
        try {
            var provider = new firebase.auth.GoogleAuthProvider();
            firebase.auth().signInWithPopup(provider).then(function (result) {
                console.log("Success: signed in with Google for " + JSON.stringify(result));
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(result.user));
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    SignInWithTwitter: function (objectName, callback, fallback) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);
        try {
            var provider = new firebase.auth.TwitterAuthProvider();
            firebase.auth().signInWithPopup(provider).then(function (result) {
                console.log("Success: signed in with Twitter for " + JSON.stringify(result));
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(result));
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    SignInWithFacebook: function (objectName, callback, fallback) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);
        try {
            var provider = new firebase.auth.FacebookAuthProvider();
            firebase.auth().signInWithPopup(provider).then(function (result) {
                console.log("Success: signed in with Facebook for " + JSON.stringify(result));
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(result.user));
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    OnAuthStateChanged: function (objectName, onUserSignedIn, onUserSignedOut) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedOnUserSignedIn = UTF8ToString(onUserSignedIn);
        var parsedOnUserSignedOut = UTF8ToString(onUserSignedOut);
        firebase.auth().onAuthStateChanged(function (user) {
            console.log("OnAuthStateChanged: " + JSON.stringify(user));
            if (user) {
                window.unityInstance.SendMessage(parsedObjectName, parsedOnUserSignedIn, JSON.stringify(user));
            } else {
                window.unityInstance.SendMessage(parsedObjectName, parsedOnUserSignedOut, "User signed out");
            }
        });
    }
});