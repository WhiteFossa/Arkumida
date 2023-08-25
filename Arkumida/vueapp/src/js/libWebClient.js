// API base URL
import {AuthGetToken, AuthRefreshToken} from "@/js/auth";

const apiBaseUrl = process.env.VUE_APP_API_URL

// Make GET request
async function WebClientSendGetRequest
(
    relativeUrl
)
{
    await AuthRefreshToken()

    const authToken = await AuthGetToken()
    let headers = {}
    if (authToken !== null)
    {
        headers = { 'Authorization': 'Bearer ' + authToken.token }
    }

    return await fetch(apiBaseUrl + relativeUrl, {
        method: 'GET',
        headers: headers
    })
}

// Make POST request
async function WebClientSendPostRequest
(
    relativeUrl,
    request
)
{
    await AuthRefreshToken()

    const authToken = await AuthGetToken()

    let headers = { 'Content-Type': 'application/json' }
    if (authToken !== null)
    {
        headers.Authorization = 'Bearer ' + authToken.token
    }

    return await fetch(apiBaseUrl + relativeUrl, {
        method: 'POST',
        body: JSON.stringify(request),
        headers: headers
    })
}

export
{
    WebClientSendGetRequest,
    WebClientSendPostRequest
}