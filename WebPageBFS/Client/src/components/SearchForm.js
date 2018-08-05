import React, { Component } from 'react';
import SearchStateConstants from '../constants/SearchStateConstants';

const SUBMIT_ACTIONS = {
    START: 'START',
    STOP: 'STOP',
    PAUSE: 'PAUSE'
}

export default class SearchForm extends Component {
    constructor(props) {
        super(props);

        this.state = {
            rootUrl: '',
            maxThreads: 1,
            maxUrls: 100,
            phrase: '',
            submitAction: SUBMIT_ACTIONS.START
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleStart = this.handleStart.bind(this);
        this.handleStop = this.handleStop.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handlePause = this.handlePause.bind(this);
    }

    handleStart() {
        this.setState({ submitAction: SUBMIT_ACTIONS.START });
    }

    handleChange(event) {
        const target = event.target;
        this.setState({
            [target.name]: target.value
        });
    }

    handleStop() {
        this.setState({ submitAction: SUBMIT_ACTIONS.STOP });
    }

    handleSubmit(event) {
        const result = event.target.checkValidity();
        event.preventDefault();

        if (!result) {
            return;
        }

        this.handleSubmitAction(this.state.submitAction);
    }

    handleSubmitAction(action) {
        switch (action) {
            case SUBMIT_ACTIONS.START:
                const {
                    rootUrl,
                    maxThreads,
                    maxUrls,
                    phrase
                } = this.state;

                this.props.onStart({
                    rootUrl,
                    maxThreads,
                    maxUrls,
                    phrase
                });
                break;
            case SUBMIT_ACTIONS.STOP:
                this.props.onStop();
                break;
            case SUBMIT_ACTIONS.PAUSE:
                this.props.onPause();
                break;
            default:
                break;
        }
    }

    handlePause() {
        this.setState({ submitAction: SUBMIT_ACTIONS.PAUSE });
    }

    render() {
        const startValue = this.props.searchStatus === SearchStateConstants.PAUSED ? 'Resume' : 'Start';
        const startPauseBtn = this.props.searchStatus === SearchStateConstants.STARTED
            ? <input type="submit" className="btn btn-warning" onClick={this.handlePause} value="Pause" />
            : <input type="submit" className="btn btn-primary" onClick={this.handleStart} value={startValue} />

        const isStopDisabled = this.props.searchStatus === SearchStateConstants.STOPED;

        return (
            <form onSubmit={this.handleSubmit}>
                <div className="form-group">
                    <label  htmlFor="rootUrl">Root url</label>
                    <input id="rootUrl"
                        name="rootUrl"
                        type="url"
                        className="form-control"
                        aria-describedby="rootUrlHelper"
                        placeholder="Enter root url."
                        required
                        onChange={this.handleChange} />
                    <small id="rootUrlHelper" className="form-text text-muted">This url will be used as root for search.</small>
                </div>
                <div className="form-group">
                    <label  htmlFor="phrase">Search phrase</label>
                    <input id="phrase"
                        name="phrase"
                        type="text"
                        className="form-control"
                        aria-describedby="phraseHelper"
                        placeholder="Search phrase"
                        required
                        onChange={this.handleChange} />
                    <small id="phraseHelper" className="form-text text-muted">This phrase will be used as main text to search for.</small>
                </div>
                <div className="form-group">
                    <label  htmlFor="maxUrls">Max urls</label>
                    <input id="maxUrls"
                        name="maxUrls"
                        type="number"
                        className="form-control"
                        aria-describedby="maxUrlsHelper"
                        placeholder="100"
                        required
                        min="1"
                        step="1"
                        pattern="\d+"
                        onChange={this.handleChange} />
                    <small id="maxUrlsHelper" className="form-text text-muted">Value to be used to reduce or increase amount of additional urls to be analyzed for phrase.</small>
                </div>
                <div className="form-group">
                    <label  htmlFor="maxThreads">Max threads</label>
                    <input id="maxThreads"
                        name="maxThreads"
                        type="number"
                        className="form-control"
                        aria-describedby="maxThreadsHelper"
                        placeholder="1"
                        required
                        min="1"
                        step="1"
                        pattern="\d+"
                        onChange={this.handleChange} />
                    <small id="maxThreadsHelper" className="form-text text-muted">Value to be used to reduce or increase amount of threads.</small>
                </div>
                <div className="float-right">
                    {startPauseBtn}
                    <input type="submit" className="btn btn-danger ml-2" onClick={this.handleStop} disabled={isStopDisabled} value="Stop" />
                </div>
            </form>
        );
    }
}