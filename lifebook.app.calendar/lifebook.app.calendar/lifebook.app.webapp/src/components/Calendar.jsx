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
                    tr = <tr className="selected-week" key={"tr"-i}>{week}</tr>;
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
        var days = this.getDaysOfTheCalendar();
        var trs = [];
        var tds = [];
        for(var i = 0; i < days.length; i++) {
            if(i!==0 && i%7===0)
            {
                trs.push((
                    <tr>{tds}</tr>
                ));
                tds = [];
                tds.push((<td>{days[i]}</td>));
            }
            else
            {
                tds.push((<td>{days[i]}</td>));
            }
        }
        trs.push((
            <tr>{tds}</tr>
        ));
        tds = [];
        console.log(tds);
        console.log(trs);
        return (
            <table>
                <caption>{this.GetMonthNameFromInt(firstDayOfMonth.getMonth())} - {date.getFullYear()}</caption>
                <thead>
                    <tr>
                        {this.RenderDaysOfTheWeekHeader()}                        
                    </tr>
                </thead>
                <tbody>
                    {trs}
                </tbody>
            </table>
        ); 
    }
    getDaysOfTheCalendar() {
        var range = function (start, edge, step) {
            // If only 1 number passed make it the edge and 0 the start
            if (arguments.length === 1) {
              edge = start;
              start = 0;
            }
          
            // Validate edge/start
            edge = edge || 0;
            step = step || 1;
          
            // Create array of numbers, stopping before the edge
            let arr = [];
            for (arr; (edge - start) * step > 0; start += step) {
              arr.push(start);
            }
            return arr;
          };
          //const weekdays = new Array("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday");
          const startOfWeek = 0; // SUNDAY
          const endOfWeek = 6; // SATURDAY
          const offset = 0;
          const date = new Date(new Date().getFullYear(), new Date().getMonth());
          const endOfMonth = new Date(date.getFullYear(), date.getMonth()+1, 0);
          var bDays = []
          if(date.getDay()-startOfWeek!==0)
          {
              const last = new Date(date.getFullYear(), date.getMonth(), 0);
              const beginningLeftOverDays = new Date(last.getFullYear(), last.getMonth(), last.getDate()-(last.getDay()-(startOfWeek+offset)));
              bDays = range(beginningLeftOverDays.getDate(), last.getDate()+1);
          }
          const endingLastFewDays = new Date(endOfMonth.getFullYear(), endOfMonth.getMonth()+1, (endOfWeek+offset)-endOfMonth.getDay());
          const days = [...bDays, ...range(1, endOfMonth.getDate()+1), ...range(1, endingLastFewDays.getDate()+1)]
          return days;
    }
    render() {
        console.log(this.getDaysOfTheCalendar())
        return (
            <div className="calendar">        
                {this.RenderCalendar(this.props.date)}
            </div>
        );
    }
}