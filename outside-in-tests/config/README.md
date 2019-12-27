# Configuration File for Tests

Since the API under Test is secured and a valid token provided by Auth0 must be provided in the header of an HTTP request, we need to provide the necessary secrets. The test suit expects a file `config/config.js` to be present. Proceed as follows:

* rename the file `config/config.js.template` to `config/config.js`
* fill in values for `<authority>`, `<client_id>`, `<client_secret>` and `<audience>`.
* You can get those values from your Auth0 account where you have defined the API
