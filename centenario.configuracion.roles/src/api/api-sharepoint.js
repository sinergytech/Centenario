const config = {
    siteUrl: (process.env.VUE_APP_SITE_URL ? process.env.VUE_APP_SITE_URL : window.appConfig.siteUrl)
};

import {CONTENT_TYPES} from '../utils';

// if(config.siteUrl.toLowerCase().indexOf('sharepoint.com') >= 0 ){
//     const baseSharepointUrl = config.siteUrl.slice(0,config.siteUrl.toLowerCase().indexOf('sharepoint.com') + 'sharepoint.com'.length);
//     const relativeFolderBase = config.siteUrl.replace(baseSharepointUrl, '');
// }else{
//     const baseSharepointUrl = config.siteUrl;
//     const relativeFolderBase = config.siteUrl;
// }
let baseSharepointUrl = config.siteUrl;
if(config.siteUrl.toLowerCase().indexOf('sharepoint.com') >= 0 ){
    baseSharepointUrl = config.siteUrl.slice(0,config.siteUrl.toLowerCase().indexOf('sharepoint.com')) + 'sharepoint.com';    
}
export const apiUrl = config.siteUrl + '/_api';
export const apiService = '/_api';
export const urlCurrentUser = `${apiUrl}/web/CurrentUser?$expand=Groups`;
export const urlCurrentUserProfile = `${apiUrl}/SP.UserProfiles.PeopleManager/GetMyProperties`;
export const urlSiteUsers = `${apiUrl}/web/siteUsers`;
export const urlSiteGroups = `${apiUrl}/web/siteGroups`;
let digestValue = null;

export function getFolderUrl(subsite, folder){
    const relativeFolderBase = config.siteUrl.replace(baseSharepointUrl, '');
    return `${config.siteUrl + (subsite ? '/' + subsite : '') + apiService}/web/GetFolderByServerRelativeUrl('${relativeFolderBase}/${folder}')`;
    // return `${apiUrl}/web/GetFolderByServerRelativeUrl('/sites/TestPlataformaLegal/${folder}')`;
}
export function getFolderUrlComplete(subsite, folderFull){
    return `${config.siteUrl + (subsite ? '/' + subsite : '') + apiService}/web/GetFileByServerRelativeUrl('${folderFull}')`;
}

export function getListUrl(subsite, listName){
    if (listName.indexOf('guid:') === 0){
        return `${config.siteUrl + (subsite ? '/' + subsite : '') + apiService}/web/lists(guid'${listName.replace('guid:','')}')`;
    }else{
        return `${config.siteUrl + (subsite ? '/' + subsite : '') + apiService}/web/lists/getbytitle('${listName}')`;
    }
}

function standardOperator(operator){
    operator = operator || 'eq';
    return operator
        .replace('<=', 'le')
        .replace('>=', 'ge')
        .replace('<>', 'ne')
        .replace('!=', 'ne')
        .replace('<', 'lt')
        .replace('>', 'gt')
        .replace('=', 'eq');
}

export function createFilter(filterDefinition, logicalOperator){
    
    if (Array.isArray(filterDefinition)){
        logicalOperator = filterDefinition[1];
        filterDefinition = filterDefinition[0];
    }

    logicalOperator = logicalOperator || 'and';
    let filter = Object.keys(filterDefinition).map( campo => {        
        let operator = standardOperator(filterDefinition[campo].operator);
        let value = filterDefinition[campo].value || filterDefinition[campo];
        if (value.toString().indexOf("|") === -1){
            value = typeof value === 'string' ? `'${value}'` : value;
            return  `${campo} ${operator} ${value}`;
        }else{
            return value.split('|').map(val => `${campo} ${operator} ${val}`).join(' or ');
        }
        
    });
    
    return '$filter=' + filter.join(' ' + logicalOperator + ' ');
}
async function getDigest(subsite){
    let jsonContext = await (await fetch(`${config.siteUrl + (subsite ? '/' + subsite : '')}/contextinfo`,{
        headers: {
            accept: "application/json;odata=verbose"
        },
        method: "POST"
    })).json();
    return jsonContext.d.GetContextWebInformation.FormDigestValue;
}
(function(){
    setInterval(function(){
        digestValue = null;
    }, 1000 * 60 * 10);
})();

