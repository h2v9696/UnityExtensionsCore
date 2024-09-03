using System;
using System.Runtime.InteropServices;

namespace H2V.ExtensionsCore.WebGL.Firebase
{
    /// <summary>
    /// Usage: Use firebase to authenticate with 3rd party like Twitter, Google, Facebook,...
    ///     FirebaseAuth.SignInWithTwitter(gameObject.name, nameof(OnUserSignedIn), nameof(OnError)); 
    ///     ...
    ///     protected void OnUserSignedIn(string json)
    ///     {
    /// 	    var firebaseUser = JsonConvert.DeserializeObject<FirebaseTwitterUser>(json);
    ///     }
    /// 
    /// Also add this to your index.html the firebase project infomation and setup social media developer app in firebase
    /// 
    ///     script.onload = () => {
    ///         ...
    ///         const firebaseConfig = {
    ///             apiKey: "YOUR_KEY",
    ///             authDomain: "YOUR_APP.firebaseapp.com",
    ///             projectId: "YOUR_APP",
    ///             storageBucket: "YOUR_APP.appspot.com",
    ///             messagingSenderId: "YOUR_ID",
    ///             appId: "YOUR_APP_ID",
    ///             measurementId: "YOUR_ID"
    ///         };
    /// 
    ///         const firebaseApp = firebase.initializeApp(firebaseConfig);
    ///         const firebaseAuth = firebaseApp.auth();
    ///         firebaseAuth.setPersistence(firebase.auth.Auth.Persistence.SESSION);
    ///     }
    /// </summary>
    public static class FirebaseAuth
    {
        /// <summary>
        /// Creates and signs in a user anonymous
        /// </summary>
        /// <param name="objectName"> Name of the gameobject to call the callback/fallback of </param>
        /// <param name="callback"> Name of the method to call when the operation was successful. Method must have signature: void Method(string output) </param>
        /// <param name="fallback"> Name of the method to call when the operation was unsuccessful. Method must have signature: void Method(string output). Will return a serialized FirebaseError object </param>
        [DllImport("__Internal")]
        public static extern void SignInAnonymously(string objectName, string callback, string fallback);

        /// <summary>
        /// Creates a user with email and password
        /// </summary>
        /// <param name="email"> User email </param>
        /// <param name="password"> User password </param>
        /// <param name="objectName"> Name of the gameobject to call the callback/fallback of </param>
        /// <param name="callback"> Name of the method to call when the operation was successful. Method must have signature: void Method(string output) </param>
        /// <param name="fallback"> Name of the method to call when the operation was unsuccessful. Method must have signature: void Method(string output). Will return a serialized FirebaseError object </param>
        [DllImport("__Internal")]
        public static extern void CreateUserWithEmailAndPassword(string email, string password, string objectName,
            string callback, string fallback);

        /// <summary>
        /// Signs in a user with email and password
        /// </summary>
        /// <param name="email"> User email </param>
        /// <param name="password"> User password </param>
        /// <param name="objectName"> Name of the gameobject to call the callback/fallback of </param>
        /// <param name="callback"> Name of the method to call when the operation was successful. Method must have signature: void Method(string output) </param>
        /// <param name="fallback"> Name of the method to call when the operation was unsuccessful. Method must have signature: void Method(string output). Will return a serialized FirebaseError object </param>
        [DllImport("__Internal")]
        public static extern void SignInWithEmailAndPassword(string email, string password, string objectName,
            string callback, string fallback);

        /// <summary>
        /// Signs in a user with Google
        /// </summary>
        /// <param name="objectName"> Name of the gameobject to call the callback/fallback of </param>
        /// <param name="callback"> Name of the method to call when the operation was successful. Method must have signature: void Method(string output) </param>
        /// <param name="fallback"> Name of the method to call when the operation was unsuccessful. Method must have signature: void Method(string output). Will return a serialized FirebaseError object </param>
        [DllImport("__Internal")]
        public static extern void SignInWithGoogle(string objectName, string callback, string fallback);

        /// <summary>
        /// Signs in a user with Facebook
        /// </summary>
        /// <param name="objectName"> Name of the gameobject to call the callback/fallback of </param>
        /// <param name="callback"> Name of the method to call when the operation was successful. Method must have signature: void Method(string output) </param>
        /// <param name="fallback"> Name of the method to call when the operation was unsuccessful. Method must have signature: void Method(string output). Will return a serialized FirebaseError object </param>
        [DllImport("__Internal")]
        public static extern void SignInWithFacebook(string objectName, string callback, string fallback);

        /// <summary>
        /// Signs in a user with Twitter
        /// </summary>
        /// <param name="objectName"> Name of the gameobject to call the callback/fallback of </param>
        /// <param name="callback"> Name of the method to call when the operation was successful. Method must have signature: void Method(string output) </param>
        /// <param name="fallback"> Name of the method to call when the operation was unsuccessful. Method must have signature: void Method(string output). Will return a serialized FirebaseError object </param>
        [DllImport("__Internal")]
        public static extern void SignInWithTwitter(string objectName, string callback, string fallback);

        /// <summary>
        /// Listens for changes of the auth state (sign in/sign out)
        /// </summary>
        /// <param name="objectName"> Name of the gameobject to call the onUserSignedIn/onUserSignedOut of </param>
        /// <param name="onUserSignedIn"> Name of the method to call when the user signs in. Method must have signature: void Method(string output). Will return a serialized FirebaseUser object </param>
        /// <param name="onUserSignedOut"> Name of the method to call when the operation was unsuccessful. Method must have signature: void Method(string output) </param>
        [DllImport("__Internal")]
        public static extern void OnAuthStateChanged(string objectName, string onUserSignedIn,
            string onUserSignedOut);
    }

    [Serializable]
    public struct FirebaseError
    {
        public string code;
        public string message;
        public string details;
    }
    
    [Serializable]
    public struct FirebaseUser
    {
        public string displayName;

        public string email;

        public bool isAnonymous;

        public bool isEmailVerified;

        public FirebaseUserMetadata metadata;

        public string phoneNumber;

        public FirebaseUserProvider[] providerData;

        public string providerId;

        public string uid;
    }

    [Serializable]
    public class FirebaseUserMetadata
    {
        public ulong lastSignInTimestamp;

        public ulong creationTimestamp;
    }

    [Serializable]
    public class FirebaseUserProvider
    {
        public string displayName;

        public string email;

        public string photoUrl;

        public string providerId;

        public string userId;
    }
}