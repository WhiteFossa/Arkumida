import { defineStore } from "pinia";
import {LoginResult} from "@/js/constants";
import router from "@/router";

// If token remaining lifetime is less than this value we need to refresh token
const remainingTokenLifetimeToRefresh = 3600

const apiBaseUrl = process.env.VUE_APP_API_URL

// Persistent storage (for Remember Me mode enabled)
const useAuthPersistentStore = defineStore("authPersistent", {
    state: () =>
    ({
        credentials: null,
    }),

    getters:
    {
        getCredentials: (state) => state.credentials
    },

    actions:
    {
        // Save credentials
        storeCredentials(credentials)
        {
            this.credentials = credentials
        },

        // Clear credentials
        clearCredentials()
        {
            this.credentials = null
        }
    },

    persist: true
});

// Non-persistent storage (for Remember Me mode disabled)
const useAuthNonPersistentStore = defineStore("authNonPersistent", {
    state: () =>
        ({
            credentials: null,
            token: null,
        }),

    getters:
        {
            getCredentials: (state) => state.credentials,
            getToken: (state) => state.token
        },

    actions:
        {
            // Save credentials
            storeCredentials(credentials)
            {
                this.credentials = credentials
            },

            // Clear credentials
            clearCredentials()
            {
                this.credentials = null
            },

            // Store token
            storeToken(token)
            {
                this.token = token
            },

            // Clear token
            clearToken()
            {
                this.token = null
            }
        },

    persist: {
        storage: sessionStorage
    },
});

// Get credentials
async function AuthGetCredentials()
{
    const persistentCredentials = useAuthPersistentStore().getCredentials
    if (persistentCredentials !== null)
    {
        return persistentCredentials;
    }

    return useAuthNonPersistentStore().getCredentials;
}

// Save credentials
async function AuthStoreCredentials(login, password, isSaveToPersistentStorage)
{
    const store = isSaveToPersistentStorage ? useAuthPersistentStore() : useAuthNonPersistentStore()
    store.storeCredentials({ login: login, password: password })
}

// Get authentication token
async function AuthGetToken()
{
    return useAuthNonPersistentStore().getToken
}

// Checks if user logged in
async function AuthIsUserLoggedIn()
{
    await AuthRefreshToken()

    const token = await AuthGetToken()

    return token !== null; // If we have token, then believe that user is logged in
}

// Store token to selected storage
async function AuthStoreToken(token, expiration)
{
    useAuthNonPersistentStore().storeToken({ token: token, expiration: expiration })
}

// Clear stored token
async function AuthClearToken()
{
    useAuthNonPersistentStore().clearToken()
}

// Clear stored credentials
async function AuthClearCredentials()
{
    useAuthPersistentStore().clearCredentials()
    useAuthNonPersistentStore().clearCredentials()
}

// If there are no stored credentials, then we aren't logged in - returns false
// If there are stored credentials, but they are incorrect - logs user out and returns false
// If there are stored credentials and token is not going to expire - returns true
// If there are stored credentials and token is going to expire - refreshes and returns true
async function AuthRefreshToken()
{
    const credentials = await AuthGetCredentials()
    if (credentials === null)
    {
        return false // We aren't logged in
    }

    const token = await AuthGetToken()
    if (token === null || (Date.parse(token.expiration) - Date.now() < remainingTokenLifetimeToRefresh))
    {
        // Token is not exist or going to expire
        const tokenResponse = await GetTokenByCredentials(credentials.login, credentials.password)
        if (tokenResponse === null)
        {
            // Wrong credentials
            await AuthLogUserOut()
            return false
        }

        await AuthStoreToken(tokenResponse.token, tokenResponse.expiration)
        return true // Successfully refreshed
    }

    return true
}

async function GetTokenByCredentials(login, password)
{
    const loginResponse = (await (await fetch(apiBaseUrl + `/api/Users/Login`, {
        method: 'POST',
        body: JSON.stringify({
            "loginData": {
                login: login,
                password: password
            }
        }),
        headers: {
            'Content-Type': 'application/json'
        }
    }))
        .json())
        .loginResult;

    if (!loginResponse.isSuccessful)
    {
        return null; // Wrong credentials, we can't get token
    }

    return { token: loginResponse.token, expiration: loginResponse.expiration };
}

async function AuthLogUserIn(login, password, isRememberMe)
{
    const token = await GetTokenByCredentials(login, password)

    if (token !== null)
    {
        // Logging user in
        await AuthStoreCredentials(login, password, isRememberMe) // Storing in selected storage depening on isRememberMe
        await AuthStoreToken(token.token, token.expiration) // Token always stored in non-persistent storage

        return LoginResult.OK
    }

    return LoginResult.InvalidCredentials
}

async function AuthLogUserOut()
{
    await AuthClearCredentials()
    await AuthClearToken()

    // Going to main page
    await router.push("/")
}

async function AuthRedirectToLoginPageIfNotLoggedIn()
{
    if (!await AuthIsUserLoggedIn())
    {
        await router.push("/login")
    }
}

export
{
    AuthLogUserIn,
    AuthLogUserOut,
    AuthGetCredentials,
    AuthGetToken,
    AuthRefreshToken,
    AuthIsUserLoggedIn,
    AuthRedirectToLoginPageIfNotLoggedIn
}