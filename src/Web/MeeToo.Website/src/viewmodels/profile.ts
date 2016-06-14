import {useView} from 'aurelia-framework';

@useView('views/profile.html')
export class Profile {
    get heading() {
        return "Профіль";
    }

    user = {
        firstName: "Roman",
        lastName: "Pan",
        middleName: "Pan",
        email: "panromanpan@gmail.com",
        group: "3CO-11"
    }
}
