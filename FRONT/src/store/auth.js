import config from "../config";
import axios from "axios";
import jwt from "jsonwebtoken";
import router from "../Routes";
import ApiService from "@/services/api.service.js";

export default {
  namespaced: true,
  state: {
    isFetching: false,
    errorMessage: "",
  },
  mutations: {
    LOGIN_FAILURE(state, payload) {
      state.isFetching = false;
      state.errorMessage = payload;
    },
    LOGIN_SUCCESS(state) {
      state.isFetching = false;
      state.errorMessage = "";
    },
    LOGIN_REQUEST(state) {
      state.isFetching = true;
    },
  },
  actions: {
    loginUser({ dispatch }, creds) {
      dispatch("requestLogin");

      axios
        .post("/auth/token?user=" + creds.usuario + "&password=" + creds.senha)
        .then((res) => {
          const token = res.data.toString();
          dispatch("receiveToken", token);
        })
        .catch((err) => {
          dispatch(
            "loginError",
            "Erro ao efetuar login. Verifique os dados informados"
          );
        });
    },
    receiveToken({ dispatch }, token) {
      let user = {};

      user = { id: jwt.decode(token).Id, nome: jwt.decode(token).Nome };

      localStorage.setItem("token", token);
      localStorage.setItem("user", JSON.stringify(user));
      axios.defaults.headers.common["Authorization"] = "Bearer " + token;

      ApiService.get("Usuario", jwt.decode(token).Id, (result) => {
        if (result.status == 200) {
          localStorage.setItem("usuarioDTO", JSON.stringify(result.data));
          dispatch("receiveLogin");
        } else dispatch("receiveLogin");
      });
    },
    logoutUser() {
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      localStorage.removeItem("usuarioDTO");
      document.cookie = "token=;expires=Thu, 01 Jan 1970 00:00:01 GMT;";
      axios.defaults.headers.common["Authorization"] = "";
      router.push("/login");
    },
    loginError({ commit }, payload) {
      commit("LOGIN_FAILURE", payload);
    },
    receiveLogin({ commit }) {
      commit("LOGIN_SUCCESS");
      router.push("/app/dashboard");
    },
    requestLogin({ commit }) {
      commit("LOGIN_REQUEST");
    },
  },
};
