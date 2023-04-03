import authService from "../api-authorization/AuthorizeService";

const put = 'put';
const post = 'post';
const patch = 'patch';

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
    const response = await makeUpdate(address, object, auth, post)
    onDone !== undefined && onDone();
    return response
}

export async function Put(address, object, onDone = undefined){
    const auth = await makeAuthString();
    const response = await makeUpdate(address, object, auth, put)
    onDone !== undefined && onDone();
    return response
}

export async function Patch(address, object, onDone = undefined){
    const auth = await makeAuthString()
    const response = await makeUpdate(address, object, auth, patch)
    onDone !== undefined && onDone();
    return response
}

async function makeUpdate(address, object, auth, method){
    return await fetch(address, {
        headers: { 'Content-type': 'application/json', ...auth },
        method: method,
        body: JSON.stringify(object)
    })
}

async function makeAuthString(){
    const token = await authService.getAccessToken();
    return !token ? {} : { 'Authorization': `Bearer ${token}` }
}