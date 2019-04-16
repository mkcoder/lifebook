import React, { Component } from 'react';
import logo from './logo.svg';
import consul from 'consul';
import './App.css';

class App extends Component {
  componentDidMount() {
    let _consul = new consul();
    _consul.agent.service.list(function(err, result) {
      if (err) throw err;
      console.log(result);
    });
  }

  render() {
    return (
      <ul>
        <li>Calendar app</li>
      </ul>
    );
  }
}

export default App;
