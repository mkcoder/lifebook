import React, { Component } from 'react';
import './App.css';

class App extends Component {
  constructor() {
    super();
    this.state = {
      apps: []
    };
  }

  componentDidMount() {
    fetch('https://localhost:4501/apploader_webapi/api/apps')
                .then((resp) => {
                  console.log(resp);
                  return resp.json();
                }).then(data => {
                  this.setState(Object.assign({}, this.state, {apps: data}));
                  console.log(data);
                })
                .catch(console.log);
    
  }

  loadScript(url, section, cb){
    var script = document.createElement("script"); 
    script.type = "text/javascript";
    script.src = url;
    script.async=false;
    script.defer=false;
    script.onload = cb();
    document.getElementById(section).appendChild(script);
  }

  createScript(text, section, cb){
    var script = document.createElement("script"); 
    script.type = "text/javascript";
    script.innerText = text;
    script.async=false;
    script.defer=false;
    return script;    
  }

  loadService(serviceName, name) {
    
    //document.getElementById("app").src = `https://localhost:4501/${name}/calendar`;
    fetch(`https://localhost:4501/${serviceName}/${name}`).then((resp) => {
      
      return resp.text();
    }).then(data => {
      document.getElementById("app2").innerHTML = data;
      // eslint-disable-next-line
      var regex = /<script.+?<\/script>/g;
      var found = data.match(regex);
      var cbs = []      
      var i = 0;
      found.forEach(a => {
        if(a.indexOf("src") !== -1)
        {
          let url = a.match(/src="(.+)"/)[1];
          this.loadScript(url, "app2", function () {
            console.log("loaded");
            i++;
            if(i===found.length)
            {
              cbs.forEach(cb => {
                document.getElementById("app2").appendChild(cb);
              });
            }
          })
        }
        else
        {
          console.log("don't touch");
          i++;
          // eslint-disable-next-line          
          cbs.push(this.createScript(a.match(/<script>(.+)<\/script>/)[1], "app2"));
        }
      });
    })
    .catch(console.log);
  }

  render() {
    return (
      <div> 
        {
          this.state.apps.map(a => {
            return (
              <div>
                <ul>
                  <li>{a.id}</li>
                  <li>{a.name}</li>
                  <li>{a.serviceName}</li>
                  <li onClick={() => this.loadService(a.serviceName, a.name)}><img src={"data:image/png;base64,"+a.icon} alt=""/></li>
                </ul>
              </div>
            );
          })
        }
      </div>
    );
  }
}

export default App;
