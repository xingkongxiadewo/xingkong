import { createRouter, createWebHistory } from "vue-router";


const Login = () => import('../views/Login.vue')
const Home = () => import('../views/Home.vue')
const Index = () => import('../views/Index.vue')

const EditPassword = () => import('../views/System/EditPassword.vue')
const UserList = () => import('../views/System/UserList.vue')

const RoleList = () => import('../views/System/RoleList.vue')
const MenuList = () => import('../views/System/MenuList.vue')

const MatterManage = () => import('../views/Matter/MatterManage.vue')

const routes = [
    {
        path: '/',
        redirect: '/index'
    },
    {
        path: '/login',
        name: 'login',
        component: Login,
        meta: {
            requireAuth: false
        }
    },
    {
        path: '/home',
        name: 'home',
        component: Home,
        children: [
            {
                path: '/index',
                name: 'index',
                component: Index,
                meta: {
                    title: "系统首页"
                }
            },
            {
                path: '/editPassword',
                name: 'editPassword',
                component: EditPassword,
                meta: {
                    title: "修改密码"
                }
            },
            {
                path: '/userList',
                name: 'userList',
                component: UserList,
                meta: {
                    title: "用户列表"
                }
            },
            {
                path: '/roleList',
                name: 'roleList',
                component: RoleList,
                meta: {
                    title: "角色列表"
                }
            },
            {
                path: '/menuList',
                name: 'menuList',
                component: MenuList,
                meta: {
                    title: "菜单列表"
                }
            },
            {
                path: '/matterManage',
                name: 'matterManage',
                component: MatterManage,
                meta: {
                    title: "事项管理"
                }
            }
        ]
    },

]

const router = createRouter({
    history: createWebHistory(),
    routes: routes
});


router.beforeEach((to, form, next) => {
    if (to.name != "login" && !sessionStorage.getItem("auth_access_token")) next({ name: "login" })
    else next()
});

export default router