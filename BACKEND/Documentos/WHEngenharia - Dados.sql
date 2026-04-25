DECLARE @IdUsuario INT = (SELECT U.Id FROM WHEngenharia.dbo.Usuario U WHERE U.Nome = 'Master')

IF ((SELECT COUNT(*) FROM WHEngenharia.dbo.Cargo) = 0)
BEGIN
	INSERT INTO WHEngenharia.dbo.Cargo(Descricao, Ativo, IdUsuarioCadastro, DataCadastro, IdUsuarioAlteracao, DataUltimaAlteracao)
	SELECT C.Descricao, C.Ativo, @IdUsuario, GETDATE(), @IdUsuario, GETDATE() FROM WHEngenharia_old.dbo.Cargo C
END

IF ((SELECT COUNT(*) FROM WHEngenharia.dbo.CategoriaMaterial) = 0)
BEGIN
	INSERT INTO WHEngenharia.dbo.CategoriaMaterial(Descricao, Ativo, IdUsuarioCadastro, DataCadastro, IdUsuarioAlteracao, DataUltimaAlteracao)
	SELECT CM.Descricao, CM.Ativo, @IdUsuario, GETDATE(), @IdUsuario, GETDATE() FROM WHEngenharia_old.dbo.CategoriaMaterial CM
END

IF ((SELECT COUNT(*) FROM WHEngenharia.dbo.Cidade) = 0)
BEGIN
	INSERT INTO WHEngenharia.dbo.Cidade(UF, Nome, AliquotaImpostoISS, Ativo)
	SELECT C.UF, C.Nome, C.AliquotaImpostoISS, C.Ativo FROM WHEngenharia_old.dbo.Cidade C
END

IF ((SELECT COUNT(*) FROM WHEngenharia.dbo.Cliente) = 0)
BEGIN
	INSERT INTO WHEngenharia.dbo.Cliente(IdCidade, RazaoSocial, NomeFantasia, CNPJ, InscricaoEstadual, EmailFinanceiro, EmailComercial, ResponsavelComercial, TelefoneCelular, TelefoneFixo, CEP, Endereco, Bairro, Observacao, DiasDePagamento, Ativo, IdUsuarioCadastro, DataCadastro, IdUsuarioAlteracao, DataUltimaAlteracao)
	SELECT (C.IdCidade - 5570), C.RazaoSocial, C.NomeFantasia, C.CNPJ, C.InscricaoEstadual, C.EmailFinanceiro, C.EmailComercial, C.ResponsavelComercial, C.TelefoneCelular, C.TelefoneFixo, C.CEP, C.Endereco, C.Bairro, C.Observacao, 30, C.Ativo, @IdUsuario, GETDATE(), @IdUsuario, GETDATE() FROM WHEngenharia_old.dbo.Cliente C
END

IF ((SELECT COUNT(*) FROM WHEngenharia.dbo.CondicaoPagamento) = 0)
BEGIN
	INSERT INTO WHEngenharia.dbo.CondicaoPagamento(Descricao, Ativo, IdUsuarioCadastro, DataCadastro, IdUsuarioAlteracao, DataUltimaAlteracao)
	SELECT CP.Descricao, CP.Ativo, @IdUsuario, GETDATE(), @IdUsuario, GETDATE() FROM WHEngenharia_old.dbo.CondicaoPagamento CP
END

IF ((SELECT COUNT(*) FROM WHEngenharia.dbo.CondicaoPagamento_Parcelas) = 0)
BEGIN
	INSERT INTO WHEngenharia.dbo.CondicaoPagamento_Parcelas(IdCondicaoPagamento, DiasCorridos, PorcentagemValorTotal)
	SELECT CP.IdCondicaoPagamento, CP.DiasCorridos, CP.PorcentagemValorTotal FROM WHEngenharia_old.dbo.CondicaoPagamento_Parcelas CP
END

IF ((SELECT COUNT(*) FROM WHEngenharia.dbo.DEF) = 0)
BEGIN
	INSERT INTO WHEngenharia.dbo.DEF(Codigo, Descricao, PodeAbrirPedidoInterno, Ativo, IdUsuarioCadastro, DataCadastro, IdUsuarioAlteracao, DataUltimaAlteracao)
	SELECT D.Codigo, D.Descricao, D.PodeAbrirPedidoInterno, D.Ativo, @IdUsuario, GETDATE(), @IdUsuario, GETDATE() FROM WHEngenharia_old.dbo.DEF D
END

