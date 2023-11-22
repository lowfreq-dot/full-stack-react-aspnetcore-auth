import {makeAutoObservable} from "mobx";
import AuthService from "../Services/AuthService.js";
import axios from "axios";
import {API_URL} from "../http/index.js";

export default class Store {
    user = {};
    isAuth = false;
    isLoading = false;

    constructor() {
        makeAutoObservable(this);
    }

    setAuth(bool) {
        this.isAuth = bool;
    }

    setUser(user) {
        this.user = user;
    }

    setLoading(bool) {
        this.isLoading = bool;
    }

    async login(email, password) {
        this.setLoading(true)
        try {
            const response = await AuthService.login(email, password);
            console.log(response);
            localStorage.setItem('token', response.data.accessToken);
            this.setAuth(true);
            this.setUser(response.data.email);
        } catch (e) {
            console.log(e.response?.data?.message);
            alert("Неправильный логин или пароль!");
        } finally {
            this.setLoading(false)
        }
    }

    async registration(email, password) {
        try {
            const response = await AuthService.registration(email, password);
            console.log(response);
            alert("Успешно зарегестрирован!")
        } catch (e) {
            console.log(e);
            if(e.response.status == 409) { alert(e.response?.data);}
            else alert("Что-то пошло не так, провертье введенные данные!");

        }
    }

    async logout() {
        try {
            const response = await AuthService.logout();
            console.log(response);
            localStorage.removeItem('token');
            this.setAuth(false);
            this.setUser({});
        } catch (e) {
            //console.log(e.response?.data?.message);
            console.log('Какая то хуита ', e)
            alert('Какая то хуита :/')
        }
    }

    async checkAuth() {
        this.setLoading(true);
        try {
            const response = await axios.get(`${API_URL}/refresh`, {withCredentials: true})
            console.log(response);
            localStorage.setItem('token', response.data.accessToken);
            this.setAuth(true);
            this.setUser(response.data.email);
        } catch (e) {
            console.log(e.response?.data?.message)
        } finally {
            this.setLoading(false);
        }
    }

}