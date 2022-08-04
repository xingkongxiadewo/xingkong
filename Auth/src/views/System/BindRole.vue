<template>
  <div class="bind-role-main">
    <el-table
      style="width: 100%"
      :data="roleList"
      max-height="500"
      ref="multipleTableRef"
      @selection-change="handleSelectionChange"
      border
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column prop="name" label="名称" align="center" />
    </el-table>
  </div>
</template>

<script>
import { defineComponent, reactive, ref, onBeforeMount, onMounted } from "vue";
import http from "../../axios/index";
import { ElMessage } from "element-plus";
export default defineComponent({
  name: "bindRole",
  props: { userId: Number },
  setup(props, { attrs, slots, emit }) {
    const userId = props.userId;
    const roleList = reactive([]);
    // --------- 列表查询处理-------------------------
    onBeforeMount(() => {
      loadListData();
    });

    onMounted(() => {
      loadBindRoleData();
    });

    function loadListData() {
      http.get("/api/RoleApi/GetRoleList", {}).then((res) => {
        roleList.length = 0;
        res.data.forEach((e) => {
          roleList.push(e);
        });
      });
    }
    const multipleTableRef = ref(null);
    function loadBindRoleData() {
      http.get("/api/RoleApi/GetBoundRoles", { userId: userId }).then((res) => {
        if (res.data.length < 1) return;
        roleList.forEach((row) => {
          if (res.data.filter((x) => x.id == row.id).length > 0)
            multipleTableRef.value.toggleRowSelection(row, true);
        });
      });
    }
    const multipleSelection = ref(null);
    const handleSelectionChange = (val) => {
      multipleSelection.value = val;
    };

    const bindRole = () => {
      http
        .post("/api/RoleApi/BindRole", {
          UserId: userId,
          RoleIds: multipleSelection.value.map((x) => x.id),
        })
        .then((res) => {
          ElMessage.success(res.message);
          emit("closeBindRole");
        });
    };

    return {
      roleList,
      multipleTableRef,
      handleSelectionChange,
      bindRole,
    };
  },
});
</script>

<style></style>
