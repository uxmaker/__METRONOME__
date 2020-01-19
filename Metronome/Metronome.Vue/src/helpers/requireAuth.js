import AuthService from '../services/AuthService'

/**
 * Filter for routes requiring an authenticated user
 * @param {*} to 
 * @param {*} from 
 * @param {*} next 
 */
export default function requireAuth(to, from, next) {
    if (!AuthService.isConnected) {
        console.log('REQUIRE AUTH - NOT LOGGED !')
        next({
            path: '/',
            query: { redirect: to.fullPath }
        });
        return;
    }
    
    console.log('REQUIRE AUTH - LOGGED !')

    var requiredProviders = to.meta.requiredProviders;

    if (requiredProviders && !AuthService.isBoundToProvider(requiredProviders)) {
        next({
            path: '/'
        });

        return;
    };

    next();
}