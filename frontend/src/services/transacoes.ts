import { api, type Paginated, type PaginatedQuery } from "../api/axios";

export const TipoTransacao = {
  despesa: 0,
  receita: 1,
} as const;

export type TipoTransacao = typeof TipoTransacao[keyof typeof TipoTransacao];

export interface CriarTransacaoCommand {
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  pessoaId: string;
  categoriaId: string;
  data?: string;
}

export interface Transacao {
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  pessoaId: string;
  categoriaId: string;
  data?: string;
}

export const transacoesService = {
  criar: async (data: CriarTransacaoCommand, signal: AbortSignal | undefined = undefined): Promise<void> =>
    (await api.post('/transacoes', data, { signal: signal })).data,

  listarDetalhado: async (query: PaginatedQuery, signal: AbortSignal | undefined = undefined): Promise<Paginated<Transacao>> =>
    (await api.get('/transacoes/detalhado', { params: query, signal: signal })).data,
};

