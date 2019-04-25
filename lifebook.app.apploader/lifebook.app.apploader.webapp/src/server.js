import express from 'express';
import morgan from 'morgan';
import path from 'path';
import consul from 'consul';

var cors = require('cors');

const app = express();
const buildPath = path.join(__dirname, "../", "build");
app.use(cors());
app.use(morgan('dev'));
app.use(express.static(path.join(__dirname, "../", "build")))

app.get('/', function(req, res) {
    console.log(path.join(buildPath, "index.html"))
    res.sendFile(path.join(buildPath, "index.html"));
});

var server = app.listen(3009, function () {
    var host = server.address().address === '::' ? "localhost" : server.address().address;
    var port = server.address().port
    var c = consul();
    debugger;
    console.log(server.address());
    c.agent.service.register({
        name: "apploader",
        id: "primary.apploader",
        address: host,
        port: port
    }, cb => {
        console.log(cb);
    });
});

console.log("apploader is runing on server");


