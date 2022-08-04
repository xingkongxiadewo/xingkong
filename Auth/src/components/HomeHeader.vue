<template>
  <div class="header">
    <div class="left-sign">
      <span>
        <el-icon
          ><component
            :is="shrinkMenu ? Expand : Fold"
            class="shrink-icon"
            @click="editShrinkMenu()"
          ></component
        ></el-icon>
      </span>
      <span>框架</span>
    </div>
    <div class="right-set">
      <span><Bell class="home-icon" /></span>
      <span><Setting class="home-icon" /></span>
      <span
        ><img
          src="../public/user2-160x160.jpg"
          class="user-img"
          @click.left="showUserSet = !showUserSet"
      /></span>
      <span class="user-name" @click.left="showUserSet = !showUserSet">{{
        userName
      }}</span>
    </div>
    <div class="user-set" v-if="showUserSet">
      <div>
        <img
          src="../public/user2-160x160.jpg"
          class="user-set-img"
          @click.left="showUserSet = !showUserSet"
        />
      </div>
      <div style="text-align: center">{{ fullName }}</div>
      <div class="user-set-btn">
        <el-button
          size="mini"
          style="float: left; margin-left: 10px; margin-top: 4px"
          @click="userInfo"
          >用户信息</el-button
        >
        <el-button
          size="mini"
          style="float: right; margin-right: 10px; margin-top: 4px"
          @click="loginOut"
          >退出</el-button
        >
      </div>
    </div>
  </div>
</template>

<script>
import { defineComponent, computed, ref } from "vue";
import { useStore } from "vuex";
import { useRouter } from "vue-router";
import { Bell, Setting, Expand, Fold } from "@element-plus/icons-vue";

export default defineComponent({
  name: "Index",
  components: {
    Bell,
    Setting,
    Expand,
    Fold,
  },
  setup() {
    const store = useStore();
    const router = useRouter();
    const shrinkMenu = computed(() => {
      return store.state.shrinkMenu;
    });

    const editShrinkMenu = () => {
      store.commit("setShrinkMenu");
    };

    const userName = ref(localStorage.getItem("auth_login_username"));

    const showUserSet = ref(false);

    const loginOut = () => {
      sessionStorage.removeItem("auth_access_token");
      localStorage.removeItem("auth_login_username");
      router.push("/");
    };

    const userInfo = () => {
      router.push("/userInfo");
    };

    return {
      Bell,
      Setting,
      Expand,
      Fold,

      shrinkMenu,
      userName,
      showUserSet,
      editShrinkMenu,
      loginOut,
      userInfo,
    };
  },
});
</script>

<style>
.header {
  width: 100%;
  height: 50px;
  background-color: rgb(9, 118, 243);
  color: white;
}

.left-sign {
  padding-left: 10px;
  font-size: 20px;
  float: left;
  height: 100%;
  line-height: 50px;
}

.shrink-icon {
  width: 1em;
  height: 1em;
  margin-right: 4px;
  cursor: pointer;
  position: relative;
  top: 3px;
}

/* right */

.right-set {
  height: 50px;
  text-align: right;
  padding-right: 40px;
}
.home-icon {
  width: 1em;
  height: 50px;
  margin-right: 8px;
  cursor: pointer;
}

.right-set span {
  margin-right: 10px;
}

.user-img {
  width: 41px;
  height: 41px;
  border-radius: 50%;
  margin-bottom: 5px;
}
.user-name {
  line-height: 50px;
  display: block;
  height: 100%;
  float: right;
}

/* --------------- */

.user-set {
  position: relative;
  width: 250px;
  height: 230px;
  background-color: rgb(9, 118, 243);
  float: right;
  z-index: 10;
  margin-top: 1px;
  margin-right: 10px;
  border: 1px solid rgb(9, 118, 243);
}

.user-set-img {
  width: 120px;
  border-radius: 50%;
  margin: 20px 26%;
}
.user-set-btn {
  position: absolute;
  bottom: 0px;
  background-color: white;
  width: 100%;
  height: 36px;
  padding-bottom: 5px;
}
/* --------------- */
</style>
