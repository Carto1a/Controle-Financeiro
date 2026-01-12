import { api, type Paginated } from "../api/axios";

export const TipoTransacao = {
  receita: 0,
  despesa: 1,
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

  listarDetalhado: async (signal: AbortSignal | undefined = undefined): Promise<Paginated<Transacao>> =>
    (await api.get('/transacoes/detalhado', { signal: signal })).data,
};

