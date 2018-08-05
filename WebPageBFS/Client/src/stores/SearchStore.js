
import EventEmmiter from 'events';
import Dispatcher from '../Dispatcher';
import SearchConstants from '../constants/SearchConstants';

const SESSION_KEY = 'search_session';

class SearchStore extends EventEmmiter {
    constructor() {
        super();

        this.searchResults = [];
    }

    emitChange() {
        this.emit('change');
    }

    fetchSearchResults(searchResults) {
        this.searchResults = [...searchResults];

        this.emitChange();
    }

    getAll() {
        return this.searchResults;
    }

    getSessionId() {
        return this.sessionId;
    }

    handleActions(action) {
        switch (action.type) {
            case SearchConstants.FETCH_RESULTS:
                this.fetchSearchResults(action.searchResults);
                break;
            case SearchConstants.SET_SESSION:
                this.sessionId = action.sessionId;
                break;
            default:
                break;
        }
    }

    get sessionId () {
        localStorage.getItem(SESSION_KEY);
    }
    set sessionId (value) {  
        if(this.sessionId === value) {
            return;
        }
        
        localStorage.setItem(SESSION_KEY, value);

        this.emitChange();
    }
}

const searchStore = new SearchStore();

Dispatcher.register(searchStore.handleActions.bind(searchStore));

export default searchStore;