import authService from "../api-authorization/AuthorizeService";

export async function Get(address){
    const token = await authService.getAccessToken();
    const response = await fetch(address, {
        headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });

    const data = await response.json();
    console.log(data)
    return data;
}

export async function Post(address, object, onDone = undefined){
    const auth = await makeAuthString()
    const response = await makePostOrPut(address, object, auth, true)
    onDone !== undefined && onDone();
    return response
}

export async function Put(address, object, onDone = undefined){
    const auth = await makeAuthString();
    const response = await makePostOrPut(address, object, auth, false)
    onDone !== undefined && onDone();
    return response
}

async function makePostOrPut(address, object, auth, isPost){
    return await fetch(address, {
        headers: { 'Content-type': 'application/json', ...auth },
        method: isPost? "POST" : 'PUT',
        body: JSON.stringify(object)
    })
}

async function makeAuthString(){
    const token = await authService.getAccessToken();
    return !token ? {} : { 'Authorization': `Bearer ${token}` }
}