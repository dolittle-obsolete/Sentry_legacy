const path = require('path');
const dotenv = require('dotenv-webpack');
require('dotenv').config();

process.env.DOLITTLE_WEBPACK_ROOT = path.resolve('.');
process.env.DOLITTLE_WEBPACK_OUT = path.resolve('./public');
process.env.DOLITTLE_FEATURES_DIR = path.resolve('./Features');
process.env.DOLITTLE_COMPONENT_DIR = path.resolve('./Components');

const config = require('@dolittle/build.aurelia/webpack.config.js');

module.exports = (env) => {
    const obj = config.apply(null, arguments);
    obj.plugins.push(
        new dotenv({
            path: './Environments/' + env.DOLITTLE_ENVIRONMENT + '.env',
        })
    );
    obj.devServer = {
        historyApiFallback: true,
        port: 8080,
        proxy: {
            '/api': 'http://localhost:5000'
        }
    };
    obj.resolve.alias = {
        DolittleStyles: path.resolve(__dirname, './styles')
    };
    return obj;
};
