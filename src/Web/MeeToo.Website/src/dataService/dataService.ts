import {HttpClient, json} from 'aurelia-fetch-client';
import {autoinject} from 'aurelia-framework';

let httpClient = new HttpClient();

@autoinject()
export class dataService {
    static configure(baseUrl) {
        httpClient.configure(config => {
             config
                 .withBaseUrl(baseUrl)
                 .withDefaults({
                     credentials: 'same-origin',
                     headers: {
                         'Accept': 'application/json',
                         'X-Requested-With': 'Fetch'
                     }
                 })
                 .withInterceptor({
                     request(request) {
                         console.log(`Requesting ${request.method} ${request.url}`);
                         return request;
                     },
                     response(response) {
                         console.log(`Received ${response.status} ${response.url}`);
                         return response;
                     }
                 });
         });
     }

     static get students() {
        return {
            getAll() {
                return httpClient.fetch('students/getAll');
            },
            register(user) {
                return httpClient.fetch('oauth/register',
                {
                    method: 'post',
                    body: json(user)
                });
            }
        }
    }

     static get tests() {
        return {
            getAll() {
                return httpClient.fetch('tests/getAll');
            }
        }
    }
}
