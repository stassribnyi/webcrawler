import Dispatcher from '../Dispatcher'

import SearchApi from '../apis/SearchApi';
import SearchConstants from '../constants/SearchConstants';

export function pause(sessionId) {
    SearchApi.pause(sessionId);
}

export function resume(sessionId) {
    SearchApi.resume(sessionId);
}

export function start(params) {
    SearchApi.start(params).then((response) => {
        Dispatcher.dispatch({
            type: SearchConstants.SET_SESSION,
            sessionId: response.data
        });
    });
}

export function stop(sessionId) {
    SearchApi.stop(sessionId);
}

export function getStatus(sessionId) {
    SearchApi.getStatus(sessionId).then((response) => {
        Dispatcher.dispatch({
            type: SearchConstants.FETCH_RESULTS,
            searchResults: response.data
        });
    });
}