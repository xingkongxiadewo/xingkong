<template>
  <div class="bind-role-main">
    <el-tree :data="menuList" show-checkbox node-key="id" ref="treeRef" />
  </div>
</template>

<script>
import { defineComponent, reactive, ref, onBeforeMount, onMounted } from "vue";
import http from "../../axios/index";
import { ElMessage } from "element-plus";
export default defineComponent({
  name: "bindMenu",
  props: { roleId: Number },
  setup(props, { attrs, slots, emit }) {
    const roleId = props.roleId;
    const menuList = reactive([]);
    // --------- 列表查询处理-------------------------
    onBeforeMount(() => {
      loadListData();
    });
    onMounted(() => {
      loadBindMenuData();
    });

    function loadListData() {
      http.get("/api/MenuApi/GetMenuList", {}).then((res) => {
        menuList.length = 0;
        res.data.forEach((e) => {
          menuList.push(e);
        });
      });
    }

    const treeRef = ref(null);
    const checkedMenus = reactive([]);
    function loadBindMenuData() {
      http.get("/api/MenuApi/GetMenuListByRoleId", { roleId: roleId }).then((res) => {
        if (res.data.length < 1) return;
        treeRef.value.setCheckedKeys(
          res.data.map((x) => x.id),
          false
        );
      });
    }

    const bindMenu = () => {
      http
        .post("/api/MenuApi/BindMenu", {
          RoleId: roleId,
          MenuIds: treeRef.value.getCheckedKeys(),
        })
        .then((res) => {
          ElMessage.success(res.message);
          emit("closeBindMenu");
        });
    };

    return {
      menuList,
      checkedMenus,
      treeRef,
      bindMenu,
    };
  },
});
</script>

<style></style>
