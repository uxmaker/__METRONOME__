import AuthService from './services/AuthService'

const host = process.env.VUE_APP_BACKEND;

/**
 * Configuration of the authentication service
 */

// Allowed urls to access the application (if your website is http://mywebsite.com, you have to add it)
AuthService.allowedOrigins = [host];

// Server-side endpoint to logout
AuthService.logoutEndpoint = host + '/Auth/Logout';

// Allowed providers to log in our application, and the corresponding server-side endpoints
AuthService.providers = {
  'Base': {
    endpoint: host + '/Auth/Login'
  }
};

AuthService.appRedirect = () => router.replace('/');
