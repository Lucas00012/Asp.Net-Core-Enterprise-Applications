using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using NSE.Pedidos.API.Application.DTO;
using NSE.Pedidos.Domain.Pedidos;

namespace NSE.Pedidos.API.Application.Queries
{
    public interface IPedidoQueries
    {
        Task<PedidoDTO> ObterUltimoPedido(Guid clienteId);
        Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId(Guid clienteId);
        Task<PedidoDTO> ObterPedidosAutorizados();
    }

    public class PedidoQueries : IPedidoQueries
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoQueries(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<PedidoDTO> ObterUltimoPedido(Guid clienteId)
        {
            var conexao = _pedidoRepository.ObterConexao();

            var pedido = (await conexao.QueryAsync<dynamic>(@"
                SELECT TOP(1)
                    ID AS 'ProdutoId', 
	                CODIGO, 
	                VOUCHERUTILIZADO, 
	                DESCONTO, 
	                VALORTOTAL,
	                PEDIDOSTATUS,
                    LOGRADOURO,
	                NUMERO, 
	                BAIRRO, 
	                CEP, 
	                COMPLEMENTO, 
	                CIDADE, 
	                ESTADO
                FROM PEDIDOS
                WHERE 
	                CLIENTEID = @clienteId AND
	                PEDIDOSTATUS = 1 
                ORDER BY DATACADASTRO DESC
            ", new { clienteId })).FirstOrDefault();

            if (pedido == null)
                return null;

            var items = await conexao.QueryAsync<dynamic>(@"
                SELECT 
                    PRODUTONOME,
                    VALORUNITARIO,
                    QUANTIDADE,
                    PRODUTOIMAGEM
                FROM PEDIDOITEMS WHERE PEDIDOID = @pedidoId
            ", new { pedidoId = pedido.ProdutoId });

            return MapearPedido(pedido, items);
        }

        public async Task<PedidoDTO> ObterPedidosAutorizados()
        {
            // Correção para pegar todos os itens do pedido e ordernar pelo pedido mais antigo
            const string sql = @"SELECT 
                                P.ID as 'PedidoId', P.ID, P.CLIENTEID, 
                                PI.ID as 'PedidoItemId', PI.ID, PI.PRODUTOID, PI.QUANTIDADE 
                                FROM PEDIDOS P 
                                INNER JOIN PEDIDOITEMS PI ON P.ID = PI.PEDIDOID 
                                WHERE P.PEDIDOSTATUS = 1                                
                                ORDER BY P.DATACADASTRO";

            // Utilizacao do lookup para manter o estado a cada ciclo de registro retornado
            var lookup = new Dictionary<Guid, PedidoDTO>();

            await _pedidoRepository.ObterConexao().QueryAsync<PedidoDTO, PedidoItemDTO, PedidoDTO>(sql,
                (p, pi) =>
                {
                    if (!lookup.TryGetValue(p.Id, out var pedidoDTO))
                        lookup.Add(p.Id, pedidoDTO = p);

                    pedidoDTO.PedidoItems ??= new List<PedidoItemDTO>();
                    pedidoDTO.PedidoItems.Add(pi);

                    return pedidoDTO;

                }, splitOn: "PedidoId,PedidoItemId");

            // Obtendo dados o lookup
            return lookup.Values.OrderBy(p => p.Data).FirstOrDefault();
        }

        public async Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId(Guid clienteId)
        {
            var pedidos = await _pedidoRepository.ObterListaPorClienteId(clienteId);

            return pedidos.Select(PedidoDTO.ParaPedidoDTO);
        }

        private PedidoDTO MapearPedido(dynamic pedido, IEnumerable<dynamic> items)
        {
            return new PedidoDTO
            {
                Codigo = pedido.CODIGO,
                Status = pedido.PEDIDOSTATUS,
                ValorTotal = pedido.VALORTOTAL,
                Desconto = pedido.DESCONTO,
                VoucherUtilizado = pedido.VOUCHERUTILIZADO,
                Endereco = new EnderecoDTO
                {
                    Logradouro = pedido.LOGRADOURO,
                    Bairro = pedido.BAIRRO,
                    Cep = pedido.CEP,
                    Cidade = pedido.CIDADE,
                    Complemento = pedido.COMPLEMENTO,
                    Estado = pedido.ESTADO,
                    Numero = pedido.NUMERO
                },
                PedidoItems = items.Select(item => new PedidoItemDTO
                {
                    Nome = item.PRODUTONOME,
                    Valor = item.VALORUNITARIO,
                    Quantidade = item.QUANTIDADE,
                    Imagem = item.PRODUTOIMAGEM
                }).ToList()
            };
        }
    }
}