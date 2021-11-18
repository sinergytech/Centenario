<template>
  <div class="content">
    <div class="block">
        <div class="block-header">
            <h3 class="block-title">Configuración de Listas</h3>
        </div>
        <div class="block-content">
            <div class="form-horizontal">
                <div class="form-group row">
                    <div class="col-md-3 col-sm-6 u-mt10">
                        <div class="col-md-12 col-sm-6 u-mt10">
                            <div class="busqueda form-group">
                                <input placeholder="Buscar" class="form-control" type="text" v-model="textoBusqueda">
                            </div>
                        </div>
                    </div>
                     <div class="col-md-4 col-sm-6 u-mt10">
                        <div class="busqueda form-group">
                            <b-button variant="danger" title="Buscar" @click="BuscarListas()"><i class="fa fa-search u-hover"></i></b-button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="table-responsive">
                        <table class="table MainTable MainTable--shorterPadding">
                            <thead class="u-pointer"> 
                                <tr>
                                    <th class="w-10" text-align="left">Lista</th>
                                    <th class="w-50" text-align="center"></th>
                                </tr>
                            </thead>
                            <tbody class="u-pointer">
                                <tr :key="index" v-for="(acceso, index) in configuracionAccesos">
                                    <td>{{ acceso.Title }}</td>
                                    <td><b-button @click="HrefNav(acceso.Url)" variant="success"><i class="fa fa-cog" title="Configurar"></i></b-button></td>
                                </tr>
                                <tr>
                                    <td v-if="!configuracionAccesos.length" colspan="8" class="text-center">
                                        <i class="fa fa-inbox fa-3x" aria-hidden="true"></i>
                                        <p>Esta sección está vacía</p>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
  </div>
</template>

<script>
import API from '../api';
import { CONSTANTES, LISTAS, SUBSITIOS, removerTildes} from "../utils";

export default {
  name: 'Home',
  components: {
  },
  data(){
      return {
          roles: [],
          configuracionAccesos: [],
          configuracionAccesosTemp: [],
          ejecutivos: [],
          textoBusqueda: ''
      }
  },
  created(){
    API.getListItems(SUBSITIOS.Renta, LISTAS.EJECUTIVOS_MINKA, null, ['Ejecutivo/Id', 'Ejecutivo/Title', 'Rol/Codigo', 'Rol/Title'], ['Ejecutivo', 'Rol']).then( ejecutivosminka => {
        this.ejecutivos = ejecutivosminka;
        API.getListItems(SUBSITIOS.Renta, LISTAS.EJECUTIVOS_RI, null, ['Ejecutivo/Id', 'Ejecutivo/Title', 'Rol/Codigo', 'Rol/Title'], ['Ejecutivo', 'Rol']).then( ejecutivosri => {
            if(this.ejecutivos.length > 0)
            {
                let ejecutivosUnidos = this.ejecutivos.concat(ejecutivosri);
                this.ejecutivos = ejecutivosUnidos;
            }
        })
    }).then(() => {
        API.getListItems(null, LISTAS.CONFIGURACIONACCESOS, {SubSitio: CONSTANTES.RENTA_INMOBILIARIA}, null).then( configuracionAccesos => {
            let idUsuario = this.$store.state.currentUser.Id;
            this.ejecutivos = this.ejecutivos.filter(x => x.Ejecutivo.Id == idUsuario);
            let confAccesos =configuracionAccesos.filter(x => x.SubSitio == CONSTANTES.RENTA_INMOBILIARIA);
            let confAccesostemp = [];
            for (let i = 0; i < this.ejecutivos.length; i++) {
                confAccesostemp = confAccesostemp.concat(confAccesos.filter(x => x.CodigoRol == this.ejecutivos[i].Rol.Codigo))
            }
            this.configuracionAccesos = confAccesostemp.filter(function(item, pos, self) {
                return self.indexOf(item) == pos;
            });
            this.configuracionAccesosTemp = this.configuracionAccesos;
        })
    })
    
    API.getListItems(SUBSITIOS.Renta, LISTAS.ROLES, null, ['Id', 'Title']).then( roles => {
        this.roles = roles;            
    })
  },
  methods:{
    HrefNav(url)
    {
        window.open(url, '_blank')
    },
    BuscarListas()
    {
        this.configuracionAccesos =  !this.textoBusqueda ? this.configuracionAccesosTemp : this.configuracionAccesosTemp.filter( c => {
                let cadena = [
                    c.Title,
                    c.Url
                ].join('|').toLowerCase();
                return removerTildes(cadena).indexOf(removerTildes(this.textoBusqueda.toLowerCase())) > -1;
            })
    }
  }
}
</script>