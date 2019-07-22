const path = require('path');
const dotenv = require('dotenv-webpack');
require('dotenv').config();
const http = require('http');
const bodyParser = require('body-parser');

process.env.DOLITTLE_WEBPACK_ROOT = path.resolve('.');
process.env.DOLITTLE_WEBPACK_OUT = path.resolve('./public');
process.env.DOLITTLE_FEATURES_DIR = path.resolve('./Features');
process.env.DOLITTLE_COMPONENT_DIR = path.resolve('./Components');

const config = require('@dolittle/build.aurelia/webpack.config.js');

// gateway:
// /something/auth

// https://dolittle.studio/.well-known/
// https://dolittle.studio/authenticate/

// https://dolittle.studio/sentry/login
// https://dolittle.studio/sentry/select_tenant
// https://dolittle.studio/sentry/app.asdasdbundle.js
// https://dolittle.studio/sentry/vendor.sdasdbundle.js

// https://edge.dolittle.studio/
// https://sentry.dolittle.studio/

// https://mz.cbs.org/analytics
// https://mz.cbs.org/reporting
// https://mz.cbs.org/ -> Default

// https://mz.cbs.org/sentry/accounts/tenantselector -> /sentry/8c828f1d-1303-4b20-afc3-f6afe4d3242c/index.js   - routing
// https://mz.cbs.org/sentry/accounts/tenantselector -> /sentry/8c828f1d-1303-4b20-afc3-f6afe4d3242c/index.html - routing
// https://mz.cbs.org/sentry/accounts/tenantselector -> /sentry/8c828f1d-1303-4b20-afc3-f6afe4d3242c/index.css  - routing


module.exports = (env) => {
    const obj = config.apply(null, arguments);
    obj.plugins.push(
        new dotenv({
            path: './Environments/' + env.DOLITTLE_ENVIRONMENT + '.env',
        })
    );
    obj.devServer = {
        historyApiFallback: true,
        proxy: {
        },
        port: 8081,
        before: function (app, server) {
            app.use(bodyParser.text());

            const forward = function(host, port, req, res, next) {
                const options = {
                    hostname: host,
                    port: port,
                    path: req.path,
                    method: req.method,
                    headers: req.headers,
                    cookies: req.cookies,
                    query: req.query,
                    search: req.search,
                    params: req.query
                };

                let segments = req.originalUrl.split('?');
                if( segments.length > 1 ) options.path += `?${segments[1]}`;

                let raw = '';

                req.on('data', chunk => {
                    raw += chunk;
                });

                req.on('end', () => {
                    console.log(`Forwarding to : ${host}:${port}${req.path}`);
                    const r = http.request(options, response => {
                        console.log(response.statusCode);
                        console.log(response.statusMessage);
                        res.status(response.statusCode);
                        res.set(response.headers);

                        response.on('data', chunk => {
                            res.write(chunk);
                        });

                        response.on('end', () => {
                            res.end();
                        });
                    });
                    r.end(raw);
                });
            };
            app.use((req, res, next) => {
                console.log(`Request : ${req.path} - ${req.method}`);

                const options = {
                    hostname: 'localhost',
                    port: 5050,
                    path: '/auth',
                    method: req.method,
                    headers: req.headers,
                    cookies: req.cookies
                };

                if (req.path.indexOf('/signin-oidc') == 0 ||
                    req.path.indexOf('/.well-known') == 0 ||
                    req.path.indexOf('/auth/signin') == 0 ||
                    req.path.indexOf('/connect') == 0 ||
                    req.path.indexOf('/device') == 0) {

                    console.log('Signing OIDC');
                    forward('localhost', 5050, req, res, next);
                } else if (req.path.indexOf('/sentry') == 0) {
                    console.log('Sentry request');
                    //req.path = req.path.substr('/authentication'.length);
                    forward('localhost', 8080, req, res, next);
                } else {

                    const r = http.request(options, response => {
                        console.log(response.statusCode);

                        if (response.statusCode == 200) {
                            if( response.headers.tenant ) {
                                req.headers.tenant = response.headers.tenant;
                                console.log(`Tenant : ${response.headers.tenant}`);
                            }
                            if( response.headers.correlation ) {
                                req.headers.correlation = response.headers.correlation;
                                console.log(`Correlation : ${response.headers.correlation}`);
                            }
                            if( response.headers.microservice ) {
                                req.headers.microservice = response.headers.microservice;
                                console.log(`Microservice : ${response.headers.microservice}`);
                            }

                            if (req.path.indexOf('/api') == 0) forward('localhost', 5002, req, res, next);
                            else forward('localhost', 8082, req, res, next);
                            //next();
                        } else {
                            console.log(response.headers);
                            res.status(response.statusCode);
                            res.set(response.headers);

                            response.on('data', chunk => {
                                res.write(chunk);
                            });

                            response.on('end', () => {
                                res.end();
                            });
                        }
                    });

                    r.end();
                }
            });
        }
    };
    obj.resolve.alias = {
        DolittleStyles: path.resolve(__dirname, './styles')
    };
    return obj;
};
