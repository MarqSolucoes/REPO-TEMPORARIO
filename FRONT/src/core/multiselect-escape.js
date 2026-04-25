/**
 * multiselect-escape.js
 * ---------------------
 * Fix global para o popup do `vue-multiselect`.
 *
 * PROBLEMA
 * --------
 * Os 96 usos de `<multiselect>` no projeto não passam
 * `:append-to-body="true"`. Resultado: quando um select é aberto dentro
 * de uma linha de tabela (`<td>` do `.table-card`), a lista de opções
 * (`.multiselect__content-wrapper`) é clipada pelo `overflow: auto` do
 * card e/ou sobrepõe as linhas de baixo da tabela.
 *
 * SOLUÇÃO
 * -------
 * Observa o DOM globalmente. Quando um `.multiselect` recebe a classe
 * `--active` (indicando que o select foi aberto):
 *   1. Teleporta o `.multiselect__content-wrapper` para o `<body>`
 *      (uma única vez por select).
 *   2. Aplica `position: fixed` e calcula `top`/`left` a partir do
 *      `getBoundingClientRect()` do próprio `.multiselect`.
 *   3. Se não couber abaixo, reposiciona acima (com a lista crescendo
 *      pra cima).
 *   4. Reposiciona em `scroll` e `resize` para acompanhar o input.
 *   5. Quando o select fecha (classe `--active` removida), o popup
 *      volta ao `display: none` nativamente; o elemento fica no body
 *      aguardando a próxima abertura — isso não causa problemas porque
 *      é invisível e não recebe eventos.
 *
 * Mesmo padrão do core/datepicker-escape.js.
 */

const ATTR_TELEPORTED = 'data-ms-teleported';
const CLASSE_ATIVA = 'multiselect--active';

function isVisivel(el) {
  if (!el) return false;
  const cs = window.getComputedStyle(el);
  return cs.display !== 'none' && cs.visibility !== 'hidden';
}

function reposicionaPopup(wrapper) {
  const origem = wrapper.__multiselectOrigem;
  if (!origem || !origem.isConnected) return;

  const rect = origem.getBoundingClientRect();
  const margem = 1;
  const folga = 8;

  let left = rect.left;
  // Força a largura do popup = largura do select (comportamento padrão
  // do vue-multiselect quando append-to-body está desligado)
  wrapper.style.width = rect.width + 'px';

  // Mede depois de aplicar a largura
  const alturaPopup = wrapper.offsetHeight || 200;

  // Padrão: abre abaixo
  let top = rect.bottom + margem;

  // Se não couber abaixo, tenta acima
  if (top + alturaPopup > window.innerHeight - folga) {
    const topAcima = rect.top - alturaPopup - margem;
    if (topAcima >= folga) top = topAcima;
  }

  wrapper.style.position = 'fixed';
  wrapper.style.top = top + 'px';
  wrapper.style.left = left + 'px';
  wrapper.style.zIndex = '99998';
}

function teleportaSePreciso(multiselect) {
  if (!multiselect.classList.contains(CLASSE_ATIVA)) return;

  const wrapper = multiselect.querySelector('.multiselect__content-wrapper');
  if (!wrapper) return;

  if (wrapper.getAttribute(ATTR_TELEPORTED) === 'true') {
    // Já teleportado antes — só reposiciona
    if (isVisivel(wrapper)) reposicionaPopup(wrapper);
    return;
  }

  wrapper.__multiselectOrigem = multiselect;
  document.body.appendChild(wrapper);
  wrapper.setAttribute(ATTR_TELEPORTED, 'true');
  reposicionaPopup(wrapper);
}

function reposicionaTodosVisiveis() {
  const wrappers = document.querySelectorAll(
    '.multiselect__content-wrapper[' + ATTR_TELEPORTED + '="true"]'
  );
  wrappers.forEach((w) => {
    // Só reposiciona se o multiselect-fonte ainda estiver ativo (aberto)
    const origem = w.__multiselectOrigem;
    if (!origem || !origem.isConnected) return;
    if (!origem.classList.contains(CLASSE_ATIVA)) return;
    if (isVisivel(w)) reposicionaPopup(w);
  });
}

function instalar() {
  // Processa multiselects já abertos no DOM (improvável, mas por garantia)
  document
    .querySelectorAll('.multiselect.' + CLASSE_ATIVA)
    .forEach(teleportaSePreciso);

  // MutationObserver: detecta mudanças de classe em `.multiselect`
  const observer = new MutationObserver((mutations) => {
    for (const m of mutations) {
      if (
        m.type === 'attributes' &&
        m.attributeName === 'class' &&
        m.target.classList &&
        m.target.classList.contains('multiselect')
      ) {
        teleportaSePreciso(m.target);
      }
    }
  });

  observer.observe(document.body, {
    subtree: true,
    attributes: true,
    attributeFilter: ['class'],
  });

  // Reposiciona em scroll/resize
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
