import authService from "../api-authorization/AuthorizeService";

const put = 'put';
const post = 'post';
const patch = 'patch';

export default class Backend{
    private static _instance: Backend;

    public static GetInstance(): Backend{
        if (!!!this._instance)
            this._instance = new Backend()
        return this._instance
    }

    public async Get(address){
        const token = await authService.getAccessToken();
        const response = await fetch(address, {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });

        const data = await response.json();
        console.log(data)
        return data;
    }

    public async Delete(address){
        const token = await authService.getAccessToken();
        const response = await fetch(address, {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` },
            method: 'delete'
        });

        const data = await response.json();
        console.log(data);
        return data;
    }

    public async Post(address, object, onDone: Function | null = null){
        const auth = await this.makeAuthString()
        const response = await this.makeUpdate(address, object, auth, post)
        onDone !== null && onDone();
        return response
    }

    public async Put(address, object, onDone: Function | null = null){
        const auth = await this.makeAuthString();
        const response = await this.makeUpdate(address, object, auth, put)
        onDone !== null && onDone();
        return response
    }

    public async Patch(address, object, onDone: Function | null = null){
        const auth = await this.makeAuthString()
        const response = await this.makeUpdate(address, object, auth, patch)
        onDone !== null && onDone();
        return response
    }

    private async makeUpdate(address, object, auth, method){
        return await fetch(address, {
            headers: { 'Content-type': 'application/json', ...auth },
            method: method,
            body: JSON.stringify(object)
        })
    }

    private async makeAuthString(){
        const token = await authService.getAccessToken();
        return !token ? {} : { 'Authorization': `Bearer ${token}` }
    }
}