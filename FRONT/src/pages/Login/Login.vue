<template>
  <div class="auth-page">
    <b-container>
      <Widget
        class="widget-auth mx-auto"
        customHeader
        style="background-color: white"
      >
        <p class="widget-auth-info">
          <img
            class="img-fluid"
            src="../../assets/logo-wh.png"
            alt="logo"
            width="150px"
          />
        </p>
        <form class="mt" @submit.prevent="login">
          <b-alert class="alert-sm" variant="danger" :show="!!errorMessage">
            {{ errorMessage }}
          </b-alert>
          <b-form-group
            label="Usuário"
            label-for="usuario"
            style="color: #000000"
          >
            <b-input-group>
              <b-input-group-text
                style="background-color: #000000"
                slot="prepend"
                ><i class="la la-user text-white"></i
              ></b-input-group-text>
              <input
                id="usuario"
                ref="usuario"
                class="form-control input-transparent pl-4"
                type="text"
                required
                placeholder="Usuário"
                style="
                  color: #000000;
                  background-color: transparent;
                  border: none;
                  border-bottom: #000000 1px solid;
                  border-width: 1px;
                  place-content: white; ;
                "
              />
            </b-input-group>
          </b-form-group>

          <br />

          <b-form-group label="Senha" label-for="senha" style="color: #000000">
            <b-input-group>
              <b-input-group-text
                style="background-color: #000000"
                slot="prepend"
                ><i class="la la-user text-white"></i
              ></b-input-group-text>
              <input
                id="senha"
                ref="senha"
                class="form-control input-transparent pl-4"
                type="password"
                required
                placeholder="Senha"
                style="
                  color: #000000;
                  background-color: transparent;
                  border: none;
                  border-bottom: #000000 1px solid;
                  border-width: 1px;
                "
              />
            </b-input-group>
          </b-form-group>
          <div class="auth-widget-footer" style="text-align: right">
                <b-button
                  type="submit"
                  variant=""
                  class="auth-btn"
                  style="
                    background-color: #fed400;
                    border-color: #000;
                    color: #000;
                    font-weight: bold;
                  "
                >
                  {{ this.isFetching ? "Loading..." : "Acessar" }}
                </b-button>
                <p class="widget-auth-info mt-4"></p>
              </div>
        </form>
      </Widget>
      
    </b-container>
    <footer class="auth-footer" style="color: black">
      WH Engenharia - Desenvolvido por Solinski Corp
    </footer>
  </div>
</template>

<script>
import Widget from "@/components/Widget/Widget";
import { mapState, mapActions } from "vuex";
import NavLink from "../../components/Sidebar/NavLink/NavLink";
import { sha256 } from "js-sha256";

export default {
  name: "LoginPage",
  components: { NavLink, Widget },
  computed: {
    ...mapState("auth", {
      isFetching: (state) => state.isFetching,
      errorMessage: (state) => state.errorMessage,
    }),
  },
  methods: {
    ...mapActions("auth", ["loginUser", "receiveToken", "receiveLogin"]),
    login() {
      const usuario = this.$refs.usuario.value;
      const senha = sha256(String(this.$refs.senha.value));

      if (usuario.length !== 0 && senha.length !== 0) {
        this.loginUser({ usuario, senha });
      }
    },
  },
  created() {
    const token = this.$route.query.token;

    if (token) {
      this.receiveToken(token);
    } else if (this.isAuthenticated(localStorage.getItem("token"))) {
      this.receiveLogin();
    }
  },
  mounted() {},
};
</script>
