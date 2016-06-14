import {useView} from 'aurelia-framework';

@useView('views/group.html')
export class Group {
    get heading() {
        return "Група";
    }
}
