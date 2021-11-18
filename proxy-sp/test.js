const express = require('express');
let app = express();
let server = app.listen(3000, "localhost", () => {
  console.log(server.address());
  let host = server.address().address;
  let port = server.address().port;

  // console.log('Conexion realizada con el usuario: %s', user);

  console.log("La página está escuchando en http://%s:%s", host, port);
});