import { api } from "../api/axios";

export const TipoTransacao = {
  receita: 0,
  despesa: 1,
} as const;

export type TipoTransacao = typeof TipoTransacao[keyof typeof TipoTransacao];

export interface CriarTransacaoCommand {
  descricao?: string;
  valor: number;
  tipo: TipoTransacao;
  pessoaId: string;
  categoriaId: string;
  data?: string;
}

export interface Transacao {
  descricao?: string;
  valor: number;
  tipo: TipoTransacao;
  pessoaId: string;
  categoriaId: string;
  data?: string;
}

export const transacoesService = {
  criar: async (data: CriarTransacaoCommand): Promise<void> =>
    (await api.post('/transacoes', data)).data,
  listarDetalhado: async (): Promise<Transacao[]> =>
    (await api.get('/transacoes/detalhado')).data,
};

