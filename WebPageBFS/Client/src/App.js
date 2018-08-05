import React, { Component } from 'react';
import * as SignalR from '@aspnet/signalr';

import './App.css';

import config from './assets/config';
import SearchStore from './stores/SearchStore';
import * as SearchActions from './actions/SearchActions';
import SearchStateConstants from './constants/SearchStateConstants';

import SearchForm from './components/SearchForm';
import SearchResults from './components/SearchResults';

const connection = new SignalR.HubConnectionBuilder()
    .withUrl(`${config.API_URL}/searchinformer`)
    .configureLogging(SignalR.LogLevel.Information)
    .build();

function startConnection() {
    connection.start()
        .then(() => {
            console.log('Hub connection started');
        })
        .catch(function (err) {
            console.log('Error while establishing connection');
        });

    connection.on('Changed', (sessionId, searchResult) => {
        if (SearchStore.sessionId === sessionId) {
            SearchActions.createOrUpdate(searchResult);
        }
    });

    connection.on('Stoped', (sessionId) => {
        if (SearchStore.sessionId === sessionId) {
            SearchActions.setStatus(SearchStateConstants.STOPED);
        }
    });

    connection.onclose(e => {
        console.log('Connection disconnected ' + e);
        connection.start()
            .then(() => {
                console.log('Connection started again!');
            })
            .catch(function (err) {
                console.log('Error while establishing connection');
            });
    });
}

function closeConnection() {
    connection.stop();
}

class App extends Component {
    constructor(props) {
        super(props);

        this.state = {
            searchStatus: SearchStore.searchStatus,
            searchResults: SearchStore.getAll()
        }

        this.updateState = this.updateState.bind(this);
        this.handlePause = this.handlePause.bind(this);
        this.handleStart = this.handleStart.bind(this);
        this.handleStop = this.handleStop.bind(this);
    }

    componentWillMount() {
        startConnection();

        SearchStore.on('change', this.updateState);
    }

    componentWillUnmount() {
        closeConnection();

        SearchStore.removeListener('change', this.updateState);
    }

    handlePause() {
        SearchActions.pause(this.state.sessionId);
    }

    handleStart(params) {
        if(this.state.searchStatus === SearchStateConstants.PAUSED) {
            SearchActions.resume(this.state.sessionId);
        }
        else {
            SearchActions.start(params);
        }
    }

    handleStop() {
        SearchActions.stop(this.state.sessionId);
    }

    render() {
        return (
            <div className="container mt-3">
                <div className="row">
                    <div className="col text-center">
                        <h1>Web crawler</h1>
                    </div>
                </div>
                <div className="row mt-3">
                    <div className="col-12 col-sm-12 col-lg-4 col-xl-4 mb-3">
                        <div className="card">
                            <div className="card-header text-center">
                                Search form
                             </div>
                            <div className="card-body">
                                <SearchForm searchStatus={this.state.searchStatus}
                                    onPause={this.handlePause}
                                    onStart={this.handleStart}
                                    onStop={this.handleStop}
                                />
                            </div>
                        </div>
                    </div>
                    <div className="col-12 col-sm-12 col-lg-8 col-xl-8 mb-3">
                        <div className="card">
                            <div className="card-header text-center">
                                Search results
                             </div>
                            <div className="card-body scroll-result p-0">
                                {
                                    this.state.searchResults && this.state.searchResults.length
                                        ? (<SearchResults data={this.state.searchResults} />)
                                        : (<h5 className="card-title text-center mt-3">No results</h5>)
                                }
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    updateState() {
        this.setState({
            searchResults: SearchStore.getAll(),
            sessionId: SearchStore.sessionId,
            searchStatus: SearchStore.searchStatus
        });
    }
}

export default App;
