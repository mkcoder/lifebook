import React from 'react'; 

export default class Calendar extends React.Component {
    GetMonthNameFromInt(monthInt) {
        const monthNames = ["January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        ];
        
        return monthNames[monthInt]; 
    }
    RenderDaysOfTheWeekHeader() {
        var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
        var daysRender = [];
        for (var day of days)
        {
            daysRender.push((
                <th key={day}>{day}</th>
            ));
        }
        return daysRender;
    }
    RenderDayInWeek(start=0, end=6) {
        var week = [];
        for(var i = start; i <= end; i++){
            week.push((
                <td>{i}</td>
            ));
        }
        return week;
    }
    RenderWeeksOfTheWeekHeader(start, end) {
        let firstWeekOffset = start.getDay();
        let weeks = [];
        // draw the first week 
        var firstWeek = [];
        let j = 1;
        for (let i = 0; i <= 6; i++)
        {
            if(i < firstWeekOffset)
            {
                firstWeek.push(<td></td>);
            }
            else
            {
                firstWeek.push(<td>{j++}</td>);
            }
        }
        weeks.push((
            <tr>{firstWeek}</tr>
        ));
        for(let i = 7; i < end.getDate(); i+=6) {
            weeks.push((
                <tr>
                    {this.RenderDayInWeek(j, j+6)}
                </tr>
            ));
            j += 7;
        }
        firstWeek = [];
        for (let i = 0; i <= 6; i++)
        {
            if(j > end.getDate())
            {
                firstWeek.push(<td></td>);
            }
            else
            {
                firstWeek.push(<td>{j++}</td>);
            }
        }
        weeks.push((
            <tr>{firstWeek}</tr>
        ));
        // draw the last week
        return weeks;
    }

    RenderCalendar(date) {
        var firstDayOfMonth = new Date(date.getFullYear(), date.getMonth());
        var lastDayOfMonth = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        console.log(firstDayOfMonth, lastDayOfMonth);
        return (
            <table>
                <thead>
                    <tr>
                        {this.RenderDaysOfTheWeekHeader()}                        
                    </tr>
                </thead>
                <tbody>
                    {this.RenderWeeksOfTheWeekHeader(firstDayOfMonth, lastDayOfMonth)}
                </tbody>
            </table>
        ); 
    }
    render() {
        return (
            <React.Fragment>        
                {this.RenderCalendar(this.props.state.date)}
            </React.Fragment>
        );
    }
}