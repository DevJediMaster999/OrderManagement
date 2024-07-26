import axios from 'axios';

const instance = axios.create({
    baseURL: 'https://localhost:7160/api/'
});

export default instance;