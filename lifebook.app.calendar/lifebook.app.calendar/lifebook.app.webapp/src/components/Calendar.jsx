import React from 'react'; 

export default class Calendar extends React.Component {
    constructor() {
        super();

        this.state = {
            month: 0,
            year: 0
        }
    }

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

    RenderCalendar(date) {        
        var firstDayOfMonth = new Date(date.getFullYear()+this.state.year, date.getMonth()+this.state.month);
        var days = this.getDaysOfTheCalendar(new Date(new Date().getFullYear()+this.state.year, new Date().getMonth()+this.state.month));
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

        return (
            <table>
                <caption>
                <button onClick={() => this.removeMonth()}>-</button>
                    {this.GetMonthNameFromInt(firstDayOfMonth.getMonth())} - {firstDayOfMonth.getFullYear()}
                    <button onClick={() => this.addMonth()}>+</button> 
                </caption>
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

    getDaysOfTheCalendar(date) {
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
          date = date || new Date(new Date().getFullYear(), new Date().getMonth());
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

    addMonth () {
        var thisMonth = new Date().getMonth();
        let month = this.state.month;
        let year = 0;
        if(thisMonth+1 > 12)
        {
            month = 0;
            year = 1;
        }
        else
        {
            month++;            
        }
        this.setState({month: month, year: year});
    }

    removeMonth() {
        let month = this.state.month;
        this.setState({month: --month});
    }

    render() {
        return (
            <div className="calendar">                       
                {this.RenderCalendar(this.props.date)}
            </div>
        );
    }
}