import {useView} from 'aurelia-framework';

@useView('views/stats.html')
export class Stats {
    get heading() {
        return "Статистика";
    }
}
