import { PLATFORM } from 'aurelia-pal';
import style from '../styles/style.scss';

export class app {
    constructor() {}

    configureRouter(config, router) {
        config.options.pushState = true;
        config.map([
            { route: [''], name: 'Index', moduleId: PLATFORM.moduleName('index') }
        ]);

        this.router = router;
    }
}
