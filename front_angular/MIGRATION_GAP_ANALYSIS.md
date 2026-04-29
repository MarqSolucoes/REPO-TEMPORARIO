# MIGRATION GAP ANALYSIS (Vue -> Angular)

## Objetivo
Concluir paridade **100% funcional e visual** entre `/FRONT` (Vue) e `/front_angular` (Angular).

## Status real atual
As páginas Angular foram criadas e roteadas, porém a maioria está em **slice inicial** (`[~]`), sem todos os fluxos/modais/regras de negócio existentes no Vue.

## Gaps críticos por domínio

### 1) Dashboard
- Fluxos de wizard e modais incompletos (serviço/material, aprovações, validações).
- Integrações de API parciais comparadas ao Vue.
- Ausência de componentes equivalentes (DatePickerMask, multiselect, money input) com comportamento idêntico.

### 2) Compras (OrdemCompra/Cotação/Conciliação)
- Tabelas e ações complexas não migradas (edições, aprovações, anexos, filtros avançados).
- Contratos de payload precisam alinhamento fino por ação.

### 3) Obras e ObraDetalhe
- Cadastro/edição parcial; campos de negócio e modais de medição/ETO incompletos.
- Regras de cálculo e dependências entre campos ausentes.

### 4) Financeiro (todas páginas)
- Páginas criadas com listagens básicas; sem paridade de filtros, operações, ajustes, aprovações e exportações.
- Endpoints auxiliares e payloads especializados ainda não espelhados integralmente.

### 5) Configurações/RH/Error
- Estruturas iniciais; faltam CRUDs completos, permissões, componentes filhos e validações.

## Plano de fechamento (obrigatório para concluir)
1. Converter página por página replicando template, lógica, estilos, componentes filhos e contratos.
2. Validar endpoint por endpoint (request/response) sem alterar backend.
3. Validar UX visual comparativa por tela (Vue x Angular).
4. Marcar cada página como `[x]` apenas após checklist de paridade.

## Definição de pronto por página
- [ ] Templates e estilos com equivalência visual.
- [ ] Fluxos e regras de negócio equivalentes.
- [ ] Todas chamadas de API equivalentes (endpoint + payload + tratamento de erro).
- [ ] Navegação/permissões equivalentes.
- [ ] Teste manual com evidência.
