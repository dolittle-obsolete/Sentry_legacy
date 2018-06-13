import environment from './environment';
import { PLATFORM } from 'aurelia-pal';
//import 'babel-polyfill';
import * as Bluebird from 'bluebird';

import oidcConfig from './OpenIDConnectConfiguration';

// remove out if you don't want a Promise polyfill (remove also from webpack.config.js)
Bluebird.config({ warnings: { wForgottenReturn: false } });

export function configure(aurelia) {
  aurelia.use
    .standardConfiguration();

  aurelia.use.plugin(PLATFORM.moduleName('aurelia-open-id-connect'), () => oidcConfig);

  if (environment.debug) {
    aurelia.use.developmentLogging();
  }
  aurelia.start().then(() => aurelia.setRoot(PLATFORM.moduleName('app')));
}
