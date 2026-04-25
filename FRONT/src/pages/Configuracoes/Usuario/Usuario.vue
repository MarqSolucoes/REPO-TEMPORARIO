<template>
  <div class="usuario-page">
    <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
    <h1 class="page-title">Usuarios &nbsp;</h1>

    <Widget customHeader class="estiloWidget">
      <b-row>
        <b-col cols="auto" class="mr-auto p-3"></b-col>
        <b-col cols="auto" style="padding: 0">
          <button v-if="usuarioDTO != null && usuarioDTO.usuarioCadastrar" @click="
            modal_Titulo = 'Cadastro de Usuário';
          usuario_Id = 0;
          usuario_Nome = '';
          usuario_Email = '';
          usuario_HabilitaLogin = true;
          usuario_Login = '';
          usuario_Ativo = true;
          usuario_Senha = '';
          usuario_SenhaConfirmacao = '';
          usuario_CargoSelecionado = null;
          modal_ExibirBotaoCadastrar = true;
          modal_ExibirBotaoEditar = false;
          modal_ExibirCamposDeSenha = true;
          modal_InativaBotaoCadastrar = false;
          modal_InativaBotaoEditar = true;
          modal_InativarCampoLogin = false;
          modal_Exibir = true;
          " v-b-modal.cadastro type="button" class="btn width-120 mb-3 mr-4 btn-outline-success">
            Novo Usuário
          </button>

          <b-modal :no-close-on-backdrop="true" id="alteracaoSenha" class="modal-dialog modal-md afm-modal"
            body-bg-variant="modalAlteracaoSenha" header-bg-variant="bodyModalAlteracaoSenha"
            footer-bg-variant="bodyModalAlteracaoSenha" v-model="modalAlteracaoSenha_Exibir" size="lg">

            <div class="afm-hero">
              <div>
                <span class="afm-hero__eyebrow">Segurança</span>
                <h3 class="afm-hero__title">Alteração de senha</h3>
                <p class="afm-hero__description">Defina uma nova senha para o acesso do usuário.</p>
              </div>
            </div>

            <b-container fluid class="afm-sections">
              <section class="afm-section-card">
                <b-row>
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Senha</label>
                      <b-form-input type="password" v-model="usuario_SenhaAlteracao" placeholder="Senha"></b-form-input>
                    </div>
                  </b-col>
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Confirmação senha</label>
                      <b-form-input type="password" v-model="usuario_SenhaConfirmacaoAlteracao" placeholder="Senha"></b-form-input>
                    </div>
                  </b-col>
                </b-row>

                <div class="mt-3">
                  <password v-model="usuario_SenhaAlteracao" :strength-meter-only="true" />
                </div>
              </section>
            </b-container>

            <template #modal-footer>
              <div class="afm-footer">
                <div class="afm-footer__summary"></div>
                <div class="afm-footer__actions">
                  <b-button @click="modalAlteracaoSenha_Exibir = false" variant="dark" class="mb-0 mr-2">
                    Cancelar
                  </b-button>
                  <b-button v-on:click="alterarSenha()" variant="success" class="mb-0">
                    <div v-if="controle_SenhaAlterando" class="spinner-border spinner-border-sm"></div>
                    <span v-if="controle_SenhaAlterando"> Aguarde ...</span>
                    <span v-else><i class="fa fa-key mr-1"></i> Alterar Senha</span>
                  </b-button>
                </div>
              </div>
            </template>
          </b-modal>

          <b-modal :no-close-on-backdrop="true" id="cadastro" class="modal-dialog modal-md afm-modal" body-bg-variant="modal"
            header-bg-variant="bodyModal" footer-bg-variant="bodyModal" v-model="modal_Exibir" size="lg">

            <div class="afm-hero">
              <div>
                <span class="afm-hero__eyebrow">Usuário</span>
                <h3 class="afm-hero__title">{{ modal_Titulo }}</h3>
                <p class="afm-hero__description">Informe os dados do usuário e suas permissões de acesso.</p>
              </div>
            </div>

            <b-container fluid class="afm-sections">
              <section class="afm-section-card">
              <b-row>
                <b-col>
                  <label class="mr-3">Cargo do Usuário:</label>
                  <multiselect v-model="usuario_CargoSelecionado" :multiple="false" :options="cargos"
                    :custom-label="nameWithLang" select-label="Selecionar" placeholder="Selecione um cargo"
                    label="descricao" track-by="descricao" :disabled="controle_UsuarioCadastrando"
                    style="background-color: black"></multiselect>
                </b-col>
              </b-row>

              <br />

              <b-row>
                <b-col>
                  <label class="mr-3">Nome:</label>
                  <b-form-input v-model="usuario_Nome" style="color: white"></b-form-input>
                </b-col>
              </b-row>
              
              <br />

              <b-row>
                <b-col>
                  <label class="mr-3">Email:</label>
                  <b-form-input v-model="usuario_Email" style="color: white"></b-form-input>
                </b-col>
              </b-row>

              <br />

              <b-row>
                <b-col>
                  <label class="mr-3">Login:</label>
                  <b-form-input v-model="usuario_Login" :readonly="modal_InativarCampoLogin"
                    style="color: white; background-color: #040620"></b-form-input>
                </b-col>
              </b-row>

              <br />

              <b-row v-if="modal_ExibirCamposDeSenha">
                <b-col>
                  <label class="mr-3">Senha:</label>
                  <b-form-input type="password" :disabled="controle_UsuarioCadastrando" v-model="usuario_Senha"
                    placeholder="Senha"></b-form-input>
                </b-col>

                <b-col>
                  <label class="mr-3">Confirmação senha:</label>
                  <b-form-input type="password" :disabled="controle_UsuarioCadastrando" v-model="usuario_SenhaConfirmacao"
                    placeholder="Senha"></b-form-input>
                </b-col>
              </b-row>

              <br />

              <b-row v-if="modal_ExibirCamposDeSenha">
                <b-col>
                  <password v-model="usuario_Senha" :strength-meter-only="true" />
                </b-col>
                <b-col> </b-col>
              </b-row>

              <br />

              <b-row>
                <b-col>
                  <label class="mr-3">Habilita Login:</label>
                  <toggle-button v-model="usuario_HabilitaLogin" :color="{
                    checked: '#2D8515',
                    unchecked: '#FF0000',
                    disabled: '#CCCCCC',
                  }" :labels="{ checked: 'Sim', unchecked: 'Não' }" :width="80" :height="25" :font-size="14" />
                </b-col>

                <b-col>
                  <label class="mr-3">Status:</label>
                  <toggle-button v-model="usuario_Ativo" :color="{
                    checked: '#2D8515',
                    unchecked: '#FF0000',
                    disabled: '#CCCCCC',
                  }" :labels="{ checked: 'Ativo', unchecked: 'Inativo' }" :width="80" :height="25" :font-size="14" />
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
                  <label class="mr-3" style="font-weight: bold; color: #ffd600">Gerenciar Cidades</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="usuario_Cidade"
                    :disabled="controle_UsuarioCadastrando"
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
                    v-model="usuario_CidadeEditar"
                    :disabled="!usuario_Cidade"
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
                    v-model="usuario_CidadeAtivarDesativar"
                    :disabled="!usuario_Cidade"
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
                  <label class="mr-3" style="font-weight: bold; color: #ffd600">Gerenciar Cargos</label>
                </b-col>
                <b-col>
                  <toggle-button
                    v-model="usuario_Cargo"
                    :disabled="controle_UsuarioCadastrando"
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
                    v-model="usuario_CargoCadastrar"
                    :disabled="!usuario_Cargo"
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
                    v-model="usuario_CargoAtivarDesativar"
                    :disabled="!usuario_Cargo"
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
                    v-model="usuario_Cliente"
                    :disabled="controle_UsuarioCadastrando"
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
                    v-model="usuario_ClienteCadastrar"
                    :disabled="!usuario_Cliente"
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
                    v-model="usuario_ClienteEditar"
                    :disabled="!usuario_Cliente"
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
                    v-model="usuario_ClienteAtivarDesativar"
                    :disabled="!usuario_Cliente"
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
                    v-model="usuario_Fornecedor"
                    :disabled="controle_UsuarioCadastrando"
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
                    v-model="usuario_FornecedorCadastrar"
                    :disabled="!usuario_Fornecedor"
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
                    v-model="usuario_FornecedorEditar"
                    :disabled="!usuario_Fornecedor"
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
                    v-model="usuario_FornecedorAtivarDesativar"
                    :disabled="!usuario_Fornecedor"
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
                    v-model="usuario_Material"
                    :disabled="controle_UsuarioCadastrando"
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
                    v-model="usuario_MaterialCadastrar"
                    :disabled="!usuario_Material"
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
                    v-model="usuario_MaterialAtivarDesativar"
                    :disabled="!usuario_Material"
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
                    v-model="usuario_CategoriaMaterial"
                    :disabled="controle_UsuarioCadastrando"
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
                    v-model="usuario_CategoriaMaterialCadastrar"
                    :disabled="!usuario_CategoriaMaterial"
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
                    v-model="usuario_CategoriaMaterialAtivarDesativar"
                    :disabled="!usuario_CategoriaMaterial"
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
                    v-model="usuario_Usuario"
                    :disabled="controle_UsuarioCadastrando"
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
                    v-model="usuario_UsuarioCadastrar"
                    :disabled="!usuario_Usuario"
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
                    v-model="usuario_UsuarioEditar"
                    :disabled="!usuario_Usuario"
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
                    v-model="usuario_UsuarioAtivarDesativar"
                    :disabled="!usuario_Usuario"
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
                    v-model="usuario_UsuarioHabilitarDesabilitarLogin"
                    :disabled="!usuario_Usuario"
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
                    v-model="usuario_UsuarioAlterarSenha"
                    :disabled="!usuario_Usuario"
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
                    v-on:click="cadastraUsuario()" variant="success" class="mb-0 mr-2">
                    <div v-if="controle_UsuarioCadastrando" class="spinner-border spinner-border-sm"></div>
                    <span v-if="controle_UsuarioCadastrando"> Aguarde ...</span>
                    <span v-else><i class="fa fa-save mr-1"></i> Cadastrar</span>
                  </b-button>
                  <b-button v-show="modal_ExibirBotaoEditar" :disabled="modal_InativaBotaoEditar"
                    v-on:click="editarUsuario()" variant="info" class="mb-0">
                    <div v-if="controle_UsuarioEditando" class="spinner-border spinner-border-sm"></div>
                    <span v-if="controle_UsuarioEditando"> Aguarde ...</span>
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
              <th class="estilo-cabecalho">Cargo</th>
              <th class="estilo-cabecalho">Nome</th>
              <th class="estilo-cabecalho texto-centro">Habilita Login</th>
              <th class="estilo-cabecalho">Login</th>
              <th class="estilo-cabecalho texto-centro">Status</th>
              <th class="estilo-cabecalho texto-centro">Ações</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(row, index) in usuarios" :key="'usuario-' + (row.id || index)">
              <td class="estilo-celula">{{ row.cargoUsuario ? row.cargoUsuario.descricao : '' }}</td>
              <td class="estilo-celula">{{ row.nome }}</td>
              <td class="estilo-celula texto-centro">{{ row.habilitaLogin }}</td>
              <td class="estilo-celula">{{ row.login }}</td>
              <td class="estilo-celula texto-centro">{{ row.ativo }}</td>
              <td class="estilo-celula texto-centro">
                <button
                  type="button"
                  class="btn btn-outline-info"
                  @click="abrirModalAcoesUsuario(row)"
                >
                  Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                </button>
              </td>
            </tr>
            <tr v-if="!usuarios || usuarios.length === 0">
              <td class="estilo-celula texto-centro" colspan="6">Nenhum registro encontrado</td>
            </tr>
          </tbody>
        </table>
      </div>
    </Widget>

    <!-- Modal de ações da tabela -->
    <ModalAcoes
      :exibir.sync="modalAcoes_Exibir"
      :titulo="modalAcoes_Titulo"
      :itens="modalAcoes_Itens"
    />
  </div>
