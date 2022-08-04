<template>
  <el-row>
    <el-col :span="22" v-if="tagLength > 0">
      <el-tag
        v-for="(tag, index) in tagsList"
        :key="tag.title"
        :effect="isActive(tag.path) ? 'dark' : 'plain'"
        @close="closeTag(index)"
        @click="changeTag(index)"
        closable
      >
        <router-link :to="tag.path" class="tag-title">{{ tag.title }}</router-link>
      </el-tag>
    </el-col>
    <el-col :span="22" v-else></el-col>
    <el-col :span="2" class="tag-operations">
      <el-dropdown>
        <el-button type="primary" size="small">
          菜单选项<el-icon class="el-icon--right"><arrow-down /></el-icon>
        </el-button>
        <template #dropdown>
          <el-dropdown-menu>
            <el-dropdown-item @click="closeOtherTags">关闭其他菜单</el-dropdown-item>
            <el-dropdown-item @click="closeAllTag">关闭所有标签</el-dropdown-item>
          </el-dropdown-menu>
        </template>
      </el-dropdown>
    </el-col>
  </el-row>
</template>

<script>
import { defineComponent, computed, ref } from "vue";
import { useStore } from "vuex";
import { useRoute, useRouter, onBeforeRouteUpdate } from "vue-router";

export default defineComponent({
  name: "Tags",
  setup() {
    const store = useStore();

    var tagsList = computed(() => store.state.tagsList);

    var tagLength = computed(() => store.state.tagsList.length);

    const clickIndex = ref(0);
    // 获取到当前路由
    const route = useRoute();
    const router = useRouter();
    // 设置tag
    const setTag = (route) => {
      if (tagsList.value.filter((x) => x.path == route.fullPath).length < 1) {
        store.commit("setTag", {
          title: route.meta.title,
          path: route.fullPath,
        });
      }
    };
    // 设置第一次的路由
    setTag(route);
    // 关闭tag
    const closeTag = (index) => {
      const delItem = tagsList.value[index];
      store.commit("deleteTagItem", { index });
      const item = tagsList.value[index]
        ? tagsList.value[index]
        : tagsList.value[index - 1];
      if (item) {
        delItem.path === route.fullPath && router.push(item.path);
      } else {
        router.push("/");
      }
    };
    // 切换tag
    const changeTag = (index) => {
      clickIndex.value = index;
    };

    const closeAllTag = () => {
      store.commit("closeAllTag");
    };

    const closeOtherTags = () => {
      let tag = tagsList.value.filter((x) => x.path == route.fullPath);
      store.commit("closeOtherTags", tag);
    };

    const isActive = (path) => {
      return path == route.path;
    };

    // 当url 发送变化时 将新的菜单加入到tag
    onBeforeRouteUpdate((to) => {
      setTag(to);
    });

    return {
      tagsList,
      tagLength,
      closeTag,
      changeTag,
      closeAllTag,
      closeOtherTags,
      isActive,
    };
  },
});
</script>

<style>
.el-tag {
  cursor: pointer;
  margin-right: 10px;
}
.tag-operations {
  display: flex;
}
.tag-title {
  text-decoration: none;
  color: #409eff;
}
.el-tag--dark .tag-title {
  color: white;
}
.el-col {
  padding-left: 10px;
}
.el-dropdown {
  margin-left: 20px;
}
</style>
