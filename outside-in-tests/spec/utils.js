const axios = require('axios');
var Config = require('config-js');
var config = new Config('./config/config.js');

module.exports = {
    getToken: async () => {
        const url = config.get("auth0.url");
        const body = {
            client_id: config.get("auth0.client_id"),
            client_secret: config.get("auth0.client_secret"),
            audience: config.get("auth0.audience"),
            grant_type: "client_credentials"
        };
        const response = await axios.post(url, body);
        token = response.data.access_token;
        return token;
    }
}