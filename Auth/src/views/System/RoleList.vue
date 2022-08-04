<template>
  <div class="list-main">
    <el-form :inline="true" :model="search">
      <el-form-item>
        <el-input v-model="search.Name">
          <template #prepend>名称</template>
        </el-input>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="loadListData">搜索</el-button>
        <el-button type="primary" @click="openDialog(1)">新增</el-button>
      </el-form-item>
    </el-form>
    <el-table :data="roleList" style="width: 100%" height="91%" border>
      <el-table-column type="index" width="50" align="center" />
      <el-table-column prop="name" label="名称" align="center" />
      <el-table-column prop="isEnable" label="状态" width="70" align="center">
        <template #default="scope">
          <el-switch
            v-model="scope.row.isEnable"
            active-color="#13ce66"
            inactive-color="#ff4949"
            @click="onEditStatus(scope.row.id)"
          />
        </template>
      </el-table-column>
      <el-table-column prop="createUserName" label="创建人" align="center" />
      <el-table-column
        prop="createTime"
        label="创建时间"
        align="center"
        :formatter="dateFormat"
        sortable
      />
      <el-table-column prop="modifieyUserName" label="修改人" align="center" />
      <el-table-column
        prop="modifieyTime"
        label="修改时间"
        align="center"
        :formatter="dateFormat"
        sortable
      />
      <el-table-column label="操作" align="center" width="230">
        <template #default="scope">
          <el-button type="primary" size="small" @click="onBindMenu(scope.row)"
            >绑定菜单</el-button
          >
          <el-button type="warning" size="small" @click="openDialog(2, scope.row)"
            >修改</el-button
          >
          <el-button type="danger" size="small" @click="onDelete(scope.row.id)"
            >删除</el-button
          >
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

    <el-dialog
      :title="openType == 1 ? '新增' : '修改'"
      width="30%"
      v-model="isOpen"
      center
    >
      <el-form :model="formData" :rules="rules" ref="formRef">
        <el-form-item prop="Name">
          <el-input autocomplete="off" v-model="formData.Name">
            <template #prepend><span style="color: red">*</span>名称</template>
          </el-input>
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button type="primary" @click="handleRole">保存</el-button>
          <el-button type="primary" @click="openDialog(0)">取消</el-button>
        </span>
      </template>
    </el-dialog>

    <el-dialog
      title="绑定菜单"
      width="30%"
      v-model="openBindMenu"
      destroy-on-close
      center
    >
      <BindMenu :roleId="roleId" ref="refChild" @closeBindMenu="closeBindMenu" />
      <template #footer>
        <span class="dialog-footer">
          <el-button type="primary" @click="bindMenu">保存</el-button>
          <el-button type="primary" @click="openBindMenu = !openBindMenu">取消</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script>
import { defineComponent, reactive, ref, onBeforeMount } from "vue";
import { ElMessage } from "element-plus";
import http from "../../axios/index";
import moment from "moment";
import BindMenu from "./BindMenu.vue";
export default defineComponent({
  name: "userList",
  components: {
    BindMenu,
  },
  setup() {
    const roleList = reactive([]);
    const totalCount = ref(0);
    const search = reactive({
      Limit: 10,
      Page: 1,
      Name: "",
    });
    // --------- 列表查询处理-------------------------
    onBeforeMount(() => {
      loadListData();
    });

    function loadListData() {
      http.post("/api/RoleApi/GetRolePageList", search).then((res) => {
        roleList.length = 0;
        totalCount.value = res.count;
        res.data.forEach((e) => {
          roleList.push(e);
        });
      });
    }

    function dateFormat(row, column) {
      var date = row[column.property];
      if (date == undefined) {
        return "";
      }
      //修改时间格式 我要修改的是"YYYY-MM-DD"
      return moment(date).format("YYYY-MM-DD HH:mm:ss");
    }

    function handleSizeChange(size) {
      search.Limit = size;
      loadListData();
    }

    function handleCurrentChange(current) {
      search.Page = current;
      loadListData();
    }
    // --------------- end ----------------------

    const openType = ref(0);
    const isOpen = ref(false);
    const formData = reactive({
      Name: "",
      Id: 0,
    });

    function openDialog(type, row) {
      formData.Id = 0;
      formData.Name = "";
      openType.value = type;
      isOpen.value = openType.value > 0;
      if (type == 2 && row != null) {
        formData.Id = row.id;
        formData.Name = row.name;
      }
    }

    const rules = reactive({
      Name: [
        {
          required: true,
          message: "请输入角色名称!",
          trigger: "blur",
        },
      ],
    });

    const formRef = ref(null);

    function handleRole() {
      let url = "/api/RoleApi/AddRole";
      if (openType.value == 2) url = "/api/RoleApi/EditRole";
      // 表单校验
      formRef.value.validate((valid) => {
        if (valid) {
          http.post(url, formData).then((res) => {
            ElMessage.success(res.message);
            loadListData();
            isOpen.value = false;
            openType.value = 0;
            Object.keys(formData).map((key) => {
              delete formData[key];
            });
          });
        } else {
          return false;
        }
      });
    }

    function onDelete(id) {
      http.post("/api/RoleApi/DeleteRole?id=" + id, null).then((res) => {
        ElMessage.success(res.message);
        loadListData();
      });
    }

    // 绑定菜单
    const openBindMenu = ref(false);
    const roleId = ref(0);

    function onBindMenu(row) {
      roleId.value = row.id;
      openBindMenu.value = !openBindMenu.value;
    }

    const refChild = ref();
    function bindMenu() {
      refChild.value.bindMenu();
    }

    const closeBindMenu = () => {
      openBindMenu.value = false;
    };

    const onEditStatus = (id) => {
      http.post("/api/RoleApi/EditStatus?id=" + id, null).then((res) => {
        ElMessage.success(res.message);
        loadListData();
      });
    };
    return {
      roleList,
      totalCount,
      search,
      openType,
      formData,
      isOpen,
      rules,
      formRef,
      dateFormat,
      loadListData,
      handleSizeChange,
      handleCurrentChange,
      openDialog,
      handleRole,
      onDelete,
      onEditStatus,

      BindMenu,
      openBindMenu,
      roleId,
      onBindMenu,
      refChild,
      bindMenu,
      closeBindMenu,
    };
  },
});
</script>

<style>
.el-form--inline {
  height: 40px;
}
.el-input-group__prepend {
  width: 40px;
  text-align: right;
}
</style>
