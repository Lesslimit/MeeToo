import {Router, RouterConfiguration} from 'aurelia-router'
import {useView} from 'aurelia-framework';

@useView('views/layout/app.html')
export class App {
    get showLayout() {
        return sessionStorage.getItem('MeeToo:isauthorized') === 'true';
    }
    router: Router;

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = 'MeeToo';

        config.map([
            { route: 'profile', name: 'profile', moduleId: 'viewmodels/profile', nav: true, title: '�������' },
            { route: ['', 'tests'], name: 'tests', moduleId: 'viewmodels/tests', nav: true, title: '�����' },
            { route: 'test/:id', moduleId: 'viewmodels/test', nav: false, href: 'test/:id' },
            { route: 'stats', name: 'stats', moduleId: 'viewmodels/stats', nav: true, title: '����������' },
            { route: 'group', name: 'group', moduleId: 'viewmodels/group', nav: true, title: '�����' },
            { route: 'login', name: 'login', moduleId: 'viewmodels/login', nav: false, title: '����' }
        ]);

        this.router = router;
    }

    attached() {
        if (sessionStorage.getItem('MeeToo:isauthorized') === 'false') {
            this.router.navigate('login');
        }
    }
}
