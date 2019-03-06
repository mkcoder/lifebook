import React from 'react'; 
import Calendar from './components/Calendar'

export default class App extends React.Component {

    state = {
        date: new Date(),
        intervals: 30,
        reminders: {}        
    };

    render() {
        return (
            <React.Fragment>
                <Calendar state={this.state}/>
            </React.Fragment>
        );
    }
}