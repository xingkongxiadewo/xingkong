<template>
  <el-menu
    class="sidebar-el-menu"
    background-color="#EBEEF5"
    :collapse="shrink"
    :default-active="onRoutes"
    unique-opened
    router
  >
    <template v-for="menu in menus" :key="menu.index" :index="menu.index">
      <el-sub-menu
        :index="menu.index"
        v-if="menu.children != undefined && menu.children.length > 0"
      >
        <template #title>
          <el-icon v-if="menu.icon != ''" ><component :is="menu.icon"></component></el-icon>
          <span>{{ menu.title }}</span>
        </template>
        <template
          v-for="twoMenu in menu.children"
          :key="twoMenu.index"
          :index="twoMenu.index"
        >
          <el-sub-menu
            v-if="twoMenu.children != undefined && twoMenu.children.length > 0"
            :index="twoMenu.index"
          >
            <template #title>
              <el-icon v-if="twoMenu.icon != ''"><component :is="twoMenu.icon"></component></el-icon>
              <span>{{ twoMenu.title }}</span>
            </template>
            <template
              v-for="threeMenu in twoMenu.children"
              :key="threeMenu.index"
              :index="threeMenu.index"
            >
              <el-menu-item :index="threeMenu.index">
                <el-icon v-if="threeMenu.icon != ''"><component :is="threeMenu.icon"></component></el-icon
                ><span>{{ threeMenu.title }}</span>
              </el-menu-item>
            </template>
          </el-sub-menu>
          <el-menu-item :index="twoMenu.index" v-else>
            <el-icon v-if="twoMenu.icon != ''"><component :is="twoMenu.icon"></component></el-icon
            ><span>{{ twoMenu.title }}</span>
          </el-menu-item>
        </template>
      </el-sub-menu>
      <el-menu-item :index="menu.index" v-else>
        <el-icon v-if="menu.icon != ''"><component :is="menu.icon"></component></el-icon
        ><span>{{ menu.title }}</span>
      </el-menu-item>
    </template>
  </el-menu>
</template>

<script>
import { defineComponent, reactive, onBeforeMount, computed } from "vue";
import { useRoute } from "vue-router";
import { useStore } from "vuex";
import http from "../axios/index";

export default defineComponent({
  components: {},
  setup() {
    const route = useRoute();
    const store = useStore();
    const menus = reactive([]);

    onBeforeMount(() => {
      http.post("/api/SystemApi/Menus", {}).then((res) => {
        res.data.forEach((e) => {
          menus.push(e);
        });
      });
    });

    const onRoutes = computed(() => {
      return route.path;
    });

    const shrink = computed(() => store.state.shrinkMenu);

    return {
      menus,
      shrink,
      onRoutes,
    };
  },
});
</script>

<style>
.sidebar-el-menu {
  display: block;
  height: calc(100vh - 50px);
  position: relative;
  left: 0;
  bottom: 0;
  overflow: hidden;
}

.sidebar-el-menu:not(.el-menu--collapse) {
  width: 250px;
}
</style>
