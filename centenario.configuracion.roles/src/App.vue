<template>
  <div id="app">
    <!-- <div id="nav">
      <router-link to="/">Home</router-link> |
      <router-link to="/about">About</router-link>
    </div> -->
    <router-view/>
  </div>
</template>

<script>
import { mapActions } from "vuex";
export default {
    name: 'App',
    components: {
    },
    created(){
        this.cargarConfiguracion();        
    },
    watch: {
    },
    computed: {
        configLista(){
            return this.$store.state.configuracionCargada;
        }
    },
    methods:{
        ...mapActions([
            'cargarConfiguracion'
        ])
    }
}
</script>
