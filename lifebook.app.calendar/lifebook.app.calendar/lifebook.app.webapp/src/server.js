import express from 'express';
import morgan from 'morgan';
import ReactDOMServer from 'react-dom/server';
import App from './App';

const app = express();
app.use(morgan('dev'));
app.use('/calendar', express.static(ReactDOMServer.renderToString(App)));

app.use('/blue-buy', (req, res) => {
    res.send(App.renderToString());
});

app.listen(3001);
console.log(`🔵  team blue running. fragments are available here:
>> http://127.0.0.1:3001/blue-buy
>> http://127.0.0.1:3001/blue-basket`);