export async function post(url, {jsonBody}){
    if (!digestValue){
        digestValue = await getDigest();
    }
    let respuesta =  await fetch(url, {
        method: 'POST',
        headers: {
            "accept":"application/json;odata=verbose",
            "content-type": "application/json;odata=verbose",
            "Content-Length": JSON.stringify(jsonBody).length.toString(),
            "X-RequestDigest": digestValue
        },
        body: JSON.stringify(jsonBody)
    });
    let contenido;
    if (respuesta.json){
        contenido = await respuesta.json();
        contenido = contenido.d || contenido;
    }else{
        contenido = await respuesta.text();
    }
    return contenido;
}
export async function postFile(folderUrl, file, fileName){
    
    let url = `${folderUrl}/Files/add(url='${fileName}',overwrite=true)`;
    if (!digestValue){
        digestValue = await getDigest();
    }
    return await fetch(url, {
        method:"POST",
        headers: {
            "accept":"application/json;odata=verbose",
            "content-type": file.type,
            "Content-Length": file.size,
            "X-RequestDigest": digestValue
        },
        body: file,
    });
}
export async function attachFile(subsite, listName, ItemId, file, fileName) {
    let url = `${getListUrl(subsite, listName)}/items(${ItemId})/AttachmentFiles/add(FileName='${fileName}')`;
    if (!digestValue){
        digestValue = await getDigest();
    }
    return fetch(url, {
        method:"POST",
        headers: {
            "accept":"application/json;odata=verbose",
            // "content-type": file.type,
            "Content-Length": file.size,
            "X-RequestDigest": digestValue
        },
        body: file,
    });    
}
export async function getAttachmentFiles(subsite, listName, ItemId){
    let url = `${getListUrl(subsite, listName)}/items(${ItemId})/AttachmentFiles`;
    let files = await get(url);    
    return files.map( f => { 
        return {fileName: f.FileName, url: baseSharepointUrl + f.ServerRelativeUrl}; 
    });
}
// Carga un nuevo documento a la biblioteca correspondiente segun el tipo de documento
// Una vez creado, obtiene el registro de lista asociado al documento en la
// biblioteca y actualiza en él el campo Title con el nombre del
// archivo. Por ultimo, retorna el registro del documento
export async function uploadFile(subsite, file, fileName, listName, fields, folder){
    let folderURL = getFolderUrl(subsite,folder != null && folder != undefined ? folder : listName);
    return postFile(folderURL, file, fileName).then( respuesta => {
        return respuesta.json();
    }).then(jsonValue => {
        return get(jsonValue.d.ListItemAllFields.__deferred.uri);
    }).then( documentItem => {
        let listaName = listName;
        let url =  getListUrl(subsite, listaName) + `/items(${documentItem.Id})`;
        let actualizacion = Object.assign({}, fields,{
            __metadata: {type: `SP.Data.${listaName}Item`},
            Title: fields.Title || fileName
        });
        return put(url, {jsonBody: actualizacion}).then( () => {
            documentItem.Title = fields.Title || fileName;
            return documentItem;
        });
    }).catch( e => {
        throw {error: e, mensaje: `No se pudo cargar el documento ${fileName} a la carpeta ${folder}.`};
    });
}

export function removeFileFromUrl(subsite, FolderNameFull){    
    let url = getFolderUrlComplete(subsite, FolderNameFull);
    return del(url);
}

export async function getFilesFromFolder(subsite, folder)
{
    let folderURL = getFolderUrl(subsite, folder) + '?$expand=Files,Files/ListItemAllFields';
    return get(folderURL);    
}

export async function getFileUrl(subsite, fileId, folder){   

    let resultado = await getListItems(subsite,folder, {Id: fileId});
    let respuesta = await get(resultado[0].File.__deferred.uri + '/GetPreAuthorizedAccessUrl(1)');

    return respuesta.GetPreAuthorizedAccessUrl;
}

export async function put(url, {jsonBody}){
    if (!digestValue){
        digestValue = await getDigest();
    }

    return await fetch(url, {
        method: 'POST',
        headers: {
            "accept":"application/json;odata=verbose",
            "content-type": "application/json;odata=verbose",
            "Content-Length": JSON.stringify(jsonBody).length.toString(),
            "X-RequestDigest": digestValue,
            "IF-MATCH": "*",
            "X-HTTP-Method":"MERGE",
        },
        body: JSON.stringify(jsonBody)
    });
}

