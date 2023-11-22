// API base URL
import {AuthGetToken, AuthLogCreatureOut, AuthRefreshToken} from "@/js/auth";

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

    const response = await fetch(apiBaseUrl + relativeUrl, {
        method: 'GET',
        headers: headers
    })

    if (response.status === 401)
    {
        // Suddenly 401 while we have a valid token (because called AuthRefreshToken()). Looks like credentials are changed
        // on server side
        await AuthLogCreatureOut()
    }

    return response;
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

    const response = await fetch(apiBaseUrl + relativeUrl, {
        method: 'POST',
        body: JSON.stringify(request),
        headers: headers
    })

    if (response.status === 401)
    {
        // Suddenly 401 while we have a valid token (because called AuthRefreshToken()). Looks like credentials are changed
        // on server side
        await AuthLogCreatureOut()
    }

    return response
}

// Post a form (mostly probably with a file)
async function WebClientPostForm
(
    relativeUrl,
    request
)
{
    await AuthRefreshToken()

    const authToken = await AuthGetToken()

    let headers = { }
    if (authToken !== null)
    {
        headers.Authorization = 'Bearer ' + authToken.token
    }

    const response = await fetch(apiBaseUrl + relativeUrl, {
        method: 'POST',
        body: request,
        headers: headers
    })

    if (response.status === 401)
    {
        // Suddenly 401 while we have a valid token (because called AuthRefreshToken()). Looks like credentials are changed
        // on server side
        await AuthLogCreatureOut()
    }

    return response
}

export
{
    WebClientSendGetRequest,
    WebClientSendPostRequest,
    WebClientPostForm
}