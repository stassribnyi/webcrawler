import Dispatcher from '../Dispatcher'

import SearchApi from '../apis/SearchApi';
import SearchConstants from '../constants/SearchConstants';
import SearchStateConstants from '../constants/SearchStateConstants';

export function pause(sessionId) {
    SearchApi.pause(sessionId).then(() => setStatus(SearchStateConstants.PAUSED))
}

export function resume(sessionId) {
    SearchApi.resume(sessionId).then(() => setStatus(SearchStateConstants.STARTED))
}

export function start(params) {
    SearchApi.start(params).then((response) => {
        Dispatcher.dispatch({
            type: SearchConstants.SET_SESSION,
            sessionId: response.data
        });

        Dispatcher.dispatch({
            type: SearchConstants.FETCH_RESULTS,
            searchResults: []
        });

        setStatus(SearchStateConstants.STARTED);
    });

}

export function stop(sessionId) {
    SearchApi.stop(sessionId).then(() => setStatus(SearchStateConstants.STOPED));
    ;
}

export function getStatus(sessionId) {
    SearchApi.getStatus(sessionId).then((response) => {
        Dispatcher.dispatch({
            type: SearchConstants.FETCH_RESULTS,
            searchResults: response.data
        });
    });
}

export function setStatus(status) {
    Dispatcher.dispatch({
        type: SearchConstants.SET_STATUS,
        searchStatus: status
    });
}

export function createOrUpdate(searchResult) {
    Dispatcher.dispatch({
        type: SearchConstants.CREATE_OR_UPDATE,
        searchResult: searchResult
    });
}