import React, { Component } from 'react';

function statusToText(status) {
    switch (status) {
        case 0:
            return 'Error'
        case 1:
            return 'Found'
        case 2:
            return 'Loading'
        case 3:
            return 'Not Found'
        case 4:
            return 'Paused'
        case 5:
            return 'Pending'
        default:
            return '';
    }
}

function statusToClass(status) {
    switch (status) {
        case 0:
            return 'badge-danger'
        case 1:
            return 'badge-success'
        case 2:
            return 'badge-info'
        case 3:
            return 'badge-secondary'
        case 4:
            return 'badge-warning'
        case 5:
            return 'badge-info'
        default:
            return '';
    }
}

export default class SearchResults extends Component {
    constructor(props) {
        super(props);

        this.state = {
            selectedUrl: null
        };
    }

    handleShowDetails(url) {
        this.setState({
            selectedUrl: this.state.selectedUrl === url
                ? null
                : url
        });
    }

    render() {
        const tbodies = this.props.data.map((d) => (
            <tbody key={d.url}>
                <tr>
                    <td className="text-center">
                        <span className={`badge badge-pill status-badge ${statusToClass(d.status)}`}>
                            {statusToText(d.status)}
                        </span>
                    </td>
                    <td>
                        <a href={d.url}>{d.url}</a>
                    </td>
                    <td className="text-center"
                        onClick={() => this.handleShowDetails(d.url)}>
                        <a href="#">Show details</a>
                    </td>
                </tr>
                {d.url === this.state.selectedUrl
                    ? (<tr>
                        <td colSpan="3">
                            <div>{d.details}</div>
                        </td>
                    </tr>)
                    : null}
            </tbody>
        ));
        return (
            <table className="table table-striped table-sm">
                <thead>
                    <tr>
                        <th scope="col" className="text-center bt-none info-col">Status</th>
                        <th scope="col" className="text-center bt-none">Url</th>
                        <th scope="col" className="text-center bt-none info-col">Details</th>
                    </tr>
                </thead>
                {tbodies}
            </table>
        );
    }
}