
import EventEmmiter from 'events';
import Dispatcher from '../Dispatcher';
import SearchConstants from '../constants/SearchConstants';
import SearchStateConstants from '../constants/SearchStateConstants';

const SESSION_KEY = 'search_session';

class SearchStore extends EventEmmiter {
    constructor() {
        super();

        this.searchResults = [];
        this.status = SearchStateConstants.STOPED;
    }

    createOrUpdate(searchResult) {
        const index = this.searchResults.findIndex(x => x.url === searchResult.url);

        if (index > -1) {
            this.searchResults.splice(index, 1, searchResult);
        }
        else {
            this.searchResults.push(searchResult);
        }

        this.emitChange();
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

    handleActions(action) {
        switch (action.type) {
            case SearchConstants.FETCH_RESULTS:
                this.fetchSearchResults(action.searchResults);
                this.emitChange();
                break;
            case SearchConstants.SET_SESSION:
                this.sessionId = action.sessionId;
                break;
            case SearchConstants.SET_STATUS:
                this.searchStatus = action.searchStatus;
                break;
            case SearchConstants.CREATE_OR_UPDATE:
                this.createOrUpdate(action.searchResult);
                break;
            default:
                break;
        }
    }

    get searchStatus() {
        return this.status;
    }
    set searchStatus(value) {
        if (this.status === value) {
            return;
        }

        this.status = value;
        this.emitChange();
    }

    get sessionId() {
        return localStorage.getItem(SESSION_KEY);
    }
    set sessionId(value) {
        if (this.sessionId === value) {
            return;
        }

        localStorage.setItem(SESSION_KEY, value);

        this.emitChange();
    }
}

const searchStore = new SearchStore();

Dispatcher.register(searchStore.handleActions.bind(searchStore));

export default searchStore;