import {useView} from 'aurelia-framework';

@useView('views/group-table.html')
export class GroupTableCustomElement {

    students = [{
        firstName: "�����",
        lastName: "���",
        tests :[{
            id: 1212131313,
            result: 80
        },{
            id: 1212131314,
            result: 65
        },{
            id: 1212131353,
            result: 88
        },{
            id: 1212166613,
            result: 44
        },{
            id: 1212134113,
            result: 0
        }],
        exam: 0
    },{
        firstName: "�����2",
        lastName: "���2",
        tests :[{
            id: 1212131313,
            result: 75
        }, {
            id: 1212131314,
            result: 55
        }, {
            id: 1212131353,
            result: 77
        }, {
            id: 1212166613,
            result: 97
        }, {
            id: 1212134113,
            result: 66
        }],
        exam: 99
    },{
        firstName: "�����3",
        lastName: "���3",
        tests :[{
            id: 1212131313,
            result: 41
        }, {
            id: 1212131314,
            result: 65
        }, {
            id: 1212131353,
            result: 86
        }, {
            id: 1212166613,
            result: 0
        }, {
            id: 1212134113,
            result: 0
        }],
        exam: 0
    }]
}
