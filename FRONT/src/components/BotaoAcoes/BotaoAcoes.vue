<template>
  <div class="botao-acoes" ref="raiz">
    <button
      ref="botao"
      type="button"
      class="botao-acoes__botao"
      :class="classeVariante"
      :disabled="desabilitado"
      @click.stop="alternar"
    >
      <span class="botao-acoes__texto">{{ texto }}</span>
      <span class="botao-acoes__caret" aria-hidden="true"></span>
    </button>

    <ul
      v-show="aberto"
      ref="menu"
      class="botao-acoes__menu"
      :style="estiloMenu"
      role="menu"
      @click.stop
    >
      <li
        v-for="(item, i) in itensVisiveis"
        :key="i"
        class="botao-acoes__item"
        :class="{ 'botao-acoes__item--desabilitado': item.disabled }"
        role="menuitem"
        @click="selecionar(item)"
      >
        <i
          v-if="item.icone"
          :class="['fa', 'fa-' + item.icone, 'botao-acoes__icone']"
          aria-hidden="true"
        ></i>
        <span>{{ item.label }}</span>
      </li>
      <li
        v-if="itensVisiveis.length === 0"
        class="botao-acoes__item botao-acoes__item--desabilitado"
      >
        <span>Sem ações disponíveis</span>
      </li>
    </ul>
  </div>
</template>

<script>
/**
 * BotaoAcoes
 * ----------
 * Substituto do padrão <b-dropdown text="Ações" ...> + <b-dropdown-item> usado em
 * tabelas de listagem.
 *
 * Por que existir:
 *   - O b-dropdown do BootstrapVue é renderizado dentro do wrapper da tabela.
 *     Quando o wrapper tem overflow (ex.: .table-card com overflow-x: auto),
 *     o menu é cortado. Os "hacks" anteriores (boundary="window",
 *     :has(.dropdown-menu.show) { overflow: visible }) quebravam o layout da
 *     tabela ao abrir o menu.
 *   - Este componente renderiza o menu com position: fixed e calcula as
 *     coordenadas em JS a partir do getBoundingClientRect() do botão, então o
 *     menu NUNCA é cortado por um ancestral com overflow, e o wrapper da
 *     tabela NÃO precisa ser modificado quando o menu abre.
 *
 * API:
 *
 *   <BotaoAcoes
 *     :itens="[
 *       { label: 'Editar', onClick: () => editar(row.id) },
 *       { label: 'Cancelar', onClick: () => cancelar(row), visible: podeCancelar },
 *       { label: 'Excluir', onClick: () => excluir(row.id), disabled: !podeExcluir, icone: 'trash' },
 *     ]"
 *     variante="success"
 *     texto="Ações"
 *     alinhamento="direita"
 *   />
 *
 * Cada item aceita:
 *   - label    (string)     : texto exibido (obrigatório)
 *   - onClick  (function)   : callback ao clicar (obrigatório)
 *   - visible  (boolean)    : se falso, o item não aparece (padrão true)
 *   - disabled (boolean)    : se verdadeiro, item fica acinzentado e não clicável
 *   - icone    (string)     : classe do FontAwesome sem o prefixo "fa-" (opcional)
 *
 * Props:
 *   - itens        : Array<Item>                 (obrigatório)
 *   - texto        : string                      (padrão: "Ações")
 *   - variante     : 'success' | 'info' | 'primary' | 'danger' | 'warning' | 'secondary'
 *                                                (padrão: 'success')
 *   - desabilitado : boolean                     (padrão: false)
 *   - alinhamento  : 'direita' | 'esquerda'      (padrão: 'direita')
 */
