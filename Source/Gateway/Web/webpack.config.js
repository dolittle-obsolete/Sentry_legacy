const path = require('path');
const dotenv = require('dotenv-webpack');
require('dotenv').config();

process.env.DOLITTLE_WEBPACK_ROOT = path.resolve('.');
process.env.DOLITTLE_WEBPACK_OUT = path.resolve('./public');
process.env.DOLITTLE_FEATURES_DIR = path.resolve('./Features');
process.env.DOLITTLE_COMPONENT_DIR = path.resolve('./Components');
process.env.DOLITTLE_WEBPACK_BASE_URL = '/sentry/';

const config = require('@dolittle/build.aurelia/webpack.config.js');

module.exports = (env) => {
    const obj = config.apply(null, arguments);
    obj.plugins.push(
        new dotenv({
            path: './Environments/' + env.DOLITTLE_ENVIRONMENT + '.env',
        })
    );
    obj.devServer = {
        //contentBase: obj.output.path,
        historyApiFallback: {
            index: `${process.env.DOLITTLE_WEBPACK_BASE_URL}/index.html`
        },
        port: 8080,
        proxy: {
            '/.well-known': 'http://localhost:5050',
            '/connect': 'http://localhost:5050',
            '/auth': 'http://localhost:5050',
            '/device': 'http://localhost:5050',
            '/signin': 'http://localhost:5050',
            '/signin-oidc': 'http://localhost:5050'
        }
    };
    obj.resolve.alias = {
        DolittleStyles: path.resolve(__dirname, './styles')
    };
    return obj;
};
