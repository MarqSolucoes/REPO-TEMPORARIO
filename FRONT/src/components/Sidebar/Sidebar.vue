<template>
  <b-collapse
    class="sidebar-collapse"
    id="sidebar-collapse"
    :visible="sidebarOpened"
  >
    <nav :class="{ sidebar: true }">
      <header class="logo">
        <router-link to="/app"
          >WH <span class="fw-semi-bold">Engenharia</span></router-link
        >
      </header>

      <ul class="nav">
        <NavLink
          :activeItem="activeItem"
          header="Inicio"
          link="/app/dashboard"
          iconName="flaticon-home"
          index="dashboard"
          isHeader
        />

        <NavLink
          :activeItem="activeItem"
          v-if="permissao_MenuCadastros"
          header="Cadastros"
          link="/app/configuracoes"
          iconName="flaticon-home"
          index="configuracoes"
          :childrenLinks="cadastros"
        />

        <NavLink
          :activeItem="activeItem"
          header="Obras"
          link="/app/obras"
          iconName="flaticon-list"
          index="obras"
          isHeader
        />

        <NavLink
          :activeItem="activeItem"
          header="Compras"
          link="/app/compras"
          iconName="flaticon-more"
          index="compras"
          :childrenLinks="[
            { header: 'Processo de Compra', link: '/app/compras/ordemCompra/EmCotacao' },
            { header: 'Conciliação NF', link: '/app/compras/conciliacao' },
          ]"
        />

        <NavLink
          :activeItem="activeItem"
          header="Financeiro"
          link="/app/financeiro"
          iconName="flaticon-home"
          index="financeiro"
          :childrenLinks="[
            { header: 'Agenda', link: '/app/relatorio/agenda' },
            { header: 'ETO', link: '/app/financeiro/ETO' },
            { header: 'Entradas', link: '/app/financeiro/Entradas' },
            { header: 'Faturamento', link: '/app/financeiro/faturamento' },
            // { header: 'Notas Fiscais', link: '/app/financeiro/notasFiscais' },
            // { header: 'PI', link: '/app/financeiro/pedidoInterno' },
            { header: 'PI Recorrente', link: '/app/financeiro/pedidoInternoRecorrente' },
            { header: 'Fluxo Caixa', link: '/app/financeiro/fluxoCaixaConsolidado' },
          ]"
        />

        <NavLink
          :activeItem="activeItem"
          header="RH"
          link="/app/rh"
          iconName="flaticon-home"
          index="rh"
          :childrenLinks="[
            { header: 'RH', link: '/app/rh' },
          ]"
        />

        <NavLink
          :activeItem="activeItem"
          header="Relatorios"
          link="/app/relatorios"
          iconName="flaticon-document"
          index="relatorios"
          :childrenLinks="[
            { header: 'Faturamento', link: '/app/relatorio/faturamento' },
            // { header: 'Ajuste ETO', link: '/app/relatorio/ETO' },
            { header: 'Pedidos Compra', link: '/app/relatorio/pedidoCompra' },
            { header: 'Histórico', link: '/app/relatorio/historico' },
            { header: 'Materiais', link: '/app/relatorio/material' },
            // { header: 'Obra - CC', link: '/app/relatorio/faturamento' },
            // { header: 'Obra - ETO', link: '/app/relatorio/faturamento' },
            // { header: 'Obra - Resumo ETO', link: '/app/relatorio/faturamento' },
            
          ]"
        />
        
      </ul>
    </nav>
  </b-collapse>
</template>

<script>
import { mapState, mapActions } from "vuex";
import NavLink from "./NavLink/NavLink";

export default {
  name: "Sidebar",
  components: { NavLink },
  data() {
    return {
      cadastros: [],
      
      permissao_MenuCadastros: true,
      permissao_Cargos: false,
      permissao_Clientes: false,
      permissao_Fornecedor: false,
      permissao_Material: false,
      permissao_MaterialCategoria: false,
      permissao_Usuarios: false,
    };
  },
  methods: {
    ...mapActions("layout", ["changeSidebarActive", "switchSidebar"]),
    setActiveByRoute() {
      const paths = this.$route.fullPath.split("/");
      paths.pop();
      this.changeSidebarActive(paths.join("/"));
    },
  },
  created() {
    this.setActiveByRoute();
  },
  computed: {
    ...mapState("layout", {
      sidebarOpened: (state) => !state.sidebarClose,
      activeItem: (state) => state.sidebarActiveElement,
    }),
  },
  mounted(){
    // var perfil = JSON.parse(localStorage.getItem('usuarioDTO')).perfil;
    
    // this.permissao_MenuCadastros = perfil.menuCadastros;

    this.cadastros.push({ header: 'Cidades', link: '/app/configuracoes/cidade',  });
    // if(perfil.cargo) this.cadastros.push({ header: 'Cargos', link: '/app/configuracoes/cargo',  });
    // if(perfil.cliente) this.cadastros.push({ header: 'Clientes', link: '/app/configuracoes/cliente',  });
    // if(perfil.fornecedor) this.cadastros.push({ header: 'Fornecedores', link: '/app/configuracoes/fornecedor',  });
    // if(perfil.material) this.cadastros.push({ header: 'Materiais', link: '/app/configuracoes/material/material',  });
    // if(perfil.materialCategoria) this.cadastros.push({ header: 'Categorias Materiais', link: '/app/configuracoes/material/materialCategoria',  });
    // if(perfil.perfis) this.cadastros.push({ header: 'Perfis', link: '/app/configuracoes/perfil',  });
    // if(perfil.usuario) this.cadastros.push({ header: 'Usuários', link: '/app/configuracoes/usuario',  });

    this.cadastros.push({ header: 'Cargos', link: '/app/configuracoes/cargo',  });
    this.cadastros.push({ header: 'Clientes', link: '/app/configuracoes/cliente',  });
    this.cadastros.push({ header: 'DEF', link: '/app/financeiro/DEF',  });
    this.cadastros.push({ header: 'Fornecedores', link: '/app/configuracoes/fornecedor',  });
    this.cadastros.push({ header: 'Materiais / Serviços', link: '/app/configuracoes/material/material',  });
    this.cadastros.push({ header: 'Categorias Materiais', link: '/app/configuracoes/material/materialCategoria',  });
    this.cadastros.push({ header: 'Usuários', link: '/app/configuracoes/usuario',  });
    
  }
};
</script>

<!-- Sidebar styles should be scoped -->
<style src="./Sidebar.scss" lang="scss" scoped/>
