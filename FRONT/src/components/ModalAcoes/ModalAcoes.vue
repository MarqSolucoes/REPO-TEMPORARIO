<template>
  <b-modal
    v-model="abrir"
    :title="titulo"
    hide-footer
    centered
    size="md"
    body-class="modal-acoes__corpo"
    header-class="modal-acoes__cabecalho"
    no-fade
  >
    <div class="modal-acoes__lista">
      <button
        v-for="(item, i) in itensVisiveis"
        :key="i"
        type="button"
        class="modal-acoes__item"
        :class="'modal-acoes__item--' + (item.variante || 'padrao')"
        :disabled="item.disabled"
        @click="selecionar(item)"
      >
        <i
          v-if="item.icone"
          :class="['fa', 'fa-' + item.icone, 'modal-acoes__icone']"
          aria-hidden="true"
        ></i>
        <span class="modal-acoes__texto">
          <span class="modal-acoes__label">{{ item.label }}</span>
          <span v-if="item.descricao" class="modal-acoes__descricao">
            {{ item.descricao }}
          </span>
        </span>
        <i class="fa fa-angle-right modal-acoes__chevron" aria-hidden="true"></i>
      </button>
      <div v-if="itensVisiveis.length === 0" class="modal-acoes__vazio">
        Nenhuma ação disponível
      </div>
    </div>
  </b-modal>
</template>

<script>
/**
 * ModalAcoes
 * ----------
 * Alternativa a dropdowns de "Ações" em linhas de tabela. Em vez de abrir
 * um menu flutuante (que sofre com overflow dos wrappers), abre um modal
 * centralizado com botões grandes empilhados — uma ação por linha.
 *
 * Vantagens frente ao dropdown:
 *   - Modal vive no <body>, imune a qualquer overflow de ancestral
 *   - Funciona bem em mobile (área de toque maior)
 *   - Ações ficam mais visíveis e acessíveis
 *   - Suporta agrupamento por cor (variante) e ícone
 *
 * Uso:
 *
 *   <ModalAcoes
 *     :exibir.sync="modalAcoes_Exibir"
 *     :titulo="modalAcoes_Titulo"
 *     :itens="modalAcoes_Itens"
 *   />
 *
 * Onde cada item aceita:
 *   - label     (string)   : texto principal (obrigatório)
 *   - onClick   (function) : callback ao clicar (obrigatório)
 *   - visible   (boolean)  : se falso, o item não aparece (padrão true)
 *   - disabled  (boolean)  : se verdadeiro, item fica desabilitado
 *   - icone     (string)   : classe FontAwesome sem prefixo "fa-" (opcional)
 *   - variante  (string)   : 'padrao' | 'success' | 'danger' | 'info' |
 *                            'warning' | 'primary' (opcional)
 *   - descricao (string)   : subtítulo explicativo (opcional)
 *
 * Fluxo:
 *   1. O callback onClick só dispara após o fechamento do modal, no
 *      próximo tick, evitando conflitos de DOM quando a ação abre outro
 *      modal (ex.: abrir modal de comentários, cancelar etc.).
 */
export default {
  name: "ModalAcoes",

  props: {
    exibir: {
      type: Boolean,
      default: false,
    },
    titulo: {
      type: String,
      default: "Ações",
    },
    itens: {
      type: Array,
      default: () => [],
    },
  },

  computed: {
    abrir: {
      get() {
        return this.exibir;
      },
      set(v) {
        this.$emit("update:exibir", v);
      },
    },
    itensVisiveis() {
      return (this.itens || []).filter((i) => i && i.visible !== false);
    },
  },

  methods: {
    selecionar(item) {
      if (!item || item.disabled) return;
      this.abrir = false;
      if (typeof item.onClick === "function") {
        // roda no próximo tick pra não colidir com o fechamento do
        // próprio modal (especialmente quando a ação abre outro modal)
        this.$nextTick(() => item.onClick());
      }
    },
  },
};
</script>

