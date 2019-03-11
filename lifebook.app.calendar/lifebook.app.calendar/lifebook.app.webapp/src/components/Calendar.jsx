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
    RenderWeeksOfTheWeekHeader(start, end, selectedDate) {
        // refactoring
        var calendarDayCount = 1;
        let firstWeekOffset = start.getDay();
        let weeks = [];
        let week = [];
        let selected = false;
        for(var i = 1; calendarDayCount <= end.getDate(); i++)
        {
            if(i < firstWeekOffset+1)
            {                
                week.push(<td key={i}></td>);
            }
            else
            {
                var td = <td key={i}>{calendarDayCount}</td>;
                if(calendarDayCount === selectedDate.getDate())
                {                   
                    selected = true; 
                    td = <td className="selected-date" key={i}>{calendarDayCount}</td>;
                }                
                week.push(td);
                calendarDayCount++;
            }

            if(i%7===0) // end of the week
            {
                var tr = <tr key="tr">{week}</tr>;
                if(selected)
                {
                    tr = <tr className="selected-week" key="tr">{week}</tr>;
                    selected = false;
                }
                weeks.push(tr);
                week = [];
            }
        }
        for(let i = 0; i < 6-end.getDay(); i++) {
            week.push(<td key={i}></td>);
        }

        weeks.push((
            <tr key="tr">{week}</tr>
        ));
        return weeks;
    }

    RenderCalendar(date) {
        var firstDayOfMonth = new Date(date.getFullYear(), date.getMonth());
        var lastDayOfMonth = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        console.log(firstDayOfMonth, lastDayOfMonth);
        return (
            <table>
                <caption>{this.GetMonthNameFromInt(firstDayOfMonth.getMonth())} - {date.getFullYear()}</caption>
                <thead>
                    <tr>
                        {this.RenderDaysOfTheWeekHeader()}                        
                    </tr>
                </thead>
                <tbody>
                    {this.RenderWeeksOfTheWeekHeader(firstDayOfMonth, lastDayOfMonth, date)}
                </tbody>
            </table>
        ); 
    }

    render() {
        return (
            <React.Fragment>        
                {this.RenderCalendar(this.props.date)}
            </React.Fragment>
        );
    }
}