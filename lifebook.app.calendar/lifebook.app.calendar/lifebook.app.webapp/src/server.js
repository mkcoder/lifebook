import express from 'express';
import morgan from 'morgan';
import path from 'path';
import ReactDOMServer from 'react-dom/server';

const app = express();
const buildPath = path.join(__dirname, "../", "build");
app.use(morgan('dev'));

app.use(express.static(path.join(__dirname, "../", "build")))


app.get('/calendar', function(req, res) {
    console.log(path.join(buildPath, "index.html"))
    res.sendFile(path.join(buildPath, "index.html"));
});

app.get('/about', function (req, res) {
    res.send('about')
})

app.listen(3001);
console.log(`🔵  team blue running. fragments are available here:
>> http://127.0.0.1:3001/blue-buy
>> http://127.0.0.1:3001/blue-basket`);



