var SPOAppParameters = {};
var funcSPO = {
    initial: function () {
        SPOAppParameters.CurrentContext = SP.ClientContext.get_current();
        SPOAppParameters.SPHost = decodeURIComponent(Utils.getQueryStringParameter('SPHostUrl'));;
        SPOAppParameters.SPAppWebUrl = decodeURIComponent(Utils.getQueryStringParameter('SPAppWebUrl'));
        SPOAppParameters.CurrentUser = this.getCurrentUser();
    },
    getCurrentUser: function () {
        var url = SPOAppParameters.SPAppWebUrl + "/_api/SP.AppContextSite(@target)/Web/CurrentUser?$select=Title,Email,Id,LoginName&@target='" + SPOAppParameters.SPHost + "'"
        $.ajax({
            url: url,
            type: "GET",
            headers: { "Accept": "application/json;odata=verbose" }, // return data format
            async: false,
            //beforeSend:{ app.LoaderShow()},
            success: function (data) {
                SPOAppParameters.CurrentUser = data.d;
                if (SPOAppParameters.CurrentUser.Email == '')
                    SPOAppParameters.CurrentUser.Email = SPOAppParameters.CurrentUser.LoginName.replace('i:0#.f|membership|', '').toUpperCase();
            },
            error: function (data) {

                $("#message").html("Failed to read items from host web.");
            }
        });
    },
    getListItems: function (listName, fnSuccess, fnOption, sync, subsite) {
        var bAsync = true;
        if (sync)
            bAsync = false;
        var url = SPOAppParameters.SPAppWebUrl + "/_api/SP.AppContextSite(@target)/web/lists/getbytitle('" + listName + "')/items?@target='" + SPOAppParameters.SPHost + subsite + "'";
        $.ajax({
            url: url,
            type: "GET",
            headers: { "Accept": "application/json;odata=verbose" }, // return data format
            async: bAsync,
            //beforeSend:{ app.LoaderShow()},
            success: function (data) {
                if (fnOption == undefined)
                    fnSuccess(data.d.results);
                else
                    fnSuccess(data.d.results, fnOption);
            },
            error: function (data) {

                Utils.swalError(data.responseJSON.error.message.value);
            }
        });
    },
    getListItemsFilter: function (listName, filter, fnSuccess, fnOption, sync, subsite) {
        var bAsync = true;
        if (sync)
            bAsync = false;
        var url = SPOAppParameters.SPAppWebUrl + "/_api/SP.AppContextSite(@target)/web/lists/getbytitle('" + listName + "')/items?$filter=" + filter + "&@target='" + SPOAppParameters.SPHost + subsite + "'";
        $.ajax({
            url: url,
            type: "GET",
            headers: { "Accept": "application/json;odata=verbose" }, // return data format
            async: bAsync,
            //beforeSend:{ app.LoaderShow()},
            success: function (data) {
                if (fnOption == undefined)
                    fnSuccess(data.d.results);
                else
                    fnSuccess(data.d.results, fnOption);
            },
            error: function (data) {

                Utils.swalError(data.responseJSON.error.message.value);
            }
        });
    },
    getListItemById: function (listName, idItem, fnSuccess, fnError) {
        var url = SPOAppParameters.SPAppWebUrl + "/_api/SP.AppContextSite(@target)/web/lists/getbytitle('" + listName + "')/items(" + idItem + ")?@target='" + SPOAppParameters.SPHost + "'";
        $.ajax({
            url: url,
            type: "GET",
            headers: { "Accept": "application/json;odata=verbose" }, // return data format
            //beforeSend:{ app.LoaderShow()},
            success: function (data) {
                fnSuccess(data.d);
                //$("#message").html("Found " + data.d.results.length + " items.");
            },
            error: function (data) {
                Utils.swalError(data.responseJSON.error.message.value);
            }
        });
    },
    getListItemByIdSelect: function (listName, idItem, strSelect, fnSuccess, fnError, sync) {
        var bAsync = true;
        if (sync)
            bAsync = false;
        var url = SPOAppParameters.SPAppWebUrl + "/_api/SP.AppContextSite(@target)/web/lists/getbytitle('" + listName + "')/items(" + idItem + ")?$select=" + strSelect + "&@target='" + SPOAppParameters.SPHost + "'";
        $.ajax({
            url: url,
            type: "GET",
            headers: { "Accept": "application/json;odata=verbose" }, // return data format
            async: bAsync,
            //beforeSend:{ app.LoaderShow()},
            success: function (data) {
                fnSuccess(data.d);
                //$("#message").html("Found " + data.d.results.length + " items.");
            },
            error: function (data) {
                Utils.swalError(data.responseJSON.error.message.value);
            }
        });
    },
    callExternalService: function (requestUrl, fn_succes, fn_error) {
        Utils.LoaderShow();
        var request = new SP.WebRequestInfo();
        request.set_url(requestUrl);
        request.set_method("GET");
        funcSPO.responseExternalService = SP.WebProxy.invoke(SPOAppParameters.CurrentContext, request);
        SPOAppParameters.CurrentContext.executeQueryAsync(fn_succes, fn_error);
    },
    EnsureUserFn: function (email, fnSuccess) {
        var url = SPOAppParameters.SPAppWebUrl + "/_api/SP.AppContextSite(@target)/web/ensureuser?@target='" + SPOAppParameters.SPHost + "'";
        var requestBody = JSON.stringify({ 'logonName': email });
        var exec = new SP.RequestExecutor(SPOAppParameters.SPAppWebUrl);
        exec.executeAsync
            ({
                url: url,
                method: "POST",
                contentType: "application/json;odata=verbose",
                body: requestBody,
                headers:
                {
                    "accept": "application/json;odata=verbose",
                    "Content-Type": "application/json;odata=verbose"
                },
                success: function (data, status) {
                    var jsonbody = JSON.parse(data.body);
                    var data = jsonbody.d;
                    fnSuccess(data);
                },
                error: function (data, status, error) {
                    var jsonbody = JSON.parse(data.body);
                    //$("#message").html("Failed to read items from host web. " + jsonbody.error);
                }
            });
    },
    EnsureUserFn2: function (email, fnSuccess) {
        var url = SPOAppParameters.SPAppWebUrl + "/_api/SP.AppContextSite(@target)/web/ensureuser?@target='" + SPOAppParameters.SPHost + "'";
        var requestBody = JSON.stringify({ 'logonName': email });
        var exec = new SP.RequestExecutor(SPOAppParameters.SPAppWebUrl);
        exec.executeAsync
            ({
                url: url,
                method: "POST",
                contentType: "application/json;odata=verbose",
                body: requestBody,
                headers:
                {
                    "accept": "application/json;odata=verbose",
                    "Content-Type": "application/json;odata=verbose"
                },
                success: function (data, status) {
                    var jsonbody = JSON.parse(data.body);
                    var data = jsonbody.d;
                    fnSuccess(data);
                },
                error: function (data, status, error) {
                    var jsonbody = JSON.parse(data.body);
                    //$("#message").html("Failed to read items from host web. " + jsonbody.error);
                }
            });
    },
    EnsureUserFn3: function (email, fnSuccess) {
        var url = SPOAppParameters.SPAppWebUrl + "/_api/SP.AppContextSite(@target)/web/ensureuser?@target='" + SPOAppParameters.SPHost + "'";
        var requestBody = JSON.stringify({ 'logonName': email });
        var exec = new SP.RequestExecutor(SPOAppParameters.SPAppWebUrl);
        exec.executeAsync
            ({
                url: url,
                method: "POST",
                contentType: "application/json;odata=verbose",
                body: requestBody,
                headers:
                {
                    "accept": "application/json;odata=verbose",
                    "Content-Type": "application/json;odata=verbose"
                },
                success: function (data, status) {
                    var jsonbody = JSON.parse(data.body);
                    var data = jsonbody.d;
                    fnSuccess(data);
                },
                error: function (data, status, error) {
                    var jsonbody = JSON.parse(data.body);
                    //$("#message").html("Failed to read items from host web. " + jsonbody.error);
                }
            });
    },
    EnsureUserFn4: function (email, fnSuccess) {
        var url = SPOAppParameters.SPAppWebUrl + "/_api/SP.AppContextSite(@target)/web/ensureuser?@target='" + SPOAppParameters.SPHost + "'";
        var requestBody = JSON.stringify({ 'logonName': email });
        var exec = new SP.RequestExecutor(SPOAppParameters.SPAppWebUrl);
        exec.executeAsync
            ({
                url: url,
                method: "POST",
                contentType: "application/json;odata=verbose",
                body: requestBody,
                headers:
                {
                    "accept": "application/json;odata=verbose",
                    "Content-Type": "application/json;odata=verbose"
                },
                success: function (data, status) {
                    var jsonbody = JSON.parse(data.body);
                    var data = jsonbody.d;
                    fnSuccess(data);
                },
                error: function (data, status, error) {
                    var jsonbody = JSON.parse(data.body);
                    //$("#message").html("Failed to read items from host web. " + jsonbody.error);
                }
            });
    },
    EnsureUserFn5: function (email, fnSuccess) {
        var url = SPOAppParameters.SPAppWebUrl + "/_api/SP.AppContextSite(@target)/web/ensureuser?@target='" + SPOAppParameters.SPHost + "'";
        var requestBody = JSON.stringify({ 'logonName': email });
        var exec = new SP.RequestExecutor(SPOAppParameters.SPAppWebUrl);
        exec.executeAsync
            ({
                url: url,
                method: "POST",
                contentType: "application/json;odata=verbose",
                body: requestBody,
                headers:
                {
                    "accept": "application/json;odata=verbose",
                    "Content-Type": "application/json;odata=verbose"
                },
                success: function (data, status) {
                    var jsonbody = JSON.parse(data.body);
                    var data = jsonbody.d;
                    fnSuccess(data);
                },
                error: function (data, status, error) {
                    var jsonbody = JSON.parse(data.body);
                    //$("#message").html("Failed to read items from host web. " + jsonbody.error);
                }
            });
    },
    addListItem: function (listName, itemData, fn_succes, fn_error, subsite) {
        var url = SPOAppParameters.SPAppWebUrl + "/_api/SP.AppContextSite(@target)/web/lists/getbytitle('" + listName + "')/items" + "?@target='" + SPOAppParameters.SPHost + subsite + "'";
        var requestBody = JSON.stringify(itemData);
        var exec = new SP.RequestExecutor(SPOAppParameters.SPAppWebUrl);
        exec.executeAsync
            ({
                url: url,
                method: "POST",
                body: requestBody,
                headers:
                {
                    "Accept": "application/json;odata=verbose",
                    "Content-Type": "application/json;odata=verbose"
                },
                success: function (data, status) {
                    if (data.body != '') {
                        var jsonbody = JSON.parse(data.body);
                        fn_succes(jsonbody.d);
                    }
                    else
                        fn_succes(data.statusCode);
                },
                error: function (xhr, status, error) {
                    var jsonbody = JSON.parse(xhr.body);
                    fn_error(jsonbody.error);
                }
            });
    },
    updateListItem: function (listName, itemData, itemID, fn_succes, fn_error, subsite) {
        var url = SPOAppParameters.SPAppWebUrl + "/_api/SP.AppContextSite(@target)/web/lists/getbytitle('" + listName + "')/items(" + itemID + ")" + "?@target='" + SPOAppParameters.SPHost + subsite + "'";
        var requestBody = JSON.stringify(itemData);
        var exec = new SP.RequestExecutor(SPOAppParameters.SPAppWebUrl);
        exec.executeAsync
            ({
                url: url,
                method: "POST",
                body: requestBody,
                headers:
                {
                    "Accept": "application/json;odata=verbose",
                    "Content-Type": "application/json;odata=verbose",
                    "X-HTTP-Method": "MERGE",
                    "IF-MATCH": "*"
                },
                success: function (data, status) {
                    if (data.body != '') {
                        var jsonbody = JSON.parse(data.body);
                        fn_succes(jsonbody);
                    }
                    else
                        fn_succes(data.statusCode);
                },
                error: function (xhr, status, error) {
                    var jsonbody = JSON.parse(xhr.body);
                    fn_error(jsonbody.error);
                }
            });
    },
    uploadFileList: function (listName, id, file, fn_succes, fn_error) {
        var deferred = $.Deferred();
        var fileName = file.name;
        getFileBuffer(file).then(
            function (buffer) {
                var bytes = new Uint8Array(buffer);
                var binary = '';
                for (var b = 0; b < bytes.length; b++) {
                    binary += String.fromCharCode(bytes[b]);
                }
                var url = SPOAppParameters.SPAppWebUrl + "/_api/SP.AppContextSite(@target)/web/lists/GetByTitle('" + listName + "')/items(" + id + ")/AttachmentFiles/add(FileName='" + file.name + "')" + "?@target='" + SPOAppParameters.SPHost + "'";
                var exec = new SP.RequestExecutor(SPOAppParameters.SPAppWebUrl);
                exec.executeAsync
                    ({
                        url: url,
                        method: "POST",
                        headers:
                        {
                            "Accept": "application/json;odata=verbose",
                            "Content-Type": "application/json;odata=verbose"
                        },
                        binaryStringRequestBody: true,
                        body: binary,
                        state: "Update",
                        success: function (data) {
                            console.log(data + ' uploaded successfully');
                            deferred.resolve(data);
                            var jsonbody = JSON.parse(data.body);
                            fn_succes(jsonbody.d);
                        },
                        error: function (data) {
                            console.log(fileName + "not uploaded error");
                            deferred.reject(data);
                            var jsonbody = JSON.parse(data.body);
                            fn_error(jsonbody.d);
                        }
                    });
            },
            function (err) {
                deferred.reject(err);
            }
        );
        return deferred.promise();
    },
    uploadFileLibrary: function (listName, filename, file, fn_succes, fn_error) {
        var deferred = $.Deferred();
        getFileBuffer(file).then(
            function (buffer) {
                var binary = Utils.bufferToBinary(buffer);
                var url = SPOAppParameters.SPAppWebUrl + "/_api/SP.AppContextSite(@target)/web/lists/getByTitle('" + listName + "')/RootFolder/files/add(url='" + filename + "',overwrite='true')" + "?@target='" + SPOAppParameters.SPHost + "'";
                var exec = new SP.RequestExecutor(SPOAppParameters.SPAppWebUrl);
                exec.executeAsync
                    ({
                        url: url,
                        method: "POST",
                        headers:
                        {
                            "Accept": "application/json;odata=verbose",
                            "Content-Type": "application/json;odata=verbose"
                        },
                        binaryStringRequestBody: true,
                        body: binary,
                        state: "Update",
                        success: function (data) {
                            console.log(data + ' uploaded successfully');
                            deferred.resolve(data);
                            var jsonbody = JSON.parse(data.body);
                            var getItem = getListItem(jsonbody.d.ListItemAllFields.__deferred.uri);
                            getItem.done(function (listItem, status, xhr) {
                                fn_succes(listItem.d);
                                // Change the display name and title of the list item.
                                //var changeItem = updateListItem(listItem.d.__metadata);
                                //changeItem.done(function (data, status, xhr) {
                                //    alert('file uploaded and updated');
                                //});
                                //changeItem.fail(onError);
                            });
                            //getItem.fail(onError);
                            //fn_succes(jsonbody.d);
                        },
                        error: function (data) {
                            //console.log(fileName + "not uploaded error");
                            deferred.reject(data);
                            var jsonbody = JSON.parse(data.body);
                            fn_error(jsonbody.d);
                        }
                    });
            },
            function (err) {
                deferred.reject(err);
            }
        );
        return deferred.promise();
    }
};
$(document).ready(function () {
    funcSPO.initial();
});