export default {
  name: "BotaoAcoes",

  props: {
    itens: {
      type: Array,
      required: true,
    },
    texto: {
      type: String,
      default: "Ações",
    },
    variante: {
      type: String,
      default: "success",
      validator: (v) =>
        ["success", "info", "primary", "danger", "warning", "secondary"].includes(v),
    },
    desabilitado: {
      type: Boolean,
      default: false,
    },
    alinhamento: {
      type: String,
      default: "direita",
      validator: (v) => ["direita", "esquerda"].includes(v),
    },
  },

  data() {
    return {
      aberto: false,
      estiloMenu: {
        top: "0px",
        left: "0px",
      },
    };
  },

  computed: {
    classeVariante() {
      return "botao-acoes__botao--" + this.variante;
    },
    itensVisiveis() {
      return (this.itens || []).filter((i) => i && i.visible !== false);
    },
  },

  methods: {
    alternar() {
      if (this.desabilitado) return;
      if (this.aberto) this.fechar();
      else this.abrir();
    },

    abrir() {
      this.aberto = true;
      // espera o menu ser renderizado pra medir e posicionar
      this.$nextTick(() => {
        this.atualizarPosicao();
        document.addEventListener("click", this.aoClicarFora, true);
        document.addEventListener("keydown", this.aoTeclar, true);
        window.addEventListener("scroll", this.fechar, true);
        window.addEventListener("resize", this.fechar, true);
      });
    },

    fechar() {
      this.aberto = false;
      document.removeEventListener("click", this.aoClicarFora, true);
      document.removeEventListener("keydown", this.aoTeclar, true);
      window.removeEventListener("scroll", this.fechar, true);
      window.removeEventListener("resize", this.fechar, true);
    },

    aoClicarFora(ev) {
      const raiz = this.$refs.raiz;
      const menu = this.$refs.menu;
      if (!raiz) return;
      if (raiz.contains(ev.target)) return;
      if (menu && menu.contains(ev.target)) return;
      this.fechar();
    },

    aoTeclar(ev) {
      if (ev.key === "Escape") this.fechar();
    },

    atualizarPosicao() {
      const botao = this.$refs.botao;
      const menu = this.$refs.menu;
      if (!botao || !menu) return;

      const rBotao = botao.getBoundingClientRect();
      const larguraMenu = menu.offsetWidth;
      const alturaMenu = menu.offsetHeight;
      const margem = 4;
      const folga = 8;

      let top = rBotao.bottom + margem;
      let left =
        this.alinhamento === "esquerda"
          ? rBotao.left
          : rBotao.right - larguraMenu;

      // Se não couber em baixo, abre pra cima
      if (top + alturaMenu > window.innerHeight - folga) {
        const topAcima = rBotao.top - alturaMenu - margem;
        if (topAcima >= folga) top = topAcima;
      }
      // Evita sair pela esquerda/direita
      if (left < folga) left = folga;
      if (left + larguraMenu > window.innerWidth - folga) {
        left = window.innerWidth - larguraMenu - folga;
      }

      this.estiloMenu = {
        top: top + "px",
        left: left + "px",
      };
    },

    selecionar(item) {
      if (!item || item.disabled) return;
      this.fechar();
      if (typeof item.onClick === "function") {
        item.onClick();
      }
    },
  },

  beforeDestroy() {
    document.removeEventListener("click", this.aoClicarFora, true);
    document.removeEventListener("keydown", this.aoTeclar, true);
    window.removeEventListener("scroll", this.fechar, true);
    window.removeEventListener("resize", this.fechar, true);
  },
};
</script>

<style lang="scss">
.botao-acoes {
  display: inline-block;
  position: relative;
}

.botao-acoes__botao {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 6px 14px;
  border: 1px solid transparent;
  border-radius: 4px;
  color: #fff;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  user-select: none;
  transition: background-color 0.12s ease, border-color 0.12s ease,
    filter 0.12s ease;
}

.botao-acoes__botao:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.botao-acoes__botao:focus {
  outline: none;
  box-shadow: 0 0 0 3px rgba(255, 255, 255, 0.15);
}

/* Variantes de cor — usam as cores padrão do Bootstrap */
.botao-acoes__botao--success {
  background-color: #28a745;
  border-color: #28a745;
}
.botao-acoes__botao--success:hover:not(:disabled) {
  background-color: #218838;
  border-color: #1e7e34;
}

.botao-acoes__botao--info {
  background-color: #17a2b8;
  border-color: #17a2b8;
}
.botao-acoes__botao--info:hover:not(:disabled) {
  background-color: #138496;
  border-color: #117a8b;
}

.botao-acoes__botao--primary {
  background-color: #007bff;
  border-color: #007bff;
}
.botao-acoes__botao--primary:hover:not(:disabled) {
  background-color: #0069d9;
  border-color: #0062cc;
}

.botao-acoes__botao--danger {
  background-color: #dc3545;
  border-color: #dc3545;
}
.botao-acoes__botao--danger:hover:not(:disabled) {
  background-color: #c82333;
  border-color: #bd2130;
}

.botao-acoes__botao--warning {
  background-color: #ffc107;
  border-color: #ffc107;
  color: #212529;
}
.botao-acoes__botao--warning:hover:not(:disabled) {
  background-color: #e0a800;
  border-color: #d39e00;
}

.botao-acoes__botao--secondary {
  background-color: #6c757d;
  border-color: #6c757d;
}
.botao-acoes__botao--secondary:hover:not(:disabled) {
  background-color: #5a6268;
  border-color: #545b62;
}

.botao-acoes__caret {
  display: inline-block;
  width: 0;
  height: 0;
  margin-left: 4px;
  border-left: 4px solid transparent;
  border-right: 4px solid transparent;
  border-top: 5px solid currentColor;
}

/* Menu flutuante — position: fixed para escapar de qualquer wrapper com
   overflow (table-card, modais, etc.). As coordenadas são atribuídas via JS. */
.botao-acoes__menu {
  position: fixed;
  z-index: 2000;
  list-style: none;
  margin: 0;
  padding: 4px 0;
  min-width: 180px;
  background: #1a1a1a;
  border: 1px solid rgba(255, 255, 255, 0.12);
  border-radius: 6px;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.5);
}

.botao-acoes__item {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 14px;
  color: #fff;
  font-size: 14px;
  line-height: 1.3;
  cursor: pointer;
  user-select: none;
  white-space: nowrap;
}

.botao-acoes__item:hover:not(.botao-acoes__item--desabilitado) {
  background: rgba(255, 255, 255, 0.08);
}

.botao-acoes__item--desabilitado {
  opacity: 0.5;
  cursor: not-allowed;
}

.botao-acoes__icone {
  width: 14px;
  text-align: center;
  flex-shrink: 0;
}
</style>
