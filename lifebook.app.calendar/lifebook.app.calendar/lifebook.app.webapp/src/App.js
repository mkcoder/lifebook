import React from 'react'; 
import Calendar from './components/Calendar'
import Reminders from './components/Reminders';
import DetailedView from './components/DetailedView'
import { Router, Route, Link } from 'react-router'
import './css/style.css';

export default class App extends React.Component {

    state = {
        selectedDate: new Date(),        
        intervals: 30,
        reminders: {},
        selectedDates: {
            start: () => {
                var date = this.state.selectedDate;
                if(date.getDate() === 1) return date;
                var offset = date.getDay()-this.state.userPreferences.startOfWeek;
                return new Date(date.getFullYear(), date.getMonth(), date.getDate()-offset);
            },
            end: () => {
                var date = this.state.selectedDate;
                var offset = 6-date.getDay()+this.state.userPreferences.startOfWeek;
                return new Date(date.getFullYear(), date.getMonth(), date.getDate()+offset);
            }
        },
        userPreferences: {
            startOfWeek: 0,
        }
    };

    constructor () {
        // load state for user
        var x = new URLSearchParams();
        console.log(x);
        super();
    };
    
    render () {
        return (
            <React.Fragment>
                <aside className="sidebar">
                    <Calendar className="calendar" date={this.state.selectedDate} selectedDates={this.state.selectedDates} />
                    <Reminders className="reminders" />
                </aside>
                <div className="details-view" >
                    <DetailedView date={this.state.selectedDate} intervals={this.state.intervals} selectedDates={this.state.selectedDates} />
                </div>
            </React.Fragment>
        );
    }
}