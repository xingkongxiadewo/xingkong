<template>
  <div class="list-main">
    <el-tabs model-value="needMatter" @tab-click="loadData">
      <el-tab-pane label="待办事项" name="needMatter">
        <MatterTable :matterType="1" :key="new Date().getTime()" />
      </el-tab-pane>
      <el-tab-pane label="已办事项" name="doneMatter">
        <MatterTable :matterType="2" :key="new Date().getTime()" />
      </el-tab-pane>
      <el-tab-pane label="垃圾箱" name="dustbin">
        <MatterTable :matterType="3" :key="new Date().getTime()" />
      </el-tab-pane>
    </el-tabs>
  </div>
</template>

<script>
import { defineComponent } from "vue";
import MatterTable from "./MatterTable.vue";
export default defineComponent({
  name: "matterManage",
  components: {
    MatterTable,
  },
  setup() {
    return {
      MatterTable,
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