export async function del(url){
    if (!digestValue){
        digestValue = await getDigest();
    }

    return await fetch(url, {
        method: 'POST',
        headers: {
            "accept":"application/json;odata=verbose",
            "content-type": "application/json;odata=verbose",
            "X-RequestDigest": digestValue,
            "IF-MATCH": "*",
            "X-HTTP-Method":"DELETE",
        }
    });
}
export async function get(url){
    let data = (await (await fetch(url, {
        headers: {
            accept:"application/json;odata=verbose"
        }
    })).json());
    data = data.d || data;
    return data.results || data;
}

export function getSiteUsers(filter, select, expand){
    let url = urlSiteUsers;
    let _filter = filter ? createFilter(filter) : null;
    let _select = select ? '$select=' + select.join(',') : null;
    let _expand = expand ? '$expand=' + expand.join(',') : null;
    url += filter || select || expand ? '?' : '';
    url += [_filter, _select, _expand].filter( e => e).join('&');
    return get(url);    
}
export function getSiteGroups(filter, select, expand){
    let url = urlSiteGroups;
    let _filter = filter ? createFilter(filter) : null;
    let _select = select ? '$select=' + select.join(',') : null;
    let _expand = expand ? '$expand=' + expand.join(',') : null;
    url += filter || select || expand ? '?' : '';
    url += [_filter, _select, _expand].filter( e => e).join('&');
    return get(url);    
}

export function getCurrentUser(){
    return get(urlCurrentUser);    
}

export function getCurrentUserProfile(){
    return get(urlCurrentUserProfile);    
}

export function getListItems(subsite, listName, filter, select, expand, top){
    let url = getListUrl(subsite, listName) + '/items';
    top = top || 5000;
    let _filter = filter ? createFilter(filter) : null;
    let _select = select ? '$select=' + select.join(',') : null;
    let _expand = expand ? '$expand=' + expand.join(',') : null;
    let _top = top ? '$top=' + top : null;    
    url += filter || select || expand || top ? '?' : '';
    url += [_filter, _select, _expand, _top].filter( e => e).join('&');
    return get(url);
}

export function addListItem(subsite, listName, newItem){    
    let url = getListUrl(subsite, listName) + '/items';
    newItem['__metadata'] = newItem['__metadata'] || { 'type': `SP.Data.${listName}ListItem` };

    return post(url, {jsonBody: newItem});
}
export function updateListItem(subsite, listName, itemId, changes){    
    let url = `${getListUrl(subsite, listName)}/items(${itemId})`;    
    changes['__metadata'] = changes['__metadata'] || { 'type': `SP.Data.${listName}ListItem` };

    return put(url,{jsonBody: changes});
}
export function removeListItem(subsite, listName, itemId){    
    let url = `${getListUrl(subsite, listName)}/items(${itemId})`;

    return del(url);
}

export function getUserPictureUrl(userLogin, size){
    size = size || 'M';
    return `${baseSharepointUrl}/_layouts/15/userphoto.aspx?size=${size}&accountname=${userLogin}`;
}

export function getWebInfo(subsite){
    return get(`${config.siteUrl + (subsite ? '/' + subsite : '')}/web`);
}

export async function createContractDocumentSet(subsite, bibUrl, name){
    let webinfo = await getWebInfo();
    if (!digestValue){
        digestValue = await getDigest();
    }

    let docSetOptions = {
        Path: bibUrl,
        Title: name,
        ContentTypeId: CONTENT_TYPES.DOCUMENTSET_ID,
    }

    return await fetch(config.siteUrl + (subsite ? '/' + subsite : '') + "/_vti_bin/listdata.svc/" + bibUrl, {
        method: "POST",
        headers: {
            "Accept": "application/json;odata=verbose",
            "content-type": "application/json;odata=verbose",
            "X-RequestDigest": digestValue,
            "Slug": `${webinfo.ServerRelativeUrl}/${docSetOptions.Path}/${docSetOptions.Title}|${docSetOptions.ContentTypeId}`,
        }
    }).then(function(resp) {
        return resp.json();
    });
}