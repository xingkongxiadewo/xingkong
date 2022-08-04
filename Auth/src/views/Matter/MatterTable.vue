<template>
  <el-table :show-header="false" :data="todoList" style="width: 100%">
    <el-table-column prop="title" align="center" />
    <el-table-column align="right" width="180">
      <template #default="scope">
        <div v-if="search.MatterType == 1">
          <el-button
            type="primary"
            size="small"
            @click="onEdit(scope.row.id, 2)"
            :icon="Reading"
          ></el-button>
          <el-button
            type="danger"
            size="small"
            @click="onDelete(scope.row.id)"
            :icon="Delete"
          ></el-button>
        </div>
        <div v-else-if="search.MatterType == 2">
          <el-button
            type="primary"
            size="small"
            @click="onEdit(scope.row.id, 1)"
            :icon="Calendar"
          ></el-button>
          <el-button
            type="danger"
            size="small"
            @click="onDelete(scope.row.id)"
            :icon="Delete"
          ></el-button>
        </div>
        <div v-else-if="search.MatterType == 3">
          <el-button
            type="primary"
            size="small"
            @click="onEdit(scope.row.id, 1)"
            :icon="Calendar"
          ></el-button>
          <el-button
            type="primary"
            size="small"
            @click="onEdit(scope.row.id, 2)"
            :icon="Reading"
          ></el-button>
        </div>
      </template>
    </el-table-column>
  </el-table>
  <el-pagination
    v-model:currentPage="search.Page"
    :page-sizes="[10, 20, 50, 100]"
    :page-size="search.Limit"
    layout="total, sizes, prev, pager, next, jumper"
    :total="totalCount"
    @size-change="handleSizeChange"
    @current-change="handleCurrentChange"
    background
  >
  </el-pagination>
</template>

<script>
import { defineComponent, reactive, ref, onBeforeMount } from "vue";
import http from "../../axios/index";
import { Delete, Reading, Calendar } from "@element-plus/icons-vue";
export default defineComponent({
  name: "matterManage",
  props: { matterType: Number },
  setup(props) {
    const todoList = reactive([]);
    const totalCount = ref(0);
    const search = reactive({
      Limit: 10,
      Page: 1,
      MatterType: props.matterType, // 待办事项
    });

    // --------- 列表查询处理-------------------------
    onBeforeMount(() => {
      loadListData();
    });

    function loadListData() {
      http.post("/api/MatterApi/GetMatterPageList", search).then((res) => {
        todoList.length = 0;
        totalCount.value = res.count;
        res.data.forEach((e) => {
          todoList.push(e);
        });
      });
    }

    function handleSizeChange(size) {
      search.Limit = size;
      loadListData();
    }

    function handleCurrentChange(current) {
      search.Page = current;
      loadListData();
    }

    const onEdit = (id, type) => {
      http
        .post("/api/MatterApi/DustbinHandle?id=" + id + "&type=" + type, null)
        .then((res) => {
          loadListData();
        });
    };

    return {
      todoList,
      totalCount,
      search,

      handleSizeChange,
      handleCurrentChange,
      onEdit,

      Delete,
      Reading,
      Calendar,
    };
  },
});
</script>

<style>
.matter-tab {
  height: calc(100vh - 95px);
}

.el-table {
  height: calc(100vh - 180px);
}
</style>
