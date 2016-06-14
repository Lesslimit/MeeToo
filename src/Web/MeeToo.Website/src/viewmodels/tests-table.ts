import {useView} from 'aurelia-framework';

@useView('views/tests-table.html')
export class TestsTableCustomElement {
    tests = [{
        id: "131233130",
        result: 80,
        isComplete: true,
    }, {
        id: "1313135461",
        result: 80,
        isComplete: true,
    }, {
        id: "1313555241",
        result: 80,
        isComplete: true,
    }, {
        id: "1313133134",
        result: 80,
        isComplete: true,
    }, {
        id: "13131444",
        result: 0,
        isComplete: false,
    }];

}
