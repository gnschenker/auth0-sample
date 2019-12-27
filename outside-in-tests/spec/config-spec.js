describe('Suite testing the existence of the "config/config.js" file', () => {
    const fs = require("fs");
    var Config = require('config-js');
    var config = new Config('./config/config.js');

    it('should test that file "config/config.js" exists', () =>{
        expect(fs.existsSync('./config/config.js')).toBe(true);
    });
    it('should contain a value for "auth0.url"',() => {
        expect(config.get('auth0.url').endsWith('auth0.com/oauth/token')).toBe(true);
    })
});
