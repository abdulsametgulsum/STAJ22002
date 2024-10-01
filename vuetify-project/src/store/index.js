import { createStore } from 'vuex'

const store = createStore({
  state() {
    return {
      user: null, // Giriş yapan kullanıcıyı saklamak için
    }
  },
  mutations: {
    setUser(state, user) {
      state.user = user
    },
    clearUser(state) {
      state.user = null
    },
  },
  actions: {
    login({ commit }, user) {
      debugger;
      commit('setUser', user)
    },
    logout({ commit }) {
      commit('clearUser')
    },
  },
  getters: {
    isAuthenticated(state) {
      return !!state.user // Kullanıcı varsa true döner, yoksa false döner
    },
    getUser(state) {
      return state.user // Kullanıcı bilgisini döner
    },
  },
})

export default store
