import { api, type Paginated, type PaginatedQuery } from "../api/axios";

export interface CriarPessoaCommand {
  nome?: string;
  dataNascimento: string;
}

export interface Pessoa {
  id: string;
  nome: string;
  dataNascimento: string;
}

export interface PessoaSimples {
  id: string;
  nome: string;
}

export const pessoaService = {
  listar: async (signal: AbortSignal | undefined = undefined): Promise<PessoaSimples[]> =>
    (await api.get('/pessoa', { signal: signal })).data,

  listarDetalhado: async (query: PaginatedQuery, signal: AbortSignal | undefined = undefined): Promise<Paginated<Pessoa>> =>
    (await api.get('/pessoa/detalhado', { params: query, signal: signal })).data,

  criar: async (data: CriarPessoaCommand, signal: AbortSignal | undefined = undefined): Promise<void> =>
    (await api.post('/pessoa', data, { signal: signal })).data,

  deletar: async (id: string, signal: AbortSignal | undefined = undefined): Promise<void> =>
    (await api.delete(`/pessoa/${id}`, { signal: signal })).data,
};

