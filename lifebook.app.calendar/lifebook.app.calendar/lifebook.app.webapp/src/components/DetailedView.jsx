import React from 'react';

export default class DetailedView extends React.Component {
    RenderHeader() {
        var header = [];
        return header;
    }
    RenderDetailedView (date, intv, cols, selectedDates) {
        console.log(selectedDates.start());
        return (<p>test</p>);
    }
    render() {
        return (
            <React.Fragment>
                {this.RenderDetailedView(this.props.date, this.props.intervals, 7, this.props.selectedDates)}
            </React.Fragment>
        );
    }
}