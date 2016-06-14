import {useView} from 'aurelia-framework';

@useView('views/test.html')
export class Test {
    test = {
        id: "13131444",
        result: 0,
        isComplete: false,
    };
}