IF ((SELECT COUNT(*) FROM WHEngenharia.dbo.FluxoCaixaEstimativaMensal) = 0)
BEGIN
	DECLARE @Mes INT = 1
	DECLARE @Ano INT = 2024

	WHILE(@Ano <= 2100)
	BEGIN
		WHILE(@Mes <= 12)
		BEGIN
			INSERT INTO WHEngenharia.dbo.FluxoCaixaEstimativaMensal(Mes, Ano, OrdemCompraPedidoInterno, NotaFiscal, ETO, FolhaPagamento, Imposto, DespesasFixas, Reserva, Outros, Transferencias, TotalDiario, AReceberFaturado, AReceberAFaturar, Estornos, Saldo)
			VALUES (@Mes, @Ano, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)

			INSERT INTO WHEngenharia.dbo.FluxoCaixaSaldoInicial(Mes, Ano, Saldo, SaldoCalculado)
			VALUES (@Mes, @Ano, 0, 0)

			SET @Mes = @Mes + 1
		END

		SET @Mes = 1
		SET @Ano = @Ano + 1
	END
END

IF ((SELECT COUNT(*) FROM WHEngenharia.dbo.TipoFornecedor) = 0)
BEGIN
	INSERT INTO WHEngenharia.dbo.TipoFornecedor(Descricao)
	SELECT TF.Descricao FROM WHEngenharia_old.dbo.TipoFornecedor TF
END

IF ((SELECT COUNT(*) FROM WHEngenharia.dbo.TipoCentroCusto) = 0)
BEGIN
	INSERT INTO WHEngenharia.dbo.TipoCentroCusto(Descricao)
	SELECT TCC.Descricao FROM WHEngenharia_old.dbo.TipoCentroCusto TCC
END

IF ((SELECT COUNT(*) FROM WHEngenharia.dbo.StatusFaturamento) = 0)
BEGIN
	INSERT INTO WHEngenharia.dbo.StatusFaturamento(Descricao)
	SELECT SF.Descricao FROM WHEngenharia_old.dbo.StatusFaturamento SF
END

IF ((SELECT COUNT(*) FROM WHEngenharia.dbo.StatusPedidoCompra) = 0)
BEGIN
	INSERT INTO WHEngenharia.dbo.StatusPedidoCompra(Descricao)
	SELECT SPC.Descricao FROM WHEngenharia_old.dbo.StatusPedidoCompra SPC
END

IF ((SELECT COUNT(*) FROM WHEngenharia.dbo.StatusSolicitacaoCompra) = 0)
BEGIN
	INSERT INTO WHEngenharia.dbo.StatusSolicitacaoCompra(Descricao)
	SELECT SSC.Descricao FROM WHEngenharia_old.dbo.StatusSolicitacaoCompra SSC
END

IF ((SELECT COUNT(*) FROM WHEngenharia.dbo.MotivoDevolucaoSaldo) = 0)
BEGIN
	INSERT INTO WHEngenharia.dbo.MotivoDevolucaoSaldo(Descricao, Ativo, IdUsuarioCadastro, DataCadastro, IdUsuarioAlteracao, DataUltimaAlteracao)
	SELECT MDS.Descricao, MDS.Ativo, @IdUsuario, GETDATE(), @IdUsuario, GETDATE() FROM WHEngenharia_old.dbo.MotivoDevolucaoSaldo MDS
END

IF ((SELECT COUNT(*) FROM WHEngenharia.dbo.UnidadeMaterial) = 0)
BEGIN
	INSERT INTO WHEngenharia.dbo.UnidadeMaterial VALUES ('Unidade', 'UN')
END

IF ((SELECT COUNT(*) FROM WHEngenharia.dbo.Material) = 0)
BEGIN
	INSERT INTO WHEngenharia.dbo.Material(IdCategoriaMaterial, IdUnidadeMaterial, Descricao, Ativo, Servico, IdUsuarioCadastro, DataCadastro, IdUsuarioAlteracao, DataUltimaAlteracao)
	SELECT M.IdCategoriaMaterial - 26, 1, M.Descricao, M.Ativo, 0, @IdUsuario, GETDATE(), @IdUsuario, GETDATE() FROM WHEngenharia_old.dbo.Material M
END

IF NOT EXISTS (SELECT 1 FROM Cargo WHERE Descricao = 'Diretor')
BEGIN
	INSERT INTO Cargo VALUES ('Diretor', 1, @IdUsuario, GETDATE(), @IdUsuario, GETDATE())
END

IF ((SELECT COUNT(*) FROM WHEngenharia.dbo.Fornecedor) = 0)
BEGIN
	INSERT INTO WHEngenharia.dbo.Fornecedor
	SELECT F.IdTipoFornecedor, F.IdCidade - 5570, F.IdCondicaoPagamento, F.RazaoSocial, F.NomeFantasia, F.CNPJ, F.InscricaoEstadual, F.NumeroCadastral, F.Nome, F.CPF, F.Email, F.NomeVendedor, F.TelefoneCelular, F.TelefoneFixo, F.CEP, F.Endereco, F.Bairro, F.Observacao, NULL, NULL, NULL, NULL, F.Ativo, @IdUsuario, GETDATE(), @IdUsuario, GETDATE() FROM WHEngenharia_old.dbo.Fornecedor F
END


--SELECT * FROM WHEngenharia.dbo.Fornecedor
--SELECT * FROM WHEngenharia_old.dbo.Fornecedor