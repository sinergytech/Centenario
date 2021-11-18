import {getCurrentUser, getSiteUsers, getSiteGroups, getCurrentUserProfile, getUserPictureUrl, getListItems, addListItem, updateListItem, removeListItem, attachFile, getAttachmentFiles, uploadFile, getFileUrl, getListUrl, createFilter, get, createContractDocumentSet, getFilesFromFolder, removeFileFromUrl} from './api-sharepoint';

export default {   
    getSiteUsers, 
    getSiteGroups,
    getCurrentUser,
    getCurrentUserProfile,
    getUserPictureUrl,
    getListItems,
    addListItem,
    updateListItem,
    removeListItem,
    getListFields(subsite, lista, filtro){
        let _filtro = filtro ? '?' + createFilter(filtro) : '';
        let url = getListUrl(subsite, lista) + '/fields' + _filtro;
        return get(url);
    },
    getListItemCount(subsite, lista){
        let url =  getListUrl(subsite, lista) + '/ItemCount';
        return get(url);
    },
    attachFile,
    getAttachmentFiles,
    uploadFile,
    getFileUrl,
    createContractDocumentSet,
    getFilesFromFolder,
    removeFileFromUrl
}
