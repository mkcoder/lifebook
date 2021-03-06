import express from 'express';
import morgan from 'morgan';
import path from 'path';
import ReactDOMServer from 'react-dom/server';
import consul from 'consul';

var cors = require('cors');

const app = express();
const buildPath = path.join(__dirname, "../", "build");
app.use(cors());
app.use(morgan('dev'));


app.use(express.static(path.join(__dirname, "../", "build")))

app.use(function (err, req, res, next) {
    res.status(500).send("wait a");
});

app.get('/calendar', function(req, res) {
    console.log(path.join(buildPath, "index.html"))
    res.sendFile(path.join(buildPath, "index.html"));
});

app.get('/about', function (req, res) {
    res.send('about')
})

app.get('/', function (req, res) {
    res.send('hello')
})

var server = app.listen(3331, function () {
    var host = server.address().address === '::' ? "localhost" : server.address().address;
    var port = server.address().port
    var c = consul();
    debugger;
    console.log(server.address());
    c.agent.service.deregister("primary.calendar_ui", () => {});
    c.agent.service.deregister("calendar_ui", () => {});
    c.agent.service.deregister("calendar_ui_123345", () => {});
    c.agent.service.register({
        name: "calendar_ui",
        id: "primary.calendar_ui",
        address: host,
        port: port
    }, cb => {
        console.log(cb);
    });
});

console.log(`🔵  team blue running. fragments are available here:
>> http://127.0.0.1:3001/blue-buy
>> http://127.0.0.1:3001/blue-basket`);



