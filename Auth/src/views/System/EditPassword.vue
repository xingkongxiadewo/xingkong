<template>
  <div class="list-main">
    <el-form
      ref="ruleFormRef"
      :model="ruleForm"
      status-icon
      :rules="rules"
      style="max-width: 460px"
      label-width="120px"
    >
      <el-form-item label="旧密码" prop="oldPass">
        <el-input
          v-model="ruleForm.oldPass"
          type="password"
          autocomplete="off"
        ></el-input>
      </el-form-item>
      <el-form-item label="新密码" prop="pass">
        <el-input v-model="ruleForm.pass" type="password" autocomplete="off"></el-input>
      </el-form-item>
      <el-form-item label="确认密码" prop="checkPass">
        <el-input
          v-model="ruleForm.checkPass"
          type="password"
          autocomplete="off"
        ></el-input>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submitForm">修改</el-button>
        <el-button @click="resetForm">重置</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script>
import { defineComponent, reactive, ref } from "vue";
import http from "../../axios/index";
import { ElMessage } from "element-plus";
export default defineComponent({
  name: "editPassword",

  setup() {
    const ruleFormRef = ref(null);
    const ruleForm = reactive({
      oldPass: "",
      pass: "",
      checkPass: "",
    });

    const validatePass = (rule, value, callback) => {
      if (value === "") {
        callback(new Error("请输入密码"));
      } else {
        if (ruleForm.checkPass !== "") {
          if (!ruleFormRef.value) return;
          ruleFormRef.value.validateField("checkPass", () => null);
        }
        callback();
      }
    };

    const validateOldPass = (rule, value, callback) => {
      if (value === "") {
        callback(new Error("请输入密码"));
      } else {
        callback();
      }
    };

    const validatePass2 = (rule, value, callback) => {
      if (value === "") {
        callback(new Error("请输入密码"));
      } else if (value !== ruleForm.pass) {
        callback(new Error("密码不一致"));
      } else {
        callback();
      }
    };

    const rules = reactive({
      oldPass: [{ validator: validateOldPass, trigger: "blur" }],
      pass: [{ validator: validatePass, trigger: "blur" }],
      checkPass: [{ validator: validatePass2, trigger: "blur" }],
    });

    function submitForm() {
      http
        .post("/api/UserApi/EditPassword", {
          oldPassword: ruleForm.oldPass,
          newPassword: ruleForm.pass,
        })
        .then((res) => {
          ElMessage.success(res.message);
          resetForm();
        });
    }
    function resetForm() {
      Object.keys(formData).map((key) => {
        delete formData[key];
      });
    }

    return {
      ruleForm,
      rules,
      ruleFormRef,
      submitForm,
      resetForm,
    };
  },
});
</script>

<style></style>
