<template>
  <el-container>
    <el-header>
      <HomeHeader />
    </el-header>
    <el-container>
      <HomeAside />
      <el-main>
        <div class="home-tag">
          <HomeTag />
        </div>
        <div class="home-content">
          <router-view v-slot="{ Component }">
            <transition name="move" mode="out-in">
              <keep-alive :include="tagsList">
                <component :is="Component" />
              </keep-alive>
            </transition>
          </router-view>
        </div>
      </el-main>
    </el-container>
  </el-container>
</template>

<script>
import { computed, defineComponent } from "vue";
import { useStore } from "vuex";
import HomeHeader from "../components/HomeHeader.vue";
import HomeTag from "../components/HomeTag.vue";
import HomeAside from "../components/HomeAside.vue";

export default defineComponent({
  name: "Index",
  components: {
    HomeHeader,
    HomeTag,
    HomeAside,
  },
  setup() {
    const store = useStore();

    const tagsList = computed(() => store.state.tagsList);

    return {
      HomeHeader,
      HomeTag,
      HomeAside,

      tagsList,
    };
  },
});
</script>

<style>
.el-header {
  --el-header-height: 50px;
  padding: 0;
}

.el-main {
  padding: 5px 1px 0 0;
}
.el-container {
  position: relative;
  z-index: 1;
}

.home-content {
  margin-top: 5px;
  height: calc(100vh - 86px);
  padding: 1px 1px 0 1px;
  background-color: #c0c4cc;
}
</style>
