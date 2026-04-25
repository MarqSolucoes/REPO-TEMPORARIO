<template>
  <div class="perfil-page">
    <loading
      :active.sync="isLoading"
      :can-cancel="false"
      :is-full-page="true"
      color="#ffd639"
    ></loading>
    <h1 class="page-title">Perfil &nbsp;</h1>

    <Widget customHeader class="estiloWidget">
      <b-row>
        <b-col cols="auto" class="mr-auto p-3"></b-col>
        <b-col cols="auto" style="padding: 0">
          <button
            v-if="permissao_Cadastrar"
            @click="
              modal_Titulo = 'Cadastro de Perfil';
              perfil_Id = 0;
              perfil_Descricao = '';
              perfil_Ativo = true;
              perfil_Cargo = true;
              perfil_CargoCadastrar = true;
              perfil_CargoAtivarDesativar = true;
              perfil_Cliente = true;
              perfil_ClienteCadastrar = true;
              perfil_ClienteEditar = true;
              perfil_ClienteAtivarDesativar = true;
              perfil_Fornecedor = true;
              perfil_FornecedorCadastrar = true;
              perfil_FornecedorEditar = true;
              perfil_FornecedorAtivarDesativar = true;
              perfil_Material = true;
              perfil_MaterialCadastrar = true;
              perfil_MaterialAtivarDesativar = true;
              perfil_CategoriaMaterial = true;
              perfil_CategoriaMaterialCadastrar = true;
              perfil_CategoriaMaterialAtivarDesativar = true;
              perfil_Perfil = true;
              perfil_PerfilCadastrar = true;
              perfil_PerfilEditar = true;
              perfil_PerfilAtivarDesativar = true;
              perfil_Usuario = true;
              perfil_UsuarioCadastrar = true;
              perfil_UsuarioEditar = true;
              perfil_UsuarioAtivarDesativar = true;
              perfil_UsuarioHabilitarDesabilitarLogin = true;
              perfil_UsuarioAlterarSenha = true;
              modal_ExibirBotaoCadastrar = true;
              modal_ExibirBotaoEditar = false;
              modal_InativaBotaoCadastrar = false;
              modal_InativaBotaoEditar = true;
              modal_InativaAtivaoInativo = false;
              modal_Exibir = true;
            "
            v-b-modal.cadastro
            type="button"
            class="btn width-120 mb-3 mr-4 btn-outline-success"
          >
            Novo Perfil
          </button>

          <b-modal
            id="cadastro"
            class="modal-dialog modal-md afm-modal"
            body-bg-variant="modal"
            header-bg-variant="bodyModal"
            footer-bg-variant="bodyModal"
            v-model="modal_Exibir"
            size="lg"
          >
            <div class="afm-hero">
              <div>
                <span class="afm-hero__eyebrow">Perfil</span>
                <h3 class="afm-hero__title">{{ modal_Titulo }}</h3>
                <p class="afm-hero__description">Defina descrição, status e permissões do perfil de acesso.</p>
              </div>
            </div>

            <b-container fluid class="afm-sections">
              <section class="afm-section-card">
              <b-row>
                <b-col>
                  <label class="mr-3">Descrição:</label>
                  <b-form-input
                    v-model="perfil_Descricao"
                    placeholder="Descrição do perfil"
                    style="color: white"
                  ></b-form-input>
                </b-col>
              </b-row>

              <br />

              <b-row>
                <b-col>
                  <toggle-button
                    v-model="perfil_Ativo"
                    :disabled="modal_InativaAtivaoInativo"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#000000',
                    }"
                    :labels="{ checked: 'Ativo', unchecked: 'Inativo' }"
                    :width="80"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>

              <br />

              <hr style="background-color: white" />

              <b-row>
                <b-col style="text-align: center">
                  <label class="mr-3" style="font-weight: bold; color: #ffffff"
                    >Cadastros</label
                  >
                </b-col>
              </b-row>

              <hr style="background-color: white" />

              <b-row>
                <b-col cols="8">
                  <label class="mr-3" style="font-weight: bold; color: #ffd600">Gerenciar Cargos</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_Cargo"
                    :disabled="controle_PerfilCadastrando"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Cadastrar</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_CargoCadastrar"
                    :disabled="!perfil_Cargo"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Ativar / Desativar</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_CargoAtivarDesativar"
                    :disabled="!perfil_Cargo"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>

              <hr style="background-color: white" />

              <b-row>
                <b-col cols="8">
                  <label class="mr-3" style="font-weight: bold; color: #ffd600">Gerenciar Clientes</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_Cliente"
                    :disabled="controle_PerfilCadastrando"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Cadastrar</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_ClienteCadastrar"
                    :disabled="!perfil_Cliente"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Editar</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_ClienteEditar"
                    :disabled="!perfil_Cliente"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Ativar / Desativar</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_ClienteAtivarDesativar"
                    :disabled="!perfil_Cliente"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>

              <hr style="background-color: white" />

              <b-row>
                <b-col cols="8">
                  <label class="mr-3" style="font-weight: bold; color: #ffd600">Gerenciar Fornecedores</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_Fornecedor"
                    :disabled="controle_PerfilCadastrando"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Cadastrar</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_FornecedorCadastrar"
                    :disabled="!perfil_Fornecedor"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Editar</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_FornecedorEditar"
                    :disabled="!perfil_Fornecedor"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Ativar / Desativar</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_FornecedorAtivarDesativar"
                    :disabled="!perfil_Fornecedor"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>

              <hr style="background-color: white" />

              <b-row>
                <b-col cols="8">
                  <label class="mr-3" style="font-weight: bold; color: #ffd600">Gerenciar Materiais</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_Material"
                    :disabled="controle_PerfilCadastrando"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Cadastrar</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_MaterialCadastrar"
                    :disabled="!perfil_Material"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Ativar / Desativar</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_MaterialAtivarDesativar"
                    :disabled="!perfil_Material"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>

              <hr style="background-color: white" />

              <b-row>
                <b-col cols="8">
                  <label class="mr-3" style="font-weight: bold; color: #ffd600">Gerenciar Categorias de Material</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_CategoriaMaterial"
                    :disabled="controle_PerfilCadastrando"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Cadastrar</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_CategoriaMaterialCadastrar"
                    :disabled="!perfil_CategoriaMaterial"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Ativar / Desativar</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_CategoriaMaterialAtivarDesativar"
                    :disabled="!perfil_CategoriaMaterial"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>

              <hr style="background-color: white" />

              <b-row>
                <b-col cols="8">
                  <label class="mr-3" style="font-weight: bold; color: #ffd600">Gerenciar Perfis</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_Perfil"
                    :disabled="controle_PerfilCadastrando"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Cadastrar</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_PerfilCadastrar"
                    :disabled="!perfil_Perfil"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Editar</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_PerfilEditar"
                    :disabled="!perfil_Perfil"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Ativar / Desativar</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_PerfilAtivarDesativar"
                    :disabled="!perfil_Perfil"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>

              <hr style="background-color: white" />

              <b-row>
                <b-col cols="8">
                  <label class="mr-3" style="font-weight: bold; color: #ffd600">Gerenciar Usuarios</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_Usuario"
                    :disabled="controle_PerfilCadastrando"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Cadastrar</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_UsuarioCadastrar"
                    :disabled="!perfil_Usuario"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Editar</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_UsuarioEditar"
                    :disabled="!perfil_Usuario"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Ativar / Desativar</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_UsuarioAtivarDesativar"
                    :disabled="!perfil_Usuario"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Habilitar / Desabilitar Login</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_UsuarioHabilitarDesabilitarLogin"
                    :disabled="!perfil_Usuario"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col cols="8">
                  <label class="mr-3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Alterar Senha</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="perfil_UsuarioAlterarSenha"
                    :disabled="!perfil_Usuario"
                    :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }"
                    :labels="{ checked: 'Sim', unchecked: 'Não' }"
                    :width="70"
                    :height="25"
                    :font-size="14"
                  />
                </b-col>
              </b-row>


              </section>
            </b-container>

            <template #modal-footer>
              <div class="afm-footer">
                <div class="afm-footer__summary"></div>
                <div class="afm-footer__actions">
                  <b-button :disabled="modal_InativaBotaoCancelar" @click="modal_Exibir = false" variant="dark" class="mb-0 mr-2">
                    Cancelar
                  </b-button>
                  <b-button v-show="modal_ExibirBotaoCadastrar" :disabled="modal_InativaBotaoCadastrar"
                    v-on:click="cadastraPerfil()" variant="success" class="mb-0 mr-2">
                    <div v-if="controle_PerfilCadastrando" class="spinner-border spinner-border-sm"></div>
                    <span v-if="controle_PerfilCadastrando"> Aguarde ...</span>
                    <span v-else><i class="fa fa-save mr-1"></i> Cadastrar</span>
                  </b-button>
                  <b-button v-show="modal_ExibirBotaoEditar" :disabled="modal_InativaBotaoEditar"
                    v-on:click="editarPerfil()" variant="info" class="mb-0">
                    <div v-if="controle_PerfilEditando" class="spinner-border spinner-border-sm"></div>
                    <span v-if="controle_PerfilEditando"> Aguarde ...</span>
                    <span v-else><i class="fa fa-save mr-1"></i> Editar</span>
                  </b-button>
                </div>
              </div>
            </template>
          </b-modal>
        </b-col>
      </b-row>

      <div class="table-card">
        <table class="estilo-tabela tabela-identidade">
          <thead>
            <tr>
              <th class="estilo-cabecalho">Descrição</th>
              <th class="estilo-cabecalho texto-centro">Status</th>
              <th class="estilo-cabecalho texto-centro">Ações</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(row, index) in perfis" :key="'perfil-' + (row.id || index)">
              <td class="estilo-celula">{{ row.descricao }}</td>
              <td class="estilo-celula texto-centro">{{ row.ativo }}</td>
              <td class="estilo-celula texto-centro">
                <button
                  type="button"
                  v-if="permissao_Editar"
                  @click="
                    modal_Titulo = 'Edição de Usuário';
                    perfil_Id = row.id;
                    perfil_Descricao = row.descricao;
                    perfil_Ativo = row.ativo == 'Ativo' ? true : false;
                    perfil_Cargo = row.cargo;
                    perfil_CargoCadastrar = row.cargoCadastrar;
                    perfil_CargoAtivarDesativar = row.cargoAtivarDesativar;
                    perfil_Cliente = row.cliente;
                    perfil_ClienteCadastrar = row.clienteCadastrar;
                    perfil_ClienteEditar = row.clienteEditar;
                    perfil_ClienteAtivarDesativar = row.clienteAtivarDesativar;
                    perfil_Fornecedor = row.fornecedor;
                    perfil_FornecedorCadastrar = row.fornecedorCadastrar;
                    perfil_FornecedorEditar = row.fornecedorEditar;
                    perfil_FornecedorAtivarDesativar = row.fornecedorAtivarDesativar;
                    perfil_Material = row.material;
                    perfil_MaterialCadastrar = row.materialCadastrar;
                    perfil_MaterialAtivarDesativar = row.materialAtivarDesativar;
                    perfil_CategoriaMaterial = row.materialCategoria;
                    perfil_CategoriaMaterialCadastrar = row.materialCategoriaCadastrar;
                    perfil_CategoriaMaterialAtivarDesativar = row.materialCategoriaAtivarDesativar;
                    perfil_Perfil = row.perfis;
                    perfil_PerfilCadastrar = row.perfilCadastrar;
                    perfil_PerfilEditar = row.perfilEditar;
                    perfil_PerfilAtivarDesativar = row.perfilAtivarDesativar;
                    perfil_Usuario = row.usuario;
                    perfil_UsuarioCadastrar = row.usuarioCadastrar;
                    perfil_UsuarioEditar = row.usuarioEditar;
                    perfil_UsuarioAtivarDesativar = row.usuarioAtivarDesativar;
                    perfil_UsuarioHabilitarDesabilitarLogin = row.usuarioHabilitarDesabilitarLogin;
                    perfil_UsuarioAlterarSenha = row.usuarioTrocarSenha;
                    modal_ExibirBotaoCadastrar = false;
                    modal_ExibirBotaoEditar = true;
                    modal_InativaBotaoCadastrar = true;
                    modal_InativaBotaoEditar = false;
                    modal_InativaAtivaoInativo = true;
                    modal_Exibir = true;
                  "
                  class="btn width-100 mb-3 mr-3 btn-outline-info"
                >
                  Editar
                </button>
                <button
                  type="button"
                  @click="ativarDesativarPerfil(row.id, row.ativo)"
                  class="btn width-100 mb-3 mr-3 btn-outline-danger"
                  v-if="row.ativo == 'Ativo' && permissao_AtivarDesativar"
                >
                  Desativar
                </button>
                <button
                  type="button"
                  @click="ativarDesativarPerfil(row.id, row.ativo)"
                  class="btn width-100 mb-3 mr-3 btn-outline-success"
                  v-if="row.ativo == 'Inativo' && permissao_AtivarDesativar"
                >
                  Ativar
                </button>
              </td>
            </tr>
            <tr v-if="!perfis || perfis.length === 0">
              <td class="estilo-celula texto-centro" colspan="3">Nenhum registro encontrado</td>
            </tr>
          </tbody>
        </table>
      </div>
    </Widget>
  </div>
