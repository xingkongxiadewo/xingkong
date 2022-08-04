<template>
  <div class="list-main">
    <el-skeleton style="width: 300px">
      <template #template>
        <el-card :body-style="{ padding: '0px', marginBottom: '1px' }">
          <img src="../public/head.jpg" class="image" />
          <div style="padding: 14px; text-align: center">
            <span>{{ userName }} , 欢迎登录</span>
          </div>
        </el-card>
      </template>
    </el-skeleton>
  </div>
</template>

<script>
import { defineComponent, ref } from "vue";

export default defineComponent({
  name: "index",

  setup() {
    const userName = ref(localStorage.getItem("auth_login_username"));
    return {
      userName,
    };
  },
});
</script>

<style>
.image {
  width: 100%;
  height: 100%;
}
</style>
