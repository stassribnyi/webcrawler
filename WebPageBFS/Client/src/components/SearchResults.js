import React from 'react';

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

export default function SearchResults(props) {
    const trs = props.data.map((d, i) => (
        <tr>
            <td>
                <span className={`badge badge-pill status-badge ${statusToClass(d.status)}`}>
                {statusToText(d.status)}
                </span>
            </td>
            <td>{d.url}</td>
            <td>{d.details}</td>
        </tr>
    ));
    return (
        <table className="table table-striped table-sm">
            <thead>
                <tr>
                    <th scope="col" className="bt-none status-col">Status</th>
                    <th scope="col" className="bt-none">Url</th>
                    <th scope="col" className="bt-none">Details</th>
                </tr>
            </thead>
            <tbody>
                {trs}
            </tbody>
        </table>
    );
}