/**
 * datepicker-escape.js
 * --------------------
 * Fix global para o popup do `vue2-datepicker-mask`.
 *
 * PROBLEMA
 * --------
 * O componente `DatePickerMask` usado no projeto (pacote
 * `vue2-datepicker-mask`) não propaga a prop `:append-to-body="true"`
 * para o `vue2-datepicker` interno. Resultado: o popup do calendário
 * é renderizado como filho do `<div class="mx-datepicker">`, que fica
 * dentro de um `<td>` da tabela — e é visualmente cortado pelo
 * `overflow: auto` do `.table-card`.
 *
 * SOLUÇÃO
 * -------
 * Observa o DOM globalmente. Quando detecta um `.mx-datepicker-popup`
 * ficando visível:
 *   1. Teleporta o popup para o `<body>` (uma única vez por popup).
 *   2. Aplica `position: fixed` e calcula `top`/`left` a partir do
 *      `getBoundingClientRect()` do `.mx-datepicker` pai original.
 *   3. Se o popup não couber abaixo do input, reposiciona acima.
 *   4. Reposiciona em eventos `scroll` e `resize` para acompanhar o
 *      input caso a página role com o popup aberto.
 *
 * OBSERVAÇÕES
 * -----------
 * - O popup continua sendo controlado pelo Vue (show/hide via
 *   `style.display`). Só o elemento DOM muda de parent.
 * - Não mexe em nenhum componente Vue — é um add-on DOM puro.
 * - Se no futuro trocarmos o wrapper por um que suporte append-to-body
 *   nativamente, este arquivo pode ser descartado sem efeitos colaterais.
 */

const ATTR_TELEPORTED = 'data-dp-teleported';

function isVisivel(el) {
  if (!el) return false;
  if (el.style && el.style.display === 'none') return false;
  const cs = window.getComputedStyle(el);
  return cs.display !== 'none' && cs.visibility !== 'hidden';
}

function reposicionaPopup(popup) {
  // Pega o datepicker-fonte: guardamos ele em propriedade custom no
  // momento do teleporte (o popup não tem mais o parent original).
  const origem = popup.__datepickerOrigem;
  if (!origem || !origem.isConnected) return;

  const rect = origem.getBoundingClientRect();
  const margem = 2;
  const folga = 8;

  // Largura: se o popup tem largura menor que a do input, alinha pela
  // esquerda do input; senão deixa a esquerda do input.
  let left = rect.left;
  // Garante que não vaze pela direita da viewport
  const larguraPopup = popup.offsetWidth || 250;
  if (left + larguraPopup > window.innerWidth - folga) {
    left = Math.max(folga, window.innerWidth - larguraPopup - folga);
  }

  // Padrão: abre abaixo do input
  let top = rect.bottom + margem;
  const alturaPopup = popup.offsetHeight || 260;

  // Se não couber abaixo, tenta acima
  if (top + alturaPopup > window.innerHeight - folga) {
    const topAcima = rect.top - alturaPopup - margem;
    if (topAcima >= folga) top = topAcima;
  }

  popup.style.position = 'fixed';
  popup.style.top = top + 'px';
  popup.style.left = left + 'px';
  popup.style.zIndex = '99999';
}

function teleportaSePreciso(popup) {
  // Só faz o trabalho uma vez por popup
  if (popup.getAttribute(ATTR_TELEPORTED) === 'true') {
    if (isVisivel(popup)) reposicionaPopup(popup);
    return;
  }
  if (!isVisivel(popup)) return;

  // Localiza o datepicker-fonte ANTES de mover o popup
  const origem = popup.closest('.mx-datepicker');
  if (!origem) return;

  // Se já está no body (caso alguém configure append-to-body no futuro),
  // só marca e reposiciona.
  if (popup.parentNode === document.body) {
    popup.__datepickerOrigem = origem;
    popup.setAttribute(ATTR_TELEPORTED, 'true');
    reposicionaPopup(popup);
    return;
  }

  popup.__datepickerOrigem = origem;
  document.body.appendChild(popup);
  popup.setAttribute(ATTR_TELEPORTED, 'true');
  reposicionaPopup(popup);
}

function reposicionaTodosVisiveis() {
  const popups = document.querySelectorAll(
    '.mx-datepicker-popup[' + ATTR_TELEPORTED + '="true"]'
  );
  popups.forEach((p) => {
    if (isVisivel(p)) reposicionaPopup(p);
  });
}

function instalar() {
  // 1) Processa popups já no DOM (caso algum tenha sido renderizado
  //    antes do script rodar)
  document
    .querySelectorAll('.mx-datepicker-popup')
    .forEach(teleportaSePreciso);

  // 2) MutationObserver: detecta popups novos E mudanças de style
  //    (o popup abre/fecha via style.display alterado pelo Vue)
  const observer = new MutationObserver((mutations) => {
    for (const m of mutations) {
      if (m.type === 'childList') {
        m.addedNodes.forEach((node) => {
          if (node.nodeType !== 1) return;
          if (
            node.classList &&
            node.classList.contains('mx-datepicker-popup')
          ) {
            teleportaSePreciso(node);
          } else if (node.querySelectorAll) {
            node
              .querySelectorAll('.mx-datepicker-popup')
              .forEach(teleportaSePreciso);
          }
        });
      } else if (
        m.type === 'attributes' &&
        m.attributeName === 'style' &&
        m.target.classList &&
        m.target.classList.contains('mx-datepicker-popup')
      ) {
        teleportaSePreciso(m.target);
      }
    }
  });

  observer.observe(document.body, {
    childList: true,
    subtree: true,
    attributes: true,
    attributeFilter: ['style'],
  });

  // 3) Reposiciona em scroll/resize para acompanhar o input se a
  //    página rolar com o popup aberto.
  window.addEventListener('scroll', reposicionaTodosVisiveis, true);
  window.addEventListener('resize', reposicionaTodosVisiveis);
}

if (typeof window !== 'undefined') {
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', instalar);
  } else {
    instalar();
  }
}
