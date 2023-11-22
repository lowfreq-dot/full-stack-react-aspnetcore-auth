import $api from "../http/index.js";

export default class UserService {
    static fetchUsers() {
        return $api.get('/users');
    }
}