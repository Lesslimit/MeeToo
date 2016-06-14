import 'bootstrap';
import {Aurelia} from 'aurelia-framework';
import {dataService} from "dataService/dataService";

export function configure(aurelia: Aurelia) {
    aurelia.use
        .standardConfiguration()
        .developmentLogging();

    dataService.configure('/api/v1.1');
    aurelia.use.plugin('aurelia-animator-css');

    //Anyone wanting to use HTMLImports to load views, will need to install the following plugin.
    //aurelia.use.plugin('aurelia-html-import-template-loader')

    aurelia.start().then(() => aurelia.setRoot());
}
