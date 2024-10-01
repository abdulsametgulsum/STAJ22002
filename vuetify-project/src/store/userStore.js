import { createStore } from 'vuex'
import axios from 'axios'


export default createStore({
  state: {
    user: null,
  },
  mutations: {
    setUser(state, user) {
      state.user = user;
    },
    clearUser(state) {
      state.user = null;
    }
  },
  actions: {
    async login({ commit }, loginModel) {
      try {
        const response = await axios.post('http://localhost:5225/api/Kullanici/login', loginModel);
        commit('setUser', response.data);
        
      } catch (error) {
          alert(error.response.data || 'Bir hata oluştu!');
      }
    },
    logout({ commit }) {
      commit('clearUser');
      this.$router.push('/login');
    }
  },
  getters: {
    isAuthenticated: state => !!state.user,
    isAdmin: state => !!state.user && state.user.admin,
    getUser: state => state.user,
  }
});

