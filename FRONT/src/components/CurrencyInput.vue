<template>
  <input
    ref="inputRef"
    type="text"
    style="background-color: #040620; border-radius: 5px; border-style: solid; border-color: #040620; color: white; height: 32px; min-width: 100%; margin: 2px; padding: 2px;"
  />
</template>

<script>
import { useCurrencyInput, setValue } from "vue-currency-input";

export default {
  name: "CurrencyInput",
  props: {
    modelValue: Number,
    options: Object,
  },
  setup(props) {
    const { inputRef } = useCurrencyInput(props.options);

    return { inputRef };
  },
  created(){
    const component = this;
      this.handler = function (e) {
        component.$emit('keyup', e);
      }
      window.addEventListener('keyup', this.handler);
  },
  beforeDestroy() {
      window.removeEventListener('keyup', this.handler);
    },
  methods: {
    atualiza(valor) {
      setValue(this.$refs.inputRef, valor);
    },
  },
};
</script>
