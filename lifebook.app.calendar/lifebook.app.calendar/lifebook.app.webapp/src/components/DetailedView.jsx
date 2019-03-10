import React from 'react';

export default class DetailedView extends React.Component {
    RenderHeader(selectedDates) {
        var header = [];
        // timestamp column
        header.push((
            <th></th> 
        ));
        for (let start = selectedDates.start().getDate(); start <= selectedDates.end().getDate(); start++)
        {
            header.push((
                <th>{start}</th>
            ));
        }
        return header;
    }
    GetEventCols(selectedDates, time, intv) {
        // we will have to make an api call here
        var header = [];
        // timestamp column
        for (let start = selectedDates.start().getDate(); start <= selectedDates.end().getDate(); start++)
        {
            header.push((
                <td></td>
            ));
        }
        return header;
    }
    RenderBody(selectedDates, intervals) {
        debugger;
        var tr = [];
        var hoursDiv = 60/intervals;
        for(let i = 0, time = 12, intv=0; i < 24*hoursDiv; i++) {
            var ths = [];
            ths.push((
                <td>{time}:{intv < 9 ? "0"+intv : intv}</td>                
            ));            
            ths.push(this.GetEventCols(selectedDates, time, intv));
            tr.push((
                <tr>{ths}</tr>
            ))
            intv += intervals;
            if(intv === 60) {
                intv = 0;
                if(time===12) time = 1;
                else time++;
            }
        }
        return tr;
    }
    RenderDetailedView (selectedDates, intv) {
        console.log(selectedDates.start());
        console.log(selectedDates.end());
        return (
            <React.Fragment>
                <table>
                    <thead>
                        <tr>
                            {this.RenderHeader(selectedDates)}
                        </tr>
                    </thead>
                    <tbody>
                        {this.RenderBody(selectedDates, intv)}
                    </tbody>
                </table>
            </React.Fragment>
        );
    }
    render() {
        return (
            <React.Fragment>
                {this.RenderDetailedView(this.props.selectedDates, this.props.intervals)}
            </React.Fragment>
        );
    }
}