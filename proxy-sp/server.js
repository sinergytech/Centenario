// https://paulryan.com.au/2014/spo-remote-authentication-rest/
const user = process.argv.find( arg => arg.indexOf('_user_') === 0).replace('_user_', '');
const pass = process.argv.find( arg => arg.indexOf('_pass_') === 0).replace('_pass_', '');
const port = (process.argv.find( arg => arg.indexOf('_port_') === 0) || '').replace('_port_', '');
const endpoint = 'https://grupocentenario.sharepoint.com';
const endpointApi = `${endpoint}/sites/calculocomisiones/_api`;
const localPort = port && !isNaN(port) ? Number(port) : 3000;
const localHost = '127.0.0.1';
const localEndpoint = `http://${localHost}:${localPort}`;

if (!user || !pass){
    console.log('No se especificaron las credenciales.');
    return;
}


const chalk = require('chalk');
const request = require('postman-request');
// request.debug = true;
const domParser = new (require('xmldom')).DOMParser;
const fs = require('fs'); 

let resolveSharepointAccess = null;
let rejectSharepointAccess = null;
const ready = new Promise( (resolve, reject) => {
    resolveSharepointAccess = resolve;
    rejectSharepointAccess = reject;
});

let getSecurityToken = fs.readFileSync('getSecurityToken.xml', 'utf8');
getSecurityToken = getSecurityToken.replace('{user}', user)
                    .replace('{password}', pass)
                    .replace('{endpoint}', endpoint);

let securityToken = request({
    url: 'https://login.microsoftonline.com/extSTS.srf',
    method: 'POST',
    body: getSecurityToken,
    headers: {'Accept': "application/json; odata=verbose"}
}, (error, response, body) => {    
    let dom = domParser.parseFromString(body);
    let element = dom.getElementsByTagName('wsse:BinarySecurityToken')[0];
    
    if (!element){
        rejectSharepointAccess('Usuario, cotraseña o endpoint incorrectos.');
        return;
    }

    let token = element.firstChild.nodeValue;

    request.post({
        url: endpoint + '/_forms/default.aspx?wa=wsignin1.0',
        body: token,
        jar: true
    }, function (error, response) {
        if (!error){
            resolveSharepointAccess();
        }else{
            rejectSharepointAccess(error);
        }
        
    });

});

function getDigest(){
    return new Promise( (resolve, reject) => {
        request.post({
            url: `${endpointApi}/contextinfo`,
            jar: true,
            headers:{
                "accept":"application/json;odata=verbose",    
                "accept-language":"es-ES,es;q=0.9",
                "upgrade-insecure-requests":"1"
            }
        }, (error, response, body) => {        
            
            if (!error){
                resolve(body);
            }else{
                reject({error, response});
            }            
        })
    })    
}

function copyHeader(headerKey, origen, destino){
    header = Object.keys(origen).find( h => h.toLowerCase() === headerKey.toLowerCase());
    if (header){
        destino[header] = origen[header];
    }
}

ready.then( () => {    
    
    const express = require('express');
    const bodyParser = require('body-parser');    
    const cors = require('cors');    
    let app = express();

    const rawBodySaver = function (req, res, buf, encoding) {
        if (buf && buf.length) {
            req.rawBody = buf.toString(encoding || 'utf8');
        }
    }
   
    app.use(bodyParser.json({  }));
    app.use(bodyParser.urlencoded({ extended: true }));

    // parse application/json
    // app.use(cors());  
    app.use(function(req, res, next) {        
        res.header("Access-Control-Allow-Origin", req.headers.origin);        
        res.header("Access-Control-Allow-Credentials", true);
        res.header('Access-Control-Allow-Methods', 'GET,PUT,POST,DELETE,OPTIONS');
         //intercepts OPTIONS method
        if ('OPTIONS' === req.method) {            
            res.header('Access-Control-Allow-Headers', 'Content-Type, Authorization, Content-Length, X-Requested-With,x-requestdigest,x-http-method,if-match');
            res.sendStatus(200);
        }
        else {
            next();
        }
    });

    app.get('*', (req, res) => {
        console.log(chalk.green('GET: ' + endpointApi.replace(/[\/]?\_api/, '') + req.originalUrl));
        request.get({
            url: endpointApi.replace(/[\/]?\_api/, '') + req.originalUrl,
            jar: true,
            credentials: "include",
            headers: {
                "accept": req.headers.accept
            }
            // headers: req.headers
        }, (error, response, body) => {     
            if (response && response.headers)       {
                res.setHeader("content-type", response.headers["content-type"]);
            }
            console.log(chalk.gray(body));
            while (body && body.indexOf(endpoint) !== -1){
                body = body.replace(endpoint, localEndpoint);
            }
            res.send(body);
        });        
    });

    app.post('*', (req, res) => {        
        console.log(chalk.blue('POST: ' + req.originalUrl));
        console.log(chalk.yellow('POSTBODY: ' + JSON.stringify(req.body)));

        if (req.originalUrl.indexOf('AttachmentFiles') !== -1 || req.originalUrl.indexOf('files/') !== -1){
            res.status(200);
            res.send(req.body); 
            console.log('File updating skiped.');
            return;            
        }

        let headers = {
            "accept": req.headers.accept
        }
        copyHeader('Content-Type', req.headers, headers);
        copyHeader('Content-Length', req.headers, headers);
        copyHeader('X-HTTP-Method', req.headers, headers);
        copyHeader('X-RequestDigest', req.headers, headers);
        copyHeader('IF-MATCH', req.headers, headers);
        copyHeader('Cache-Control', req.headers, headers);

        request.post({
            url: endpointApi + req.originalUrl.replace(/[\/]?\_api/, ''),
            jar: true,
            credentials: "include",
            headers: headers,
            body: JSON.stringify(req.body)
        }, (error, response, body) => {       
            if (response.headers["content-type"]){
                res.setHeader("content-type", response.headers["content-type"]);
            }
            console.log(chalk.gray(body));     
            while (body.indexOf(endpoint) !== -1){
                body = body.replace(endpoint, localEndpoint);
            }
            res.send(body);
        });
    });

    let server = app.listen(localPort, localHost, () => {
        let host = server.address().address;
        let port = server.address().port;

        // console.log('Conexion realizada con el usuario: %s', user);

        console.log("La página está escuchando en http://%s:%s", host, port);
    });

}).catch( e =>{
    console.log('No se pudo iniciar el servidor.', e);
});