function onError(data) {
    var test = data;
}

function getFileBuffer(file) {
    var deferred = $.Deferred();
    var reader = new FileReader();
    reader.onload = function (e) {
        deferred.resolve(e.target.result);
    }
    reader.onerror = function (e) {
        deferred.reject(e.target.error);
    }
    reader.readAsArrayBuffer(file);
    return deferred.promise();
}

// Get the list item that corresponds to the file by calling the file's ListItemAllFields property.
function getListItem(fileListItemUri) {
    // Construct the endpoint.
    // The list item URI uses the host web, but the cross-domain call is sent to the
    // add-in web and specifies the host web as the context site.
    fileListItemUri = fileListItemUri.replace(SPOAppParameters.SPHost, '{0}');
    fileListItemUri = fileListItemUri.replace('_api/Web', '_api/sp.appcontextsite(@target)/web');

    var listItemAllFieldsEndpoint = String.format(fileListItemUri + "?@target='{1}'", SPOAppParameters.SPAppWebUrl, SPOAppParameters.SPHost);

    // Send the request and return the response.
    return jQuery.ajax({
        url: listItemAllFieldsEndpoint,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

// Change the display name and title of the list item.
function updateListItem(itemMetadata) {
    // Construct the endpoint.
    // Specify the host web as the context site.
    var listItemUri = itemMetadata.uri.replace('_api/Web', '_api/sp.appcontextsite(@target)/web');
    var listItemEndpoint = String.format(listItemUri + "?@target='{0}'", SPOAppParameters.SPHost);

    // Define the list item changes. Use the FileLeafRef property to change the display name.
    // For simplicity, also use the name as the title.
    // The example gets the list item type from the item's metadata, but you can also get it from the
    // ListItemEntityTypeFullName property of the list.
    var body = String.format("{{'__metadata':{{'type':'{0}'}},'FileLeafRef':'{1}','Title':'{2}'}}",
        itemMetadata.type, "Hola Mundo", "Hola Mundo");

    // Send the request and return the promise.
    // This call does not return response content from the server.
    return jQuery.ajax({
        url: listItemEndpoint,
        type: "POST",
        data: body,
        headers: {
            "X-RequestDigest": jQuery("#__REQUESTDIGEST").val(),
            "content-type": "application/json;odata=verbose",
            "content-length": body.length,
            "IF-MATCH": itemMetadata.etag,
            "X-HTTP-Method": "MERGE"
        }
    });
}