<style lang="scss">
/* O b-modal do BootstrapVue renderiza o .modal no <body> com Vue Portal,
   então as classes de body/header/content precisam de especificidade
   global — não escopar por `scoped`. */

.modal-acoes__cabecalho {
  background: #1a1a1a;
  border-bottom: 1px solid rgba(255, 255, 255, 0.12);
  color: #fff;

  .modal-title {
    color: #fff;
    font-weight: 600;
  }

  .close {
    color: #fff;
    text-shadow: none;
    opacity: 0.7;
    transition: opacity 0.15s ease;
  }

  .close:hover {
    opacity: 1;
  }
}

.modal-acoes__corpo {
  background: #1a1a1a;
  color: #fff;
  padding: 16px;
}

.modal-acoes__lista {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.modal-acoes__item {
  display: flex;
  align-items: center;
  gap: 12px;
  width: 100%;
  padding: 12px 14px;
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.12);
  border-radius: 6px;
  color: #fff;
  font-size: 14px;
  font-weight: 500;
  text-align: left;
  cursor: pointer;
  transition: background-color 0.15s ease, border-color 0.15s ease,
    transform 0.08s ease;
}

.modal-acoes__item:hover:not(:disabled) {
  background: rgba(255, 255, 255, 0.12);
  border-color: rgba(255, 255, 255, 0.22);
}

.modal-acoes__item:active:not(:disabled) {
  transform: translateY(1px);
}

.modal-acoes__item:focus {
  outline: none;
  box-shadow: 0 0 0 3px rgba(255, 255, 255, 0.15);
}

.modal-acoes__item:disabled {
  opacity: 0.45;
  cursor: not-allowed;
}

/* Variantes de cor (tom sutil, cor de destaque no fundo + borda) */
.modal-acoes__item--success {
  background: rgba(40, 167, 69, 0.14);
  border-color: rgba(40, 167, 69, 0.45);
}
.modal-acoes__item--success:hover:not(:disabled) {
  background: rgba(40, 167, 69, 0.22);
  border-color: rgba(40, 167, 69, 0.65);
}

.modal-acoes__item--danger {
  background: rgba(220, 53, 69, 0.14);
  border-color: rgba(220, 53, 69, 0.45);
}
.modal-acoes__item--danger:hover:not(:disabled) {
  background: rgba(220, 53, 69, 0.22);
  border-color: rgba(220, 53, 69, 0.65);
}

.modal-acoes__item--info {
  background: rgba(23, 162, 184, 0.14);
  border-color: rgba(23, 162, 184, 0.45);
}
.modal-acoes__item--info:hover:not(:disabled) {
  background: rgba(23, 162, 184, 0.22);
  border-color: rgba(23, 162, 184, 0.65);
}

.modal-acoes__item--warning {
  background: rgba(255, 193, 7, 0.14);
  border-color: rgba(255, 193, 7, 0.45);
}
.modal-acoes__item--warning:hover:not(:disabled) {
  background: rgba(255, 193, 7, 0.22);
  border-color: rgba(255, 193, 7, 0.65);
}

.modal-acoes__item--primary {
  background: rgba(0, 123, 255, 0.14);
  border-color: rgba(0, 123, 255, 0.45);
}
.modal-acoes__item--primary:hover:not(:disabled) {
  background: rgba(0, 123, 255, 0.22);
  border-color: rgba(0, 123, 255, 0.65);
}

.modal-acoes__icone {
  width: 20px;
  text-align: center;
  flex-shrink: 0;
  font-size: 16px;
  opacity: 0.9;
}

.modal-acoes__texto {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 2px;
  min-width: 0;
}

.modal-acoes__label {
  font-weight: 500;
}

.modal-acoes__descricao {
  font-size: 12px;
  opacity: 0.65;
  font-weight: 400;
  line-height: 1.3;
}

.modal-acoes__chevron {
  opacity: 0.35;
  font-size: 14px;
  flex-shrink: 0;
}

.modal-acoes__vazio {
  padding: 24px 16px;
  text-align: center;
  color: rgba(255, 255, 255, 0.5);
  font-style: italic;
  font-size: 14px;
}
</style>
