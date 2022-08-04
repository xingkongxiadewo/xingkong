<script>
import { defineComponent, ref } from "vue";
import { useRouter } from "vue-router";
import { ElMessage } from "element-plus";
import { Avatar, Lock } from "@element-plus/icons-vue";
import { useStore } from "vuex";
import http from "../axios/index";

export default defineComponent({
  name: "login",
  setup() {
    const account = ref("");
    const password = ref("");

    const store = useStore();
    const router = useRouter();

    function login() {
      var data = {
        Account: account.value,
        Password: password.value,
      };

      http.post("/api/SystemApi/Login", data).then((res) => {
        ElMessage({
          message: res.message,
          type: "success",
        });

        sessionStorage.setItem(
          "auth_access_token",
          res.data.token_type + " " + res.data.access_token
        );
        localStorage.setItem("auth_login_username", res.data.profile.name);

        store.commit("setUserInfo", res.data);

        router.push("/");
      });
    }

    return {
      Avatar,
      Lock,
      account,
      password,
      login,
    };
  },
});
</script>

<template>
  <div class="login">
    <div class="login-body">
      <el-form ref="">
        <el-form-item>
          <el-input :prefix-icon="Avatar" v-model="account" placeholder="请输入账号">
          </el-input>
        </el-form-item>
        <el-form-item>
          <el-input
            :prefix-icon="Lock"
            v-model="password"
            placeholder="请输入密码"
            @keyup.enter="login"
            show-password
          />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="login">登录</el-button>
        </el-form-item>
      </el-form>
    </div>
  </div>
</template>

<style scoped>
.el-input {
  width: 300px;
}
.el-button {
  width: 300px;
}

.login {
  display: flex;
  justify-content: center;
  width: 100vw;
  height: 100vh;
  background: url("../public/login-bg.jpg");
}

.login-body {
  width: 300px;
  height: 180px;
  text-align: center;
  padding: 40px 40px 20px 40px;
  border-radius: 10px; /* 圆角 */
  border: 1px solid #b8bac2;
  background-color: rgba(225, 222, 222, 0.3);
  margin-top: 220px;
}
</style>