</template>

<script>
import Vue from "vue";
import Widget from "@/components/Widget/Widget";
import VueSweetalert2 from "vue-sweetalert2";
import "sweetalert2/dist/sweetalert2.min.css";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.css";
import ToggleButton from "vue-js-toggle-button";
import Multiselect from "vue-multiselect";
import { VueMaskDirective } from "v-mask";
import ApiService from "@/services/api.service.js";

Vue.directive("mask", VueMaskDirective);

Vue.use(VueSweetalert2);
Vue.use(ToggleButton);

export default {
  name: "Perfil",
  components: { Widget, Loading, Multiselect },
  data() {
    return {
      isLoading: false,

      modal_Exibir: false,
      modal_Titulo: "Cadastro de Perfil",
      modal_ExibirBotaoCadastrar: true,
      modal_ExibirBotaoEditar: false,
      modal_InativaBotaoCancelar: false,
      modal_InativaBotaoCadastrar: false,
      modal_InativaBotaoEditar: true,
      modal_InativaAtivaoInativo: false,

      perfil_Id: 0,
      perfil_Descricao: "",
      perfil_Ativo: true,

      perfil_MenuCadastros: true,
      perfil_Cargo: true,
      perfil_CargoCadastrar: true,
      perfil_CargoAtivarDesativar: true,
      perfil_Cliente: true,
      perfil_ClienteCadastrar: true,
      perfil_ClienteEditar: true,
      perfil_ClienteAtivarDesativar: true,
      perfil_Fornecedor: true,
      perfil_FornecedorCadastrar: true,
      perfil_FornecedorEditar: true,
      perfil_FornecedorAtivarDesativar: true,
      perfil_Material: true,
      perfil_MaterialCadastrar: true,
      perfil_MaterialAtivarDesativar: true,
      perfil_CategoriaMaterial: true,
      perfil_CategoriaMaterialCadastrar: true,
      perfil_CategoriaMaterialAtivarDesativar: true,
      perfil_Perfil: true,
      perfil_PerfilCadastrar: true,
      perfil_PerfilEditar: true,
      perfil_PerfilAtivarDesativar: true,
      perfil_Usuario: true,
      perfil_UsuarioCadastrar: true,
      perfil_UsuarioEditar: true,
      perfil_UsuarioAtivarDesativar: true,
      perfil_UsuarioHabilitarDesabilitarLogin: true,
      perfil_UsuarioAlterarSenha: true,

      permissao_Cadastrar: true,
      permissao_Editar: true,
      permissao_AtivarDesativar: true,

      controle_PerfilCadastrando: false,
      controle_PerfilEditando: false,

      perfis: [],

      columns: ["descricao", "ativo", "acoes"],

      options: {
        perPage: 10,
        headings: {
          descricao: "Descrição",
          ativo: "Status",
          acoes: "Ações",
        },
        clientSorting: true,
        sortable: ["descricao", "ativo"],
        pagination: { chunk: 2, dropdown: true },
        texts: {
          filterPlaceholder: "Procurar por",
          count:
            "Exibindo {from} de {to} --- total de {count} itens|{count} itens|Um item",
          first: "Primeiro",
          last: "último",
          filter: "",
          limit: "Itens:",
          page: "Página:",
          noResults: "Não encontrado",
        },
      },
    };
  },
  methods: {

    cadastraPerfil: function () {
      this.isLoading = true;

      let validado = true;

      if (this.perfil_Descricao.trim() == "") {
        validado = false;
        this.$swal("Informe uma descrição válida", "", "error");
      }

      if (validado) {
        let objPerfil = {
          Id: this.perfil_Id,
          Descricao: this.perfil_Descricao,
          Ativo: this.perfil_Ativo,
          MenuCadastros: this.perfil_MenuCadastros,
          Cargo: this.perfil_Cargo,
          CargoCadastrar: this.perfil_CargoCadastrar,
          CargoAtivarDesativar: this.perfil_CargoAtivarDesativar,
          Cliente: this.perfil_Cliente,
          ClienteCadastrar: this.perfil_ClienteCadastrar,
          ClienteEditar: this.perfil_ClienteEditar,
          ClienteAtivarDesativar: this.perfil_ClienteAtivarDesativar,
          Fornecedor: this.perfil_Fornecedor,
          FornecedorCadastrar: this.perfil_FornecedorCadastrar,
          FornecedorEditar: this.perfil_FornecedorEditar,
          FornecedorAtivarDesativar: this.perfil_FornecedorAtivarDesativar,
          Material: this.perfil_Material,
          MaterialCadastrar: this.perfil_MaterialCadastrar,
          MaterialEditar: this.perfil_MaterialEditar,
          MaterialAtivarDesativar: this.perfil_MaterialAtivarDesativar,
          MaterialCategoria: this.perfil_CategoriaMaterial,
          MaterialCategoriaCadastrar: this.perfil_CategoriaMaterialCadastrar,
          MaterialCategoriaAtivarDesativar: this.perfil_CategoriaMaterialAtivarDesativar,
          Perfis: this.perfil_Perfil,
          PerfilCadastrar: this.perfil_PerfilCadastrar,
          PerfilEditar: this.perfil_PerfilEditar,
          PerfilAtivarDesativar: this.perfil_PerfilAtivarDesativar,
          Usuario: this.perfil_Usuario,
          UsuarioCadastrar: this.perfil_UsuarioCadastrar,
          UsuarioEditar: this.perfil_UsuarioEditar,
          UsuarioTrocarSenha: this.perfil_UsuarioAlterarSenha,
          UsuarioAtivarDesativar: this.perfil_UsuarioAtivarDesativar,
          UsuarioHabilitarDesabilitarLogin: this.perfil_UsuarioHabilitarDesabilitarLogin,
          Obra: this.perfil_Obra,
          ObraCadastrar: this.perfil_ObraCadastrar,
          ObraEditar: this.perfil_ObraEditar,
          ObraVisualizar: this.perfil_ObraVisualizar,
        };

        ApiService.post("Perfil", objPerfil, (result) => {
          if (result.status != 201) {
            this.$swal("Erro ao cadastrar perfil", result.message, "error");
          } else {
            this.listaPerfis();
            this.modal_Exibir = false;
          }
        });
      }

      this.isLoading = false;
    },

    editarPerfil: function () {
      this.isLoading = true;

      let validado = true;

      if (this.perfil_Descricao.trim() == "") {
        validado = false;
        this.$swal("Informe uma descrição válida", "", "error");
      }

      if (validado) {
        let objPerfil = {
          Id: this.perfil_Id,
          Descricao: this.perfil_Descricao,
          Ativo: this.perfil_Ativo,
          MenuCadastros: this.perfil_MenuCadastros,
          Cargo: this.perfil_Cargo,
          CargoCadastrar: this.perfil_CargoCadastrar,
          CargoAtivarDesativar: this.perfil_CargoAtivarDesativar,
          Cliente: this.perfil_Cliente,
          ClienteCadastrar: this.perfil_ClienteCadastrar,
          ClienteEditar: this.perfil_ClienteEditar,
          ClienteAtivarDesativar: this.perfil_ClienteAtivarDesativar,
          Fornecedor: this.perfil_Fornecedor,
          FornecedorCadastrar: this.perfil_FornecedorCadastrar,
          FornecedorEditar: this.perfil_FornecedorEditar,
          FornecedorAtivarDesativar: this.perfil_FornecedorAtivarDesativar,
          Material: this.perfil_Material,
          MaterialCadastrar: this.perfil_MaterialCadastrar,
          MaterialEditar: this.perfil_MaterialEditar,
          MaterialAtivarDesativar: this.perfil_MaterialAtivarDesativar,
          MaterialCategoria: this.perfil_CategoriaMaterial,
          MaterialCategoriaCadastrar: this.perfil_CategoriaMaterialCadastrar,
          MaterialCategoriaAtivarDesativar: this.perfil_CategoriaMaterialAtivarDesativar,
          Perfis: this.perfil_Perfil,
          PerfilCadastrar: this.perfil_PerfilCadastrar,
          PerfilEditar: this.perfil_PerfilEditar,
          PerfilAtivarDesativar: this.perfil_PerfilAtivarDesativar,
          Usuario: this.perfil_Usuario,
          UsuarioCadastrar: this.perfil_UsuarioCadastrar,
          UsuarioEditar: this.perfil_UsuarioEditar,
          UsuarioTrocarSenha: this.perfil_UsuarioAlterarSenha,
          UsuarioAtivarDesativar: this.perfil_UsuarioAtivarDesativar,
          UsuarioHabilitarDesabilitarLogin: this.perfil_UsuarioHabilitarDesabilitarLogin,
          Obra: this.perfil_Obra,
          ObraCadastrar: this.perfil_ObraCadastrar,
          ObraEditar: this.perfil_ObraEditar,
          ObraVisualizar: this.perfil_ObraVisualizar,
        };

        ApiService.put("Perfil", objPerfil, (result) => {
          if (result.status != 201) {
            this.$swal("Erro ao atualizar perfil", result.message, "error");
          } else {
            this.listaPerfis();
            this.modal_Exibir = false;
          }
        });
      }

      this.isLoading = false;
    },

    listaPerfis: function () {
      this.perfis = [];

      ApiService.getAll("Perfil", false, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          result.data.forEach((perfil) => {
            perfil.ativo = perfil.ativo ? "Ativo" : "Inativo";
            this.perfis.push(perfil);
          });
        }
      });
    },

    ativarDesativarPerfil: function (idPerfil, ativo) {
      ApiService.ativarDesativar(
        "Perfil",
        idPerfil,
        ativo == "Ativo" ? false : true,
        (result) => {
          if (result.status != 200) {
            this.$swal(
              "Erro ao ativar/desativar usuário",
              result.message,
              "error"
            );
          } else {
            this.listaPerfis();
          }
        }
      );
    },
  },
  mounted() {
    this.isLoading = true;

    var perfil = JSON.parse(localStorage.getItem("usuarioDTO")).perfil;
    this.permissao_Cadastrar = perfil.perfilCadastrar;
    this.permissao_AtivarDesativar = perfil.perfilAtivarDesativar;
    this.permissao_Editar = perfil.perfilEditar;

    this.listaPerfis();

    this.isLoading = false;
  },
};
</script>

<style src="./Perfil.scss" lang="scss" />
