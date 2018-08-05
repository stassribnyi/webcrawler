import axios from 'axios';

import config from '../assets/config';

const instance = axios.create({
    baseURL: `${config.API_URL}/api`
});

class SearchApi {
    getStatus(sessionId) {
        return instance.get(`/search/status/${sessionId}`)
    }

    start(params) {
        return instance.post(`/search/start`, params);
    }

    pause(sessionId) {
        return instance.post(`/search/pause/${sessionId}`);
    }

    resume(sessionId) {
        return instance.post(`/search/resume/${sessionId}`);
    }

    stop(sessionId) {
        return instance.post(`/search/stop/${sessionId}`);
    }
}

export default new SearchApi();