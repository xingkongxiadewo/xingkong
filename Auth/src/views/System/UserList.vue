<template>
  <div class="list-main">
    <el-form :inline="true" :model="search">
      <el-form-item>
        <el-input v-model="search.Name">
          <template #prepend>名称</template>
        </el-input>
      </el-form-item>
      <el-form-item>
        <el-input v-model="search.Account">
          <template #prepend>账号</template>
        </el-input>
      </el-form-item>
      <el-form-item>
        <el-input v-model="search.Phone">
          <template #prepend>手机</template>
        </el-input>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="loadListData">搜索</el-button>
        <el-button type="primary" @click="openAdd = !openAdd">新增</el-button>
      </el-form-item>
    </el-form>
    <el-table :data="userList" style="width: 100%" height="91%" border>
      <el-table-column type="index" width="50" align="center" />
      <el-table-column prop="name" label="姓名" align="center" />
      <el-table-column prop="account" label="账号" align="center" />
      <el-table-column prop="phone" label="手机" align="center" />
      <el-table-column prop="email" label="邮箱" align="center" min-width="70" />
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
          <el-button type="primary" size="small" @click="onBindRole(scope.row)"
            >绑定角色</el-button
          >
          <el-button type="warning" size="small" @click="onEdit(scope.row)"
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

    <el-dialog title="新增" width="30%" v-model="openAdd" center>
      <el-form :model="formData" :rules="rules" ref="formRef">
        <el-form-item prop="Name">
          <el-input autocomplete="off" v-model="formData.Name">
            <template #prepend><span style="color: red">*</span>姓名</template>
          </el-input>
        </el-form-item>
        <el-form-item prop="Account">
          <el-input autocomplete="off" v-model="formData.Account">
            <template #prepend><span style="color: red">*</span>账号</template>
          </el-input>
        </el-form-item>
        <el-form-item prop="Phone">
          <el-input autocomplete="off" v-model="formData.Phone">
            <template #prepend><span style="color: red">*</span>手机</template>
          </el-input>
        </el-form-item>
        <el-form-item prop="Password">
          <el-input
            type="password"
            autocomplete="off"
            v-model="formData.Password"
            show-password
            ><template #prepend><span style="color: red">*</span>密码</template></el-input
          >
        </el-form-item>
        <el-form-item prop="Email">
          <el-input autocomplete="off" v-model="formData.Email">
            <template #prepend>邮箱</template>
          </el-input>
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button type="primary" @click="AddUser">新增</el-button>
          <el-button type="primary" @click="openAdd = !openAdd">取消</el-button>
        </span>
      </template>
    </el-dialog>

    <el-dialog title="修改" width="30%" v-model="openEdit" center>
      <el-form :model="formData" :rules="rules" ref="formRef">
        <el-form-item prop="Name">
          <el-input autocomplete="off" v-model="formData.Name">
            <template #prepend><span style="color: red">*</span>姓名</template>
          </el-input>
        </el-form-item>
        <el-form-item prop="Account">
          <el-input autocomplete="off" v-model="formData.Account">
            <template #prepend><span style="color: red">*</span>账号</template>
          </el-input>
        </el-form-item>
        <el-form-item prop="Phone">
          <el-input autocomplete="off" v-model="formData.Phone">
            <template #prepend><span style="color: red">*</span>手机</template>
          </el-input>
        </el-form-item>
        <el-form-item prop="Email">
          <el-input autocomplete="off" v-model="formData.Email">
            <template #prepend>邮箱</template>
          </el-input>
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button type="primary" @click="EditUser">保存</el-button>
          <el-button type="primary" @click="openEdit = !openEdit">取消</el-button>
        </span>
      </template>
    </el-dialog>

    <el-dialog
      title="绑定角色"
      width="30%"
      v-model="openBindRole"
      destroy-on-close
      center
    >
      <BindRole :userId="userId" ref="refChild" @closeBindRole="closeBindRole" />
      <template #footer>
        <span class="dialog-footer">
          <el-button type="primary" @click="bindRole">保存</el-button>
          <el-button type="primary" @click="openBindRole = !openBindRole">取消</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script>
import { defineComponent, reactive, ref, onBeforeMount } from "vue";
import { ElMessage } from "element-plus";
import moment from "moment";
import http from "../../axios/index";
import BindRole from "./BindRole.vue";

export default defineComponent({
  name: "userList",
  components: {
    BindRole,
  },
  setup() {
    const userList = reactive([]);
    const totalCount = ref(0);
    const search = reactive({
      Limit: 10,
      Page: 1,
      Account: "",
      Name: "",
      Phone: "",
    });

    const openEdit = ref(false);
    const openAdd = ref(false);

    const formData = reactive({
      Account: "",
      Name: "",
      Email: "",
      Phone: "",
      Password: "",
      Id: 0,
    });

    const rules = reactive({
      Account: [
        {
          required: true,
          message: "请输入账号!",
          trigger: "blur",
        },
      ],
      Name: [
        {
          required: true,
          message: "请输入姓名!",
          trigger: "blur",
        },
      ],
      Phone: [
        {
          required: true,
          message: "请输入手机!",
          trigger: "blur",
        },
      ],
      Password: [
        {
          required: true,
          message: "请输入密码!",
          trigger: "blur",
        },
      ],
    });

    const formRef = ref(null);

    onBeforeMount(() => {
      loadListData();
    });

    function loadListData() {
      http.post("/api/UserApi/GetUserPageList", search).then((res) => {
        userList.length = 0;
        totalCount.value = res.count;
        res.data.forEach((e) => {
          userList.push(e);
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

    function AddUser() {
      // 表单校验
      formRef.value.validate((valid) => {
        if (valid) {
          http.post("/api/UserApi/AddUser", formData).then((res) => {
            ElMessage.success(res.message);
            loadListData();
            openAdd.value = false;
            Object.keys(formData).map((key) => {
              delete formData[key];
            });
          });
        } else {
          return false;
        }
      });
    }

    function onEdit(row) {
      openEdit.value = !openEdit.value;
      formData.Name = row.name;
      formData.Account = row.account;
      formData.Phone = row.phone;
      formData.Email = row.email;
      formData.Id = row.id;
    }

    function EditUser() {
      // 表单校验
      formRef.value.validate((valid) => {
        if (valid) {
          http.post("/api/UserApi/EditUser", formData).then((res) => {
            ElMessage.success(res.message);
            loadListData();
            openEdit.value = false;
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
      http.post("/api/UserApi/DeleteUser?id=" + id, null).then((res) => {
        ElMessage.success(res.message);
        loadListData();
      });
    }

    // 绑定角色
    const openBindRole = ref(false);
    const userId = ref(0);

    function onBindRole(row) {
      userId.value = row.id;
      openBindRole.value = !openBindRole.value;
    }

    const refChild = ref();
    function bindRole() {
      refChild.value.bindRole();
    }

    const closeBindRole = () => {
      openBindRole.value = false;
    };

    const onEditStatus = (id) => {
      http.post("/api/UserApi/EditStatus?id=" + id, null).then((res) => {
        ElMessage.success(res.message);
        loadListData();
      });
    };
    return {
      userList,
      totalCount,
      search,
      openAdd,
      openEdit,
      formData,
      rules,
      formRef,

      loadListData,
      handleSizeChange,
      handleCurrentChange,
      dateFormat,
      AddUser,
      onEdit,
      EditUser,
      onDelete,
      onEditStatus,

      openBindRole,
      userId,
      onBindRole,
      bindRole,
      refChild,
      BindRole,
      closeBindRole,
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
