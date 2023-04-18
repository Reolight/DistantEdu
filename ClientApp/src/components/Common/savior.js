export function SaveItem(address, object){
    localStorage.setItem(address, JSON.stringify(object))
}

export function GetItem(address){
    const json = localStorage.getItem(address);
    return JSON.parse(json);
}

export function RemoveItem(address){
    localStorage.removeItem(address);
}