const host = process.env.VUE_APP_BACKEND;

class AuthService
{

    constructor()
    {
        this.allowedOrigins = [];
        this.providers = {};
        this.logoutEndpoint = null;
        this.appRedirect = () => null;
        this.authenticatedCallbacks = [];
        this.signedOutCallbacks = [];
        this.identity = null;
        window.addEventListener("message", (e) => this.onMessage(e), false);
    }

    get isConnected()
    {
        return this.identity != null;
    }

    get accessToken()
    {
        var identity = this.identity;
        return identity ? identity.bearer.accessToken : null;
    }

    get email()
    {
        var identity = this.identity;
        return identity ? identity.email : null;
    }

    get boundProviders()
    {
        var identity = this.identity;
        return identity ? identity.boundProviders : [];
    }

    isBoundToProvider(expectedProviders)
    {
        var isBound = false;
        for (var p of expectedProviders) {
            if (this.boundProviders.indexOf(p) > -1) isBound = true;
        }
        return isBound;
    }

    onMessage(e)
    {
        if (!e.origin || this.allowedOrigins.indexOf(e.origin) < 0) return;
        var data = e.data;
        if (data.type == 'authenticated') this.onAuthenticated(data.payload);
        else if (data.type == 'signedOut') this.onSignedOut();
    }

    login(selectedProvider)
    {
        var provider = this.providers[selectedProvider];
        var popup = window.open("http://localhost:5000/Auth/Login", "Connexion à Metronome", "menubar=no, status=no, scrollbars=no, menubar=no, width=540, height=540");
    }

    registerAuthenticatedCallback(cb)
    {
        this.authenticatedCallbacks.push(cb);
    }

    removeAuthenticatedCallback(cb)
    {
        this.authenticatedCallbacks.splice(this.authenticatedCallbacks.indexOf(cb), 1);
    }

    onAuthenticated(i)
    {
        console.log("AuthService --> Authenticated");

        this.identity = i;
        for (var cb of this.authenticatedCallbacks) {
            cb();
        }
    }

    logout()
    {
        console.log("AuthService --> LogOut");

        var popup = window.open( this.logoutEndpoint, "Déconnexion de Metronome", "menubar=no, status=no, scrollbars=no, menubar=no, width=540, height=540" );
    }

    registerSignedOutCallback(cb)
    {
        this.signedOutCallbacks.push(cb);
    }

    removeSignedOutCallback(cb)
    {
        this.signedOutCallbacks.splice(this.signedOutCallbacks.indexOf(cb), 1);
    }

    onSignedOut()
    {
        this.identity = null;
        for (var cb of this.signedOutCallbacks)
        {
            cb();
        }
    }

    async getToken() {

        console.log("AuthService --> GetToken()");

        let result = await fetch( 'http://localhost:5000/Auth/Token', {
            credentials: 'include',
            method: 'GET',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (result.ok) {
            let token = await result.json();
            if (token.success)
            {
                console.log("AuthService --> GetToken --> Success");
                return token;
            } 
        }
        console.log("AuthService --> GetToken --> Fail");
        return null;

    }

    async refreshToken() {
        console.log("AuthService --> Refresh Token");
        this.identity = await this.getToken();
    }

    async init() {
        console.log("AuthService --> Init");
        let token = await this.getToken();
        if (token !== null) this.onAuthenticated(token);
    }
}

export default new AuthService();