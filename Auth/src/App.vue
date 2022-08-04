<template>
  <el-config-provider :locale="locale">
    <router-view></router-view>
  </el-config-provider>
</template>

<script>
import { defineComponent,ref } from "vue";
import { ElConfigProvider } from "element-plus";
import zhLocale from 'element-plus/lib/locale/lang/zh-cn';

export default defineComponent({
  name: "index",
  components: {
    [ElConfigProvider.name]: ElConfigProvider,
  },
  setup() {
    const locale = ref(zhLocale);
    return {
      locale,
    };
  },
});
</script>

<style>
@import "./assets/main.css";
</style>
