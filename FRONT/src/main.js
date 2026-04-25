// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from "vue";
import BootstrapVue from "bootstrap-vue";
import * as VueGoogleMaps from "vue2-google-maps";
import VueTouch from "vue-touch";
import Trend from "vuetrend";
import { ClientTable } from "vue-tables-2";
import VueTextareaAutosize from "vue-textarea-autosize";
import mavonEditor from "mavon-editor";
import { VueMaskDirective } from "v-mask";
import VeeValidate from "vee-validate";
import VueFormWizard from "vue-form-wizard";
import axios from "axios";
import Toasted from "vue-toasted";
import VCalendar from "v-calendar";
import VueApexCharts from "vue-apexcharts";
import CKEditor from "@ckeditor/ckeditor5-vue";
import bFormSlider from "vue-bootstrap-slider";
import { component as VueCodeHighlight } from "vue-code-highlight";

import store from "./store";
import router from "./Routes";
import App from "./App";
import layoutMixin from "./mixins/layout";
import { AuthMixin } from "./mixins/auth";
import config from "./config";
import Widget from "./components/Widget/Widget";
import Scrollspy from "./documentation/pages/ScrollSpyComponent";
import money from "v-money";

// Fix global para o popup do DatePickerMask escapar do overflow das
// tabelas. Ver core/datepicker-escape.js para detalhes.
import "./core/datepicker-escape";

// Fix global para o popup do vue-multiselect escapar do overflow das
// tabelas. Ver core/multiselect-escape.js para detalhes.
import "./core/multiselect-escape";

axios.defaults.baseURL = config.baseURLApi;
axios.defaults.headers.common["Content-Type"] = "application/json";
axios.defaults.timeout = 180000;
const token = localStorage.getItem("token");
if (token) {
  axios.defaults.headers.common["Authorization"] = "Bearer " + token;
}

axios.interceptors.response.use(
  function(response) {
    return response;
  },
  function(error) {
    if (error.response.status === 401) {
      
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      document.cookie = "token=;expires=Thu, 01 Jan 1970 00:00:01 GMT;";
      axios.defaults.headers.common["Authorization"] = "";
      router.push("/login");
    }
    else
    {
      return error.response;
    }
  }
);

Vue.use(BootstrapVue);
Vue.use(VCalendar, {
  firstDayOfWeek: 2,
});
Vue.use(VueTouch);
Vue.use(Trend);
Vue.component("vue-code-highlight", VueCodeHighlight);
Vue.component("Widget", Widget);
Vue.component("Scrollspy", Scrollspy);
Vue.use(bFormSlider);
Vue.use(VueGoogleMaps, {
  load: {
    key: "AIzaSyB7OXmzfQYua_1LEhRdqsoYzyJOPh9hGLg",
  },
});
Vue.use(ClientTable, { theme: "bootstrap4" });
Vue.use(VueTextareaAutosize);
Vue.use(CKEditor);
Vue.use(mavonEditor);
Vue.component("apexchart", VueApexCharts);
Vue.directive("mask", VueMaskDirective);
Vue.use(VeeValidate, { fieldsBagName: "fieldsbag" });
Vue.use(VueFormWizard);
Vue.mixin(layoutMixin);
Vue.mixin(AuthMixin);
Vue.use(Toasted, { duration: 10000 });
Vue.use(money, { precision: 4 });

Vue.config.productionTip = false;

/* eslint-disable no-new */
new Vue({
  el: "#app",
  store,
  router,
  render: (h) => h(App),
});
