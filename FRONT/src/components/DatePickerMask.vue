<template>
  <Vue2DatePickerMask
    v-bind="forwardedAttrs"
    :append-to-body="appendToBody"
    :popup-class="popupClass"
    v-on="$listeners"
  >
    <slot />
  </Vue2DatePickerMask>
</template>

<script>
import Vue2DatePickerMask from "vue2-datepicker-mask";
import "vue2-datepicker/index.css";
import "vue2-datepicker/locale/pt-br";

export default {
  name: "DatePickerMask",
  components: {
    Vue2DatePickerMask,
  },
  inheritAttrs: false,
  computed: {
    appendToBody() {
      return this.$attrs["append-to-body"] !== undefined
        ? this.$attrs["append-to-body"]
        : true;
    },
    popupClass() {
      return [this.$attrs["popup-class"], "app-datepicker-popup"]
        .filter(Boolean)
        .join(" ");
    },
    forwardedAttrs() {
      const attrs = { ...this.$attrs };
      delete attrs["append-to-body"];
      delete attrs["popup-class"];
      return attrs;
    },
  },
};
</script>
