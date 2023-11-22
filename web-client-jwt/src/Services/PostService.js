import $api from "../http/index.js";

export default class PostService {
    static fetchPosts() {
        return $api.get('/posts');
    }

    static createPost(title, body) {
        return $api.post('/posts', {title, body});
    }
}