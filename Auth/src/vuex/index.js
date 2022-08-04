import { createStore } from 'vuex'

export default createStore({
    state: {
        shrinkMenu: false,
        tagsList: []
    },
    mutations: {
        setUserInfo(state, obj) {
            state.userInfo = obj
        },
        setShrinkMenu(state) {
            state.shrinkMenu = !state.shrinkMenu
        },
        deleteTagItem(state, index) {
            state.tagsList.splice(index, 1)
        },
        closeAllTag(state) {
            state.tagsList = [];
        },
        closeOtherTags(state, data) {
            state.tagsList = data;
        },
        setTag(state, data) {
            state.tagsList.push(data)
        }
    }
})
