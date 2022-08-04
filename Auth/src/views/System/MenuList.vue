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
    <el-table :data="menuList" style="width: 100%" height="91%" border>
      <el-table-column type="index" width="50" align="center" />
      <el-table-column prop="name" label="名称" align="center" />
      <el-table-column prop="icon" label="图标" align="center" />
      <el-table-column prop="url" label="Url" align="center" />
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
      <el-table-column label="操作" align="center" width="140">
        <template #default="scope">
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
      @open="loadMenuSelect"
      center
    >
      <el-form :model="formData" :rules="rules" ref="formRef">
        <el-form-item>
          <span class="cascader-label">父级菜单</span>
          <el-cascader
            :options="menuSelectList"
            :props="props"
            v-model="formData.ParentId"
            clearable
          />
        </el-form-item>
        <el-form-item prop="Name">
          <el-input autocomplete="off" v-model="formData.Name">
            <template #prepend><span style="color: red">*</span>名称</template>
          </el-input>
        </el-form-item>
        <el-form-item prop="Url">
          <el-input autocomplete="off" v-model="formData.Url">
            <template #prepend><span style="color: red">*</span>Url</template>
          </el-input>
        </el-form-item>
        <el-form-item prop="Icon">
          <el-input autocomplete="off" v-model="formData.Icon">
            <template #prepend>图标</template>
          </el-input>
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button type="primary" @click="handleMenu">保存</el-button>
          <el-button type="primary" @click="openDialog(0)">取消</el-button>
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
export default defineComponent({
  name: "userList",

  setup() {
    const menuList = reactive([]);
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
      http.post("/api/MenuApi/GetMenuPageList", search).then((res) => {
        menuList.length = 0;
        totalCount.value = res.count;
        res.data.forEach((e) => {
          menuList.push(e);
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

    function onDelete(id) {
      http.post("/api/MenuApi/DeleteMenu?id=" + id, null).then((res) => {
        ElMessage.success(res.message);
        loadListData();
      });
    }

    const openType = ref(0);
    const isOpen = ref(false);

    const formData = reactive({
      Name: "",
      Url: "",
      Icon: "",
      ParentId: "",
      Id: 0,
    });
    const rules = reactive({
      Name: [
        {
          required: true,
          message: "请输入菜单名称!",
          trigger: "blur",
        },
      ],
      Url: [
        {
          required: true,
          message: "请输入Url!",
          trigger: "blur",
        },
      ],
    });
    const formRef = ref(null);

    function openDialog(type, row) {
      Object.keys(formData).map((key) => {
        delete formData[key];
      });
      openType.value = type;
      isOpen.value = openType.value > 0;
      if (type == 2 && row != null) {
        formData.Id = row.id;
        formData.Name = row.name;
        formData.Url = row.url;
        formData.Icon = row.icon;
        formData.ParentId = row.parentId.toString();
      }
    }

    function handleMenu() {
      let url = "/api/MenuApi/AddMenu";
      if (openType.value == 2) url = "/api/MenuApi/EditMenu";
      // 表单校验
      formRef.value.validate((valid) => {
        if (valid) {
          http.post(url, formData).then((res) => {
            ElMessage.success(res.message);
            loadListData();
            isOpen.value = false;
            openType.value = 0;
          });
        } else {
          return false;
        }
      });
    }
    const menuSelectList = reactive([]);

    const props = reactive({
      checkStrictly: true,
      emitPath: false,
    });

    function loadMenuSelect() {
      if (menuSelectList.length > 0) return;
      http.post("/api/MenuApi/GetMenuSelect", null).then((res) => {
        menuSelectList.length = 0;
        res.data.forEach((e) => {
          menuSelectList.push(e);
        });
      });
    }

    const onEditStatus = (id) => {
      http.post("/api/MenuApi/EditStatus?id=" + id, null).then((res) => {
        ElMessage.success(res.message);
        loadListData();
      });
    };

    return {
      menuList,
      totalCount,
      search,
      openType,
      isOpen,
      formData,
      rules,
      formRef,
      menuSelectList,
      props,

      dateFormat,
      loadListData,
      handleSizeChange,
      handleCurrentChange,
      onDelete,
      openDialog,
      handleMenu,
      loadMenuSelect,
      onEditStatus,
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

.cascader-label {
  text-align: center;
  background-color: var(--el-bg-color);
  color: var(--el-color-info);
  display: table-cell;
  position: relative;
  border: 1px solid #dcdfe6;
  border-radius: var(--el-input-border-radius);
  padding: 0 12px;
  white-space: nowrap;
  height: 30px;
}
.el-cascader {
  width: calc(100% - 82px);
}
.el-input__inner {
  border-top-left-radius: 0;
  border-bottom-left-radius: 0;
}
</style>
