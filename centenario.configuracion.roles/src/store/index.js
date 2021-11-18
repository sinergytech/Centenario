import Vue from 'vue'
import Vuex from 'vuex'
import API from '../api';

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    configuracionCargada: false,
    currentUser: null
  },
  mutations: {
    updateConfigState(state, configState){
      state.configuracionCargada = configState;
    },
    updateCurrentUserState(state, currentUser) {
      state.currentUser = currentUser;
    },
  },
  actions: {
    async cargarConfiguracion({ commit, dispatch }) {
      let pConfig = [];
      pConfig.push(dispatch('cargarUsuarioActual'));
      Promise.all(pConfig).then( () => {
          commit('updateConfigState', true);
      })
      
    },
    async cargarUsuarioActual({ commit }) {            
      commit('updateCurrentUserState', await API.getCurrentUser());
    },
  },
  modules: {
  }
})