</template>

<script>
import Vue from "vue";
import Widget from "@/components/Widget/Widget";
import VueSweetalert2 from "vue-sweetalert2";
import "sweetalert2/dist/sweetalert2.min.css";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.css";
import Multiselect from "vue-multiselect";
import ToggleButton from "vue-js-toggle-button";
import Password from "vue-password-strength-meter";
import { VueMaskDirective } from "v-mask";
import { sha256 } from "js-sha256";

import ApiService from "@/services/api.service.js";
import ModalAcoes from "../../../components/ModalAcoes/ModalAcoes.vue";

Vue.directive("mask", VueMaskDirective);
Vue.use(VueSweetalert2);
Vue.use(ToggleButton);

export default {
  name: "Usuario",
  components: { Widget, Loading, Multiselect, Password, ModalAcoes },
  data() {
    return {
      // Estado do <ModalAcoes>
      modalAcoes_Exibir: false,
      modalAcoes_Titulo: "",
      modalAcoes_Itens: [],

      isLoading: false,

      modalAlteracaoSenha_Exibir: false,

      modal_Exibir: false,
      modal_Titulo: "Cadastro de Cliente",
      modal_ExibirBotaoCadastrar: true,
      modal_ExibirBotaoEditar: false,
      modal_ExibirCamposDeSenha: true,
      modal_InativaBotaoCancelar: false,
      modal_InativaBotaoCadastrar: false,
      modal_InativaBotaoEditar: true,
      modal_InativarCampoLogin: false,

      usuarioDTO: null,

      usuario_Id: 0,
      usuario_Nome: "",
      usuario_Email: "",
      usuario_HabilitaLogin: true,
      usuario_Login: "",
      usuario_Senha: "",
      usuario_SenhaConfirmacao: "",
      usuario_Ativo: true,

      usuario_MenuCadastros: true,
      usuario_Cidade: true,
      usuario_CidadeEditar: true,
      usuario_CidadeAtivarDesativar: true,
      usuario_Cargo: true,
      usuario_CargoCadastrar: true,
      usuario_CargoAtivarDesativar: true,
      usuario_Cliente: true,
      usuario_ClienteCadastrar: true,
      usuario_ClienteEditar: true,
      usuario_ClienteAtivarDesativar: true,
      usuario_Fornecedor: true,
      usuario_FornecedorCadastrar: true,
      usuario_FornecedorEditar: true,
      usuario_FornecedorAtivarDesativar: true,
      usuario_Material: true,
      usuario_MaterialCadastrar: true,
      usuario_MaterialAtivarDesativar: true,
      usuario_CategoriaMaterial: true,
      usuario_CategoriaMaterialCadastrar: true,
      usuario_CategoriaMaterialAtivarDesativar: true,
      usuario_Perfil: true,
      usuario_PerfilCadastrar: true,
      usuario_PerfilEditar: true,
      usuario_PerfilAtivarDesativar: true,
      usuario_Usuario: true,
      usuario_UsuarioCadastrar: true,
      usuario_UsuarioEditar: true,
      usuario_UsuarioAtivarDesativar: true,
      usuario_UsuarioHabilitarDesabilitarLogin: true,
      usuario_UsuarioAlterarSenha: true,

      usuario_SenhaAlteracao: "",
      usuario_SenhaConfirmacaoAlteracao: "",

      usuario_CargoSelecionado: {},

      controle_UsuarioCadastrando: false,
      controle_UsuarioEditando: false,
      controle_SenhaAlterando: false,

      usuarios: [],
      perfis: [],
      cargos: [],

      columns: [
        "cargoDescricao",
        "nome",
        "habilitaLogin",
        "login",
        "ativo",
        "acoes",
      ],

      options: {
        perPage: 10,
        headings: {
          id: 0,
          cargoDescricao: "Cargo",
          nome: "Nome",
          habilitaLogin: "Habilita Login",
          login: "Login",
          ativo: "Status",
          acoes: "Ações",
        },
        clientSorting: true,
        sortable: [
          "cargo",
          "nome",
          "habilitaLogin",
          "login",
          "ativo",
        ],
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
    // ============================================================
    // Ações da tabela (usadas pelo <ModalAcoes>).
    // Ver components/ModalAcoes/ModalAcoes.vue para a API completa.
    // ============================================================
    montaAcoesUsuario(row) {
      const u = this.usuarioDTO;
      return [
        {
          label: "Editar",
          descricao: "Editar os dados do usuário",
          icone: "pencil",
          variante: "primary",
          onClick: () => {
            this.modal_Titulo = "Edição de Usuário";
            this.usuario_Id = row.id;
            this.usuario_Cargo = row.cargo;
            this.usuario_Nome = row.nome;
            this.usuario_Email = row.email;
            this.usuario_HabilitaLogin = row.habilitaLogin == "Sim";
            this.usuario_Login = row.login;
            this.usuario_Ativo = row.ativo == "Ativo";
            this.usuario_Senha = "";
            this.usuario_SenhaConfirmacao = "";
            this.usuario_CargoSelecionado = this.cargos.filter((cargo) => {
              return cargo.id == row.cargo.id;
            });
            this.modal_ExibirBotaoCadastrar = false;
            this.modal_ExibirBotaoEditar = true;
            this.modal_ExibirCamposDeSenha = false;
            this.modal_InativaBotaoCadastrar = true;
            this.modal_InativaBotaoEditar = false;
            this.modal_InativarCampoLogin = true;
            this.modal_Exibir = true;
          },
          visible: u != null && u.usuarioEditar,
        },
        {
          label: "Desativar",
          descricao: "Desativar este usuário",
          icone: "ban",
          variante: "danger",
          onClick: () => this.ativarDesativarUsuario(row.id, row.ativo),
          visible: row.ativo == "Ativo" && u != null && u.usuarioAtivarDesativar,
        },
        {
          label: "Ativar",
          descricao: "Ativar este usuário",
          icone: "check",
          variante: "success",
          onClick: () => this.ativarDesativarUsuario(row.id, row.ativo),
          visible: row.ativo == "Inativo" && u != null && u.usuarioAtivarDesativar,
        },
        {
          label: "Desabilitar Login",
          descricao: "Impedir o usuário de entrar no sistema",
          icone: "lock",
          variante: "danger",
          onClick: () => this.habilitarDesabilitarLogin(row.id, row.habilitaLogin),
          visible:
            row.habilitaLogin == "Sim" &&
            u != null &&
            u.usuarioHabilitarDesabilitarLogin,
        },
        {
          label: "Habilitar Login",
          descricao: "Permitir o usuário entrar no sistema",
          icone: "unlock",
          variante: "success",
          onClick: () => this.habilitarDesabilitarLogin(row.id, row.habilitaLogin),
          visible:
            row.habilitaLogin == "Não" &&
            u != null &&
            u.usuarioHabilitarDesabilitarLogin,
        },
        {
          label: "Alterar Senha",
          descricao: "Definir uma nova senha para o usuário",
          icone: "key",
          variante: "warning",
          onClick: () => {
            this.usuario_Id = row.id;
            this.usuario_SenhaAlteracao = "";
            this.usuario_SenhaConfirmacaoAlteracao = "";
            this.modalAlteracaoSenha_Exibir = true;
          },
          visible: u != null && u.usuarioTrocarSenha,
        },
      ];
    },
    abrirModalAcoesUsuario(row) {
      this.modalAcoes_Itens = this.montaAcoesUsuario(row);
      this.modalAcoes_Titulo = "Ações do usuário " + row.nome;
      this.modalAcoes_Exibir = true;
    },

    nameWithLang({ descricao }) {
      return `${descricao}`;
    },

    cadastraUsuario: function () {
      this.isLoading = true;

      let validado = true;

      if (this.usuario_CargoSelecionado == null) {
        validado = false;
        this.$swal("Selecione um cargo", "", "error");
      }

      if (this.usuario_Nome.trim() == "") {
        validado = false;
        this.$swal("Informe um nome válido", "", "error");
      }

      if (this.usuario_Email.trim() == "") {
        validado = false;
        this.$swal("Informe um email válido", "", "error");
      }

      if (this.usuario_Login.trim() == "") {
        validado = false;
        this.$swal("Informe um login válido", "", "error");
      }

      if (
        this.usuario_Senha.trim() == "" ||
        this.usuario_SenhaConfirmacao.trim() == ""
      ) {
        validado = false;
        this.$swal("Informe uma senha válida", "", "error");
      }

      if (this.usuario_Senha != this.usuario_SenhaConfirmacao) {
        validado = false;
        this.$swal("As senhas não correspondem", "", "error");
      }

      if (validado) {
        let objetoUsuario = {
          Id: this.usuario_Id,
          IdCargo: Array.isArray(this.usuario_CargoSelecionado)
            ? this.usuario_CargoSelecionado[0].id
            : this.usuario_CargoSelecionado.id,
          Nome: this.usuario_Nome,
          Email: this.usuario_Email,
          HabilitaLogin: this.usuario_HabilitaLogin,
          Login: this.usuario_Login,
          Senha: sha256(String(this.usuario_Senha)),
          Ativo: this.usuario_Ativo,
        };

        ApiService.post("Usuario", objetoUsuario, (result) => {
          if (result.status != 201) {
            this.$swal("Erro ao cadastrar usuário", result.message, "error");
          } else {
            this.listaUsuarios();
            this.modal_Exibir = false;
          }
        });
      }

      this.isLoading = false;
    },

    editarUsuario: function () {
      this.isLoading = true;

      let validado = true;

      if (this.usuario_CargoSelecionado == null) {
        validado = false;
        this.$swal("Selecione um cargo", "", "error");
      }

      if (this.usuario_Nome.trim() == "") {
        validado = false;
        this.$swal("Informe um nome válido", "", "error");
      }

      if (this.usuario_Email.trim() == "") {
        validado = false;
        this.$swal("Informe um Email válido", "", "error");
      }

      if (validado) {
        let objetoUsuario = {
          Id: this.usuario_Id,
          IdCargo: Array.isArray(this.usuario_CargoSelecionado)
            ? this.usuario_CargoSelecionado[0].id
            : this.usuario_CargoSelecionado.id,
          Nome: this.usuario_Nome,
          Email: this.usuario_Email,
          HabilitaLogin: this.usuario_HabilitaLogin,
          Login: this.usuario_Login,
          Ativo: this.usuario_Ativo,
        };

        ApiService.put("Usuario", objetoUsuario, (result) => {
          if (result.status != 201) {
            this.$swal("Erro ao editar usuário", result.message, "error");
          } else {
            this.listaUsuarios();
            this.modal_Exibir = false;
          }
        });
      }

      this.isLoading = false;
    },

    ativarDesativarUsuario: function (idUsuario, ativo) {
      ApiService.ativarDesativar(
        "Usuario",
        idUsuario,
        ativo == "Ativo" ? false : true,
        (result) => {
          if (result.status != 200) {
            this.$swal(
              "Erro ao ativar/desativar usuário",
              result.message,
              "error"
            );
          } else {
            this.listaUsuarios();
          }
        }
      );
    },

    habilitarDesabilitarLogin: function (idUsuario, ativo) {
      ApiService.habilitarDesabilitarLogin(
        idUsuario,
        ativo == "Sim" ? false : true,
        (result) => {
          if (result.status != 200) {
            this.$swal(
              "Erro ao habilitar/desabilitar login",
              result.message,
              "error"
            );
          } else {
            this.listaUsuarios();
          }
        }
      );
    },

    alterarSenha: function () {
      this.isLoading = true;

      let validado = true;

      if (
        this.usuario_SenhaAlteracao.trim() == "" ||
        this.usuario_SenhaConfirmacaoAlteracao.trim() == ""
      ) {
        validado = false;
        this.$swal("Informe uma senha válida", "", "error");
      }

      if (
        this.usuario_SenhaAlteracao != this.usuario_SenhaConfirmacaoAlteracao
      ) {
        validado = false;
        this.$swal("As senhas não correspondem", "", "error");
      }

      if (validado) {
        ApiService.alterarSenha(
          this.usuario_Id,
          sha256(String(this.usuario_SenhaAlteracao)),
          (result) => {
            if (result.status != 200) {
              this.$swal("Erro ao alterar senha", result.message, "error");
            } else {
              this.listaUsuarios();
              this.modalAlteracaoSenha_Exibir = false;
            }
          }
        );
      }

      this.isLoading = false;
    },

    listaCargos: function () {
      this.cargos = [];

      ApiService.getAll("Cargo", true, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          result.data.forEach((cargo) => {
            this.cargos.push(cargo);
          });
        }
      });
    },

    listaUsuarios: function () {
      this.usuarios = [];

      ApiService.getAll("Usuario", false, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          result.data.forEach((usuario) => {
            usuario.ativo = usuario.ativo ? "Ativo" : "Inativo";
            usuario.habilitaLogin = usuario.habilitaLogin ? "Sim" : "Não";
            this.usuarios.push(usuario);
          });

          console.log(this.usuarios);
        }
      });
    },

  },
  mounted() {
    this.isLoading = true;

    this.usuarioDTO = JSON.parse(localStorage.getItem("usuarioDTO"));

    this.listaCargos();
    this.listaUsuarios();

    this.isLoading = false;
  },
};
</script>

<style src="./Usuario.scss" lang="scss" />
<style src="vue-multiselect/dist/vue-multiselect.min.css"></style>