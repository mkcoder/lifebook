import React from 'react';
import {render} from 'react-dom';
import ReactDOMServer from 'react-dom/server';
import App from './App'

class CalendarCustomHTMLElement extends HTMLElement {
    constructor() {
        super();
        this.innerHTML = ReactDOMServer.renderToString(<App />);
    }
}

window.customElements.define("app-calendar", CalendarCustomHTMLElement);
render(<App />, document.querySelector("#calendar"));